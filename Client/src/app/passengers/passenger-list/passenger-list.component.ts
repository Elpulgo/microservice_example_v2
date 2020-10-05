import { Component, Input, OnInit } from '@angular/core';
import { Passenger } from '../models/passenger';
import { PassengerService } from '../services/passenger.service';

@Component({
  selector: 'app-passenger-list',
  templateUrl: './passenger-list.component.html',
  styleUrls: ['./passenger-list.component.scss']
})
export class PassengerListComponent implements OnInit {

  @Input() passengers: Passenger[];
  @Input() flightId: string;

  public isPassengerCreateVisible: boolean = false;
  public passengerName: string;

  constructor(private passengerService: PassengerService) { }

  ngOnInit(): void {
  }

  public async createPassenger(): Promise<void> {
    if (!this.validateInput())
      return;

    const response = await this.passengerService.createPassenger({ name: this.passengerName, flightId: this.flightId });
    if (response.success) {
      const passenger = await this.passengerService.getPassengerById(response.id);
      this.passengers.push(passenger);
    }

    this.clearInput();
  }

  public togglePassengerCreate(): void {
    this.isPassengerCreateVisible = !this.isPassengerCreateVisible;
  }

  private clearInput(): void {
    this.passengerName = "";
  }

  private validateInput(): boolean {
    if (!this.passengerName || this.passengerName === "" || !this.flightId || this.flightId == "") {
      alert("Passenger name/flight id can't be empty!");
      return false;
    }

    return true;
  }
}
