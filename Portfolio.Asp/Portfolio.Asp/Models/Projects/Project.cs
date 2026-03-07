using Portfolio.Asp.Models.User;
using Portfolio.Asp.Skills;

namespace Portfolio.Asp.Models.Projects
{
    public class Project
    {
        public int Id { get; set; } 
        public string Name { get; set; }    

        public string Description { get; set; } 

        public DateTime AddAt { get; set; } 
        public DateTime UpdateAt { get; set; }

        public int Like {  get; set; }


        // One-To-Many 
        public List<ProjectSubCategories> ProjectSubCategories { get; set; } = new List<ProjectSubCategories>();    

        public List<ProjectMedia> projectsMedia { get; set; }    =new List<ProjectMedia>();


        public int ProfileId { get; set; }  

        public Profile Profile { get; set; }

        public List<Tools> Tools { get; set; } = new List<Tools>();




    }
}
