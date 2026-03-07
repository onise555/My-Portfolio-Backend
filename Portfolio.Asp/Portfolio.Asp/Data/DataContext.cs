using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Portfolio.Asp.Models.Academies;
using Portfolio.Asp.Models.Contacts;
using Portfolio.Asp.Models.languages;
using Portfolio.Asp.Models.Projects;
using Portfolio.Asp.Models.Skills;
using Portfolio.Asp.Models.SocialLinks;
using Portfolio.Asp.Models.User;
using Portfolio.Asp.Models.WorkExperiations;
using ProjectEntity = Portfolio.Asp.Models.Projects.Project;

namespace Portfolio.Asp.Data
{
    public class DataContext:DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }   



        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<ProjectCategories> ProjectCategories { get; set; }
        public DbSet<ProjectMedia> ProjectMedia { get; set; }   
        public DbSet<ProjectSubCategories> ProjectSubCategories { get; set; }
        public DbSet<language> languages { get; set; }  
        public DbSet<Contact> contacts  { get; set; }
        public DbSet<SoftSkills> SoftSkills { get; set; }
        public DbSet<Tools> Tools { get; set; } 
        public DbSet<Academy> academies { get; set; }   
        public DbSet<Education> educations { get; set; }
        public DbSet<EducationSkill> educationsSkill { get; set; }
        public DbSet<WorkExperiation> workExperiations { get; set; }
        public DbSet<WorkExperiationSkills> WorkExperiationSkills { get; set; }
        public DbSet<SocialLink> socialLinks { get; set; }


    }
}
