using Portfolio.Asp.Enum;
using Portfolio.Asp.Models.Users;

namespace Portfolio.Asp.Models.WorkExperiations
{
    public class WorkExperiation
    {
       
            public int Id { get; set; }
            public string JobTitle { get; set; }
            public string CompanyName { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Location { get; set; }
            public string Description { get; set; }
            public EmploymentType EmploymentType { get; set; }

            // One-To-Many კავშირი 
            public int profileid { get; set; }
            public Profile profile { get; set; }

        
    }
}
