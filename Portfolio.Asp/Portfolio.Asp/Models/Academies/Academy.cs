using Portfolio.Asp.Models.Users;

namespace Portfolio.Asp.Models.Academies
{
    public class Academy
    {
        public int Id { get; set; }

        public string AcademyName { get; set; }
        public string Logo { get; set; }

        // One-To-Many
        public int ProfileId { get; set; }  
        public Profile Profile { get; set; }

        public List<Education> educations { get; set; } = new List<Education>();





    }
}
