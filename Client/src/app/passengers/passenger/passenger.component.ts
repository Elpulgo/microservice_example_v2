import { Component, Input, OnInit } from '@angular/core';
import { EventService } from 'src/app/events/event.service';
import { Passenger } from '../models/passenger';
import { PassengerStatus } from '../models/passengerStatus';
import { PassengerService } from '../services/passenger.service';

@Component({
  selector: 'app-passenger',
  templateUrl: './passenger.component.html',
  styleUrls: ['./passenger.component.scss']
})
export class PassengerComponent implements OnInit {

  @Input() passenger: Passenger;
  @Input() flightId: string;

  public hasBoarded: boolean;

  constructor(
    private passengerService: PassengerService,
    private eventService: EventService) { }

  ngOnInit(): void {
    this.setBoardButtonEnabledMode();
  }


  public displayStatus(status: PassengerStatus): string {
    return PassengerStatus[status];
  }

  public async boardPassenger(): Promise<void> {
    let passenger: Passenger = { ...this.passenger, status: PassengerStatus.Boarded };

    const response = await this.passengerService.updatePassenger(passenger);
    if (response != null && response.success) {
      const passenger = await this.passengerService.getPassengerById(this.passenger.id);
      this.passenger = passenger;
      this.hasBoarded = true;
      this.eventService.passengerBoarded(this.passenger, this.flightId);
    }
  }

  private setBoardButtonEnabledMode(): void {
    this.hasBoarded =
      this.passenger.status != PassengerStatus.None &&
      this.passenger.status != PassengerStatus.CheckedIn;
  }
}
