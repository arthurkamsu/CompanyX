using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyXApi.ViewModels
{
    public class BasicDisplayEmployeeVM
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Title { get; set; }
        public decimal Salary { get; set; }
        public string Image { get; set; }
        public long UCTStartDate { get; set; }
        public long UCTRegisteredOn { get; set; }
    }
}
