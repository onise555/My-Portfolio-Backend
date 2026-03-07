namespace Portfolio.Asp.Models.Projects
{
    public class ProjectSubCategories
    {
        public int Id { get; set; } 
        public string Name { get; set; }    

        public int ProjectCategoriesId {  get; set; }
        public ProjectCategories ProjectCategories { get; set; } = new ProjectCategories(); 

        public List<Project> projects { get; set; } =new List<Project>();

    }
}
