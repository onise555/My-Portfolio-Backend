namespace Portfolio.Asp.Models.Projects
{
    public class ProjectCategories
    {
        public int Id { get; set; }

        public string Name { get; set; }    

        public List<ProjectSubCategories> ProjectSubCategories { get; set; } = new List<ProjectSubCategories>();    
    }
}
