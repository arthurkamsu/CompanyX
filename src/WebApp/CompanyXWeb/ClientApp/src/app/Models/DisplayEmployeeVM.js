"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var util_1 = require("util");
var DisplayEmployeeVM = /** @class */ (function () {
    function DisplayEmployeeVM(employee, employeeManager, employeeSubordinates) {
        this.employee = employee;
        this.employeeManager = util_1.isNullOrUndefined(employeeManager) ? undefined : employeeManager;
        this.employeeSubordinates = ((!util_1.isNullOrUndefined(employeeSubordinates)) && employeeSubordinates.length > 0) ? employeeSubordinates : undefined;
    }
    return DisplayEmployeeVM;
}());
exports.DisplayEmployeeVM = DisplayEmployeeVM;
//# sourceMappingURL=DisplayEmployeeVM.js.map