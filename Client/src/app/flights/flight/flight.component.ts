import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FlightStatus } from '../models/flightStatus';
import { Flight } from '../models/flight';
import { FlightService } from '../services/flight-service';
import { EventService } from 'src/app/events/event.service';
import { filter } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { NotificationService } from 'src/app/notifications/notification.service';

@Component({
  selector: 'app-flight',
  templateUrl: './flight.component.html',
  styleUrls: ['./flight.component.scss']
})
export class FlightComponent implements OnInit, OnDestroy {

  @Input() flight: Flight;

  public isDisembarkButtonVisible: boolean;
  public isLandButtonVisible: boolean;
  public isDeleteButtonVisible: boolean;

  public isLanding: boolean = false;
  public isDisembarking: boolean = false;
  public isDeleting: boolean = false;

  private _allPassengersBoardedSubscription: Subscription;

  constructor(
    private flightService: FlightService,
    public eventService: EventService,
    private notificationService: NotificationService) { }


  ngOnInit(): void {
    this._allPassengersBoardedSubscription = this.eventService.allPassengersBoardedForFlight$
      .pipe(filter(f => f.flightId === this.flight.id))
      .subscribe(s => {
        this.flight.status = FlightStatus.AllBoarded;
        this.setButtonVisibility();
      });

    this.setButtonVisibility();
  }

  ngOnDestroy(): void {
    this._allPassengersBoardedSubscription.unsubscribe();
  }

  public displayStatus(status: FlightStatus): string {
    return FlightStatus[status];
  }

  public async disembark(): Promise<void> {
    if (this.flight.status !== FlightStatus.AllBoarded) {
      this.notificationService.warn("All passengers has not yet boarded!");
      return;
    }

    this.isDisembarking = true;

    const flight = { ...this.flight, status: FlightStatus.Disembarked };
    const response = await this.flightService.updateFlight(flight);

    if (response == null || !response.success) {
      this.isDisembarking = false;
      return;
    }

    this.flight = flight;
    this.setButtonVisibility();
    this.eventService.flightDisembarked(this.flight);
    this.isDisembarking = false;
  }

  public async land(): Promise<void> {
    if (this.flight.status !== FlightStatus.Disembarked) {
      this.notificationService.warn("Flight has not yet disembarked and can't land!");
      return;
    }

    this.isLanding = true;

    const flight = { ...this.flight, status: FlightStatus.Arrived };
    const response = await this.flightService.updateFlight(flight);

    if (response == null || !response.success) {
      this.isLanding = false;
      return;
    }
    this.flight = flight;
    this.setButtonVisibility();
    this.eventService.flightArrived(this.flight);
    this.isLanding = false;
  }

  public async delete(): Promise<void> {
    if (this.flight.status !== FlightStatus.Arrived) {
      this.notificationService.warn("Flight has not yet arrived and can't be deleted!");
      return;
    }

    this.isDeleting = true;

    const response = await this.flightService.deleteFlight(this.flight.id);

    if (response == null || !response.success) {
      this.isDeleting = false;
      return;
    }

    this.eventService.flightDeleted(this.flight);
    this.isDeleting = false;
  }



  private setButtonVisibility(): void {
    this.isDisembarkButtonVisible = this.flight.status == FlightStatus.AllBoarded;
    this.isLandButtonVisible = this.flight.status == FlightStatus.Disembarked;
    this.isDeleteButtonVisible = this.flight.status == FlightStatus.Arrived;
  }
}
