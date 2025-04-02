using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    using global::Core.DTOs.Personel;
    using System;

    namespace Core.DTOs
    {
        public class SiteDto
        {
            public long SiteID { get; set; }
            public long SiteNumber { get; set; }
            public string SiteName { get; set; } = string.Empty;
            public string? When { get; set; }
            public string? Address1 { get; set; }
            public string? Address2 { get; set; }
            public string? Address3 { get; set; }
            public string? GradeLevels { get; set; }
            public string? Phone { get; set; }
            public string? PhoneType { get; set; }
            public string? TimeAvailable { get; set; }
            public string? RoomNumber { get; set; }
            public string? StartTime { get; set; }
            public string? StopTime { get; set; }
            public DateTime? StartDate { get; set; }
            public decimal? Full { get; set; }
            public decimal? Min { get; set; }
            public decimal? Daily { get; set; }
            public decimal? AMPM { get; set; }
            public string? Permit { get; set; }
            public DateTime? Issued { get; set; }
            public DateTime? Started { get; set; }
            public DateTime? Closed { get; set; }
            public DateTime? Expires { get; set; }
            public string? Notes { get; set; }
            public bool AutoRenew { get; set; }
            public DateTime? OriginalContract { get; set; }
            public string? DistrictFee { get; set; }
            public string? DSSRep { get; set; }
            public string? DSSPhone { get; set; }
            public int? Capacity { get; set; }
            public int? SiteCapacity { get; set; }
            public string? Class { get; set; }
            public string? County { get; set; }
            public string? Principal { get; set; }
            public string? SchoolPhone { get; set; }
            public string? Closings { get; set; }
            public string? Emergency { get; set; }
            public string? PoliceAddress1 { get; set; }
            public string? PoliceAddress2 { get; set; }
            public string? PoliceAddress3 { get; set; }
            public string? PolicePhone { get; set; }
            public string? FireAddress1 { get; set; }
            public string? FireAddress2 { get; set; }
            public string? FireAddress3 { get; set; }
            public string? FirePhone { get; set; }
            public string? Evacuation1Address1 { get; set; }
            public string? Evacuation1Address2 { get; set; }
            public string? Evacuation1Address3 { get; set; }
            public string? Evacuation1Phone { get; set; }
            public decimal? VisitId { get; set; }
            public string? WaitListDate { get; set; }
            public decimal? WaitListCount { get; set; }
            public DateTime? PcDate { get; set; }
            public decimal? PcAmount { get; set; }
            public string? Transport { get; set; }
            public string? TransportPhone { get; set; }
            public string? TelephoneVolume { get; set; }
            public string? DataVolume { get; set; }
            public string? Security { get; set; }
            public string? SecurityPhone { get; set; }
            public string? SafePlace { get; set; }
            public string? Lockdown { get; set; }
            public string? RelocationRoom1 { get; set; }
            public string? RelocationRoom2 { get; set; }
            public string? LandlineLocation { get; set; }
            public string? AdditionalLocation { get; set; }
            public string? AttendanceNote { get; set; }
            public string? SourceSchools { get; set; }
            public string? ResponsiblePerson { get; set; }
            public string? TwoNames { get; set; }
            public DateTime? HCPSent { get; set; }
            public DateTime? HCPApproved { get; set; }
            public decimal? Enrollment { get; set; }
            public string? COExp { get; set; }
            public DateTime? WaiverExp { get; set; }
            public DateTime? NurseVisit { get; set; }
            public bool EpiPen { get; set; }
            public bool Inhaler { get; set; }
            public bool Benadryl { get; set; }
            public DateTime? HCPReviewed { get; set; }
            public DateTime? SecondNurseVisit { get; set; }
            public string? SFax { get; set; }
            public string? AdditionalLocation2 { get; set; }
            public string? AdditionalLocation3 { get; set; }
            public string? AdditionalLocation4 { get; set; }
            public DateTime? ThirdNurseVisit { get; set; }
            public string? ScopeFax { get; set; }
            public int? ASCAP1 { get; set; }
            public int? ASCAP2 { get; set; }
            public int? ASCAP3 { get; set; }
            public int? ASCAP4 { get; set; }
            public string? ParentEmail { get; set; }
            public bool PreKindergarten { get; set; }
            public bool UniversalPreKindergarten { get; set; }
            public string? Evacuation2Address1 { get; set; }
            public string? Evacuation2Address2 { get; set; }
            public string? Evacuation2Address3 { get; set; }
            public string? Evacuation2Phone { get; set; }
            public string? AmbulancePhone { get; set; }
            public string? AdditionalEmergencyInfo { get; set; }
            public string? RoomNumber2 { get; set; }
            public string? RoomNumber3 { get; set; }
            public string? RoomNumber4 { get; set; }
            public int? CAP1 { get; set; }
            public int? CAP2 { get; set; }
            public int? CAP3 { get; set; }
            public int? CAP4 { get; set; }
            public string? OSSPlace { get; set; }
            public string? ScopeEmail { get; set; }
            public string? ScopePassword { get; set; }
            public string? MidPoint { get; set; }
            public string? StaffingAssistantName { get; set; }
            public string? StaffingAssistantPhone { get; set; }
            public string? StaffingAssistantFax { get; set; }
            public string? StaffingAssistantEmail { get; set; }
            public string? StaffingAssistantOthers { get; set; }
            public string? StaffingAssistantExt { get; set; }
            public string? StaffingAssistantForms { get; set; }
            public string? FoodContactName { get; set; }
            public string? FoodContactPhone { get; set; }
            public string? FoodContactFax { get; set; }
            public string? FoodContactEmail { get; set; }
            public string? FoodContactOthers { get; set; }
            public string? FoodContactExt { get; set; }
            public string? FoodContactForms { get; set; }
            public string? SupplyContactName { get; set; }
            public string? SupplyContactPhone { get; set; }
            public string? SupplyContactFax { get; set; }
            public string? SupplyContactEmail { get; set; }
            public string? SupplyContactOthers { get; set; }
            public string? SupplyContactExt { get; set; }
            public string? SupplyContactForms { get; set; }
            public string? PettyCashSpecialEventContactName { get; set; }
            public string? PettyCashSpecialEventContactPhone { get; set; }
            public string? PettyCashSpecialEventContactFax { get; set; }
            public string? PettyCashSpecialEventContactEmail { get; set; }
            public string? PettyCashSpecialEventContactOthers { get; set; }
            public string? PettyCashSpecialEventContactExt { get; set; }
            public string? PettyCashSpecialEventContactForms { get; set; }
            public string? FieldSupervisorName { get; set; }
            public string? FieldSupervisorPhone { get; set; }
            public string? FieldSupervisorFax { get; set; }
            public string? FieldSupervisorEmail { get; set; }
            public string? FieldSupervisorOthers { get; set; }
            public string? FieldSupervisorExt { get; set; }
            public string? FieldSupervisorForms { get; set; }
            public string? FieldTrainerName { get; set; }
            public string? FieldTrainerPhone { get; set; }
            public string? FieldTrainerFax { get; set; }
            public string? FieldTrainerEmail { get; set; }
            public string? FieldTrainerOthers { get; set; }
            public string? FieldTrainerExt { get; set; }
            public string? FieldTrainerForms { get; set; }
            public string? HealthCareConsultantName { get; set; }
            public string? HealthCareConsultantPhone { get; set; }
            public string? HealthCareConsultantFax { get; set; }
            public string? HealthCareConsultantEmail { get; set; }
            public string? HealthCareConsultantOthers { get; set; }
            public string? HealthCareConsultantExt { get; set; }
            public string? HealthCareConsultantForms { get; set; }
            public string? RegistrarName { get; set; }
            public string? RegistrarPhone { get; set; }
            public string? RegistrarFax { get; set; }
            public string? RegistrarEmail { get; set; }
            public string? RegistrarOthers { get; set; }
            public string? RegistrarExt { get; set; }
            public string? RegistrarForms { get; set; }
            public string? AccountBillingName { get; set; }
            public string? AccountBillingPhone { get; set; }
            public string? AccountBillingFax { get; set; }
            public string? AccountBillingEmail { get; set; }
            public string? AccountBillingOthers { get; set; }
            public string? AccountBillingExt { get; set; }
            public string? AccountBillingForms { get; set; }
            public string? SubstitutesName { get; set; }
            public string? SubstitutesPhone { get; set; }
            public string? SubstitutesFax { get; set; }
            public string? SubstitutesEmail { get; set; }
            public string? SubstitutesOthers { get; set; }
            public string? SubstitutesExt { get; set; }
            public string? SubstitutesForms { get; set; }
            public string? SuppliesName { get; set; }
            public string? SuppliesPhone { get; set; }
            public string? SuppliesFax { get; set; }
            public string? SuppliesEmail { get; set; }
            public string? SuppliesOthers { get; set; }
            public string? SuppliesExt { get; set; }
            public string? SuppliesForms { get; set; }
            public string? AccountAssistantOthers { get; set; }
            public long? Type { get; set; }
            public long? Priority { get; set; }
            public string? ScopeDSSName { get; set; }
            public string? ScopeDSSPhone { get; set; }
            public string? ScopeDSSFax { get; set; }
            public string? ScopeDSSEmail { get; set; }
            public string? ScopeDSSOthers { get; set; }
            public string? ScopeDSSExt { get; set; }
            public string? ScopeDSSForms { get; set; }
            public string? PresentersName { get; set; }
            public string? PresentersPhone { get; set; }
            public string? PresentersFax { get; set; }
            public string? PresentersEmail { get; set; }
            public string? PresentersOthers { get; set; }
            public string? PresentersExt { get; set; }
            public string? PresentersForms { get; set; }
            public string? AccountAssistantName { get; set; }
            public string? AccountAssistantPhone { get; set; }
            public string? AccountAssistantFax { get; set; }
            public string? AccountAssistantEmail { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
        }

        public class SiteTimesheetDto 
        {
            public SiteDto Site { get; set; }
            public TimesheetDto? Timesheet { get; set; }
            public PersonelDto? Personel { get; set; }
        }

        public class SiteNamesDto
        {
            public long SiteID { get; set; }
            public string? SiteName { get; set; }
        }
    }

}
