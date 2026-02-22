using ClosedXML.Excel;
using CustomerServicesSystem.Data;
using CustomerServicesSystem.Models;
using CustomerServicesSystem.ViewModels;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CustomerServicesSystem.Controllers
{
    [Authorize]
    public class CallCenterController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CallCenterController(ApplicationDbContext db) => _db = db;

        // ── Helpers ──────────────────────────────────────────────────
        private void LoadDropdowns(CallCenterFormVM? m = null)
        {
            ViewBag.Nationalities = SelectList(_db.Nationalities, m?.NationalityId);
            ViewBag.CallPurposes = SelectList(_db.CallPurposes, m?.CallPurposeId);
            ViewBag.VisitTypes = SelectList(_db.VisitTypes, m?.VisitTypeId);
            ViewBag.OutcomesOfCall = SelectList(_db.OutcomesOfCall, m?.OutcomeOfCallId);
            ViewBag.Departments = SelectList(_db.Departments, m?.DepartmentId);

            // Doctors filtered by the selected department; empty when no department chosen
            var doctorsQ = m?.DepartmentId.HasValue == true
                ? _db.Doctors.Where(x => x.IsActive && x.DepartmentId == m.DepartmentId)
                : _db.Doctors.Where(x => false);
            ViewBag.Doctors = new SelectList(doctorsQ.OrderBy(x => x.Name).ToList(), "Id", "Name", m?.DoctorId);

            ViewBag.BookedStatuses = SelectList(_db.BookedStatuses, m?.BookedStatusId);
            ViewBag.StaffMembers = SelectList(_db.StaffMembers, m?.StaffMemberId);
            ViewBag.Sources = SelectList(_db.Sources, m?.SourceId);
        }

        // ── AJAX: doctors for a given department ─────────────────────
        [HttpGet]
        public JsonResult GetDoctorsByDepartment(int departmentId) =>
            Json(_db.Doctors
                .Where(x => x.DepartmentId == departmentId && x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new { id = x.Id, name = x.Name })
                .ToList());

        private SelectList SelectList<T>(IQueryable<T> q, int? selected = null) where T : LookupBase =>
            new(q.Where(x => x.IsActive).OrderBy(x => x.Name).ToList(), "Id", "Name", selected);

        private IQueryable<CallCenterRecord> BaseQuery() =>
            _db.CallCenterRecords
               .Include(x => x.Nationality).Include(x => x.CallPurpose)
               .Include(x => x.VisitType).Include(x => x.OutcomeOfCall)
               .Include(x => x.Doctor).Include(x => x.Department)
               .Include(x => x.BookedStatus).Include(x => x.StaffMember)
               .Include(x => x.Source);

        // ── INDEX ─────────────────────────────────────────────────────
        public async Task<IActionResult> Index(CallCenterFilterVM filter, int page = 1)
        {
            var q = BaseQuery().AsQueryable();

            if (!string.IsNullOrEmpty(filter.PatientName))
                q = q.Where(x => x.PatientName!.Contains(filter.PatientName));
            if (filter.CallPurposeId.HasValue) q = q.Where(x => x.CallPurposeId == filter.CallPurposeId);
            if (filter.StaffMemberId.HasValue) q = q.Where(x => x.StaffMemberId == filter.StaffMemberId);
            if (filter.BookedStatusId.HasValue) q = q.Where(x => x.BookedStatusId == filter.BookedStatusId);
            if (filter.DepartmentId.HasValue) q = q.Where(x => x.DepartmentId == filter.DepartmentId);
            if (filter.DateFrom.HasValue) q = q.Where(x => x.RecordDate >= filter.DateFrom.Value);
            if (filter.DateTo.HasValue) q = q.Where(x => x.RecordDate <= filter.DateTo.Value);

            const int pageSize = 20;
            var total = await q.CountAsync();
            var records = await q.OrderByDescending(x => x.RecordDate)
                                  .ThenByDescending(x => x.Id)
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            ViewBag.Filter = filter;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);
            ViewBag.TotalRecords = total;

            // Filter dropdowns
            ViewBag.CallPurposes = new SelectList(_db.CallPurposes.OrderBy(x => x.Name).ToList(), "Id", "Name", filter.CallPurposeId);
            ViewBag.StaffMembers = new SelectList(_db.StaffMembers.OrderBy(x => x.Name).ToList(), "Id", "Name", filter.StaffMemberId);
            ViewBag.BookedStatuses = new SelectList(_db.BookedStatuses.OrderBy(x => x.Name).ToList(), "Id", "Name", filter.BookedStatusId);
            ViewBag.Departments = new SelectList(_db.Departments.OrderBy(x => x.Name).ToList(), "Id", "Name", filter.DepartmentId);

            return View(records);
        }

        // ── CREATE ────────────────────────────────────────────────────
        public IActionResult Create()
        {
            LoadDropdowns();
            return View("Form", new CallCenterFormVM());
        }

        // ── EDIT ──────────────────────────────────────────────────────
        public async Task<IActionResult> Edit(int id)
        {
            var r = await _db.CallCenterRecords.FindAsync(id);
            if (r == null) return NotFound();
            var vm = new CallCenterFormVM
            {
                Id = r.Id,
                RecordDate = r.RecordDate,
                PatientName = r.PatientName,
                Gender = r.Gender,
                FileNo = r.FileNo,
                ContactNo = r.ContactNo,
                NationalityId = r.NationalityId,
                CallPurposeId = r.CallPurposeId,
                VisitTypeId = r.VisitTypeId,
                OutcomeOfCallId = r.OutcomeOfCallId,
                DoctorId = r.DoctorId,
                DepartmentId = r.DepartmentId,
                BookedStatusId = r.BookedStatusId,
                StaffMemberId = r.StaffMemberId,
                SourceId = r.SourceId,
                Notes = r.Notes
            };
            LoadDropdowns(vm);
            return View("Form", vm);
        }

        // ── SAVE (Create + Edit) ──────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(CallCenterFormVM vm)
        {
            if (!ModelState.IsValid) { LoadDropdowns(vm); return View("Form", vm); }

            if (vm.Id == 0)
            {
                var r = new CallCenterRecord
                {
                    RecordDate = vm.RecordDate,
                    PatientName = vm.PatientName,
                    Gender = vm.Gender,
                    FileNo = vm.FileNo,
                    ContactNo = vm.ContactNo,
                    NationalityId = vm.NationalityId,
                    CallPurposeId = vm.CallPurposeId,
                    VisitTypeId = vm.VisitTypeId,
                    OutcomeOfCallId = vm.OutcomeOfCallId,
                    DoctorId = vm.DoctorId,
                    DepartmentId = vm.DepartmentId,
                    BookedStatusId = vm.BookedStatusId,
                    StaffMemberId = vm.StaffMemberId,
                    SourceId = vm.SourceId,
                    Notes = vm.Notes,
                    CreatedBy = User.Identity?.Name,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _db.CallCenterRecords.Add(r);
                TempData["Success"] = "Record added successfully.";
            }
            else
            {
                var r = await _db.CallCenterRecords.FindAsync(vm.Id);
                if (r == null) return NotFound();
                r.RecordDate = vm.RecordDate; r.PatientName = vm.PatientName;
                r.Gender = vm.Gender; r.FileNo = vm.FileNo; r.ContactNo = vm.ContactNo;
                r.NationalityId = vm.NationalityId; r.CallPurposeId = vm.CallPurposeId;
                r.VisitTypeId = vm.VisitTypeId; r.OutcomeOfCallId = vm.OutcomeOfCallId;
                r.DoctorId = vm.DoctorId; r.DepartmentId = vm.DepartmentId;
                r.BookedStatusId = vm.BookedStatusId; r.StaffMemberId = vm.StaffMemberId;
                r.SourceId = vm.SourceId; r.Notes = vm.Notes; r.UpdatedAt = DateTime.Now;
                TempData["Success"] = "Record updated successfully.";
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ── DELETE ────────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var r = await _db.CallCenterRecords.FindAsync(id);
            if (r != null) { _db.CallCenterRecords.Remove(r); await _db.SaveChangesAsync(); }
            TempData["Success"] = "Record deleted.";
            return RedirectToAction(nameof(Index));
        }

        // ── EXPORT EXCEL ──────────────────────────────────────────────
        public async Task<IActionResult> ExportExcel(CallCenterFilterVM filter)
        {
            var q = BaseQuery().AsQueryable();
            if (!string.IsNullOrEmpty(filter.PatientName))
                q = q.Where(x => x.PatientName!.Contains(filter.PatientName));
            if (filter.CallPurposeId.HasValue) q = q.Where(x => x.CallPurposeId == filter.CallPurposeId);
            if (filter.StaffMemberId.HasValue) q = q.Where(x => x.StaffMemberId == filter.StaffMemberId);
            if (filter.BookedStatusId.HasValue) q = q.Where(x => x.BookedStatusId == filter.BookedStatusId);
            if (filter.DepartmentId.HasValue) q = q.Where(x => x.DepartmentId == filter.DepartmentId);
            if (filter.DateFrom.HasValue) q = q.Where(x => x.RecordDate >= filter.DateFrom.Value);
            if (filter.DateTo.HasValue) q = q.Where(x => x.RecordDate <= filter.DateTo.Value);

            var data = await q.OrderByDescending(x => x.RecordDate).ToListAsync();

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Call Center Records");

            // Header
            var headers = new[] { "Date","Patient Name","Gender","File No","Contact No",
                "Nationality","Call Purpose","Visit Type","Outcome of Call",
                "Doctor","Department","Booked","Staff Name","Source","Notes" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cell(1, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1a237e");
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            // Data rows
            for (int i = 0; i < data.Count; i++)
            {
                var r = data[i];
                var row = i + 2;
                ws.Cell(row, 1).Value = r.RecordDate.ToString("yyyy-MM-dd");
                ws.Cell(row, 2).Value = r.PatientName ?? "";
                ws.Cell(row, 3).Value = r.Gender ?? "";
                ws.Cell(row, 4).Value = r.FileNo ?? "";
                ws.Cell(row, 5).Value = r.ContactNo ?? "";
                ws.Cell(row, 6).Value = r.Nationality?.Name ?? "";
                ws.Cell(row, 7).Value = r.CallPurpose?.Name ?? "";
                ws.Cell(row, 8).Value = r.VisitType?.Name ?? "";
                ws.Cell(row, 9).Value = r.OutcomeOfCall?.Name ?? "";
                ws.Cell(row, 10).Value = r.Doctor?.Name ?? "";
                ws.Cell(row, 11).Value = r.Department?.Name ?? "";
                ws.Cell(row, 12).Value = r.BookedStatus?.Name ?? "";
                ws.Cell(row, 13).Value = r.StaffMember?.Name ?? "";
                ws.Cell(row, 14).Value = r.Source?.Name ?? "";
                ws.Cell(row, 15).Value = r.Notes ?? "";

                if (i % 2 == 0)
                    ws.Row(row).Style.Fill.BackgroundColor = XLColor.FromHtml("#f5f5f5");
            }

            ws.Columns().AdjustToContents();
            ws.SheetView.FreezeRows(1);

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return File(ms.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"CallCenter_{DateTime.Today:yyyyMMdd}.xlsx");
        }

        // ── IMPORT EXCEL ──────────────────────────────────────────────
        public IActionResult Import() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select an Excel file.";
                return View();
            }

            // Load lookup caches
            var purposes = await _db.CallPurposes.ToDictionaryAsync(x => x.Name.ToLower(), x => x.Id);
            var visitTypes = await _db.VisitTypes.ToDictionaryAsync(x => x.Name.ToLower(), x => x.Id);
            var outcomes = await _db.OutcomesOfCall.ToDictionaryAsync(x => x.Name.ToLower(), x => x.Id);
            var doctors = await _db.Doctors.ToDictionaryAsync(x => x.Name.ToLower(), x => x.Id);
            var departments = await _db.Departments.ToDictionaryAsync(x => x.Name.ToLower(), x => x.Id);
            var booked = await _db.BookedStatuses.ToDictionaryAsync(x => x.Name.ToLower(), x => x.Id);
            var staff = await _db.StaffMembers.ToDictionaryAsync(x => x.Name.ToLower(), x => x.Id);
            var sources = await _db.Sources.ToDictionaryAsync(x => x.Name.ToLower(), x => x.Id);
            var nations = await _db.Nationalities.ToDictionaryAsync(x => x.Name.ToLower(), x => x.Id);

            int imported = 0, skipped = 0;

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;

            using var reader = ExcelReaderFactory.CreateReader(ms);
            var dataset = reader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
            });

            var table = dataset.Tables[0];
            foreach (System.Data.DataRow row in table.Rows)
            {
                try
                {
                    string? Get(string col)
                    {
                        if (!table.Columns.Contains(col)) return null;
                        var v = row[col]?.ToString()?.Trim();
                        return string.IsNullOrEmpty(v) ? null : v;
                    }
                    int? Lookup(Dictionary<string, int> dict, string? val) =>
                        val != null && dict.TryGetValue(val.ToLower(), out int id) ? id : null;

                    var dateStr = Get("Date");
                    if (!DateTime.TryParse(dateStr, out var date)) { skipped++; continue; }

                    var record = new CallCenterRecord
                    {
                        RecordDate = date,
                        PatientName = Get("Patient Name"),
                        Gender = Get("Gender"),
                        FileNo = Get("File No.") ?? Get("File No"),
                        ContactNo = Get("Contact No.") ?? Get("Contact No"),
                        Notes = Get("Note") ?? Get("Notes"),
                        NationalityId = Lookup(nations, Get("Nationality")),
                        CallPurposeId = Lookup(purposes, Get("Call Purpose")),
                        VisitTypeId = Lookup(visitTypes, Get("Visit Type")),
                        OutcomeOfCallId = Lookup(outcomes, Get("Outcome of Call")),
                        DoctorId = Lookup(doctors, Get("Doctor Name")),
                        DepartmentId = Lookup(departments, Get("Department")),
                        BookedStatusId = Lookup(booked, Get("Booked")),
                        StaffMemberId = Lookup(staff, Get("Staff Name")),
                        SourceId = Lookup(sources, Get("Source")),
                        CreatedBy = User.Identity?.Name,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _db.CallCenterRecords.Add(record);
                    imported++;
                }
                catch { skipped++; }
            }

            await _db.SaveChangesAsync();
            TempData["Success"] = $"Import complete: {imported} records imported, {skipped} skipped.";
            return RedirectToAction(nameof(Index));
        }
    }
}