import { FlightListComponent } from '../flight-list/flight-list.component';
import { FlightStatus } from './flightStatus';

export class Flight {
    public origin: string;
    public destination: string;
    public flightNumber: string;
    public id: string;
    public status: FlightStatus;
}