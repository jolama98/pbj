
namespace pbj.Services;

public class AccountService
{
  private readonly AccountsRepository _repo;

  public AccountService(AccountsRepository repo)
  {
    _repo = repo;
  }

  private Account GetAccount(string accountId)
  {
    Account account = _repo.GetById(accountId);
    if (account == null)
    {
      throw new Exception("Invalid Account Id");
    }
    return account;
  }

  internal Account GetOrCreateAccount(Account userInfo)
  {
    Account account = _repo.GetById(userInfo.Id);
    if (account == null)
    {
      return _repo.Create(userInfo);
    }
    return account;
  }

  internal Account Edit(Account editData, string accountId)
  {
    Account original = GetAccount(accountId);
    original.Name = editData.Name ?? editData.Name;
    original.Picture = editData.Picture ?? editData.Picture;
    original.CoverImg = editData.CoverImg ?? editData.CoverImg;
    return _repo.Edit(original);
  }

  internal List<Book> GetBookByAccount(string accountId)
  {

    List<Book> book = _repo.GetAccountBooks(accountId);
    return book;
  }

  internal List<LikedPoem> GetLikedPoemByProfileId(string accountId)
  {
    List<LikedPoem> likedPoems = _repo.GetLikedPoemByAccount(accountId);
    return likedPoems;
  }


  // internal List<LikedPoem> GetLikedPoemByAccount(string profileId)
  // {
  //   List<LikedPoem> likedPoems = _repo.GetLikedPoemByAccount(profileId);
  //   return likedPoems;
  // }
}
