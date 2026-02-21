using CustomerServicesSystem.Data;
using CustomerServicesSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerServicesSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var today     = DateTime.Today;
            var monthStart= new DateTime(today.Year, today.Month, 1);
            var week      = today.AddDays(-6);

            var vm = new DashboardVM
            {
                TotalRecords   = await _db.CallCenterRecords.CountAsync(),
                TodayRecords   = await _db.CallCenterRecords.CountAsync(x => x.RecordDate.Date == today),
                BookedCount    = await _db.CallCenterRecords.CountAsync(x => x.BookedStatus != null && x.BookedStatus.Name == "Yes"),
                TotalDoctors   = await _db.Doctors.CountAsync(x => x.IsActive),
                TotalStaff     = await _db.StaffMembers.CountAsync(x => x.IsActive),
                ThisMonthCount = await _db.CallCenterRecords.CountAsync(x => x.RecordDate >= monthStart),

                ByCallPurpose = await _db.CallCenterRecords
                    .Where(x => x.CallPurpose != null)
                    .GroupBy(x => x.CallPurpose!.Name)
                    .Select(g => new ChartItem { Label = g.Key, Value = g.Count() })
                    .OrderByDescending(x => x.Value).Take(6).ToListAsync(),

                ByDepartment = await _db.CallCenterRecords
                    .Where(x => x.Department != null)
                    .GroupBy(x => x.Department!.Name)
                    .Select(g => new ChartItem { Label = g.Key, Value = g.Count() })
                    .OrderByDescending(x => x.Value).Take(6).ToListAsync(),

                ByStaff = await _db.CallCenterRecords
                    .Where(x => x.StaffMember != null)
                    .GroupBy(x => x.StaffMember!.Name)
                    .Select(g => new ChartItem { Label = g.Key, Value = g.Count() })
                    .OrderByDescending(x => x.Value).Take(6).ToListAsync(),

                ByNationality = await _db.CallCenterRecords
                    .Where(x => x.Nationality != null)
                    .GroupBy(x => x.Nationality!.Name)
                    .Select(g => new ChartItem { Label = g.Key, Value = g.Count() })
                    .OrderByDescending(x => x.Value).Take(6).ToListAsync(),

                Last7Days = await _db.CallCenterRecords
                    .Where(x => x.RecordDate >= week)
                    .GroupBy(x => x.RecordDate.Date)
                    .Select(g => new ChartItem { Label = g.Key.ToString("MM/dd"), Value = g.Count() })
                    .OrderBy(x => x.Label).ToListAsync()
            };

            return View(vm);
        }
    }
}
