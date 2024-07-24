using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Employee
{
    [Key]
    public int id { get; set; }

    [Required]
    [MaxLength(255)]
    public required string name { get; set; }

    [Required]
    [MaxLength(255)]
    public required string username { get; set; }

    [Required]
    public required string password { get; set; }

    [Compare("password", ErrorMessage = "Passwords do not match.")]
    [NotMapped]
    public string confirm_password { get; set; } = null!;

    [Required]
    public DateTime start_date { get; set; }

    public DateTime? end_date { get; set; }

    [Required]
    [MaxLength(20)]
    public required string nric { get; set; }

    [Required]
    public required int mobile { get; set; }

    [Required]
    public required int position { get; set; }

    public int? manager_id { get; set; }

    [NotMapped]
    public string? manager_name { get; set; }

    public required int department_id { get; set; }

    public int employment_status { get; set; } = 0;

    //Code below is for virtual mapping
    [NotMapped]
    public string? DepartmentName { get; set; } //Set by DeptMapping
    [NotMapped]
    public string? PositionName { get; set; } //Set by PositionMapping 
    [NotMapped]
    public string? EmploymentStatus { get; set; } //Set by EmploymentStatusMapping

}
