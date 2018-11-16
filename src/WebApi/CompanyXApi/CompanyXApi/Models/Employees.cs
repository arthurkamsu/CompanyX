using System;
using System.Collections.Generic;

namespace CompanyXApi.Models
{
    public partial class Employees
    {
        public Employees()
        {
            InverseEmpManagerNavigation = new HashSet<Employees>();
        }

        public Guid EmpId { get; set; }
        public string EmpLastName { get; set; }
        public string EmpFirstName { get; set; }
        public string EmpMiddleName { get; set; }
        public decimal EmpSalary { get; set; }
        public Guid? EmpManager { get; set; }
        public string EmpTitle { get; set; }
        public string EmpCode { get; set; }
        public long UctregisteredOn { get; set; }
        public string EmpImage { get; set; }
        public long UctstartDate { get; set; }

        public Employees EmpManagerNavigation { get; set; }
        public ICollection<Employees> InverseEmpManagerNavigation { get; set; }
    }
}
