using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CompanyXApi.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CompanyXApi.Models;

namespace CompanyXApi.Controllers
{
  
    [ApiController]
    public class PicController : ControllerBase
    {

        private readonly CompanyXDBContext _context;
        private IHostingEnvironment _envConfig;
        private IConfiguration _appConfig;

        private string _employeesImagesFolder;
        
        public PicController(CompanyXDBContext context, IHostingEnvironment envConfig, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _envConfig = envConfig;
            _appConfig = configuration;

            _employeesImagesFolder = Path.Combine(_envConfig.WebRootPath, _appConfig["EMPLOYEES_IMAGES_FOLDER"]);
            
            ((CompanyXDBContext)context).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
        }

        [HttpGet]
        [Route("api/employee/{idOrCode:minlength(8)}/pic")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetImage(string idOrCode)
        {
            var result = await _context.Employees.Where(emp => emp.EmpId.ToString().Equals(idOrCode.Trim()) || emp.EmpCode.Equals(idOrCode.Trim(), StringComparison.OrdinalIgnoreCase)).ToListAsync();
            if (result.Count != 1) return NotFound();
            else
            {
                var currentEmp = result[0];

                if (string.IsNullOrWhiteSpace(currentEmp.EmpImage)) return BadRequest("This employee does not have a picture.");
                
                string employeeImagephysicalPath = Path.Combine(_employeesImagesFolder, currentEmp.EmpImage);

                if (System.IO.File.Exists(employeeImagephysicalPath))
                {
                    var buffer = System.IO.File.ReadAllBytes(employeeImagephysicalPath);

                    return File(buffer, "image/jpeg",currentEmp.slug(), true);
                }
                else return NotFound("This employee image is not on the server it might have been moved to somewhere else.");
            }
        }

    }
}