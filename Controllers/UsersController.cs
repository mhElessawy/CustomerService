using CustomerServicesSystem.Models;
using CustomerServicesSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerServicesSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userMgr;
        private readonly RoleManager<IdentityRole> _roleMgr;

        public UsersController(UserManager<ApplicationUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }

        // ── INDEX ─────────────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var users = await _userMgr.Users.OrderBy(x => x.FullName).ToListAsync();
            var list = new List<UserListItemVM>();
            foreach (var u in users)
            {
                var roles = await _userMgr.GetRolesAsync(u);
                list.Add(new UserListItemVM
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email ?? "",
                    IsActive = u.IsActive,
                    Role = roles.FirstOrDefault() ?? "-",
                    CreatedAt = u.CreatedAt
                });
            }
            return View(list);
        }

        // ── CREATE ────────────────────────────────────────────────────
        public IActionResult Create() => View("Form", new UserFormVM());

        // ── EDIT ──────────────────────────────────────────────────────
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userMgr.FindByIdAsync(id);
            if (user == null) return NotFound();
            var roles = await _userMgr.GetRolesAsync(user);
            var vm = new UserFormVM
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? "",
                Role = roles.FirstOrDefault() ?? "Agent",
                IsActive = user.IsActive
            };
            return View("Form", vm);
        }

        // ── SAVE ──────────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(UserFormVM vm)
        {
            // Password required when creating
            if (string.IsNullOrWhiteSpace(vm.Id) && string.IsNullOrWhiteSpace(vm.Password))
                ModelState.AddModelError(nameof(vm.Password), "Password is required for new users.");

            if (!ModelState.IsValid) return View("Form", vm);

            if (string.IsNullOrWhiteSpace(vm.Id))
            {
                // Create
                if (await _userMgr.FindByEmailAsync(vm.Email) != null)
                {
                    ModelState.AddModelError(nameof(vm.Email), "This email is already registered.");
                    return View("Form", vm);
                }

                var user = new ApplicationUser
                {
                    UserName = vm.Email,
                    Email = vm.Email,
                    FullName = vm.FullName,
                    IsActive = vm.IsActive,
                    CreatedAt = DateTime.Now
                };
                var result = await _userMgr.CreateAsync(user, vm.Password!);
                if (!result.Succeeded)
                {
                    foreach (var e in result.Errors)
                        ModelState.AddModelError("", e.Description);
                    return View("Form", vm);
                }
                await _userMgr.AddToRoleAsync(user, vm.Role);
                TempData["Success"] = $"User '{vm.FullName}' created successfully.";
            }
            else
            {
                // Edit
                var user = await _userMgr.FindByIdAsync(vm.Id);
                if (user == null) return NotFound();

                // Check duplicate email (excluding self)
                var existing = await _userMgr.FindByEmailAsync(vm.Email);
                if (existing != null && existing.Id != vm.Id)
                {
                    ModelState.AddModelError(nameof(vm.Email), "This email is already registered.");
                    return View("Form", vm);
                }

                user.FullName = vm.FullName;
                user.Email = vm.Email;
                user.UserName = vm.Email;
                user.IsActive = vm.IsActive;
                await _userMgr.UpdateAsync(user);

                // Update password if provided
                if (!string.IsNullOrWhiteSpace(vm.Password))
                {
                    var token = await _userMgr.GeneratePasswordResetTokenAsync(user);
                    var pwResult = await _userMgr.ResetPasswordAsync(user, token, vm.Password);
                    if (!pwResult.Succeeded)
                    {
                        foreach (var e in pwResult.Errors)
                            ModelState.AddModelError("", e.Description);
                        return View("Form", vm);
                    }
                }

                // Update role
                var currentRoles = await _userMgr.GetRolesAsync(user);
                await _userMgr.RemoveFromRolesAsync(user, currentRoles);
                await _userMgr.AddToRoleAsync(user, vm.Role);

                TempData["Success"] = $"User '{vm.FullName}' updated successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ── TOGGLE ACTIVE ─────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(string id)
        {
            var user = await _userMgr.FindByIdAsync(id);
            if (user == null) return NotFound();

            // Prevent disabling yourself
            if (user.Email == User.Identity?.Name)
            {
                TempData["Error"] = "You cannot deactivate your own account.";
                return RedirectToAction(nameof(Index));
            }

            user.IsActive = !user.IsActive;
            await _userMgr.UpdateAsync(user);
            TempData["Success"] = $"User '{user.FullName}' is now {(user.IsActive ? "active" : "inactive")}.";
            return RedirectToAction(nameof(Index));
        }

        // ── DELETE ────────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userMgr.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (user.Email == User.Identity?.Name)
            {
                TempData["Error"] = "You cannot delete your own account.";
                return RedirectToAction(nameof(Index));
            }

            await _userMgr.DeleteAsync(user);
            TempData["Success"] = $"User '{user.FullName}' deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
