import { HttpClient, HttpParams } from '@angular/common/http';
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
    try {
      const response = await this.httpClient.post<CreatePassengerResponseModel>(`${PASSENGER_BASE_URL}`, createPassengerModel)
        .toPromise();

      if (response.success) {
        return response;
      } else {
        // TODO: Notify some error service
        return null;
      }
    } catch (error) {
      //TODO: Notify some error service
      console.log(`Failed to create passenger: '${createPassengerModel.name}', error: ${error}`);
      return null;
    }
  }

  async updatePassenger(passenger: Passenger): Promise<BaseResponseModel> {
    try {
      const response = await this.httpClient.put<BaseResponseModel>(`${PASSENGER_BASE_URL}`, passenger)
        .toPromise();

      if (response.success) {
        return response;
      } else {
        //TODO: Notify some error service
        return null;
      }
    } catch (error) {
      //TODO: Notify some error service
      console.log(`Failed to update passenger: '${passenger.name}', error: ${error}`);
      return null;
    }
  }

  async deletePassenger(id: string): Promise<BaseResponseModel> {
    try {
      const options = { params: new HttpParams().set('id', id) };

      const response = await this.httpClient.delete<BaseResponseModel>(`${PASSENGER_BASE_URL}`, options)
        .toPromise();

      if (response.success) {
        return response;
      } else {
        //TODO: Notify some error service
        return null;
      }
    } catch (error) {
      //TODO: Notify some error service
      console.log(`Failed to delete passenger with id: '${id}', error: ${error}`);
      return null;
    }
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

  async getPassengerById(id: string): Promise<Passenger> {
    try {
      const response = await this.httpClient.get<Passenger>(`${PASSENGER_BASE_URL}/${id}`)
        .toPromise();

      return response;
    } catch (error) {
      // TODO: Notify some error service
      console.log(`Failed to get passenger: ${error}`);
      return null;
    }
  }
}
