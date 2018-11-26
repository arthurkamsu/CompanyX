import { Component, OnInit } from '@angular/core';
import { EmplyeeService } from '../employee.service';
import { PaginatedResponse } from '../Models/PaginatedResponse';
@Component({
  selector: 'app-employeeslist',
  templateUrl: './employeeslist.component.html',
  styleUrls: ['./employeeslist.component.css']
})
export class EmployeeslistComponent implements OnInit {

  public employeesToDisplay: PaginatedResponse;
  public setsOfThree = 2;

  constructor(private employeeService: EmplyeeService) { }

  ngOnInit() {

    this.getEmployeesToDisplay(6,0);

  }

  pageIndexChange(count: number, index: number): void {
    this.employeesToDisplay = undefined;
    this.getEmployeesToDisplay(count, index);
  }

  getEmployeesToDisplay(count: number, index: number) : void {
    this.employeeService.getAListOfEmployees(count,index)
      /*.subscribe(
        data => {
          this.restItems = data;
          //console.log(this.restItems);
        },*/
      .subscribe(data => this.employeesToDisplay = data),
      err => console.error(err),
      () => console.log('done loading employees')
  }




 


}
