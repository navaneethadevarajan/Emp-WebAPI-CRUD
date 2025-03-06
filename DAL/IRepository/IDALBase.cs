public interface IDALBase
    {
        List<Department> GetAllDepartments();
        List<Employee> GetAllEmployees();
        List<Employee> GetEmployeesByDeptId(int departmentId);
        void CreateDepartment(Department department);
        Employee CreateEmployee(Employee employee);
        void UpdateEmployeeById(int employeeId,Employee updatedEmployee);
        void UpdateDepartmentById(int departmentId,Department updatedDepartment);
        void RemoveDepartmentById(int departmentId);
        void RemoveEmployeeById(int employeeId);
        bool CheckIfDepartmentHasEmployees(int departmentId);
        void SaveEmployeeFile(int employeeId, string fileName);
        string? GetEmployeeFileName(int employeeId); 

    }