import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { EventService } from 'src/app/events/event.service';
import { FlightStatus } from 'src/app/flights/models/flightStatus';
import { Passenger } from '../models/passenger';
import { PassengerService } from '../services/passenger.service';

@Component({
  selector: 'app-passenger-list',
  templateUrl: './passenger-list.component.html',
  styleUrls: ['./passenger-list.component.scss']
})
export class PassengerListComponent implements OnInit, OnDestroy {

  @Input() passengers: Passenger[];
  @Input() flightId: string;

  public isPassengerCreateVisible: boolean = false;
  public passengerName: string;
  public isAddPassengerVisible: boolean = true;

  private _flightHasDisembarkedSubscription: Subscription;
  private _flightHasArrivedSubscription: Subscription;

  constructor(
    private passengerService: PassengerService,
    private eventService: EventService) { }

  ngOnInit(): void {
    this._flightHasDisembarkedSubscription = this.eventService.flightDisembarked$
      .pipe(filter(f => f.flightId === this.flightId))
      .subscribe(s => {
        this.isAddPassengerVisible = false;
      });

    this._flightHasArrivedSubscription = this.eventService.flightArrived$
      .pipe(filter(f => f.flightId === this.flightId))
      .subscribe(s => {
        this.isAddPassengerVisible = false;
        this.reloadPassengers();
      });

    const flightMap = this.eventService.flightMap.get(this.flightId);
    if (flightMap) {
      this.isAddPassengerVisible = flightMap.flight.status !== (FlightStatus.Arrived || FlightStatus.Disembarked);
    }
  }

  ngOnDestroy(): void {
    this._flightHasDisembarkedSubscription.unsubscribe();
    this._flightHasArrivedSubscription.unsubscribe();
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

  private async reloadPassengers(): Promise<void> {
    const passengers = await this.passengerService.getAllPassengersForFlight(this.flightId);
    this.passengers = passengers;
  }
}
