import { Component } from '@angular/core';
import { Flight } from './flight/flight';
import { FlightStatus } from './flight/flightStatus';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  public flightNumber: string;
  public flightOrigin: string;
  public flightDestination: string;

  public flights: Flight[] = [];

  ngOnInit() {

  }

  public createFlight(): void {
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

    this.flights.push(newFlight);

    this.flightNumber = "";
    this.flightOrigin = "";
    this.flightDestination = "";
  }
}
