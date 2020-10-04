import { Component, Input, OnInit } from '@angular/core';
import { Passenger } from '../models/passenger';

@Component({
  selector: 'app-passenger-list',
  templateUrl: './passenger-list.component.html',
  styleUrls: ['./passenger-list.component.scss']
})
export class PassengerListComponent implements OnInit {

  @Input() passengers: Passenger[];

  constructor() { }

  ngOnInit(): void {
  }

  public createPassenger(): void {

  }

}
