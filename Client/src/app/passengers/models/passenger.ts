import { PassengerStatus } from './passengerStatus'

export interface Passenger {
    name: string;
    status: PassengerStatus;
    id: string;
    flightId: string;
}