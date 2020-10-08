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
import { FlightCreatedEvent } from './flightCreatedEvent';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class EventService {

    private _allPassengersBoardedForFlight$: EventEmitter<AllPassengersBoardedEvent> = new EventEmitter<AllPassengersBoardedEvent>();
    private _flightDisembarked$: EventEmitter<FlightDisembarkedEvent> = new EventEmitter<FlightDisembarkedEvent>();
    private _flightArrived$: EventEmitter<FlightArrivedEvent> = new EventEmitter<FlightArrivedEvent>();
    private _flightDeleted$: EventEmitter<FlightDeletedEvent> = new EventEmitter<FlightDeletedEvent>();
    private _flightCreated$: EventEmitter<FlightCreatedEvent> = new EventEmitter<FlightCreatedEvent>();

    public get allPassengersBoardedForFlight$(): EventEmitter<AllPassengersBoardedEvent> {
        return this._allPassengersBoardedForFlight$;
    }

    public get flightDisembarked$(): Observable<FlightDisembarkedEvent> {
        return this._flightDisembarked$.asObservable();
    }

    public get flightArrived$(): Observable<FlightArrivedEvent> {
        return this._flightArrived$.asObservable();
    }

    public get flightDeleted$(): Observable<FlightDeletedEvent> {
        return this._flightDeleted$.asObservable();
    }

    public get flightCreated$(): Observable<FlightCreatedEvent> {
        return this._flightCreated$.asObservable();
    }

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
                this._allPassengersBoardedForFlight$.emit({ flightId: flightMap.flight.id });
            } else if (flightMap.flight.status === FlightStatus.Disembarked) {
                this._flightDisembarked$.emit({ flightId: flightMap.flight.id });
            } else if (flightMap.flight.status === FlightStatus.Arrived) {
                this._flightArrived$.emit({ flightId: flightMap.flight.id });
            }
        }
    }

    public flightDisembarked(flight: Flight): void {
        this._flightDisembarked$.emit({ flightId: flight.id });
    }

    public flightArrived(flight: Flight): void {
        this._flightArrived$.emit({ flightId: flight.id });
    }

    public flightDeleted(flight: Flight): void {
        this._flightDeleted$.emit({ flightId: flight.id });
    }

    public flightCreated(flightId: string): void {
        this._flightCreated$.emit({ flightId });
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
            this._allPassengersBoardedForFlight$.emit({ flightId });
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
