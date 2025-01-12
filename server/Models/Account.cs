namespace pbj.Models;
public class Account : Profile
{
  public string Email { get; set; }
  public Profile Creator { get; set; }
}
