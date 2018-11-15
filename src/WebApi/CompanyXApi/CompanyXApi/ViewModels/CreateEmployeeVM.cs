using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyXApi.ViewModels
{
    public class CreateEmployeeVM
    {      
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Position { get; set; }
        public decimal MonthlySalary { get; set; }
        public Guid? Manager { get; set; }
    }
}
