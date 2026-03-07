using Portfolio.Asp.Enum;
using Portfolio.Asp.Models.User;

namespace Portfolio.Asp.Models.WorkExperience;

public class WorkExperience
{
    public int Id { get; set; }
    public string JobTitle  { get; set; }
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