using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyXApi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CompanyXApi.Models
{
    public static class EmployeeExtension
    {
        public static Employees getMaNanager(this Employees employee, CompanyXDBContext _context)
        {
            return  (employee.EmpManager==null) ? null :  _context.Employees.Where(e => e.EmpId.Equals(employee.EmpManager)).SingleOrDefault();
           
        }
        public static List<Employees> getSubordinates(this Employees employee, CompanyXDBContext _context)
        {
            return  _context.Employees.Where(e => e.EmpManager.Equals(employee.EmpId)).ToList();
        }
        public static void setEmployeeImageFullUrl(this Employees employee, string baseUrl)
        {
            employee.EmpImage = (employee.EmpImage != null) ? (baseUrl + employee.EmpImage):null;
        }

    }
}
