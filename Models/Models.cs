using Microsoft.AspNetCore.Identity;

namespace CustomerServicesSystem.Models
{
    // ─────────────────────────────────────────
    //  Identity
    // ─────────────────────────────────────────
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public bool   IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    // ─────────────────────────────────────────
    //  Lookup base
    // ─────────────────────────────────────────
    public class LookupBase
    {
        public int    Id        { get; set; }
        public string Name      { get; set; } = string.Empty;
        public bool   IsActive  { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    // ─────────────────────────────────────────
    //  Lookup tables
    // ─────────────────────────────────────────
    public class CallPurpose   : LookupBase { public ICollection<CallCenterRecord> Records { get; set; } = new List<CallCenterRecord>(); }
    public class VisitType     : LookupBase { public ICollection<CallCenterRecord> Records { get; set; } = new List<CallCenterRecord>(); }
    public class OutcomeOfCall : LookupBase { public ICollection<CallCenterRecord> Records { get; set; } = new List<CallCenterRecord>(); }
    public class Doctor        : LookupBase { public ICollection<CallCenterRecord> Records { get; set; } = new List<CallCenterRecord>(); }
    public class Department    : LookupBase { public ICollection<CallCenterRecord> Records { get; set; } = new List<CallCenterRecord>(); }
    public class BookedStatus  : LookupBase { public ICollection<CallCenterRecord> Records { get; set; } = new List<CallCenterRecord>(); }
    public class StaffMember   : LookupBase { public ICollection<CallCenterRecord> Records { get; set; } = new List<CallCenterRecord>(); }
    public class Source        : LookupBase { public ICollection<CallCenterRecord> Records { get; set; } = new List<CallCenterRecord>(); }
    public class Nationality   : LookupBase { public ICollection<CallCenterRecord> Records { get; set; } = new List<CallCenterRecord>(); }

    // ─────────────────────────────────────────
    //  Main record
    // ─────────────────────────────────────────
    public class CallCenterRecord
    {
        public int      Id            { get; set; }
        public DateTime RecordDate    { get; set; } = DateTime.Today;
        public string?  PatientName   { get; set; }
        public string?  Gender        { get; set; }
        public string?  FileNo        { get; set; }
        public string?  ContactNo     { get; set; }

        public int?  NationalityId    { get; set; }  public Nationality?   Nationality   { get; set; }
        public int?  CallPurposeId    { get; set; }  public CallPurpose?   CallPurpose   { get; set; }
        public int?  VisitTypeId      { get; set; }  public VisitType?     VisitType     { get; set; }
        public int?  OutcomeOfCallId  { get; set; }  public OutcomeOfCall? OutcomeOfCall { get; set; }
        public int?  DoctorId         { get; set; }  public Doctor?        Doctor        { get; set; }
        public int?  DepartmentId     { get; set; }  public Department?    Department    { get; set; }
        public int?  BookedStatusId   { get; set; }  public BookedStatus?  BookedStatus  { get; set; }
        public int?  StaffMemberId    { get; set; }  public StaffMember?   StaffMember   { get; set; }
        public int?  SourceId         { get; set; }  public Source?        Source        { get; set; }

        public string?   Notes      { get; set; }
        public string?   CreatedBy  { get; set; }
        public DateTime  CreatedAt  { get; set; } = DateTime.Now;
        public DateTime  UpdatedAt  { get; set; } = DateTime.Now;
    }
}
