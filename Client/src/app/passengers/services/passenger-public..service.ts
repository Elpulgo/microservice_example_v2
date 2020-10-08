import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NotificationService } from 'src/app/notifications/notification.service';
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

  constructor(
    private httpClient: HttpClient,
    private notificationService: NotificationService) { }

  async createPassenger(createPassengerModel: CreatePassengerModel): Promise<CreatePassengerResponseModel> {
    try {
      const response = await this.httpClient.post<CreatePassengerResponseModel>(`${PASSENGER_BASE_URL}`, createPassengerModel)
        .toPromise();

      if (response.success) {
        return response;
      }

      this.notificationService.error(`Failed to create passenger '${createPassengerModel.name}', reason: ${response.error}`);
      return null;
    } catch (error) {
      this.notificationService.error(`Failed to create passenger '${createPassengerModel.name}'. Check logs for further info.`);
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
      }

      this.notificationService.error(`Failed to update passenger '${passenger.name}', reason: ${response.error}`);
      return null;
    } catch (error) {
      this.notificationService.error(`Failed to update passenger '${passenger.name}'. Check logs for further info.`);
      console.log(`Failed to update passenger: '${passenger.name}', error: ${error}`);
      return null;
    }
  }

  async getAllPassengersForFlight(flightId: string): Promise<Passenger[]> {
    try {
      return await this.httpClient.get<Passenger[]>(`${PASSENGER_BASE_URL}/${GET_ALL_PASSENGERS_FOR_FLIGHT_ENDPOINT}/${flightId}`)
        .toPromise();
    } catch (error) {
      this.notificationService.error(`Failed to list all passengers for flight '${flightId}'. Check logs for further info.`);
      console.log(`Failed to get all passengers for flight ${flightId}: ${error}`);
      return null;
    }
  }

  async getPassengerById(id: string): Promise<Passenger> {
    try {
      return await this.httpClient.get<Passenger>(`${PASSENGER_BASE_URL}/${id}`)
        .toPromise();
    } catch (error) {
      this.notificationService.error(`Failed to get passenger with id '${id}'. Check logs for further info.`);
      console.log(`Failed to get passenger: ${error}`);
      return null;
    }
  }
}
