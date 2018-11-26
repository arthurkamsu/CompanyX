import { isNullOrUndefined } from "util";

export class BasicDisplayEmployeeVM {
  public readonly id: string;
  public readonly code: string;
  public readonly lastName: string;
  public readonly firstName: string;
  public readonly middleName: string;
  public readonly title: string;
  public readonly salary: number;
  public readonly image: string;
  public readonly startDate: string;
  public readonly registeredOn: string;
  public readonly fullName: string;

  constructor(id: string, code: string, lastName: string, firstName: string, middleName: string, title: string, salary: number, image: string, uctStartDate: number,uctRegisteredOn:number) {
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
    this.fullName = (isNullOrUndefined(this.firstName) ? '' : this.firstName) + (isNullOrUndefined(this.middleName) ? '' : ' '+this.middleName) + ' '+this.lastName;
  }

  private setDate(ticks: number): string {    
    //ticks are in nanotime; convert to microtime
    var ticksToMicrotime = ticks / 10000;

    //ticks are recorded from 1/1/1; get microtime difference from 1/1/1/ to 1/1/1970
    var epochMicrotimeDiff = Math.abs(new Date(0, 0, 1).setFullYear(1));

    //new date is ticks, converted to microtime, minus difference from epoch microtime
    var tickDate = new Date(ticksToMicrotime - epochMicrotimeDiff);
    return tickDate.toDateString();
  }
}
