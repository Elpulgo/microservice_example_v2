import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { AuditlogComponent } from './auditlog/auditlog.component';
import { FlightComponent } from './flights/flight/flight.component';
import { PassengerComponent } from './passengers/passenger/passenger.component';
import { FlightListComponent } from './flights/flight-list/flight-list.component';
import { PassengerListComponent } from './passengers/passenger-list/passenger-list.component';
import { CreateFlightComponent } from './flights/create-flight/create-flight.component';

@NgModule({
  declarations: [
    AppComponent,
    FlightComponent,
    PassengerComponent,
    AuditlogComponent,
    FlightListComponent,
    PassengerListComponent,
    CreateFlightComponent
  ],
  imports: [
    BrowserModule,
    MatButtonModule,
    MatGridListModule,
    BrowserAnimationsModule,
    FormsModule
  ],
  exports: [
    MatButtonModule,
    MatGridListModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
