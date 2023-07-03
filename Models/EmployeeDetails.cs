

using System.ComponentModel.DataAnnotations;

namespace EmpHeirarchy.DAL.Models
{
    public class EmployeeDetails
    {
        [Key]
        public Guid EmployeeID { get; set; }
        public string EmployeeName { get; set; }
    }
}
