import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';

import { AppComponent } from './app.component';
import { FlightComponent } from './flight/flight.component';
import { PassengerComponent } from './passenger/passenger.component';
import { AuditlogComponent } from './auditlog/auditlog.component';
import { FlightListComponent } from './flight-list/flight-list.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    FlightComponent,
    PassengerComponent,
    AuditlogComponent,
    FlightListComponent
  ],
  imports: [
    BrowserModule,
    MatButtonModule,
    MatGridListModule,
    BrowserAnimationsModule
  ],
  exports: [
    MatButtonModule,
    MatGridListModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
