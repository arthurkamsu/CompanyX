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
        public string Title { get; set; }
        public decimal MonthlySalary { get; set; }
        public string Manager { get; set; }
        public DateTime UtcStartDate { get; set; }
        public string Image { get; set; }
    }
}
