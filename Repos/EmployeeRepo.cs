using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using EmpHeirarchy.DAL.Models;
using Newtonsoft.Json;
using System.Data;

namespace EmpHeirarchy.REPO.Repos
{
    public class EmployeeRepo
    {
        private readonly TempoDbContext _dbContext;

        public EmployeeRepo(TempoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<EmployeeDetails> GetManager(string employeeName)
        {

            var query = @"
                            SELECT e.EmployeeName, e.EmployeeID
                            FROM Employee emp
                            JOIN EmployeeDetails e ON emp.EmployeeID = e.EmployeeID
                            WHERE e.EmployeeID IN (
                                SELECT e2.EmployeeID
                                FROM Employee e1, supplies_to, Employee e2
                                WHERE MATCH(e2-(supplies_to)->e1)
                                AND e1.EmployeeID IN (
                            SELECT empo.EmployeeID
                            FROM Employee empo
                            JOIN EmployeeDetails ed ON empo.EmployeeID = ed.EmployeeID
                            AND ed.EmployeeName = @parameter
                            )
                            )";

            var manager = _dbContext.EmployeeDetails.FromSqlRaw(query, new SqlParameter("@parameter", employeeName)).ToList();

            return manager;

        }


        public List<EmployeeDetails> GetManagedEmployees(string employeeName)
        {

            var query = @"
                            SELECT e.EmployeeName, e.EmployeeID
                            FROM Employee emp
                            JOIN EmployeeDetails e ON emp.EmployeeID = e.EmployeeID
                            WHERE e.EmployeeID IN (
                                SELECT e2.EmployeeID
                                FROM Employee e1, supplies_to, Employee e2
                                WHERE MATCH(e1-(supplies_to)->e2)
                                AND e1.EmployeeID IN (
                                    SELECT empo.EmployeeID
                                    FROM Employee empo
                                    JOIN EmployeeDetails ed ON empo.EmployeeID = ed.EmployeeID
                                    AND ed.EmployeeName = @parameter
                                )
                            )";

            var managedEmployees = _dbContext.EmployeeDetails.FromSqlRaw(query, new SqlParameter("@parameter", employeeName)).ToList();

            return managedEmployees;
        }

        public void AddEmployee(string employeeName)
        {

            var query = @"SELECT * FROM EmployeeDetails WHERE EmployeeName = @param";
            var employee = _dbContext.EmployeeDetails.FromSqlRaw(query, new SqlParameter("@param", employeeName)).ToList();

            if (employee == null || employee.Count == 0)
            {
                var employeeDetails = new EmployeeDetails()
                {
                    EmployeeName = employeeName
                };
                _dbContext.EmployeeDetails.Add(employeeDetails);
                _dbContext.SaveChanges();

                var query_node = @"INSERT INTO Employee
                              VALUES ((SELECT e.EmployeeID FROM EmployeeDetails e WHERE e.EmployeeName = @param));";
                _dbContext.Database.ExecuteSqlRaw(query_node, new SqlParameter("@param", employeeName));

            }

        }

        public void InsertData()
        {

            var queryDeleteED = @"DELETE FROM EmployeeDetails";
            var queryDeleteEmployee = @"DELETE FROM Employee";
            var queryDeleteSuppliesTo = @"DELETE FROM Supplies_to";

            _dbContext.Database.ExecuteSqlRaw(queryDeleteSuppliesTo);
            _dbContext.Database.ExecuteSqlRaw(queryDeleteEmployee);
            _dbContext.Database.ExecuteSqlRaw(queryDeleteED);
            


            // Read the contents of the data.json file
            string filePath = @"C:\Users\H542279\source\repos\EmpHeirarchy\EmpHeirarchy.DAL\data\data.json";
            string jsonData = System.IO.File.ReadAllText(filePath);
            var data = JsonConvert.DeserializeObject<List<jsonModel>>(jsonData);

            foreach (var item in data)
            {
                // Insert data into the SQL Server database
                AddEmployee(item.Source);
                AddEmployee(item.Target);

                var query_edge = @"INSERT INTO Supplies_to
                                        VALUES
                                        (
                                         (SELECT $node_id FROM Employee emp JOIN EmployeeDetails e ON emp.EmployeeID = e.EmployeeID WHERE e.EmployeeName = @emp1),
                                         (SELECT $node_id FROM Employee emp JOIN EmployeeDetails e ON emp.EmployeeID = e.EmployeeID WHERE e.EmployeeName = @emp2),
                                         ('reports_to')
                                        )";

                SqlParameter emp1 = new SqlParameter("@emp1", item.Source);
                SqlParameter emp2 = new SqlParameter("@emp2", item.Target);
                _dbContext.Database.ExecuteSqlRaw(query_edge, emp1, emp2);

            }

        }
    }
}
