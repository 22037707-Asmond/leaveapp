using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;

namespace LeaveApplication.Controllers
{
    public class AdminController : Controller
    {
        //View Users
        public IActionResult Users()
        {
            List<Employee> list = DBUtl.GetList<Employee>("SELECT e.id, e.name, e.username, e.start_date, e.end_date, e.nric, e.mobile, e.position, e.manager_id, m.name AS manager_name, e.department_id, e.employment_status FROM employee e LEFT JOIN employee m ON e.manager_id = m.id");
            Helper.DeptMappingForUsers(list);
            Helper.PositionMappingforUsers(list);
            Helper.EmploymentStatusMappingforUsers(list);
            return View(list);
        }

        //Create User
        public IActionResult CreateUser()
        {
            //For Dynamic table
            var directors = DBUtl.GetList<Employee>("SELECT e.*, m.name AS manager_name FROM employee e LEFT JOIN employee m ON e.manager_id = m.id WHERE e.position = 1");
            var managers = DBUtl.GetList<Employee>("SELECT e.*, m.name AS manager_name FROM employee e LEFT JOIN employee m ON e.manager_id = m.id WHERE e.position = 2");
            var supervisors = DBUtl.GetList<Employee>("SELECT e.*, m.name AS manager_name FROM employee e LEFT JOIN employee m ON e.manager_id = m.id WHERE e.position = 3");

            ViewBag.Directors = directors ?? new List<Employee>();
            ViewBag.Managers = managers ?? new List<Employee>();
            ViewBag.Supervisors = supervisors ?? new List<Employee>();

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateUser(Employee usr)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                     .Select(e => e.ErrorMessage)
                                                     .ToList();

                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                ViewData["Errors"] = errorMessages;

                // Pass the same data back to the view for consistency
                ViewBag.Directors = DBUtl.GetList<Employee>("SELECT e.*, m.name AS manager_name FROM employee e LEFT JOIN employee m ON e.manager_id = m.id WHERE e.position = 1");
                ViewBag.Managers = DBUtl.GetList<Employee>("SELECT e.*, m.name AS manager_name FROM employee e LEFT JOIN employee m ON e.manager_id = m.id WHERE e.position = 2");
                ViewBag.Supervisors = DBUtl.GetList<Employee>("SELECT e.*, m.name AS manager_name FROM employee e LEFT JOIN employee m ON e.manager_id = m.id WHERE e.position = 3");


                return View("CreateUser");
            }
            else if (usr.manager_id == null)
            {
                string insert =
                    @"INSERT INTO employee (name, username, password, start_date, end_date, nric, mobile, position, manager_id, department_id)
                    VALUES ('{0}', '{1}', HASHBYTES('SHA1', '{2}'), '{3:yyyy-MM-dd}', NULL, '{5}', {6}, '{7}', NULL', '{9}')";


                if (DBUtl.ExecSQL(insert, usr.name, usr.username, usr.password, usr.start_date, usr.end_date, usr.nric, usr.mobile, usr.position, usr.manager_id, usr.department_id, usr.employment_status) == 1)
                {
                    TempData["Message"] = "User Created";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }

                return RedirectToAction("Users");
            }
            else
            {
                string insert =
                    @"INSERT INTO employee (name, username, password, start_date, end_date, nric, mobile, position, manager_id, department_id)
                    VALUES ('{0}', '{1}', HASHBYTES('SHA1', '{2}'), '{3:yyyy-MM-dd}', NULL, '{5}', {6}, '{7}', '{8}', '{9}')";


                if (DBUtl.ExecSQL(insert, usr.name, usr.username, usr.password, usr.start_date, usr.end_date, usr.nric, usr.mobile, usr.position, usr.manager_id, usr.department_id, usr.employment_status) == 1)
                {
                    TempData["Message"] = "User Created";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }

                return RedirectToAction("Users");
            }

        }

        //Edit User
        public IActionResult EditUser(string emailid)
        {
            // Fetch the employee details
            emailid = Common.ReplaceDeshWithDots(HttpUtility.UrlDecode(emailid));

            var employee = DBUtl.GetList<Employee>("SELECT e.*, m.name AS manager_name FROM employee e LEFT JOIN employee m ON e.manager_id = m.id WHERE e.username ='" + emailid + "'").FirstOrDefault();
            if (employee == null)
            {
                TempData["Message"] = "Employee not found.";
                TempData["MsgType"] = "danger";
                return RedirectToAction("Users");
            }

            // Fetch lists for dropdowns
            ViewBag.Directors = DBUtl.GetList<Employee>("SELECT e.*, m.name AS manager_name FROM employee e LEFT JOIN employee m ON e.manager_id = m.id WHERE e.position = 1") ?? new List<Employee>();
            ViewBag.Managers = DBUtl.GetList<Employee>("SELECT e.*, m.name AS manager_name FROM employee e LEFT JOIN employee m ON e.manager_id = m.id WHERE e.position = 2") ?? new List<Employee>();
            ViewBag.Supervisors = DBUtl.GetList<Employee>("SELECT e.*, m.name AS manager_name FROM employee e LEFT JOIN employee m ON e.manager_id = m.id WHERE e.position = 3") ?? new List<Employee>();

            return View(employee);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult EditUser(Employee usr)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                     .Select(e => e.ErrorMessage)
                                                     .ToList();

                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                ViewData["Errors"] = errorMessages;

                // Fetch lists again for dropdowns
                ViewBag.Directors = DBUtl.GetList<Employee>("SELECT e.*, m.name AS manager_name FROM employee e LEFT JOIN employee m ON e.manager_id = m.id WHERE e.position = 1") ?? new List<Employee>();
                ViewBag.Managers = DBUtl.GetList<Employee>("SELECT e.*, m.name AS manager_name FROM employee e LEFT JOIN employee m ON e.manager_id = m.id WHERE e.position = 2") ?? new List<Employee>();
                ViewBag.Supervisors = DBUtl.GetList<Employee>("SELECT e.*, m.name AS manager_name FROM employee e LEFT JOIN employee m ON e.manager_id = m.id WHERE e.position = 3") ?? new List<Employee>();

                return View(usr);
            }

            // Update query with correct syntax
            string updateQuery = @"
        UPDATE employee 
        SET name = @0, 
            username = @1, 
            password = @2, 
            start_date = @3, 
            end_date = @4, 
            nric = @5, 
            mobile = @6, 
            position = @7, 
            manager_id = @8, 
            department_id = @9, 
            employment_status = @10 
        WHERE id = @11";

            int result = DBUtl.ExecSQL(updateQuery,
                usr.name,
                usr.username,
                usr.password,
                usr.start_date,
                usr.end_date,
                usr.nric,
                usr.mobile,
                usr.position,
                usr.manager_id,
                usr.department_id,
                usr.employment_status,
                usr.id);

            if (result == 1)
            {
                TempData["Message"] = "User updated successfully.";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Message"] = "Error updating user.";
                TempData["MsgType"] = "danger";
            }

            return RedirectToAction("Users");
        }


        //Deactive Account
        //[HttpPost]
        /*public IActionResult DeactivateUser(int id)
        {
            / Fetch the employee by ID
            Employee employee = DBUtl.GetList<Employee>("SELECT * FROM Employee WHERE id = @0", id).FirstOrDefault();

            if (employee != null)
            {
                // Set the employment status to 'Deactivated' (1)
                employee.employment_status = 1;
                string updateQuery = "UPDATE Employee SET employment_status = @0 WHERE id = @1";
                int result = DBUtl.ExecSQL(updateQuery, employee.employment_status, employee.id);

                if (result == 1)
                {
                    TempData["Message"] = "Employee deactivated successfully.";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = "Error deactivating employee.";
                    TempData["MsgType"] = "danger";
                }
            }
            else
            {
                TempData["Message"] = "Employee not found.";
                TempData["MsgType"] = "danger";
            }

            return RedirectToAction("Users");
        } */


        //View Department
        public IActionResult Departments()
        {
            List<Departments> list = DBUtl.GetList<Departments>("SELECT d.*, e.name AS dept_hod_name FROM department d LEFT JOIN employee e ON d.dept_hod_emp_id = e.id;");
            Helper.DeptMappingForDept(list);
            return View(list);
        }

        //Create Department
        public IActionResult CreateDepartment()
        {
            //For Dynamic table
            var dept = DBUtl.GetList<Departments>("SELECT d.*, e.name AS dept_hod_name FROM department d LEFT JOIN employee e ON d.dept_hod_emp_id = e.id;");
            ViewBag.Departments = dept ?? new List<Departments>();
            var m_check = DBUtl.GetList<Departments>("SELECT id, name FROM employee WHERE position IN (1 , 2) AND id NOT IN (SELECT dept_hod_emp_id FROM department WHERE dept_hod_emp_id IS NOT NULL);");
            ViewBag.Check = m_check;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateDepartment(Departments dept)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                     .Select(e => e.ErrorMessage)
                                                     .ToList();

                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                ViewData["Errors"] = errorMessages;

                ViewBag.Departments = DBUtl.GetList<Departments>("SELECT d.*, e.name AS dept_hod_name FROM department d LEFT JOIN employee e ON d.dept_hod_emp_id = e.id;");

                return View("CreateDepartment");
            }
            else if (dept.dept_hod_emp_id == null)
            {
                string insert =
                    @"INSERT INTO department (dept_name, dept_description, dept_hod_emp_id)
                    VALUES ('{0}', '{1}', NULL)";


                if (DBUtl.ExecSQL(insert, dept.dept_name, dept.dept_description, dept.dept_hod_emp_id) == 1)
                {
                    TempData["Message"] = "Department Created";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }

                return RedirectToAction("Departments");
            }
            else
            {

                string insert =
                    @"INSERT INTO department (dept_name, dept_description, dept_hod_emp_id)
                    VALUES ('{0}', '{1}', '{2}')";


                if (DBUtl.ExecSQL(insert, dept.dept_name, dept.dept_description, dept.dept_hod_emp_id) == 1)
                {
                    TempData["Message"] = "Department Created";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }

                return RedirectToAction("Departments");
            }

        }
    }
}
