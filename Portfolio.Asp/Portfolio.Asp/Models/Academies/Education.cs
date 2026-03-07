namespace Portfolio.Asp.Models.Academies
{
    public class Education
    {
        public int Id { get; set; }

        public string FiledOfStudy { get; set; }

        public string Degree {  get; set; }

        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public string Grade { get; set; }

        public string CertigicateType { get; set; } 

        //One-To-Many კავშირი
        public int AcademyId { get; set; }

        public Academy Academy { get; set; }    


        public List<EducationSkill> Skills { get; set; } =new List<EducationSkill>();


    }
}
