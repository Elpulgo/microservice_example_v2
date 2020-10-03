import { Component, Input, OnInit } from '@angular/core';
import { Passenger } from 'src/app/passengers/passenger/passenger';
import { Flight } from '../flight/flight';

@Component({
  selector: 'app-flight-list',
  templateUrl: './flight-list.component.html',
  styleUrls: ['./flight-list.component.scss']
})

export class FlightListComponent implements OnInit {
  @Input() flights: Flight[];

  public passengers: Passenger[] = [];

  constructor() { }

  ngOnInit(): void {
    for (var i = 0; i < 4; i++) {
      this.passengers.push({ name: `Oscar - ${i}`, status: i });
    }
  }
}
