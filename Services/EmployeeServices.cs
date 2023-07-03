using EmpHeirarchy.DAL.Models;
using EmpHeirarchy.REPO.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpHeirarchy.BLL.Services
{
    public class EmployeeServices
    {
        private EmployeeRepo _employeeRepo;

        public EmployeeServices(EmployeeRepo employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public List<EmployeeDetails> GetManager(string employeeName)
        {
            return _employeeRepo.GetManager(employeeName);
    
        }

        public List<EmployeeDetails> GetManagedEmployees(string employeeName)
        {
            return _employeeRepo.GetManagedEmployees(employeeName);
        }

        public void InsertData()
        {
            _employeeRepo.InsertData();
        }

    }
}
