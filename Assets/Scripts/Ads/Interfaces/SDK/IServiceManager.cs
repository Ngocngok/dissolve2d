namespace RocketTeam.Sdk.Services.Interfaces
{
    public interface IServiceManager
    {
        void RegisterService(ServiceType serviceType, object service);

        void UnregisterService(ServiceType serviceType, object service);
    }
}