import { Component, OnInit } from '@angular/core';
import { EventService } from 'src/app/events/event.service';
import { NotificationService } from 'src/app/notifications/notification.service';
import { CreateFlightModel } from '../models/createFlightModel';
import { FlightService } from '../services/flight-service';

@Component({
  selector: 'app-create-flight',
  templateUrl: './create-flight.component.html',
  styleUrls: ['./create-flight.component.scss']
})
export class CreateFlightComponent implements OnInit {

  public flightNumber: string;
  public flightOrigin: string;
  public flightDestination: string;

  public isVisible: boolean = true;

  constructor(
    private flightService: FlightService,
    private eventService: EventService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
  }

  public async createFlight(): Promise<void> {
    if (!this.validateInput()) {
      this.notificationService.warn("Field for flight can't be empty!");
      return;
    }

    const newFlight: CreateFlightModel = {
      origin: this.flightOrigin,
      destination: this.flightDestination,
      flightNumber: this.flightNumber
    };

    const response = await this.flightService.createFlight(newFlight);

    if (response == null || !response.success)
      return;


    await this.delay(1000);
    
    this.eventService.flightCreated(response.id);
    this.notificationService.success(`Flight '${this.flightNumber}' was created`);
    this.clearInput();
  }

  public toggleCreateFlight(): void {
    this.isVisible = !this.isVisible;
  }

  private clearInput(): void {
    this.flightNumber = "";
    this.flightOrigin = "";
    this.flightDestination = "";
  }

  private validateInput(): boolean {
    if (!this.flightNumber || this.flightNumber === "")
      return false;
    if (!this.flightOrigin || this.flightOrigin === "")
      return false;
    if (!this.flightDestination || this.flightDestination === "")
      return false;

    return true;
  }

  private delay(ms: number): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
}
