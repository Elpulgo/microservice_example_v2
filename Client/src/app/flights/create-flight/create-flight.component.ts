import { Component, OnInit } from '@angular/core';
import { Flight } from '../flight/flight';
import { FlightStatus } from '../flight/flightStatus';

@Component({
  selector: 'app-create-flight',
  templateUrl: './create-flight.component.html',
  styleUrls: ['./create-flight.component.scss']
})
export class CreateFlightComponent implements OnInit {

  public flightNumber: string;
  public flightOrigin: string;
  public flightDestination: string;

  constructor() { }

  ngOnInit(): void {
  }

  public createFlight(): void {

    // TODO: Call flight service.. Then pass event to flight-list to read flights when getting OK back from server
    console.log(this.flightDestination);
    console.log(this.flightOrigin);
    console.log(this.flightNumber);

    const newFlight: Flight = {
      origin: this.flightOrigin,
      destination: this.flightDestination,
      flightNumber: this.flightNumber,
      id: "0",
      status: FlightStatus.None
    };

    // this.flights.push(newFlight);

    this.flightNumber = "";
    this.flightOrigin = "";
    this.flightDestination = "";
  }
}
