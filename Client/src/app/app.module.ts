import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AuditlogComponent } from './auditlog/auditlog.component';
import { FlightComponent } from './flights/flight/flight.component';
import { PassengerComponent } from './passengers/passenger/passenger.component';
import { FlightListComponent } from './flights/flight-list/flight-list.component';
import { PassengerListComponent } from './passengers/passenger-list/passenger-list.component';
import { CreateFlightComponent } from './flights/create-flight/create-flight.component';
import { FlightPublicService } from './flights/services/flight-public.service';
import { FlightService } from './flights/services/flight-service';
import { PassengerService } from './passengers/services/passenger.service';
import { PassengerPublicService } from './passengers/services/passenger-public..service';
import { EventService } from './events/event.service';
import { NotificationModule } from './notifications/notification.module';
import { AuditlogService } from './auditlog/services/auditlog.service';

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
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    NotificationModule
  ],
  exports: [
  ],
  providers: [
    { provide: FlightService, useClass: FlightPublicService },
    { provide: PassengerService, useClass: PassengerPublicService },
    EventService,
    AuditlogService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
