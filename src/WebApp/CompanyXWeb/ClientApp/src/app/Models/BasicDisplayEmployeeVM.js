"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var util_1 = require("util");
var BasicDisplayEmployeeVM = /** @class */ (function () {
    function BasicDisplayEmployeeVM(id, code, lastName, firstName, middleName, title, salary, image, uctStartDate, uctRegisteredOn) {
        this.id = id;
        this.code = code;
        this.lastName = lastName;
        this.firstName = firstName;
        this.middleName = middleName;
        this.title = title;
        this.salary = salary;
        this.image = image;
        this.startDate = this.setDate(uctStartDate);
        this.registeredOn = this.setDate(uctRegisteredOn);
        this.fullName = (util_1.isNullOrUndefined(this.firstName) ? '' : this.firstName) + (util_1.isNullOrUndefined(this.middleName) ? '' : ' ' + this.middleName) + ' ' + this.lastName;
    }
    BasicDisplayEmployeeVM.prototype.setDate = function (ticks) {
        //ticks are in nanotime; convert to microtime
        var ticksToMicrotime = ticks / 10000;
        //ticks are recorded from 1/1/1; get microtime difference from 1/1/1/ to 1/1/1970
        var epochMicrotimeDiff = Math.abs(new Date(0, 0, 1).setFullYear(1));
        //new date is ticks, converted to microtime, minus difference from epoch microtime
        var tickDate = new Date(ticksToMicrotime - epochMicrotimeDiff);
        return tickDate.toDateString();
    };
    return BasicDisplayEmployeeVM;
}());
exports.BasicDisplayEmployeeVM = BasicDisplayEmployeeVM;
//# sourceMappingURL=BasicDisplayEmployeeVM.js.map