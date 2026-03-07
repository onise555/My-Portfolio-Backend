using Portfolio.Asp.Enum;
using Portfolio.Asp.Models.Users;



namespace Portfolio.Asp.Models.Skills


{

    public class SoftSkills
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public SoftSkillCategory SoftSkillCategory { get; set; }


        //One-To-Many 
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }




    }
}

