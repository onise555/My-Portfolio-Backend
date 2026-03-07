namespace Portfolio.Asp.Models.WorkExperiations
{
    public class WorkExperiationSkills
    {
    
            public int id { get; set; }
            public string name { get; set; }


            // One-To-Many კავშირი 
            public int WorkExperationId { get; set; }
            public WorkExperiation WorkExperience { get; set; }
        

    }
}
