namespace Otel.Demo.AssetApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<string?> GetUserName();
    }
}
