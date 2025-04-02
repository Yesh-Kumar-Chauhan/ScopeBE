using AutoMapper;
using Core.DTOs;
using Core.DTOs.Core.DTOs;
using Core.DTOs.Inservice;
using Core.DTOs.Personel;
using Core.DTOs.Report;
using Core.DTOs.Sites;
using Core.DTOs.Workshop;
using Core.Entities;

namespace Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<District, DistrictDto>()
                .ForMember(dest => dest.DistNum, opt => opt.MapFrom(src => src.DIST_NUM))
                .ForMember(dest => dest.DistNam, opt => opt.MapFrom(src => src.DIST_NAM))
                .ReverseMap();

            CreateMap<Site, SiteNamesDto>()
               .ForMember(dest => dest.SiteName, opt => opt.MapFrom(src => src.SITE_NAM))
               .ReverseMap();

            CreateMap<Site, SiteDto>()
               .ForMember(dest => dest.SiteNumber, opt => opt.MapFrom(src => src.SITE_NUM))
               .ForMember(dest => dest.SiteName, opt => opt.MapFrom(src => src.SITE_NAM))
               .ForMember(dest => dest.When, opt => opt.MapFrom(src => src.WHEN))
               .ForMember(dest => dest.Address1, opt => opt.MapFrom(src => src.ADDR1))
               .ForMember(dest => dest.Address2, opt => opt.MapFrom(src => src.ADDR2))
               .ForMember(dest => dest.Address3, opt => opt.MapFrom(src => src.ADDR3))
               .ForMember(dest => dest.GradeLevels, opt => opt.MapFrom(src => src.GRADE_LVLS))
               .ForMember(dest => dest.PhoneType, opt => opt.MapFrom(src => src.PHONE_TYPE))
               .ForMember(dest => dest.TimeAvailable, opt => opt.MapFrom(src => src.TIME_AVAIL))
               .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.ROOM_NO))
               .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.START_TIME))
               .ForMember(dest => dest.StopTime, opt => opt.MapFrom(src => src.STOP_TIME))
               .ForMember(dest => dest.OriginalContract, opt => opt.MapFrom(src => src.ORIGCONTRC))
               .ForMember(dest => dest.DistrictFee, opt => opt.MapFrom(src => src.DIST_FEE))
               .ForMember(dest => dest.DSSRep, opt => opt.MapFrom(src => src.DSS_REP))
               .ForMember(dest => dest.DSSPhone, opt => opt.MapFrom(src => src.DSS_FON))
               .ForMember(dest => dest.SiteCapacity, opt => opt.MapFrom(src => src.SITECAP))
               .ForMember(dest => dest.SchoolPhone, opt => opt.MapFrom(src => src.SCHFONE))
               .ForMember(dest => dest.Closings, opt => opt.MapFrom(src => src.CLOSINGS))
               .ForMember(dest => dest.Emergency, opt => opt.MapFrom(src => src.EMERGENCY))
               .ForMember(dest => dest.PoliceAddress1, opt => opt.MapFrom(src => src.PADR1))
               .ForMember(dest => dest.PoliceAddress2, opt => opt.MapFrom(src => src.PADR2))
               .ForMember(dest => dest.PoliceAddress3, opt => opt.MapFrom(src => src.PADR3))
               .ForMember(dest => dest.PolicePhone, opt => opt.MapFrom(src => src.PPHONE))
               .ForMember(dest => dest.FireAddress1, opt => opt.MapFrom(src => src.FADR1))
               .ForMember(dest => dest.FireAddress2, opt => opt.MapFrom(src => src.FADR2))
               .ForMember(dest => dest.FireAddress3, opt => opt.MapFrom(src => src.FADR3))
               .ForMember(dest => dest.FirePhone, opt => opt.MapFrom(src => src.FPHONE))
               .ForMember(dest => dest.Evacuation1Address1, opt => opt.MapFrom(src => src.EADR1))
               .ForMember(dest => dest.Evacuation1Address2, opt => opt.MapFrom(src => src.EADR2))
               .ForMember(dest => dest.Evacuation1Address3, opt => opt.MapFrom(src => src.EADR3))
               .ForMember(dest => dest.Evacuation1Phone, opt => opt.MapFrom(src => src.EPHONE))
               .ForMember(dest => dest.VisitId, opt => opt.MapFrom(src => src.VISIT_ID))
               .ForMember(dest => dest.WaitListDate, opt => opt.MapFrom(src => src.WL_DATE))
               .ForMember(dest => dest.WaitListCount, opt => opt.MapFrom(src => src.WL_COUNT))
               .ForMember(dest => dest.PcDate, opt => opt.MapFrom(src => src.PC_DATE))
               .ForMember(dest => dest.PcAmount, opt => opt.MapFrom(src => src.PC_AMT))
               .ForMember(dest => dest.Transport, opt => opt.MapFrom(src => src.TRANSPORT))
               .ForMember(dest => dest.TransportPhone, opt => opt.MapFrom(src => src.TPPHONE))
               .ForMember(dest => dest.TelephoneVolume, opt => opt.MapFrom(src => src.TEL_VOL))
               .ForMember(dest => dest.DataVolume, opt => opt.MapFrom(src => src.DT_VOL))
               .ForMember(dest => dest.Security, opt => opt.MapFrom(src => src.SECURITY))
               .ForMember(dest => dest.SecurityPhone, opt => opt.MapFrom(src => src.SECPHONE))
               .ForMember(dest => dest.SafePlace, opt => opt.MapFrom(src => src.SAFEPLACE))
               .ForMember(dest => dest.Lockdown, opt => opt.MapFrom(src => src.LOCKDOWN))
               .ForMember(dest => dest.RelocationRoom1, opt => opt.MapFrom(src => src.RELOCROOM1))
               .ForMember(dest => dest.RelocationRoom2, opt => opt.MapFrom(src => src.RELOCROOM2))
               .ForMember(dest => dest.LandlineLocation, opt => opt.MapFrom(src => src.LNDLNLOC))
               .ForMember(dest => dest.AdditionalLocation, opt => opt.MapFrom(src => src.ADDLOC))
               .ForMember(dest => dest.AttendanceNote, opt => opt.MapFrom(src => src.ATTNOTE))
               .ForMember(dest => dest.SourceSchools, opt => opt.MapFrom(src => src.SRCSCHLS))
               .ForMember(dest => dest.ResponsiblePerson, opt => opt.MapFrom(src => src.RSPNSBL))
               .ForMember(dest => dest.TwoNames, opt => opt.MapFrom(src => src.TWONAMES))
               .ForMember(dest => dest.HCPSent, opt => opt.MapFrom(src => src.HCPSENT))
               .ForMember(dest => dest.HCPApproved, opt => opt.MapFrom(src => src.HCPAPPR))
               .ForMember(dest => dest.COExp, opt => opt.MapFrom(src => src.COEXP))
               .ForMember(dest => dest.WaiverExp, opt => opt.MapFrom(src => src.WAIVEREXP))
               .ForMember(dest => dest.HCPReviewed, opt => opt.MapFrom(src => src.HCPREV))
               .ForMember(dest => dest.SecondNurseVisit, opt => opt.MapFrom(src => src.NURSEVISI2))
               .ForMember(dest => dest.SFax, opt => opt.MapFrom(src => src.SFAX))
               .ForMember(dest => dest.AdditionalLocation2, opt => opt.MapFrom(src => src.ADDLOC2))
               .ForMember(dest => dest.AdditionalLocation3, opt => opt.MapFrom(src => src.ADDLOC3))
               .ForMember(dest => dest.AdditionalLocation4, opt => opt.MapFrom(src => src.ADDLOC4))
               .ForMember(dest => dest.ThirdNurseVisit, opt => opt.MapFrom(src => src.NURSEVISI3))
               .ForMember(dest => dest.ScopeFax, opt => opt.MapFrom(src => src.SCOPEFAX))
               .ForMember(dest => dest.Evacuation2Address1, opt => opt.MapFrom(src => src.EADR21))
               .ForMember(dest => dest.Evacuation2Address2, opt => opt.MapFrom(src => src.EADR22))
               .ForMember(dest => dest.Evacuation2Address3, opt => opt.MapFrom(src => src.EADR23))
               .ForMember(dest => dest.Evacuation2Phone, opt => opt.MapFrom(src => src.EPHONE2))
               .ForMember(dest => dest.AmbulancePhone, opt => opt.MapFrom(src => src.AMBPHONE))
               .ForMember(dest => dest.AdditionalEmergencyInfo, opt => opt.MapFrom(src => src.ADDEMGINFO))
               .ForMember(dest => dest.RoomNumber2, opt => opt.MapFrom(src => src.ROOM_NO2))
               .ForMember(dest => dest.RoomNumber3, opt => opt.MapFrom(src => src.ROOM_NO3))
               .ForMember(dest => dest.RoomNumber4, opt => opt.MapFrom(src => src.ROOM_NO4))
               .ForMember(dest => dest.OSSPlace, opt => opt.MapFrom(src => src.OSSPLACE))
               .ForMember(dest => dest.ScopeEmail, opt => opt.MapFrom(src => src.ScopeEmail))
               .ForMember(dest => dest.ScopePassword, opt => opt.MapFrom(src => src.ScopePassword))
               .ForMember(dest => dest.MidPoint, opt => opt.MapFrom(src => src.MidPoint))
               .ForMember(dest => dest.StaffingAssistantName, opt => opt.MapFrom(src => src.StaffingAssistantName))
               .ForMember(dest => dest.StaffingAssistantPhone, opt => opt.MapFrom(src => src.StaffingAssistantPhone))
               .ForMember(dest => dest.StaffingAssistantFax, opt => opt.MapFrom(src => src.StaffingAssistantFax))
               .ForMember(dest => dest.StaffingAssistantEmail, opt => opt.MapFrom(src => src.StaffingAssistantEmail))
               .ForMember(dest => dest.StaffingAssistantOthers, opt => opt.MapFrom(src => src.StaffingAssistantOthers))
               .ForMember(dest => dest.StaffingAssistantExt, opt => opt.MapFrom(src => src.StaffingAssistantExt))
               .ForMember(dest => dest.StaffingAssistantForms, opt => opt.MapFrom(src => src.StaffingAssistantForms))
               .ForMember(dest => dest.FoodContactName, opt => opt.MapFrom(src => src.FoodContactName))
               .ForMember(dest => dest.FoodContactPhone, opt => opt.MapFrom(src => src.FoodContactPhone))
               .ForMember(dest => dest.FoodContactFax, opt => opt.MapFrom(src => src.FoodContactFax))
               .ForMember(dest => dest.FoodContactEmail, opt => opt.MapFrom(src => src.FoodContactEmail))
               .ForMember(dest => dest.FoodContactOthers, opt => opt.MapFrom(src => src.FoodContactOthers))
               .ForMember(dest => dest.FoodContactExt, opt => opt.MapFrom(src => src.FoodContactExt))
               .ForMember(dest => dest.FoodContactForms, opt => opt.MapFrom(src => src.FoodContactForms))
               .ForMember(dest => dest.SupplyContactName, opt => opt.MapFrom(src => src.SupplyContactName))
               .ForMember(dest => dest.SupplyContactPhone, opt => opt.MapFrom(src => src.SupplyContactPhone))
               .ForMember(dest => dest.SupplyContactFax, opt => opt.MapFrom(src => src.SupplyContactFax))
               .ForMember(dest => dest.SupplyContactEmail, opt => opt.MapFrom(src => src.SupplyContactEmail))
               .ForMember(dest => dest.SupplyContactOthers, opt => opt.MapFrom(src => src.SupplyContactOthers))
               .ForMember(dest => dest.SupplyContactExt, opt => opt.MapFrom(src => src.SupplyContactExt))
               .ForMember(dest => dest.SupplyContactForms, opt => opt.MapFrom(src => src.SupplyContactForms))
               .ForMember(dest => dest.PettyCashSpecialEventContactName, opt => opt.MapFrom(src => src.PettyCashSpecialEventContactName))
               .ForMember(dest => dest.PettyCashSpecialEventContactPhone, opt => opt.MapFrom(src => src.PettyCashSpecialEventContactPhone))
               .ForMember(dest => dest.PettyCashSpecialEventContactFax, opt => opt.MapFrom(src => src.PettyCashSpecialEventContactFax))
               .ForMember(dest => dest.PettyCashSpecialEventContactEmail, opt => opt.MapFrom(src => src.PettyCashSpecialEventContactEmail))
               .ForMember(dest => dest.PettyCashSpecialEventContactOthers, opt => opt.MapFrom(src => src.PettyCashSpecialEventContactOthers))
               .ForMember(dest => dest.PettyCashSpecialEventContactExt, opt => opt.MapFrom(src => src.PettyCashSpecialEventContactExt))
               .ForMember(dest => dest.PettyCashSpecialEventContactForms, opt => opt.MapFrom(src => src.PettyCashSpecialEventContactForms))
               .ForMember(dest => dest.FieldSupervisorName, opt => opt.MapFrom(src => src.FieldSupervisorName))
               .ForMember(dest => dest.FieldSupervisorPhone, opt => opt.MapFrom(src => src.FieldSupervisorPhone))
               .ForMember(dest => dest.FieldSupervisorFax, opt => opt.MapFrom(src => src.FieldSupervisorFax))
               .ForMember(dest => dest.FieldSupervisorEmail, opt => opt.MapFrom(src => src.FieldSupervisorEmail))
               .ForMember(dest => dest.FieldSupervisorOthers, opt => opt.MapFrom(src => src.FieldSupervisorOthers))
               .ForMember(dest => dest.FieldSupervisorExt, opt => opt.MapFrom(src => src.FieldSupervisorExt))
               .ForMember(dest => dest.FieldSupervisorForms, opt => opt.MapFrom(src => src.FieldSupervisorForms))
               .ForMember(dest => dest.FieldTrainerName, opt => opt.MapFrom(src => src.FieldTrainerName))
               .ForMember(dest => dest.FieldTrainerPhone, opt => opt.MapFrom(src => src.FieldTrainerPhone))
               .ForMember(dest => dest.FieldTrainerFax, opt => opt.MapFrom(src => src.FieldTrainerFax))
               .ForMember(dest => dest.FieldTrainerEmail, opt => opt.MapFrom(src => src.FieldTrainerEmail))
               .ForMember(dest => dest.FieldTrainerOthers, opt => opt.MapFrom(src => src.FieldTrainerOthers))
               .ForMember(dest => dest.FieldTrainerExt, opt => opt.MapFrom(src => src.FieldTrainerExt))
               .ForMember(dest => dest.FieldTrainerForms, opt => opt.MapFrom(src => src.FieldTrainerForms))
               .ForMember(dest => dest.HealthCareConsultantName, opt => opt.MapFrom(src => src.HealthCareConsultantName))
               .ForMember(dest => dest.HealthCareConsultantPhone, opt => opt.MapFrom(src => src.HealthCareConsultantPhone))
               .ForMember(dest => dest.HealthCareConsultantFax, opt => opt.MapFrom(src => src.HealthCareConsultantFax))
               .ForMember(dest => dest.HealthCareConsultantEmail, opt => opt.MapFrom(src => src.HealthCareConsultantEmail))
               .ForMember(dest => dest.HealthCareConsultantOthers, opt => opt.MapFrom(src => src.HealthCareConsultantOthers))
               .ForMember(dest => dest.HealthCareConsultantExt, opt => opt.MapFrom(src => src.HealthCareConsultantExt))
               .ForMember(dest => dest.HealthCareConsultantForms, opt => opt.MapFrom(src => src.HealthCareConsultantForms))
               .ForMember(dest => dest.RegistrarName, opt => opt.MapFrom(src => src.RegistarName))
               .ForMember(dest => dest.RegistrarPhone, opt => opt.MapFrom(src => src.RegistarPhone))
               .ForMember(dest => dest.RegistrarFax, opt => opt.MapFrom(src => src.RegistarFax))
               .ForMember(dest => dest.RegistrarEmail, opt => opt.MapFrom(src => src.RegistarEmail))
               .ForMember(dest => dest.RegistrarOthers, opt => opt.MapFrom(src => src.RegistarOthers))
               .ForMember(dest => dest.RegistrarExt, opt => opt.MapFrom(src => src.RegistarExt))
               .ForMember(dest => dest.RegistrarForms, opt => opt.MapFrom(src => src.RegistarForms))

               .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.START_DATE))
               .ForMember(dest => dest.ParentEmail, opt => opt.MapFrom(src => src.PEMAIL))
               .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.CAPACTIY))
               .ForMember(dest => dest.PreKindergarten, opt => opt.MapFrom(src => src.PK))
               .ForMember(dest => dest.UniversalPreKindergarten, opt => opt.MapFrom(src => src.UPK))
               .ReverseMap();

            CreateMap<ExtendedSiteDto, SiteDto>().ReverseMap();
            CreateMap<Personel, PersonelDto>().ReverseMap();
            CreateMap<Report, ReportDto>().ReverseMap();

            CreateMap<School, SchoolDto>()
                .ForMember(dest => dest.SchNum, opt => opt.MapFrom(src => src.SCH_NUM))
                .ForMember(dest => dest.SchNam, opt => opt.MapFrom(src => src.SCH_NAM))
                .ForMember(dest => dest.Principal, opt => opt.MapFrom(src => src.PRINCIPAL))
                .ForMember(dest => dest.Addr1, opt => opt.MapFrom(src => src.ADDR1))
                .ForMember(dest => dest.Addr2, opt => opt.MapFrom(src => src.ADDR2))
                .ForMember(dest => dest.DistNum, opt => opt.MapFrom(src => src.DIST_NUM))
                .ForMember(dest => dest.DistNam, opt => opt.MapFrom(src => src.DIST_NAM))
                .ForMember(dest => dest.SiteNum, opt => opt.MapFrom(src => src.SITE_NUM))
                .ForMember(dest => dest.SiteNam, opt => opt.MapFrom(src => src.SITE_NAM))
                .ForMember(dest => dest.Dismisal, opt => opt.MapFrom(src => src.DISMISAL))
                .ForMember(dest => dest.Trans, opt => opt.MapFrom(src => src.TRANS))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                .ForMember(dest => dest.Hidden, opt => opt.MapFrom(src => src.Hidden))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ReverseMap();
            
            CreateMap<Inservice, InserviceDto>()
                .ForMember(dest => dest.StaffId, opt => opt.MapFrom(src => src.STAFF_ID))
                .ReverseMap();
            
            CreateMap<Status, StatusDto>()
                .ReverseMap();

            CreateMap<Closing, ClosingDto>()
                .ForMember(dest => dest.ParentCredit, opt => opt.MapFrom(src => src.PARENT_CR))
                .ForMember(dest => dest.StaffPaid, opt => opt.MapFrom(src => src.STAFF_ALL))
                .ReverseMap();

            CreateMap<Timesheet, TimesheetDto>()
                .ReverseMap(); 
            
            CreateMap<Visit, VisitDto>()
                .ReverseMap();
            
            CreateMap<Contact, ContactDto>()
                .ReverseMap(); 

            CreateMap<Workshop, WorkshopDto>()
                .ReverseMap();
            
            CreateMap<Schedule, ScheduleDto>()
                .ReverseMap();
            
            CreateMap<SchedularTimesheet, SchedularTimesheetDto>()
                .ReverseMap();
            CreateMap<Certificate, CertificateDto>()
                .ReverseMap();
            CreateMap<Certificate, CertificateFormDto>()
                .ReverseMap();
            
            CreateMap<WaiversSent, WaiversSentDto>()
                 .ForMember(dest => dest.StaffId, opt => opt.MapFrom(src => src.STAFF_ID))
                 .ForMember(dest => dest.Sent, opt => opt.MapFrom(src => src.SENT))
                .ReverseMap();
            
            CreateMap<WaiversReceived, WaiversReceivedDto>()
                 .ForMember(dest => dest.StaffID, opt => opt.MapFrom(src => src.STAFF_ID))
                 .ForMember(dest => dest.SiteID, opt => opt.MapFrom(src => src.SIte_ID))
                 .ForMember(dest => dest.Received, opt => opt.MapFrom(src => src.RECEIVED))
                .ReverseMap();
            
            CreateMap<Director, DirectorDto>()
                .ReverseMap();

            CreateMap<Attendance, AttendanceDto>()
                    .ForMember(dest => dest.StaffId, opt => opt.MapFrom(src => src.STAFF_ID))
                    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.DATE))
                    .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.REASON))
                    .ForMember(dest => dest.Paid, opt => opt.MapFrom(src => src.PAID))
                    .ForMember(dest => dest.Charged, opt => opt.MapFrom(src => src.CHARGED))
                    .ForMember(dest => dest.Fraction, opt => opt.MapFrom(src => src.FRACTION))
                    .ForMember(dest => dest.SiteNumber, opt => opt.MapFrom(src => src.SITENUM))
                    .ForMember(dest => dest.SiteName, opt => opt.MapFrom(src => src.SITENAM))
                    .ForMember(dest => dest.ReasonID, opt => opt.MapFrom(src => src.ReasonID))
                    .ReverseMap();

            // Mapping between Attendance and AttendanceFormDto
            CreateMap<Attendance, AttendanceFormDto>()
                .ForMember(dest => dest.StaffId, opt => opt.MapFrom(src => src.STAFF_ID))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.DATE))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.REASON))
                .ForMember(dest => dest.Paid, opt => opt.MapFrom(src => src.PAID))
                .ForMember(dest => dest.Charged, opt => opt.MapFrom(src => src.CHARGED))
                .ForMember(dest => dest.Fraction, opt => opt.MapFrom(src => src.FRACTION))
                .ForMember(dest => dest.SiteNumber, opt => opt.MapFrom(src => src.SITENUM))
                .ForMember(dest => dest.SiteName, opt => opt.MapFrom(src => src.SITENAM))
                .ForMember(dest => dest.ReasonID, opt => opt.MapFrom(src => src.ReasonID))
                .ReverseMap();
        }
    }
}
