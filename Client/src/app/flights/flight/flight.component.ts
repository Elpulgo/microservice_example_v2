import { Component, Input, OnInit } from '@angular/core';
import { Flight } from './flight';
import { FlightExtensions } from './flightExtensions';
import { FlightStatus } from './flightStatus';

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
}
