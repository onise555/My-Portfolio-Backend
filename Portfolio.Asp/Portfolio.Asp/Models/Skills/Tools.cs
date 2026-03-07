using Portfolio.Asp.Enum;
using Portfolio.Asp.Models.Projects;
using Portfolio.Asp.Models.User;

namespace Portfolio.Asp.Models.Skills
{
    public class Tools
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Badge { get; set; }

        //Enum ცხრილები
        public ToolCategory ToolCategory { get; set; }

        //Many-To-Many კავშირები
        public List<Profile > Profile { get; set; } =  new List<Profile>(); 
        public List<Project> projects { get; set; } = new List<Project> ();    

    }
}
