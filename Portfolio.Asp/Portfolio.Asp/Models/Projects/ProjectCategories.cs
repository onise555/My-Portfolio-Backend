namespace Portfolio.Asp.Models.Projects
{
    public class ProjectCategories
    {
        public int Id { get; set; }
        public string Name { get; set; }


        // One-To-Many კავშირი
        public List<ProjectSubCategories> ProjectSubCategories { get; set; } = new List<ProjectSubCategories>();    
    }
}
