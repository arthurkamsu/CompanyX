import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

//import { PaginatedResponse } from '../../classes/PaginatedResponse';
//import { Employee } from '../../classes/Employee'
import { MessageService } from './message.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmplyeeService {
  
  baseUrl = 'http://companyxapi.arthurkamsu.me/';
  //restItems: Observable<PaginatedResponse>;
  employees: Observable<any>;

  constructor(private http: HttpClient, private messageService: MessageService) { }
  //

  // Rest Items Service: Read all REST Items
  getAListOfEmployees(count: number, index: number) {
    this.employees = undefined;
    this.messageService.add('Employees fetching started...');
    this.employees = this.http
      .get<any>(this.baseUrl + 'api/v0.1.0/Employee/list?count=' + count + '&pageIndex=' + index);
    //.pipe(map(data => data));
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

