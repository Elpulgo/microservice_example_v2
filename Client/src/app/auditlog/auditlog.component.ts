import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subscription, timer } from 'rxjs';
import { AuditLog } from './models/auditlog';
import { AuditlogService } from './services/auditlog.service';

@Component({
  selector: 'app-auditlog',
  templateUrl: './auditlog.component.html',
  styleUrls: ['./auditlog.component.scss']
})
export class AuditlogComponent implements OnInit, OnDestroy {

  public auditLogs: AuditLog[] = [];

  private _timerSubscription: Subscription;

  constructor(private auditlogService: AuditlogService) { }

  async ngOnInit(): Promise<void> {
    await this.loadAuditLogs();
    this.onTimer();
  }

  ngOnDestroy(): void {
    this._timerSubscription.unsubscribe();
  }

  private async loadAuditLogs(): Promise<void> {
    const auditLogs = await this.auditlogService.getAllAuditLogs();
    if (!auditLogs)
      return;

    this.auditLogs = auditLogs.reverse();
  }

  private onTimer() {
    this._timerSubscription = timer(0, 3000)
      .subscribe(async (tick: number) => {
        await this.loadAuditLogs();
      });
  }
}
