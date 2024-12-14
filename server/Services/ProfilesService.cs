
namespace pbj.Services;

public class ProfilesService
{

    private readonly ProfilesRepository _profilesRepository;

    public ProfilesService(ProfilesRepository profilesRepository)
    {
        _profilesRepository = profilesRepository;
    }
    internal Profile GetProfileById(string profileId)
    {

        Profile profile = _profilesRepository.GetProfileById(profileId) ?? throw new Exception($"No profile found with the id of {profileId}");
        return profile;
    }
}
