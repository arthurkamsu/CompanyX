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
            _context = context ?? throw new ArgumentNullException(nameof(context));
            
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
                .ThenBy(e => e.EmpLastName)
                .ThenBy(e => e.EmpMiddleName)
                .Skip(count * pageIndex)
                .Take(count)
                .ToListAsync();

            var employeesToReturn = new List<DisplayEmployeeVM>();
            if (itemsOnPage.Count>0)
            {            
                foreach (var item in itemsOnPage)
                {
                    //item.setEmployeeImageFullUrl(Configuration["ImagesBaseUrl"]);
                    item.setEmployeeImageFullUrl(baseUrl: "ImagesBaseUrl");
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
                    BasicDisplayEmployeeVM managerToReturn = null;
                    if (originalManager != null)
                    {
                        originalManager.setEmployeeImageFullUrl(baseUrl: "ImagesBaseUrl");

                        managerToReturn =  new BasicDisplayEmployeeVM
                        {
                            Id = originalManager.EmpId.ToString(),
                            Code = originalManager.EmpCode,
                            LastName = originalManager.EmpLastName,
                            FirstName = originalManager.EmpFirstName,
                            MiddleName = originalManager.EmpMiddleName,
                            Salary = originalManager.EmpSalary,
                            UCTRegisteredOn = originalManager.UctregisteredOn,
                            Title = originalManager.EmpTitle,
                            Image = originalManager.EmpImage,
                            UCTStartDate = originalManager.UctstartDate
                        };
                    }
                    var originalListOfSubordinates = item.getSubordinates(this._context);

                    List<BasicDisplayEmployeeVM> listOfSubordinatesToReturn = null;

                    if (originalListOfSubordinates != null && originalListOfSubordinates.Count > 0)
                    {
                        listOfSubordinatesToReturn = new List<BasicDisplayEmployeeVM>();
                        foreach (var subordinate in originalListOfSubordinates)
                        {
                            subordinate.setEmployeeImageFullUrl(baseUrl: "ImagesBaseUrl");
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
                }



            Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4200");

            return Ok(new PaginatedResponse<DisplayEmployeeVM>(pageIndex, count, totalItems, employeesToReturn));

        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateEmploye([FromBody] CreateEmployeeVM employee)
        {

            var newEmp = new Employees
            {
                EmpLastName = employee.Name,
                EmpFirstName = employee.FirstName,
                EmpMiddleName = employee.MiddleName,
                EmpSalary = employee.MonthlySalary,
                EmpTitle = employee.Title,
                EmpManager = new Guid(employee.Manager),
                UctregisteredOn = DateTime.UtcNow.Ticks,
                //UctstartDate = new DateTime(employee.StartDate,)
            };

            //add the image if the employee has been saved
            _context.Employees.Add(newEmp);

            var result = await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployeeById), new { idOrCode = newEmp.EmpId.ToString() }, null);
           
        }

        [HttpGet]
        [Route("getByIdOrByCode/{idOrCode:minlength(8)}")]
        [ProducesResponseType(typeof(DisplayEmployeeVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetEmployeeById(string idOrCode)
        {
            var result = await _context.Employees.Where(emp => emp.EmpId.ToString().Equals(idOrCode.Trim()) || emp.EmpCode.Equals(idOrCode.Trim(),StringComparison.OrdinalIgnoreCase)).ToListAsync();
            if (result.Count != 1) return NotFound();
            else
            {
                var item = result[0];

                item.setEmployeeImageFullUrl(baseUrl: "ImagesBaseUrl");
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
                BasicDisplayEmployeeVM managerToReturn = null;
                if (originalManager != null)
                { 
                    originalManager.setEmployeeImageFullUrl(baseUrl: "ImagesBaseUrl");

                    managerToReturn =  new BasicDisplayEmployeeVM
                    {
                        Id = originalManager.EmpId.ToString(),
                        Code = originalManager.EmpCode,
                        LastName = originalManager.EmpLastName,
                        FirstName = originalManager.EmpFirstName,
                        MiddleName = originalManager.EmpMiddleName,
                        Salary = originalManager.EmpSalary,
                        UCTRegisteredOn = originalManager.UctregisteredOn,
                        Title = originalManager.EmpTitle,
                        Image = originalManager.EmpImage,
                        UCTStartDate = originalManager.UctstartDate
                    };
                }
                var originalListOfSubordinates = item.getSubordinates(this._context);

                List<BasicDisplayEmployeeVM> listOfSubordinatesToReturn = null;

                if (originalListOfSubordinates!=null && originalListOfSubordinates.Count > 0)
                {
                    listOfSubordinatesToReturn = new List<BasicDisplayEmployeeVM>();
                    foreach (var subordinate in originalListOfSubordinates)
                    {
                        subordinate.setEmployeeImageFullUrl(baseUrl:"ImagesBaseUrl");
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

                return Ok(new DisplayEmployeeVM(employeeToReturn,managerToReturn,listOfSubordinatesToReturn));
            }
            
           
        }

       

    }

}
