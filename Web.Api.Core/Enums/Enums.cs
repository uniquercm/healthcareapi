namespace Web.Api.Core.Enums
{
    public enum Status
    {
        Active = 1,
        Inactive = 2
    }
    public enum StaffType
    {
        Admin = 2,
        Doctor = 3,
        Nurse = 4,
        Receptionist = 5
    }
    public enum UserType
    {
        SuperAdmin = 1,
        Admin = 2,
        Doctor = 3,
        Nurse = 4,
        Receptionist = 5
    }
    public enum VisitStatus
    {
        Visited = 1,
        Pending = 2
    }
    public enum CallStatus
    {
        Called = 1,
        Pending = 2
    }
    public enum TestResult
    {
        Negative = 1,
        Positive = 2
    }
    public enum TreatmentType
    {
        Quarantine = 1,
        Isolation = 2
    }
}