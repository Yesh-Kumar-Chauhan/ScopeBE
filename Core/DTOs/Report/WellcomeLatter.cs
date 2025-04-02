using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Report
{

    public class WelcomeLatter
    {
        // Basic Personal Information
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }

        // Certification and Training Dates
        public DateTime? CPR { get; set; }
        public DateTime? MatApp { get; set; }
        public DateTime? MatDate { get; set; }
        public DateTime? FirstAid { get; set; }

        // Site Information
        public string? DistrictNameB { get; set; }
        public string? SiteNameB { get; set; }
        public string? SitePositionB { get; set; }
        public string? DistrictNameD { get; set; }
        public string? SiteNameD { get; set; }
        public string? SitePositionD { get; set; }
        public string? DistrictNameA { get; set; }
        public string? SiteNameA { get; set; }
        public string? SitePositionA { get; set; }

        // Schedule Information (Morning, Afternoon, etc.)
        public string? MON_1_B { get; set; }
        public string? MON_1_E { get; set; }
        public string? TUE_1_B { get; set; }
        public string? TUE_1_E { get; set; }
        public string? WED_1_B { get; set; }
        public string? WED_1_E { get; set; }
        public string? THU_1_B { get; set; }
        public string? THU_1_E { get; set; }
        public string? FRI_1_B { get; set; }
        public string? FRI_1_E { get; set; }
        public string? MON_2_B { get; set; }
        public string? MON_2_E { get; set; }
        public string? TUE_2_B { get; set; }
        public string? TUE_2_E { get; set; }
        public string? WED_2_B { get; set; }
        public string? WED_2_E { get; set; }
        public string? THU_2_B { get; set; }
        public string? THU_2_E { get; set; }
        public string? FRI_2_B { get; set; }
        public string? FRI_2_E { get; set; }
        public string? MON_3_B { get; set; }
        public string? MON_3_E { get; set; }
        public string? TUE_3_B { get; set; }
        public string? TUE_3_E { get; set; }
        public string? WED_3_B { get; set; }
        public string? WED_3_E { get; set; }
        public string? THU_3_B { get; set; }
        public string? THU_3_E { get; set; }
        public string? FRI_3_B { get; set; }
        public string? FRI_3_E { get; set; }

        // Pay Rates
        public string? SEP_Rate_B { get; set; }
        public string? JAN_Rate_B { get; set; }
        public string? FEB_Rate_B { get; set; }
        public string? SEP_Rate_D { get; set; }
        public string? JAN_Rate_D { get; set; }
        public string? FEB_Rate_D { get; set; }
        public string? SEP_Rate_A { get; set; }
        public string? JAN_Rate_A { get; set; }
        public string? FEB_Rate_A { get; set; }

        // Employment Dates
        public DateTime? DOEMP { get; set; }
        public DateTime? REHIRED { get; set; }

        // Other Information
        public DateTime? EffectiveDateBefore { get; set; }
        public DateTime? EffectiveDateDuring { get; set; }
        public DateTime? EffectiveDateAfter { get; set; }
        public string? Comment { get; set; }
        public DateTime? Foundations { get; set; }
    }

}
