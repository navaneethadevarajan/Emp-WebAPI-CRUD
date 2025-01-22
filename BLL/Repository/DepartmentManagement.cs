public class DepartmentManagement:IDepartmentManagement
    {
        private IDALBase _iDALBase;
        public DepartmentManagement(IDALBase iDALBase)
        {
            _iDALBase=iDALBase;
        }

        public List<Department> GetDepartments()
        {
            return _iDALBase.GetAllDepartments();
        }

        public void CreateDepartment(Department department)
        {
            _iDALBase.CreateDepartment(department);
        }

        public void UpdateDepartmentById(int departmentId, Department updatedDepartment)
        {
            _iDALBase.UpdateDepartmentById(departmentId,updatedDepartment);
        }

        public void RemoveDepartmentById(int departmentId)
        {
            bool hasEmployees=_iDALBase.CheckIfDepartmentHasEmployees(departmentId);
            if (hasEmployees)
            {
                Console.WriteLine($"DEPARTMENT WITH ID {departmentId} CANNOT BE REMOVED BECAUSE IT HAS EMPLOYEES ASSOCIATED WITH IT.");
                Console.WriteLine("PLEASE REASSIGN EMPLOYEES TO ANOTHER DEPARTMENT BEFORE REMOVING THIS DEPARTMENT.");
            }
            else
            {
                _iDALBase.RemoveDepartmentById(departmentId);
            }
        }
    }