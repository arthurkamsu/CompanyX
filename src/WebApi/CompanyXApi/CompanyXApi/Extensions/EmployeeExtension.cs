using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyXApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text;
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
            string finalImage = (employee.EmpImage != null) ? string.Format(baseUrl, employee.EmpCode) : null;
            employee.EmpImage = finalImage;
        }
        public static string slug(this Employees employee)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(employee.EmpFirstName))
            {
                sb.Append(employee.EmpFirstName.Replace(' ', '-'));
                sb.Append("-");
            }

            if (!string.IsNullOrWhiteSpace(employee.EmpMiddleName))
            {
                sb.Append(employee.EmpMiddleName.Replace(' ', '-'));
                sb.Append("-");
            }

            sb.Append(employee.EmpLastName.Replace(' ', '-'));

            return sb.ToString();
        }
    }
}
