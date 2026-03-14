namespace Portfolio.Asp.Models.Academies
{
    public class EducationSkill
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int EducationId { get; set; }
        public Education Education { get; set; }    
    }
}
