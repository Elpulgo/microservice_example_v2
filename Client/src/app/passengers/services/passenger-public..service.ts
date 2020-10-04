import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseResponseModel } from 'src/app/shared/models/baseResponseModel';
import { CreatePassengerModel } from '../models/createPassengerModel';
import { CreatePassengerResponseModel } from '../models/createPassengerResponseModel';
import { Passenger } from '../models/passenger';
import { GET_ALL_PASSENGERS_FOR_FLIGHT_ENDPOINT, PASSENGER_BASE_URL } from './endpoint-constants';
import { PassengerService } from './passenger.service';

@Injectable({
  providedIn: 'root'
})
export class PassengerPublicService implements PassengerService {

  constructor(private httpClient: HttpClient) { }

  async createPassenger(createPassengerModel: CreatePassengerModel): Promise<CreatePassengerResponseModel> {
    throw new Error('Method not implemented.');
  }

  async updatePassenger(passenger: Passenger): Promise<BaseResponseModel> {
    throw new Error('Method not implemented.');
  }

  async deletePassenger(id: string): Promise<BaseResponseModel> {
    throw new Error('Method not implemented.');
  }

  async getAllPassengersForFlight(flightId: string): Promise<Passenger[]> {
    try {
      const response = await this.httpClient.get<Passenger[]>(`${PASSENGER_BASE_URL}/${GET_ALL_PASSENGERS_FOR_FLIGHT_ENDPOINT}/${flightId}`)
        .toPromise();

      return response;
    } catch (error) {
      // TODO: Notify some error service
      console.log(`Failed to get all passengers for flight ${flightId}: ${error}`);
      return null;
    }
  }
}
