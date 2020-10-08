import { EventEmitter, Injectable } from '@angular/core';
import { FlightPassengersMap } from '../flights/flight-list/flight-passengers-map';
import { AllPassengersBoardedEvent } from './allPassengersBoardedEvent';
import { FlightArrivedEvent } from './flightArrivedEvent';
import { FlightDisembarkedEvent } from './flightDisembarkedEvent';
import { Flight } from '../flights/models/flight';
import { Passenger } from '../passengers/models/passenger';
import { PassengerStatus } from '../passengers/models/passengerStatus';
import { FlightStatus } from '../flights/models/flightStatus';
import { FlightDeletedEvent } from './flightDeletedEvent';
import { NotificationService } from '../notifications/notification.service';

@Injectable({
    providedIn: 'root'
})
export class EventService {

    public allPassengersBoardedForFlight$: EventEmitter<AllPassengersBoardedEvent> = new EventEmitter<AllPassengersBoardedEvent>();
    public flightDisembarked$: EventEmitter<FlightDisembarkedEvent> = new EventEmitter<FlightDisembarkedEvent>();
    public flightArrived$: EventEmitter<FlightArrivedEvent> = new EventEmitter<FlightArrivedEvent>();
    public flightDeleted$: EventEmitter<FlightDeletedEvent> = new EventEmitter<FlightDeletedEvent>();

    private _flightMap: Map<string, FlightPassengersMap> = new Map<string, FlightPassengersMap>();

    get flightMap(): Map<string, FlightPassengersMap> {
        return this._flightMap;
    }

    constructor(private notificationService: NotificationService) {
    }

    public updateFlightMap(map: FlightPassengersMap[]): void {
        this._flightMap = new Map(map.map(m => [m.flight.id, m]));

        for (const flightMap of map) {
            if (flightMap.flight.status === FlightStatus.AllBoarded) {
                this.allPassengersBoardedForFlight$.emit({ flightId: flightMap.flight.id });
            } else if (flightMap.flight.status === FlightStatus.Disembarked) {
                this.flightDisembarked$.emit({ flightId: flightMap.flight.id });
            } else if (flightMap.flight.status === FlightStatus.Arrived) {
                this.flightArrived$.emit({ flightId: flightMap.flight.id });
            }
        }
    }

    public flightDisembarked(flight: Flight): void {
        this.flightDisembarked$.emit({ flightId: flight.id });
    }

    public flightArrived(flight: Flight): void {
        this.flightArrived$.emit({ flightId: flight.id });
    }

    public flightDeleted(flight: Flight): void {
        this.flightDeleted$.emit({ flightId: flight.id });
    }

    public passengerBoarded(passenger: Passenger, flightId: string): void {
        const flightMap = this.getFlightMap(flightId);
        if (!flightMap)
            return;

        const existingPassenger = flightMap.passengers.find(f => f.id == passenger.id);
        if (!existingPassenger) {
            this.notificationService.warn(`Can't board passenger '${passenger.name}'. Passenger is not on flight '${flightId}'...`);
            return;
        }

        existingPassenger.status = passenger.status;

        if (this.hasAllPassengersBoarded(flightId)) {
            this.allPassengersBoardedForFlight$.emit({ flightId });
        }
    }

    private getFlightMap(flightId: string): FlightPassengersMap | undefined {
        const flightMap = this._flightMap.get(flightId);
        if (!flightMap) {
            this.notificationService.warn(`Flight with id '${flightId}' does not exist!`);
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
