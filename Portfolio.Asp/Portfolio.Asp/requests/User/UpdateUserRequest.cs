namespace Portfolio.Asp.requests.User;

public class UpdateUserRequest
{
    public int Id { get; set; }
    public string FullName { get; set; }

    public string ProfileImage { get; set; }
    
    public string ProfileVideo { get; set; }
}