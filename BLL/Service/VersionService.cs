namespace EmployeeManagementSystem.BLL.Service
{
    public class VersionService
    {
        private readonly string _version;

        public VersionService(IConfiguration configuration)
        {
            _version = configuration["Version"] ?? "Unknown";
        }

        public string GetVersion()
        {
            return _version;
        }
    }
}