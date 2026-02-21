using ClosedXML.Excel;
using CustomerServicesSystem.Data;
using CustomerServicesSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CustomerServicesSystem.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ReportsController(ApplicationDbContext db) => _db = db;

        // ── Summary Report ────────────────────────────────────────────
        public async Task<IActionResult> Summary(int? month, int? year, int? staffId, int? deptId)
        {
            var now = DateTime.Now;
            month ??= now.Month;
            year  ??= now.Year;

            var q = _db.CallCenterRecords
                .Include(x => x.CallPurpose).Include(x => x.VisitType)
                .Include(x => x.BookedStatus).Include(x => x.StaffMember)
                .Include(x => x.Department).Include(x => x.Nationality)
                .Include(x => x.OutcomeOfCall).Include(x => x.Doctor)
                .Where(x => x.RecordDate.Month == month && x.RecordDate.Year == year);

            if (staffId.HasValue) q = q.Where(x => x.StaffMemberId == staffId);
            if (deptId.HasValue)  q = q.Where(x => x.DepartmentId  == deptId);

            var records = await q.ToListAsync();

            ViewBag.Month      = month;
            ViewBag.Year       = year;
            ViewBag.StaffId    = staffId;
            ViewBag.DeptId     = deptId;
            ViewBag.StaffList  = new SelectList(await _db.StaffMembers.OrderBy(x => x.Name).ToListAsync(),  "Id", "Name", staffId);
            ViewBag.DeptList   = new SelectList(await _db.Departments.OrderBy(x => x.Name).ToListAsync(),   "Id", "Name", deptId);
            ViewBag.TotalCount = records.Count;
            ViewBag.BookedYes  = records.Count(x => x.BookedStatus?.Name == "Yes");
            ViewBag.BookedNo   = records.Count(x => x.BookedStatus?.Name == "No");

            ViewBag.ByPurpose  = records.GroupBy(x => x.CallPurpose?.Name ?? "Unknown")
                                         .Select(g => new ChartItem { Label = g.Key, Value = g.Count() })
                                         .OrderByDescending(x => x.Value).ToList();

            ViewBag.ByStaff    = records.GroupBy(x => x.StaffMember?.Name ?? "Unknown")
                                         .Select(g => new ChartItem { Label = g.Key, Value = g.Count() })
                                         .OrderByDescending(x => x.Value).ToList();

            ViewBag.ByDept     = records.GroupBy(x => x.Department?.Name ?? "Unknown")
                                         .Select(g => new ChartItem { Label = g.Key, Value = g.Count() })
                                         .OrderByDescending(x => x.Value).ToList();

            ViewBag.ByNation   = records.GroupBy(x => x.Nationality?.Name ?? "Unknown")
                                         .Select(g => new ChartItem { Label = g.Key, Value = g.Count() })
                                         .OrderByDescending(x => x.Value).ToList();

            ViewBag.ByOutcome  = records.GroupBy(x => x.OutcomeOfCall?.Name ?? "Unknown")
                                         .Select(g => new ChartItem { Label = g.Key, Value = g.Count() })
                                         .OrderByDescending(x => x.Value).ToList();

            return View(records);
        }

        // ── Export Report ─────────────────────────────────────────────
        public async Task<IActionResult> ExportSummary(int month, int year, int? staffId, int? deptId)
        {
            var q = _db.CallCenterRecords
                .Include(x => x.CallPurpose).Include(x => x.VisitType)
                .Include(x => x.BookedStatus).Include(x => x.StaffMember)
                .Include(x => x.Department).Include(x => x.Nationality)
                .Include(x => x.OutcomeOfCall).Include(x => x.Doctor).Include(x => x.Source)
                .Where(x => x.RecordDate.Month == month && x.RecordDate.Year == year);

            if (staffId.HasValue) q = q.Where(x => x.StaffMemberId == staffId);
            if (deptId.HasValue)  q = q.Where(x => x.DepartmentId  == deptId);

            var data = await q.OrderBy(x => x.RecordDate).ToListAsync();

            using var wb = new XLWorkbook();

            // ── Sheet 1: Raw Data
            var ws1 = wb.Worksheets.Add("Records");
            var h1  = new[] { "Date","Patient Name","Gender","Nationality","Call Purpose",
                              "Visit Type","Outcome","Doctor","Department","Booked","Staff","Source" };
            for (int c = 0; c < h1.Length; c++)
            {
                var cell = ws1.Cell(1, c+1);
                cell.Value = h1[c];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1a237e");
                cell.Style.Font.FontColor = XLColor.White;
            }
            for (int i = 0; i < data.Count; i++)
            {
                var r = data[i]; var row = i + 2;
                ws1.Cell(row, 1).Value  = r.RecordDate.ToString("yyyy-MM-dd");
                ws1.Cell(row, 2).Value  = r.PatientName      ?? "";
                ws1.Cell(row, 3).Value  = r.Gender           ?? "";
                ws1.Cell(row, 4).Value  = r.Nationality?.Name  ?? "";
                ws1.Cell(row, 5).Value  = r.CallPurpose?.Name  ?? "";
                ws1.Cell(row, 6).Value  = r.VisitType?.Name    ?? "";
                ws1.Cell(row, 7).Value  = r.OutcomeOfCall?.Name ?? "";
                ws1.Cell(row, 8).Value  = r.Doctor?.Name       ?? "";
                ws1.Cell(row, 9).Value  = r.Department?.Name   ?? "";
                ws1.Cell(row, 10).Value = r.BookedStatus?.Name ?? "";
                ws1.Cell(row, 11).Value = r.StaffMember?.Name  ?? "";
                ws1.Cell(row, 12).Value = r.Source?.Name       ?? "";
                if (i % 2 == 0) ws1.Row(row).Style.Fill.BackgroundColor = XLColor.FromHtml("#f5f5f5");
            }
            ws1.Columns().AdjustToContents();

            // ── Sheet 2: Summary by Staff
            var ws2 = wb.Worksheets.Add("By Staff");
            ws2.Cell(1,1).Value = "Staff Name"; ws2.Cell(1,2).Value = "Total";
            ws2.Row(1).Style.Font.Bold = true;
            var byStaff = data.GroupBy(x => x.StaffMember?.Name ?? "Unknown")
                              .Select(g => new { Name = g.Key, Count = g.Count() })
                              .OrderByDescending(x => x.Count).ToList();
            for (int i = 0; i < byStaff.Count; i++)
            {
                ws2.Cell(i+2, 1).Value = byStaff[i].Name;
                ws2.Cell(i+2, 2).Value = byStaff[i].Count;
            }
            ws2.Columns().AdjustToContents();

            // ── Sheet 3: Summary by Department
            var ws3 = wb.Worksheets.Add("By Department");
            ws3.Cell(1,1).Value = "Department"; ws3.Cell(1,2).Value = "Total";
            ws3.Row(1).Style.Font.Bold = true;
            var byDept = data.GroupBy(x => x.Department?.Name ?? "Unknown")
                             .Select(g => new { Name = g.Key, Count = g.Count() })
                             .OrderByDescending(x => x.Count).ToList();
            for (int i = 0; i < byDept.Count; i++)
            {
                ws3.Cell(i+2, 1).Value = byDept[i].Name;
                ws3.Cell(i+2, 2).Value = byDept[i].Count;
            }
            ws3.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return File(ms.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Report_{year}_{month:D2}.xlsx");
        }
    }
}
