using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyXApi.ViewModels
{
    public class DisplayEmployeeVM
    {
        private readonly BasicDisplayEmployeeVM _employee;
        private readonly BasicDisplayEmployeeVM _employeeManager;
        private readonly List<BasicDisplayEmployeeVM> _employeeSubordinates;


        public DisplayEmployeeVM(BasicDisplayEmployeeVM employee, BasicDisplayEmployeeVM employeeManager, List<BasicDisplayEmployeeVM> employeeSubordinates)
        {
            _employee = employee;
            _employeeManager = employeeManager;
            _employeeSubordinates = employeeSubordinates;
        }
    }
}
