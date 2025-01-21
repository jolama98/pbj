


namespace pbj.Repositories;
public class AccountsRepository
{
  private readonly IDbConnection _db;

  public AccountsRepository(IDbConnection db)
  {
    _db = db;
  }

  internal Account GetByEmail(string userEmail)
  {
    string sql = "SELECT * FROM accounts WHERE email = @userEmail";
    return _db.QueryFirstOrDefault<Account>(sql, new { userEmail });
  }

  internal Account GetById(string id)
  {
    string sql = "SELECT * FROM accounts WHERE id = @id";
    return _db.QueryFirstOrDefault<Account>(sql, new { id });
  }

  internal Account Create(Account newAccount)
  {
    string sql = @"
            INSERT INTO accounts
              (name, picture, email, id, coverImg)
            VALUES
              (@Name, @Picture, @Email, @Id, @coverImg)";
    _db.Execute(sql, newAccount);
    return newAccount;
  }

  internal Account Edit(Account update)
  {
    string sql = @"
            UPDATE accounts
            SET
              name = @Name,
              picture = @Picture,
              coverImg = @CoverImg
            WHERE id = @Id;";
    _db.Execute(sql, update);
    return update;
  }

  internal List<Book> GetAccountBooks(string accountId)
  {
    string sql = @"
    SELECT
    books.*
    FROM books
    WHERE books.creatorId = @accountId
    ;";

    List<Book> book = _db.Query<Book>(sql, new
    {
      accountId
    }).ToList();
    return book;
  }


  internal List<LikedPoem> GetLikedPoemByAccount(string accountId)
  {
    string sql = @"
    SELECT
    likedPoem.*
    FROM likedPoem
    WHERE likedPoem.creatorId = @accountId
    ;";
    List<LikedPoem> likedPoems = _db.Query<LikedPoem>(sql, new { accountId }).ToList();
    return likedPoems;
  }


  internal List<SavedPoem> GetAllSavedPoems(string accountId)
  {
    string sql = @"
    SELECT
    savedPoem.*
    FROM savedPoem
    WHERE savedPoem.creatorId = @accountId
    ;";
    List<SavedPoem> savedPoems = _db.Query<SavedPoem>(sql, new { accountId }).ToList();
    return savedPoems;

  }
  private Account JoinCreator(Account account, Profile profile)
  {
    account.Creator = profile;
    return account;
  }
}

