import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { PaginatedResponse } from './Models/PaginatedResponse';
import { MessageService } from './message.service';

import { Observable, throwError, from } from 'rxjs';
import { map, catchError, retry } from 'rxjs/operators';
import { DisplayEmployeeVM } from './Models/DisplayEmployeeVM';
import { BasicDisplayEmployeeVM } from './Models/BasicDisplayEmployeeVM';
import { isNullOrUndefined } from 'util';
//import { DisplayEmployeeVM } from './Models/DisplayEmployeeVM';

@Injectable({
  providedIn: 'root'
})
export class EmplyeeService {
  
  //baseUrl = 'http://localhost:61933/';
  baseUrl = 'http://companyxapi.arthurkamsu.me/v0.4.1';

  employees: Observable<PaginatedResponse>;

  constructor(private http: HttpClient, private messageService: MessageService) { }
  //

  // Rest Items Service: Read all REST Items
  getAListOfEmployees(count: number, index: number): Observable<PaginatedResponse> {
    this.employees = undefined;
    this.messageService.add('Employees fetching started...');
    this.employees = this.http
      .get<PaginatedResponse>(this.baseUrl + '/api/Employee/list?count=' + count + '&pageIndex=' + index)
      .pipe(
        retry(3),
        map((item:any) => 
          new PaginatedResponse(
            item.pageIndex,
            item.count,
            item.total,
            item.employees.map(emp => new DisplayEmployeeVM(
              new BasicDisplayEmployeeVM(emp.employee.id, emp.employee.code, emp.employee.lastName, emp.employee.firstName, emp.employee.middleName, emp.employee.title, emp.employee.salary, emp.employee.image, emp.employee.uctStartDate, emp.employee.uctRegisteredOn),
              (isNullOrUndefined(emp.employeeManager) ? undefined:
              new BasicDisplayEmployeeVM(emp.employeeManager.id, emp.employeeManager.code, emp.employeeManager.lastName, emp.employeeManager.firstName, emp.employeeManager.middleName, emp.employeeManager.title, emp.employeeManager.salary, emp.employeeManager.image, emp.employeeManager.uctStartDate, emp.employeeManager.uctRegisteredOn)),
              (((!isNullOrUndefined(emp.employeeSubordinates)) && emp.employeeSubordinates.length > 0) ?
              emp.employeeSubordinates.map(sub =>
                  new BasicDisplayEmployeeVM(sub.id, sub.code, sub.lastName, sub.firstName, sub.middleName, sub.title, sub.salary, sub.image, sub.uctStartDate, sub.uctRegisteredOn)
                )
                : undefined)
            ))
        )
      ),
      catchError(error => {
        //return throwError('Something went wrong!')
        return throwError(error);
      }
        //err => throwError(err))
    ));
      
    this.messageService.add('Employees fetched successfully...');
    return this.employees;
  }

  /*
  saveEmployee(employee: Employee) {
    this.messageService.add('Adding employee ' + employee.firstName + ' ' + employee.name + ' ...');
    this.http.post(this.baseUrl + 'api/v0.1.0/Employee/create', employee);
    this.messageService.add('Employee ' + employee.firstName + ' ' + employee.name + ' added succefully!');
  }
  */


}

