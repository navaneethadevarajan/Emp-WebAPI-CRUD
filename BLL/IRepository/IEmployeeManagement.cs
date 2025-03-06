public interface IEmployeeManagement
    {
        List<Employee> GetAllEmployees();
        List<Employee> GetEmployeesByDeptId(int departmentId);
        void CreateEmployee(Employee employee);
        void UpdateEmployeeById(int employeeId,Employee updatedEmployee);
        void RemoveEmployeeById(int employeeId);
        void SaveEmployeeFile(int employeeId, string fileName);
        string? GetEmployeeFileName(int employeeId); 

    }