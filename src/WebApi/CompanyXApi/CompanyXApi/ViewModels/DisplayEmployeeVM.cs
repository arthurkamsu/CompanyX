using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyXApi.ViewModels
{
    public class DisplayEmployeeVM
    {
        public readonly BasicDisplayEmployeeVM employee;
        public readonly BasicDisplayEmployeeVM employeeManager;
        public readonly List<BasicDisplayEmployeeVM> employeeSubordinates;


        public DisplayEmployeeVM(BasicDisplayEmployeeVM employee, BasicDisplayEmployeeVM employeeManager, List<BasicDisplayEmployeeVM> employeeSubordinates)
        {
            this.employee = employee;
            this.employeeManager = employeeManager;
            this.employeeSubordinates = employeeSubordinates;
        }






    }
}
