using Microsoft.AspNetCore.Mvc.Formatters;

namespace Portfolio.Asp.Models.Projects
{
    public class ProjectMedia
    {
        public int Id { get; set; } 
        public string Url { get; set; } 

        public MediaType MediaType { get; set; }    

        public int ProjectId { get; set; }  
        public Project Project { get; set; }    

    }
}
