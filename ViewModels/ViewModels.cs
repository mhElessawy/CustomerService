using System.ComponentModel.DataAnnotations;

namespace CustomerServicesSystem.ViewModels
{
    // ─── Login ────────────────────────────────
    public class LoginVM
    {
        [Required] [EmailAddress] public string Email    { get; set; } = string.Empty;
        [Required] [DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }

    // ─── Lookup shared ────────────────────────
    public class LookupItemVM
    {
        public int    Id           { get; set; }
        public string Name         { get; set; } = string.Empty;
        public bool   IsActive     { get; set; } = true;
        public DateTime CreatedAt  { get; set; }
        public int?   DepartmentId { get; set; }  // Doctors only
    }

    public class LookupIndexVM
    {
        public string          LookupType  { get; set; } = string.Empty;
        public string          LookupTitle { get; set; } = string.Empty;
        public string          LookupIcon  { get; set; } = string.Empty;
        public List<LookupItemVM> Items    { get; set; } = new();
    }

    public class LookupFormVM
    {
        public int    Id           { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name         { get; set; } = string.Empty;
        public bool   IsActive     { get; set; } = true;
        public string LookupType   { get; set; } = string.Empty;
        public string LookupTitle  { get; set; } = string.Empty;
        public string LookupIcon   { get; set; } = string.Empty;
        public int?   DepartmentId { get; set; }  // Doctors only
    }

    // ─── Call Center Record ───────────────────
    public class CallCenterFormVM
    {
        public int       Id            { get; set; }
        [Required] public DateTime RecordDate { get; set; } = DateTime.Today;
        public string?   PatientName   { get; set; }
        public string?   Gender        { get; set; }
        public string?   FileNo        { get; set; }
        public string?   ContactNo     { get; set; }
        public int?      NationalityId { get; set; }
        public int?      CallPurposeId { get; set; }
        public int?      VisitTypeId   { get; set; }
        public int?      OutcomeOfCallId { get; set; }
        public int?      DoctorId      { get; set; }
        public int?      DepartmentId  { get; set; }
        public int?      BookedStatusId{ get; set; }
        public int?      StaffMemberId { get; set; }
        public int?      SourceId      { get; set; }
        public string?   Notes         { get; set; }
    }

    public class CallCenterFilterVM
    {
        public string?   PatientName   { get; set; }
        public int?      CallPurposeId { get; set; }
        public int?      StaffMemberId { get; set; }
        public int?      BookedStatusId{ get; set; }
        public int?      DepartmentId  { get; set; }
        public DateTime? DateFrom      { get; set; }
        public DateTime? DateTo        { get; set; }
    }

    // ─── Dashboard ────────────────────────────
    public class DashboardVM
    {
        public int TotalRecords   { get; set; }
        public int TodayRecords   { get; set; }
        public int BookedCount    { get; set; }
        public int TotalDoctors   { get; set; }
        public int TotalStaff     { get; set; }
        public int ThisMonthCount { get; set; }

        // Charts
        public List<ChartItem> ByCallPurpose  { get; set; } = new();
        public List<ChartItem> ByDepartment   { get; set; } = new();
        public List<ChartItem> ByStaff        { get; set; } = new();
        public List<ChartItem> ByNationality  { get; set; } = new();
        public List<ChartItem> Last7Days      { get; set; } = new();
    }

    public class ChartItem
    {
        public string Label { get; set; } = string.Empty;
        public int    Value { get; set; }
    }
}
