import { Component, Input, OnInit } from '@angular/core';
import { Passenger } from '../models/passenger';
import { PassengerStatus } from '../models/passengerStatus';

@Component({
  selector: 'app-passenger',
  templateUrl: './passenger.component.html',
  styleUrls: ['./passenger.component.scss']
})
export class PassengerComponent implements OnInit {

  @Input() passenger: Passenger;

  constructor() { }

  ngOnInit(): void {
  }


  public displayStatus(status: PassengerStatus): string {
    return PassengerStatus[status];
  }
}
