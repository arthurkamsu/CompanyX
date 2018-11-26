import { DisplayEmployeeVM } from './DisplayEmployeeVM'
export class PaginatedResponse {
  public readonly pageIndex: number;
  public readonly count: number;
  public readonly total: number;
  public readonly employees: DisplayEmployeeVM[];


  constructor(pageIndex: number,count: number, total: number, employees: DisplayEmployeeVM[]) {
    this.pageIndex = pageIndex;
    this.count = count;
    this.total = total;
    this.employees = employees;
  }

}
