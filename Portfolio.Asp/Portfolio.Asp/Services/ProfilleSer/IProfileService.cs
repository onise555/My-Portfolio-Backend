using Portfolio.Asp.DTO_s.profile;
using Portfolio.Asp.requests.Profile;

public interface IProfileService
{
    Task Create(CreateProfilerequest request);
    Task Update(int UserId, UpdateProfilerequest request);
    Task Delete(int id);
    
    Task<UserProfileDTO?> GetByUserId(int userId); 
}