import { Component, Input, OnInit } from '@angular/core';
import { FlightStatus } from '../models/flightStatus';
import { Flight } from '../models/flight';

@Component({
  selector: 'app-flight',
  templateUrl: './flight.component.html',
  styleUrls: ['./flight.component.scss']
})
export class FlightComponent implements OnInit {

  @Input() flight: Flight;

  constructor() { }

  ngOnInit(): void {
  }

  public displayStatus(status: FlightStatus): string {
    return FlightStatus[status];
  }

  public changeFlightStatus(): void {

  }
}
