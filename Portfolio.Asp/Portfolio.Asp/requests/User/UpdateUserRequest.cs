namespace Portfolio.Asp.requests.User;

public class UpdateUserRequest
{
    public int Id { get; set; }
    public string FullName { get; set; }

    public IFormFile ProfileImage { get; set; }
    
    public IFormFile ProfileVideo { get; set; }
}