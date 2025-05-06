namespace Tutorial8.Services;

public interface ISharedService
{
    Task<bool> DoesTripExist(int tripId);
    Task<bool> DoesClientExist(int clientId);
}