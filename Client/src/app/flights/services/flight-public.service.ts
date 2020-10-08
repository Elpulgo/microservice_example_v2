import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { FLIGHT_BASE_URL } from './endpoint-constants';
import { CreateFlightResponseModel } from '../models/createFlightResponseModel';
import { BaseResponseModel } from '../../shared/models/baseResponseModel';
import { Flight } from '../models/flight';
import { CreateFlightModel } from '../models/createFlightModel';
import { FlightService } from './flight-service';
import { NotificationService } from 'src/app/notifications/notification.service';

@Injectable({
  providedIn: 'root'
})
export class FlightPublicService implements FlightService {

  constructor(
    private httpClient: HttpClient,
    private notificationService: NotificationService) {
  }

  async createFlight(createFlightModel: CreateFlightModel): Promise<CreateFlightResponseModel> {
    try {
      const response = await this.httpClient.post<CreateFlightResponseModel>(
        FLIGHT_BASE_URL,
        createFlightModel)
        .toPromise();

      if (response.success)
        return response;

      this.notificationService.error(`Failed to create flight '${createFlightModel.flightNumber}', reason: ${response.error}.`);
      return null;
    } catch (error) {
      this.notificationService.error(`Failed to create flight! '${createFlightModel.flightNumber}'. Check logs for further info.`);
      console.log(`Failed to create flight: '${error}'`);
      return null;
    }
  }

  async updateFlight(flight: Flight): Promise<BaseResponseModel> {
    try {
      const response = await this.httpClient.put<BaseResponseModel>(
        FLIGHT_BASE_URL,
        flight)
        .toPromise();

      if (response.success)
        return response;

      this.notificationService.error(`Failed to update flight '${flight.flightNumber}', reason: ${response.error}.`);
      return null;
    } catch (error) {
      this.notificationService.error(`Failed to update flight '${flight.flightNumber}'. Check logs for further info.`);
      console.log(`Failed to update flight: '${error}'`);
      return null;
    }
  }

  async deleteFlight(id: string): Promise<BaseResponseModel> {
    try {

      const options = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        }),
        body: {
          id
        }
      }

      const response = await this.httpClient.delete<BaseResponseModel>(
        FLIGHT_BASE_URL,
        options)
        .toPromise();

      if (response.success)
        return response;

      this.notificationService.error(`Failed to delete flight, reason: ${response.error}.`);
      return null;
    } catch (error) {
      this.notificationService.error(`Failed to delete flight. Check logs for further info.`);
      console.log(`Failed to delete flight: '${error}'`);
      return null;
    }
  }

  async getAllFlights(): Promise<Flight[]> {
    try {
      return await this.httpClient.get<Flight[]>(FLIGHT_BASE_URL).toPromise();
    } catch (error) {
      this.notificationService.error(`Failed to list flights. Check logs for further info.`);
      console.log(`Failed to get all flights: ${error}`);
      return null;
    }
  }

  async getFlight(id: string): Promise<Flight> {
    try {
      return await this.httpClient.get<Flight>(`${FLIGHT_BASE_URL}/${id}`).toPromise();
    } catch (error) {
      this.notificationService.error(`Failed to get flight. Check logs for further info.`);
      console.log(`Failed to get flight: ${error}`);
      return null;
    }
  }
}
