namespace ClinicApp.Data.Contracts
{
    public interface IDataInitializer
    {
        Task InitializeIdentityDataAsync();
    }
}