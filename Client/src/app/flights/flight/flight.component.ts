import { Component, Input, OnInit } from '@angular/core';
import { FlightStatus } from '../models/flightStatus';
import { Flight } from '../models/flight';
import { FlightService } from '../services/flight-service';

@Component({
  selector: 'app-flight',
  templateUrl: './flight.component.html',
  styleUrls: ['./flight.component.scss']
})
export class FlightComponent implements OnInit {

  @Input() flight: Flight;

  public isDisembarkButtonVisible: boolean;
  public isLandButtonVisible: boolean;

  public isLanding: boolean = false;
  public isDisembarking: boolean = false;

  constructor(private flightService: FlightService) { }

  ngOnInit(): void {
    this.setButtonVisibility();
  }

  public displayStatus(status: FlightStatus): string {
    return FlightStatus[status];
  }

  public async disembark(): Promise<void> {
    if (this.flight.status !== FlightStatus.AllBoarded) {
      alert("All passengers has not yet boarded!");
      return;
    }

    this.isDisembarking = true;

    const flight = { ...this.flight, status: FlightStatus.Disembarked };
    const response = await this.flightService.updateFlight(flight);
    if (response != null && response.success) {
      this.flight = flight;
      this.setButtonVisibility();
    } else {
      console.log("Failed to disembark flight!");
    }

    this.isDisembarking = false;
  }

  public async land(): Promise<void> {
    if (this.flight.status !== FlightStatus.Disembarked) {
      alert("Flight has not yet disembarked and can't land!");
      return;
    }

    this.isLanding = true;

    const flight = { ...this.flight, status: FlightStatus.Arrived };
    const response = await this.flightService.updateFlight(flight);

    if (response != null && response.success) {
      this.flight = flight;
      this.setButtonVisibility();
    } else {
      console.log("Failed to land flight!");
    }

    this.isLanding = false;
  }

  private setButtonVisibility(): void {
    this.isDisembarkButtonVisible = this.flight.status == FlightStatus.AllBoarded;
    this.isLandButtonVisible = this.flight.status == FlightStatus.Disembarked;
  }
}
