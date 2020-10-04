import { FlightStatus } from './flightStatus';

export interface Flight {
    origin: string;
    destination: string;
    flightNumber: string;
    id: string;
    status: FlightStatus;
}