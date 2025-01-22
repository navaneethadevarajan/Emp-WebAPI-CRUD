public class EmployeeManagement : IEmployeeManagement
    {
        private IDALBase _iDALBase;
        public EmployeeManagement(IDALBase iDALBase)
        {
            _iDALBase=iDALBase;
        }

        public void CreateEmployee(Employee employee)
        {
            _iDALBase.CreateEmployee(employee);
        }

        public List<Employee> GetAllEmployees()
        {
            return _iDALBase.GetAllEmployees();
        }

        public List<Employee> GetEmployeesByDeptId(int departmentId)
        {
            return _iDALBase.GetEmployeesByDeptId(departmentId);      
        }

        public void RemoveEmployeeById(int employeeId)
        {
            _iDALBase.RemoveEmployeeById(employeeId);
        }

        public void UpdateEmployeeById(int employeeId, Employee updatedEmployee)
        {
            _iDALBase.UpdateEmployeeById(employeeId,updatedEmployee);
        }
    }