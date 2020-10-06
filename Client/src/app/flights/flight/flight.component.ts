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

  public isDisembarkButtonVisible: boolean;
  public isLandButtonVisible: boolean;

  constructor() { }

  ngOnInit(): void {
    this.setButtonVisibility();
  }

  public displayStatus(status: FlightStatus): string {
    return FlightStatus[status];
  }

  public changeFlightStatus(): void {
    // this.isDisembarkButtonVisible = yada...
    // this.isLandButtonVisible = yada...
  }

  private setButtonVisibility(): void {
    this.isDisembarkButtonVisible = this.flight.status == FlightStatus.AllBoarded;
    this.isLandButtonVisible = this.flight.status == FlightStatus.Disembarked;
  }
}
