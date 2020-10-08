import { Component, OnInit } from '@angular/core';
import { AuditLog } from './models/auditlog';
import { AuditlogService } from './services/auditlog.service';

@Component({
  selector: 'app-auditlog',
  templateUrl: './auditlog.component.html',
  styleUrls: ['./auditlog.component.scss']
})
export class AuditlogComponent implements OnInit {

  public auditLogs: AuditLog[] = [];

  constructor(private auditlogService: AuditlogService) { }

  async ngOnInit(): Promise<void> {
    await this.loadAuditLogs();
  }

  private async loadAuditLogs(): Promise<void> {
    const auditLogs = await this.auditlogService.getAllAuditLogs();
    if (!auditLogs)
      return;

    this.auditLogs = auditLogs.reverse();
  }

}
