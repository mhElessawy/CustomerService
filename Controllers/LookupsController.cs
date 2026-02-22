using CustomerServicesSystem.Data;
using CustomerServicesSystem.Models;
using CustomerServicesSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerServicesSystem.Controllers
{
    [Authorize]
    public class LookupsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LookupsController(ApplicationDbContext db) => _db = db;

        // ── Meta dictionary ───────────────────────────────────────────
        private static readonly Dictionary<string, (string Title, string Icon)> Meta = new()
        {
            ["CallPurposes"] = ("Call Purpose", "fas fa-bullseye"),
            ["VisitTypes"] = ("Visit Type", "fas fa-hospital-user"),
            ["OutcomesOfCall"] = ("Outcome of Call", "fas fa-check-circle"),
            ["Doctors"] = ("Doctor Name", "fas fa-user-md"),
            ["Departments"] = ("Department", "fas fa-building"),
            ["BookedStatuses"] = ("Booked Status", "fas fa-calendar-check"),
            ["StaffMembers"] = ("Staff Name", "fas fa-users"),
            ["Sources"] = ("Source", "fas fa-share-alt"),
            ["Nationalities"] = ("Nationality", "fas fa-flag"),
        };

        private bool Valid(string t) => Meta.ContainsKey(t);

        // ── Data helpers ──────────────────────────────────────────────
        private async Task<List<LookupItemVM>> GetAll(string t) => t switch
        {
            "CallPurposes" => await Map(_db.CallPurposes),
            "VisitTypes" => await Map(_db.VisitTypes),
            "OutcomesOfCall" => await Map(_db.OutcomesOfCall),
            "Doctors" => await Map(_db.Doctors),
            "Departments" => await Map(_db.Departments),
            "BookedStatuses" => await Map(_db.BookedStatuses),
            "StaffMembers" => await Map(_db.StaffMembers),
            "Sources" => await Map(_db.Sources),
            "Nationalities" => await Map(_db.Nationalities),
            _ => new()
        };

        private static async Task<List<LookupItemVM>> Map<T>(IQueryable<T> q) where T : LookupBase =>
            await q.OrderBy(x => x.Name)
                   .Select(x => new LookupItemVM { Id = x.Id, Name = x.Name, IsActive = x.IsActive, CreatedAt = x.CreatedAt })
                   .ToListAsync();

        private async Task<LookupItemVM?> GetById(string t, int id) => t switch
        {
            "CallPurposes" => await MapOne(_db.CallPurposes, id),
            "VisitTypes" => await MapOne(_db.VisitTypes, id),
            "OutcomesOfCall" => await MapOne(_db.OutcomesOfCall, id),
            "Doctors" => await _db.Doctors.Where(x => x.Id == id)
                             .Select(x => new LookupItemVM { Id = x.Id, Name = x.Name, IsActive = x.IsActive, DepartmentId = x.DepartmentId })
                             .FirstOrDefaultAsync(),
            "Departments" => await MapOne(_db.Departments, id),
            "BookedStatuses" => await MapOne(_db.BookedStatuses, id),
            "StaffMembers" => await MapOne(_db.StaffMembers, id),
            "Sources" => await MapOne(_db.Sources, id),
            "Nationalities" => await MapOne(_db.Nationalities, id),
            _ => null
        };

        private static async Task<LookupItemVM?> MapOne<T>(IQueryable<T> q, int id) where T : LookupBase =>
            await q.Where(x => x.Id == id)
                   .Select(x => new LookupItemVM { Id = x.Id, Name = x.Name, IsActive = x.IsActive })
                   .FirstOrDefaultAsync();

        private async Task Add(string t, string name, bool active, int? departmentId = null)
        {
            switch (t)
            {
                case "CallPurposes": _db.CallPurposes.Add(new CallPurpose { Name = name, IsActive = active }); break;
                case "VisitTypes": _db.VisitTypes.Add(new VisitType { Name = name, IsActive = active }); break;
                case "OutcomesOfCall": _db.OutcomesOfCall.Add(new OutcomeOfCall { Name = name, IsActive = active }); break;
                case "Doctors": _db.Doctors.Add(new Doctor { Name = name, IsActive = active, DepartmentId = departmentId }); break;
                case "Departments": _db.Departments.Add(new Department { Name = name, IsActive = active }); break;
                case "BookedStatuses": _db.BookedStatuses.Add(new BookedStatus { Name = name, IsActive = active }); break;
                case "StaffMembers": _db.StaffMembers.Add(new StaffMember { Name = name, IsActive = active }); break;
                case "Sources": _db.Sources.Add(new Source { Name = name, IsActive = active }); break;
                case "Nationalities": _db.Nationalities.Add(new Nationality { Name = name, IsActive = active }); break;
            }
            await _db.SaveChangesAsync();
        }

        private async Task Update(string t, int id, string name, bool active, int? departmentId = null)
        {
            LookupBase? entity = t switch
            {
                "CallPurposes" => await _db.CallPurposes.FindAsync(id),
                "VisitTypes" => await _db.VisitTypes.FindAsync(id),
                "OutcomesOfCall" => await _db.OutcomesOfCall.FindAsync(id),
                "Doctors" => await _db.Doctors.FindAsync(id),
                "Departments" => await _db.Departments.FindAsync(id),
                "BookedStatuses" => await _db.BookedStatuses.FindAsync(id),
                "StaffMembers" => await _db.StaffMembers.FindAsync(id),
                "Sources" => await _db.Sources.FindAsync(id),
                "Nationalities" => await _db.Nationalities.FindAsync(id),
                _ => null
            };
            if (entity == null) return;
            entity.Name = name;
            entity.IsActive = active;
            if (entity is Doctor doc) doc.DepartmentId = departmentId;
            await _db.SaveChangesAsync();
        }

        private async Task DeleteItem(string t, int id)
        {
            LookupBase? entity = t switch
            {
                "CallPurposes" => await _db.CallPurposes.FindAsync(id),
                "VisitTypes" => await _db.VisitTypes.FindAsync(id),
                "OutcomesOfCall" => await _db.OutcomesOfCall.FindAsync(id),
                "Doctors" => await _db.Doctors.FindAsync(id),
                "Departments" => await _db.Departments.FindAsync(id),
                "BookedStatuses" => await _db.BookedStatuses.FindAsync(id),
                "StaffMembers" => await _db.StaffMembers.FindAsync(id),
                "Sources" => await _db.Sources.FindAsync(id),
                "Nationalities" => await _db.Nationalities.FindAsync(id),
                _ => null
            };
            if (entity == null) return;
            _db.Remove(entity);
            await _db.SaveChangesAsync();
        }

        private LookupFormVM BuildForm(string t, LookupItemVM? item = null) =>
            new()
            {
                Id = item?.Id ?? 0,
                Name = item?.Name ?? string.Empty,
                IsActive = item?.IsActive ?? true,
                LookupType = t,
                LookupTitle = Meta[t].Title,
                LookupIcon = Meta[t].Icon,
                DepartmentId = item?.DepartmentId,
            };

        // ── Actions ───────────────────────────────────────────────────
        public async Task<IActionResult> Index(string type)
        {
            if (!Valid(type)) return NotFound();
            return View(new LookupIndexVM
            {
                LookupType = type,
                LookupTitle = Meta[type].Title,
                LookupIcon = Meta[type].Icon,
                Items = await GetAll(type)
            });
        }

        public async Task<IActionResult> Form(string type, int? id = null)
        {
            if (!Valid(type)) return NotFound();
            LookupItemVM? item = id.HasValue ? await GetById(type, id.Value) : null;
            if (id.HasValue && item == null) return NotFound();
            if (type == "Doctors")
                ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                    _db.Departments.Where(x => x.IsActive).OrderBy(x => x.Name).ToList(), "Id", "Name");
            return View(BuildForm(type, item));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Form(LookupFormVM vm)
        {
            if (!Valid(vm.LookupType)) return NotFound();
            if (!ModelState.IsValid)
            {
                vm.LookupTitle = Meta[vm.LookupType].Title;
                vm.LookupIcon = Meta[vm.LookupType].Icon;
                if (vm.LookupType == "Doctors")
                    ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                        _db.Departments.Where(x => x.IsActive).OrderBy(x => x.Name).ToList(), "Id", "Name");
                return View(vm);
            }
            if (vm.Id == 0) await Add(vm.LookupType, vm.Name.Trim(), vm.IsActive, vm.DepartmentId);
            else await Update(vm.LookupType, vm.Id, vm.Name.Trim(), vm.IsActive, vm.DepartmentId);

            TempData["Success"] = vm.Id == 0
                ? $"'{vm.Name}' added successfully."
                : $"'{vm.Name}' updated successfully.";
            return RedirectToAction(nameof(Index), new { type = vm.LookupType });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string type, int id)
        {
            if (!Valid(type)) return NotFound();
            try
            {
                await DeleteItem(type, id);
                TempData["Success"] = "Deleted successfully.";
            }
            catch
            {
                TempData["Error"] = "Cannot delete — this item is used in existing records.";
            }
            return RedirectToAction(nameof(Index), new { type });
        }
    }
}