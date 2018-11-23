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
using System.Drawing;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using CompanyXApi.Tools;


namespace CompanyXApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly CompanyXDBContext _context;
        private IHostingEnvironment _envConfig;
        private IConfiguration _appConfig;

        private string _employeesImagesFolder;
        private string _employeesImagesPublicBaseUrl;

        private const int DEFAULT_EMPLOYEE_IMAGE_SIZE = 400;
        private const int DEFAULT_EMPLOYEE_IMAGE_QUALITY = 75;

        public EmployeeController(CompanyXDBContext context,IHostingEnvironment envConfig, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _envConfig = envConfig;
            _appConfig = configuration;

            _employeesImagesFolder = Path.Combine(_envConfig.WebRootPath,_appConfig["EMPLOYEES_IMAGES_FOLDER"]);
            //depending on the envirenment, the base url will be populated
            _employeesImagesPublicBaseUrl = _appConfig["BASE_URL"]+ "/api/employee/{0}/pic" ;
            
            if(_envConfig.IsDevelopment())
                ((CompanyXDBContext)context).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
        }

        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(IEnumerable<Employees>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(PaginatedResponse<Employees>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetEmployees([FromQuery]int count, [FromQuery]int pageIndex = 0)
        {
            
            var totalItems = await _context.Employees
                .LongCountAsync();

            var itemsOnPage = await _context.Employees
                .OrderBy(e => e.EmpFirstName)
                //.ThenBy(e => e.EmpLastName)
                //.ThenBy(e => e.EmpMiddleName)
                .Skip(count * pageIndex)
                .Take(count)
                .ToListAsync();

            var employeesToReturn = new List<DisplayEmployeeVM>();
            if (itemsOnPage.Count>0)
            {            
                foreach (var item in itemsOnPage)
                {
                    item.setEmployeeImageFullUrl(baseUrl: _employeesImagesPublicBaseUrl);
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
                        originalManager.setEmployeeImageFullUrl(baseUrl: _employeesImagesPublicBaseUrl);

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
                    var originalListOfSubordinates = item.getSubordinates(_context);

                    List<BasicDisplayEmployeeVM> listOfSubordinatesToReturn = null;

                    if (originalListOfSubordinates != null && originalListOfSubordinates.Count > 0)
                    {
                        listOfSubordinatesToReturn = new List<BasicDisplayEmployeeVM>();
                        foreach (var subordinate in originalListOfSubordinates)
                        {
                            subordinate.setEmployeeImageFullUrl(baseUrl: _employeesImagesPublicBaseUrl);
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


            if(_envConfig.IsDevelopment())
                Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4200");

            return Ok(new PaginatedResponse<DisplayEmployeeVM>(pageIndex, count, totalItems, employeesToReturn));

        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
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
                UctstartDate = employee.UtcStartDate.Ticks
            };            

            string employeeImagephysicalPath = null;

            try
            {
                _context.Employees.Add(newEmp);
                await _context.SaveChangesAsync();//this will raise an error of type: the affected row expected is 1 but actually 0

                if (!string.IsNullOrWhiteSpace(employee.Image))
                {
                    if (!Directory.Exists(_employeesImagesFolder)) Directory.CreateDirectory(_employeesImagesFolder);
                    Image employeeImage =ImagesTool.ResizeImage(ImagesTool.Base64ToImage(employee.Image),DEFAULT_EMPLOYEE_IMAGE_SIZE,DEFAULT_EMPLOYEE_IMAGE_QUALITY);
                    string empImg = newEmp.EmpCode + ".JPG";
                    employeeImagephysicalPath = Path.Combine(_employeesImagesFolder, empImg);                  
                    
                    employeeImage.SaveWithEncoder(employeeImagephysicalPath, DEFAULT_EMPLOYEE_IMAGE_QUALITY);
                    newEmp.EmpImage = empImg;               
                    await _context.SaveChangesAsync();
                }

                return CreatedAtAction(nameof(GetEmployeeByIdOrCode), new { idOrCode = newEmp.EmpId.ToString() }, null);
                
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(employeeImagephysicalPath))
                    if (System.IO.File.Exists(employeeImagephysicalPath)) System.IO.File.Delete(employeeImagephysicalPath);
                //log the error
                if(_envConfig.IsProduction())
                    return StatusCode((int)HttpStatusCode.InternalServerError, new {Message = "An error occured while creating the employee. Please contact the administrator."});
                else return StatusCode((int)HttpStatusCode.InternalServerError, new { Trace = ex.StackTrace, Message = ex.Message, Source = ex.Source, Inner = ex.InnerException });
                
            }

        }

        [HttpGet]
        [Route("getByIdOrByCode/{idOrCode:minlength(8)}")]
        [ProducesResponseType(typeof(DisplayEmployeeVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetEmployeeByIdOrCode(string idOrCode)
        {
            //check the field idOrCode
            var result = await _context.Employees.Where(emp => emp.EmpId.ToString().Equals(idOrCode.Trim()) || emp.EmpCode.Equals(idOrCode.Trim(),StringComparison.OrdinalIgnoreCase)).ToListAsync();
            if (result.Count != 1) return NotFound();
            else
            {
                var currentEmp = result[0];

                currentEmp.setEmployeeImageFullUrl(baseUrl: _employeesImagesPublicBaseUrl);
                var employeeToReturn = new BasicDisplayEmployeeVM
                {
                    Id = currentEmp.EmpId.ToString(),
                    Code = currentEmp.EmpCode,
                    LastName = currentEmp.EmpLastName,
                    FirstName = currentEmp.EmpFirstName,
                    MiddleName = currentEmp.EmpMiddleName,
                    Salary = currentEmp.EmpSalary,
                    UCTRegisteredOn = currentEmp.UctregisteredOn,
                    Title = currentEmp.EmpTitle,
                    Image = currentEmp.EmpImage,
                    UCTStartDate = currentEmp.UctstartDate
                };

                var originalManager = currentEmp.getMaNanager(_context);
                BasicDisplayEmployeeVM managerToReturn = null;
                if (originalManager != null)
                { 
                    originalManager.setEmployeeImageFullUrl(baseUrl: _employeesImagesPublicBaseUrl);

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
                var originalListOfSubordinates = currentEmp.getSubordinates(_context);

                List<BasicDisplayEmployeeVM> listOfSubordinatesToReturn = null;

                if (originalListOfSubordinates!=null && originalListOfSubordinates.Count > 0)
                {
                    listOfSubordinatesToReturn = new List<BasicDisplayEmployeeVM>();
                    foreach (var subordinate in originalListOfSubordinates)
                    {
                        subordinate.setEmployeeImageFullUrl(baseUrl: _employeesImagesPublicBaseUrl);
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
 
        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(UpdateEmployeeVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeVM employeeToUpdate)
        {
            //check the field idOrCode
            var result = await _context.Employees.Where(emp => emp.EmpId.ToString().Equals(employeeToUpdate.IdOrCode.Trim()) 
            || emp.EmpCode.Equals(employeeToUpdate.IdOrCode.Trim(), StringComparison.OrdinalIgnoreCase)).ToListAsync();
            //this is for avoiding the exception that single or default might throw if there is more than one result

            if (result.Count != 1) return NotFound();
            else
            {
                var currentEmp = result[0];
                currentEmp.EmpLastName = employeeToUpdate.Name;
                currentEmp.EmpFirstName = employeeToUpdate.FirstName;
                currentEmp.EmpMiddleName = employeeToUpdate.MiddleName;
                currentEmp.EmpSalary = employeeToUpdate.MonthlySalary;
                if(!string.IsNullOrEmpty(employeeToUpdate.Manager))
                    currentEmp.EmpManager = new Guid(employeeToUpdate.Manager);
                currentEmp.EmpTitle = employeeToUpdate.Title;
                currentEmp.UctstartDate = employeeToUpdate.UtcStartDate.Ticks;

                string empImg = currentEmp.EmpCode + ".JPG";
                if (!string.IsNullOrEmpty(employeeToUpdate.Image))
                {
                    if (!Directory.Exists(_employeesImagesFolder)) Directory.CreateDirectory(_employeesImagesFolder);
                    Image employeeImage = ImagesTool.ResizeImage(ImagesTool.Base64ToImage(employeeToUpdate.Image),DEFAULT_EMPLOYEE_IMAGE_SIZE,DEFAULT_EMPLOYEE_IMAGE_QUALITY) ;
                    
                    //update of the image might not be needed
                    string employeeImagephysicalPath = Path.Combine(_employeesImagesFolder, empImg);
                    employeeImage.SaveWithEncoder(employeeImagephysicalPath,DEFAULT_EMPLOYEE_IMAGE_QUALITY);

                    if (string.IsNullOrEmpty(currentEmp.EmpImage)) currentEmp.EmpImage = empImg;
                }
                else
                {
                    string potentialImagephysicalPath = Path.Combine(_employeesImagesFolder, empImg);
                    if (System.IO.File.Exists(potentialImagephysicalPath))
                        System.IO.File.Delete(potentialImagephysicalPath);
                }

                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(currentEmp.EmpImage))
                    employeeToUpdate.Image = string.Format(_employeesImagesPublicBaseUrl, currentEmp.EmpCode);

                return Ok(employeeToUpdate);
            }
        }



    }

}
