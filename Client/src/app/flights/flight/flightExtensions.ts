import { FlightStatus } from './flightStatus';

export class FlightExtensions {
    public static ReadableFlightStatus(status: FlightStatus): string {

        console.log(FlightStatus[status]);
        console.log(status.toString());
        const propertyType: Array<string> = Object.keys(FlightStatus);
        return propertyType[0];
    }
}