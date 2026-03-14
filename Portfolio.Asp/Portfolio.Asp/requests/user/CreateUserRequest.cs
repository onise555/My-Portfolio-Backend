namespace Portfolio.Asp.requests.User;

public class CreateUserRequest
{
    public string FullName { get; set; }

    public IFormFile? ProfileImage { get; set; }
    
    public IFormFile? ProfileVideo { get; set; }
}