namespace APBD_Task10.Services;

public interface ITokenService
{
    string GenerateToken(Account account);
}