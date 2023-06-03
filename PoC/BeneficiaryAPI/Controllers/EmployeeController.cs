/*
using BeneficiaryAPI.Context;
using Microsoft.AspNetCore.Mvc;

namespace BeneficiaryAPI.Controllers
{
    public class Employee
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly BeneficiaryAPIDbContext _db;
        private readonly List<Employee> _employees = new() { new Employee() { ID = 1, Name = "Louis", Address = "Home" } };

        public EmployeesController(BeneficiaryAPIDbContext db)
        {
            this._db = db;
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            return Ok(_employees);
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeByID(int id)
        {
            return Ok(_employees.FirstOrDefault(emp => emp.ID == id));
        }

        [HttpPost]
        public IActionResult InsertEmployee(Employee emp)
        {
            _employees.Add(emp);
            //_db.SaveChanges();
            return CreatedAtAction($"/minimalapi/employees/{emp.ID}", emp);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, Employee emp)
        {
            //_employees.Update(emp);
            //_db.SaveChanges();
            var empStored = _employees.FirstOrDefault(emp => emp.ID == id);
            if (empStored != null)
            {
                empStored.Name = emp.Name;
                empStored.Address = emp.Address;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var emp = _employees.FirstOrDefault(emp => emp.ID == id);
            if (emp != null)
            {
                _employees.Remove(emp);
                //var emp = _db.Employees.Find(id);
                //_db.Remove(emp);
                //_db.SaveChanges();
            }
            return NoContent();
        }
    }
}
*/