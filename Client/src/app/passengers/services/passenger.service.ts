import { Injectable } from '@angular/core';
import { BaseResponseModel } from '../../shared/models/baseResponseModel';
import { CreatePassengerModel } from '../models/createPassengerModel';
import { CreatePassengerResponseModel } from '../models/createPassengerResponseModel';
import { Passenger } from '../models/passenger';

@Injectable()
export abstract class PassengerService {
    abstract async createPassenger(createPassengerModel: CreatePassengerModel): Promise<CreatePassengerResponseModel>;
    abstract async updatePassenger(passenger: Passenger): Promise<BaseResponseModel>;
    abstract async getAllPassengersForFlight(flightId: string): Promise<Passenger[]>;
    abstract async getPassengerById(id: string): Promise<Passenger>;
}