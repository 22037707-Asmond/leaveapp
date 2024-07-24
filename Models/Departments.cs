using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LeaveApplication.Models
{
    public class Departments
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string dept_name { get; set; }

        [Required]
        public required string dept_description { get; set; }

        public int? dept_hod_emp_id { get; set; }

        //Code below is for virtual mapping
        [NotMapped]
        public string? DepartmentName { get; set; } //Set by DepartmentMapping
        [NotMapped]
        public string? dept_hod_name { get; set; } // This property to hold the department head's name
    }
}
