import { BasicDisplayEmployeeVM } from './BasicDisplayEmployeeVM'
import { isNullOrUndefined } from 'util';
export class DisplayEmployeeVM {
  public readonly employee: BasicDisplayEmployeeVM;
  public readonly employeeManager: BasicDisplayEmployeeVM;
  public readonly employeeSubordinates: BasicDisplayEmployeeVM[];

  constructor(employee: BasicDisplayEmployeeVM, employeeManager: BasicDisplayEmployeeVM, employeeSubordinates: BasicDisplayEmployeeVM[]) {
    this.employee = employee;
    this.employeeManager = isNullOrUndefined(employeeManager) ? undefined : employeeManager;
    this.employeeSubordinates = ((!isNullOrUndefined(employeeSubordinates)) && employeeSubordinates.length > 0) ? employeeSubordinates : undefined;
  }
}
