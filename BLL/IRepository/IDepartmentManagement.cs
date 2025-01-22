public interface IDepartmentManagement
    {
         List<Department> GetDepartments();
         void CreateDepartment(Department department);
         void UpdateDepartmentById(int departmentId,Department updatedDepartment);
         void RemoveDepartmentById(int departmentId);
    }