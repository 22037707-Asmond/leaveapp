using LeaveApplication.Models;
using System.Collections.Generic;

namespace LeaveApplication.Utils
{
    public class Helper
    {
        public static void DeptMappingForUsers(List<Employee> employees)
        // Dictionary to map department_id to their names for user view
        {
            var departmentNames = new Dictionary<int, string>
        {
                { 1, "Corporate" },
                { 2, "Finance" },
                { 3, "HR" },
                { 4, "IT" },
                { 5, "Sales" },
                { 6, "Marketing" }
                // Add more departments if needed
        };

            foreach (var employee in employees)
            {
                if (departmentNames.ContainsKey(employee.department_id))
                {
                    employee.DepartmentName = departmentNames[employee.department_id];
                }
                else
                {
                    employee.DepartmentName = "Unknown/Not Updated";
                }
            }
        }

        public static void PositionMappingforUsers(List<Employee> employees)
        {
            // Dictionary to map position_id to their name for user view
            var positionNames = new Dictionary<int, string>
        {
            { 0, "Admin" },
            { 1, "Director" },
            { 2, "Manger" },
            { 3, "Supervisor" },
            { 4, "Regular Employee" },
            // Add more positions if needed
        };

            foreach (var employee in employees)
            {
                if (positionNames.ContainsKey(employee.position)) // Ensure the property name matches
                {
                    employee.PositionName = positionNames[employee.position]; // Set the correct property
                }
                else
                {
                    employee.PositionName = "Unknown/Not Updated"; // Set the correct property
                }
            }
        }

        public static void EmploymentStatusMappingforUsers(List<Employee> employees)
        {
            // Dictionary to map position_id to their name for user view
            var employmentStatus = new Dictionary<int, string>
        {
            { 0, "Active" },
            { 1, "Deactivated" },
            // Add more positions if needed
        };

            foreach (var employee in employees)
            {
                if (employmentStatus.ContainsKey(employee.employment_status)) // Ensure the property name matches
                {
                    employee.EmploymentStatus = employmentStatus[employee.employment_status]; // Set the correct property
                }
                else
                {
                    employee.EmploymentStatus = "Unknown/Not Updated"; // Set the correct property
                }
            }
        }

        public static void DeptMappingForDept(List<Departments> departments)
        // Dictionary to map department_id to their names for department view
        {
            var departmentNames = new Dictionary<int, string>
        {
                { 1, "Corporate" },
                { 2, "Finance" },
                { 3, "HR" },
                { 4, "IT" },
                { 5, "Sales" },
                { 6, "Marketing" }
                // Add more departments if needed
        };

            foreach (var department in departments)
            {
                if (departmentNames.ContainsKey(department.id))
                {
                    department.DepartmentName = departmentNames[department.id];
                }
                else
                {
                    department.DepartmentName = "Unknown/Not Updated";
                }
            }
        }

    }
}
