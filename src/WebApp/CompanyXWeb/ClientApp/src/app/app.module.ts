import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { SearchbarComponent } from './searchbar/searchbar.component';
import { EmployeeslistComponent } from './employeeslist/employeeslist.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { FooterComponent } from './footer/footer.component';
import { PaginationComponent } from './pagination/pagination.component';
import { MessageComponent } from './message/message.component';

@NgModule({
  declarations: [
    AppComponent,    
    NavbarComponent,
    SearchbarComponent,
    EmployeeslistComponent,
    SidebarComponent,
    FooterComponent,
    PaginationComponent,
    MessageComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
