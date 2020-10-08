import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { EventService } from 'src/app/events/event.service';
import { FlightArrivedEvent } from 'src/app/events/flightArrivedEvent';
import { FlightCreatedEvent } from 'src/app/events/flightCreatedEvent';
import { PassengerService } from 'src/app/passengers/services/passenger.service';
import { Passenger } from '../../passengers/models/passenger';
import { FlightStatus } from '../models/flightStatus';
import { FlightService } from '../services/flight-service';
import { FlightPassengersMap } from './flight-passengers-map';

@Component({
  selector: 'app-flight-list',
  templateUrl: './flight-list.component.html',
  styleUrls: ['./flight-list.component.scss']
})

export class FlightListComponent implements OnInit, OnDestroy {

  public flightsPassengersMap: FlightPassengersMap[] = [];
  public FlightStatusType: typeof FlightStatus = FlightStatus;
  private _subscriptions: Subscription[] = [];


  constructor(
    private flightService: FlightService,
    private passengerService: PassengerService,
    private eventService: EventService) { }

  async ngOnInit(): Promise<void> {
    await this.loadFlights();

    this._subscriptions.push(this.eventService.flightArrived$
      .subscribe((s: FlightArrivedEvent) => {
        let map = this.flightsPassengersMap.find(f => f.flight.id === s.flightId);
        map.flight.status === FlightStatus.Arrived;
      }));

    this._subscriptions.push(this.eventService.flightDeleted$
      .subscribe(async s => {
        await this.reload();
      }));

    this._subscriptions.push(this.eventService.flightCreated$
      .subscribe(async (flightCreatedEvent: FlightCreatedEvent) => {
        await this.flightCreated(flightCreatedEvent.flightId);
      }));
  }

  ngOnDestroy(): void {
    this._subscriptions.forEach(f => f.unsubscribe());
  }

  private async loadFlights(): Promise<void> {
    const flights = await this.flightService.getAllFlights();

    for (const flight of flights) {
      const passengers = await this.loadPassengersForFlight(flight.id);
      this.flightsPassengersMap.push({ flight, passengers });
    }

    this.eventService.updateFlightMap(this.flightsPassengersMap);
  }

  private async reload(): Promise<void> {
    this.flightsPassengersMap = [];
    await this.loadFlights();
  }

  private async loadPassengersForFlight(flightId: string): Promise<Passenger[]> {
    return await this.passengerService.getAllPassengersForFlight(flightId);
  }

  private async flightCreated(flightId: string): Promise<void> {
    const flight = await this.flightService.getFlight(flightId);
    if (flight == null)
      return;

    this.flightsPassengersMap.push({ flight, passengers: [] });
    this.flightsPassengersMap = this.flightsPassengersMap.reverse();
    this.eventService.updateFlightMap(this.flightsPassengersMap);
  }
}
