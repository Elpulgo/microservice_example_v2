import { Injectable } from '@angular/core';
import { BaseResponseModel } from '../../shared/models/baseResponseModel';
import { CreateFlightModel } from '../models/createFlightModel';
import { CreateFlightResponseModel } from '../models/createFlightResponseModel';
import { Flight } from '../models/flight';

@Injectable()
export abstract class FlightService {
    abstract async createFlight(createFlightModel: CreateFlightModel): Promise<CreateFlightResponseModel>;
    abstract async updateFlight(flight: Flight): Promise<BaseResponseModel>;
    abstract async deleteFlight(id: string): Promise<BaseResponseModel>;
    abstract async getAllFlights(): Promise<Flight[]>;
    abstract async getFlight(id: string): Promise<Flight>;
}