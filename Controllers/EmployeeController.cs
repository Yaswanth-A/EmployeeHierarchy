using EmpHeirarchy.BLL.Services;
using EmpHeirarchy.DAL.Models;
using EmpHeirarchy.REPO.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace EmpHeirarchy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private EmployeeServices _employeeService;

        public EmployeeController(EmployeeServices employeeService)
        {
            _employeeService = employeeService;
        }

        

        [HttpGet("{employeeName}/manager")]
        public IActionResult GetManager(string employeeName)
        {
            try
            {
                var manager = _employeeService.GetManager(employeeName);
                if(manager == null || manager.Count == 0)
                {
                    return NotFound("No employee found");
                }
                return Ok(manager);   
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }

        }

        [HttpGet("{employeeName}/employees")]
        public IActionResult GetManagedEmployees(string employeeName)
        {
            try
            {
                var managedEmployees =  _employeeService.GetManagedEmployees(employeeName);

                if (managedEmployees == null || managedEmployees.Count == 0)
                {
                    return NotFound("No employee found");
                }

                return Ok(managedEmployees);

            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpPost]
        public IActionResult InsertData()
        {
            try
            {
                _employeeService.InsertData();
                return Ok("Data inserted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }



    }
}
