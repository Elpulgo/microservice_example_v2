import { Passenger } from '../../passengers/models/passenger';
import { Flight } from '../models/flight';

export interface FlightPassengersMap {
    flight: Flight;
    passengers: Passenger[];
}