import { Component, Input, OnInit } from '@angular/core';
import { PassengerService } from 'src/app/passengers/services/passenger.service';
import { Passenger } from '../../passengers/models/passenger';
import { Flight } from '../models/flight';
import { FlightService } from '../services/flight-service';
import { FlightPassengersMap } from './flight-passengers-map';

@Component({
  selector: 'app-flight-list',
  templateUrl: './flight-list.component.html',
  styleUrls: ['./flight-list.component.scss']
})

export class FlightListComponent implements OnInit {
  public flightsPassengersMap: FlightPassengersMap[] = [];

  constructor(
    private flightService: FlightService,
    private passengerService: PassengerService) { }

  async ngOnInit(): Promise<void> {
    await this.loadFlights();
  }

  private async loadFlights(): Promise<void> {
    const flights = await this.flightService.getAllFlights();

    for (const flight of flights) {
      const passengers = await this.loadPassengersForFlight(flight.id);

      // TODO: Push this list to a service, which will hold a dictionary for flights, and update passengers status
      // in this service once we board a passenger.
      // Can then do a lookup if all passengers has boarded, and subsequently light up disembark button (do it with async pipe observable..)
      this.flightsPassengersMap.push({ flight, passengers });
    }
  }

  private async loadPassengersForFlight(flightId: string): Promise<Passenger[]> {
    return await this.passengerService.getAllPassengersForFlight(flightId);
  }
}
