import { Component, OnInit } from '@angular/core';
import { CreateFlightModel } from '../models/createFlightModel';
import { FlightService } from '../services/flight-service';

@Component({
  selector: 'app-create-flight',
  templateUrl: './create-flight.component.html',
  styleUrls: ['./create-flight.component.scss']
})
export class CreateFlightComponent implements OnInit {

  public flightNumber: string;
  public flightOrigin: string;
  public flightDestination: string;

  public isVisible: boolean = false;

  constructor(private flightService: FlightService) { }

  ngOnInit(): void {
  }

  public async createFlight(): Promise<void> {

    // TODO: Call flight service.. Then pass event to flight-list to read flights when getting OK back from server
    console.log(this.flightDestination);
    console.log(this.flightOrigin);
    console.log(this.flightNumber);

    const newFlight: CreateFlightModel = {
      origin: this.flightOrigin,
      destination: this.flightDestination,
      flightNumber: this.flightNumber
    };

    const response = await this.flightService.createFlight(newFlight);

    if (response == null || !response.success) {
      console.log("apparently failed..");
      console.log(`Error: ${response.error}`);
      console.log(`Stacktrace: ${response.stacktrace}`);
    } else {
      console.log(`succeded! id: ${response.id}`);
      this.clearInput();
    }
  }

  public toggleCreateFlight(): void {
    if (this.isVisible)
      return;

    this.isVisible = true;
  }

  private clearInput(): void {
    this.flightNumber = "";
    this.flightOrigin = "";
    this.flightDestination = "";
  }
}
