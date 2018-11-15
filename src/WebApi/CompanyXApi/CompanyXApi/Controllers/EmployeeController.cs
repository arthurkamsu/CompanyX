using System;
using Microsoft.AspNetCore.Mvc;
using CompanyXApi.Infrastructure;
using CompanyXApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CompanyXApi.ViewModels;
using Microsoft.Extensions.Configuration;

namespace CompanyXApi.Controllers
{
    [Route("api/v0.1.0/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly CompanyXDBContext _context;

        public EmployeeController(CompanyXDBContext context, IConfiguration configuration)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            
            ((CompanyXDBContext)context).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
        }

        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(IEnumerable<Employees>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(PaginatedResponse<Employees>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> getEmployees([FromQuery]int count, [FromQuery]int pageIndex = 0)
        {

            var totalItems = await _context.Employees
                .LongCountAsync();

            var itemsOnPage = await _context.Employees
                .OrderBy(e => e.EmpLastName)
                .ThenBy(e=>e.EmpLastName)
                .ThenBy(e=>e.EmpMiddleName)
                .Skip(count * pageIndex)
                .Take(count)             
                .ToListAsync();

            var employeesToReturn = new List<DisplayEmployeeVM>();

            foreach(var item in itemsOnPage)
            {
                //item.setEmployeeImageFullUrl(Configuration["ImagesBaseUrl"]);
                item.setEmployeeImageFullUrl("ImagesBaseUrl");
                var employeeToReturn = new BasicDisplayEmployeeVM
                {
                    Id = item.EmpId.ToString(),
                    Code = item.EmpCode,
                    LastName = item.EmpLastName,
                    FirstName = item.EmpFirstName,
                    MiddleName = item.EmpMiddleName,
                    Salary = item.EmpSalary,
                    UCTRegisteredOn = item.UctregisteredOn,
                    Title = item.EmpTitle,
                    Image = item.EmpImage,
                    UCTStartDate = item.UctstartDate
                };

                var originalManager = item.getMaNanager(_context);

                var managerToReturn = (originalManager != null) ? new BasicDisplayEmployeeVM {
                    Id = item.EmpId.ToString(),
                    Code = item.EmpCode,
                    LastName = item.EmpLastName,
                    FirstName = item.EmpFirstName,
                    MiddleName = item.EmpMiddleName,
                    Salary = item.EmpSalary,
                    UCTRegisteredOn = item.UctregisteredOn,
                    Title = item.EmpTitle,
                    Image = item.EmpImage,
                    UCTStartDate = item.UctstartDate
                } : null;

                var originalListOfSubordinates = item.getSubordinates(this._context);

                List<BasicDisplayEmployeeVM> listOfSubordinatesToReturn=null;

                if(originalListOfSubordinates.Count>0)
                {
                    listOfSubordinatesToReturn = new List<BasicDisplayEmployeeVM>();
                    foreach (var subordinate in originalListOfSubordinates)
                    {
                        listOfSubordinatesToReturn.Add(new BasicDisplayEmployeeVM
                        {
                            Id = subordinate.EmpId.ToString(),
                            Code = subordinate.EmpCode,
                            LastName = subordinate.EmpLastName,
                            FirstName = subordinate.EmpFirstName,
                            MiddleName = subordinate.EmpMiddleName,
                            Salary = subordinate.EmpSalary,
                            UCTRegisteredOn = subordinate.UctregisteredOn,
                            Title = subordinate.EmpTitle,
                            Image = subordinate.EmpImage,
                            UCTStartDate = subordinate.UctstartDate
                        });
                    }
                }
                
               employeesToReturn.Add(new DisplayEmployeeVM(employeeToReturn, managerToReturn, listOfSubordinatesToReturn));
            }

            Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4200");

            return Ok(new PaginatedResponse<DisplayEmployeeVM>(pageIndex, count, totalItems, employeesToReturn));

        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> saveEmploye([FromBody] CreateEmployeeVM employee)
        {
            /*
            var newEmp = new Employees
            {
                EmpLastName = employee.Name,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                MonthlySalary = employee.MonthlySalary,
                Position = employee.Position,
                Manager = employee.Manager
            };


            this._context.Employees.Add(newEmp);

            await this._context.SaveChangesAsync();

            //return Created("api/v0.1.0/[controller]/get/" + createdEmp.Entity.Id, createdEmp);
            return CreatedAtAction(nameof(GetEmployee), new { id = newEmp.Id }, null);
            */
            return Created("","");
        }

        [HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(Employees), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetEmployee(string id)
        {
            /* var result = await this._context.Employee.Where(emp => emp.Id.ToString().Equals(id)).ToListAsync();
             if (result.Count != 1) return NotFound();
             else return Ok(result[0]);*/
            return Ok();
        }

        private void SetManager(List<DisplayEmployeeVM> employees)
        {
            /*
            foreach (var employee in employees)
                if (!string.IsNullOrEmpty(employee.Manager))
                    employee.ManagerNavigation = _context.Employee.Where(e => e.Id.ToString().Equals(employee.Manager)).Select(e => new DisplayEmployeeVM
                    {
                        Id = e.Id.ToString(),
                        FirstName = e.FirstName,
                        Name = e.Name,
                        Position = e.Position,
                        MonthlySalary = e.MonthlySalary,
                        MiddleName = e.MiddleName,
                        Manager = e.Manager.ToString()
                    }
                    ).SingleOrDefault();*/
        }

    }

}
