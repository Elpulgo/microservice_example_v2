import { EventEmitter, Injectable } from '@angular/core';
import { FlightPassengersMap } from '../flights/flight-list/flight-passengers-map';
import { AllPassengersBoardedEvent } from './allPassengersBoardedEvent';
import { FlightArrivedEvent } from './flightArrivedEvent';
import { FlightDisembarkedEvent } from './flightDisembarkedEvent';
import { filter } from 'rxjs/operators';
import { Flight } from '../flights/models/flight';
import { Passenger } from '../passengers/models/passenger';
import { PassengerStatus } from '../passengers/models/passengerStatus';

@Injectable({
    providedIn: 'root'
})
export class EventService {

    // TODO: Push this list to a service, which will hold a dictionary for flights, and update passengers status
    // in this service once we board a passenger.
    // Can then do a lookup if all passengers has boarded, and subsequently light up disembark button (do it with async pipe observable..)

    public allPassengersBoardedForFlight$: EventEmitter<AllPassengersBoardedEvent> = new EventEmitter<AllPassengersBoardedEvent>();
    public flightDisembarked$: EventEmitter<FlightDisembarkedEvent> = new EventEmitter<FlightDisembarkedEvent>();
    public flightArrived$: EventEmitter<FlightArrivedEvent> = new EventEmitter<FlightArrivedEvent>();


    private _flightMap: Map<string, FlightPassengersMap> = new Map<string, FlightPassengersMap>();

    get flightMap(): Map<string, FlightPassengersMap> {
        return this._flightMap;
    }

    constructor() {
    }

    public updateFlightMap(map: FlightPassengersMap[]): void {
        this._flightMap = new Map(map.map(m => [m.flight.id, m]));
    }

    public flightDisembarked(flight: Flight): void {
        this.flightDisembarked$.emit({ flightId: flight.id });
    }

    public flightArrived(flight: Flight): void {
        this.flightArrived$.emit({ flightId: flight.id });
    }

    public addPassenger(passenger: Passenger, flightId: string): void {
        const flightMap = this.getFlightMap(flightId);
        if (!flightMap)
            return;

        flightMap.passengers.push(passenger);
    }

    public passengerBoarded(passenger: Passenger, flightId: string): void {
        const flightMap = this.getFlightMap(flightId);
        if (!flightMap)
            return;

        const existingPassenger = flightMap.passengers.find(f => f.id == passenger.id);
        if (!existingPassenger) {
            console.log(`Can't find passenger '${passenger.id}' on flight '${flightId}'...`);
            return;
        }

        existingPassenger.status = passenger.status;

        if (this.hasAllPassengersBoarded(flightId)) {
            this.allPassengersBoardedForFlight$.emit({ flightId });
        }
    }


    public test() {
        this.allPassengersBoardedForFlight$.pipe(filter(f => f.flightId == "apa")).subscribe(s => {
            // do something..
        });
    }

    private getFlightMap(flightId: string): FlightPassengersMap | undefined {
        const flightMap = this._flightMap.get(flightId);
        if (!flightMap) {
            console.log(`Flight with id '${flightId}' doesn't exist.`);
            return undefined;
        }

        return flightMap;
    }

    private hasAllPassengersBoarded(flightId: string): boolean {
        const flightMap = this.getFlightMap(flightId);
        if (!flightMap)
            return false;

        return flightMap.passengers.every(e => e.status == PassengerStatus.Boarded);
    }
}
