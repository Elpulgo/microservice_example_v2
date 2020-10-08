import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { NotificationService } from '../../notifications/notification.service';
import { AUDITLOG_ALL_ENDPOINT, AUDITLOG_BASE_URL } from './endpoint-constants';
import { AuditLog } from '../models/auditlog';

@Injectable({
    providedIn: 'root'
})
export class AuditlogService {

    constructor(
        private httpClient: HttpClient,
        private notificationService: NotificationService) {
    }

    async getAllAuditLogs(): Promise<AuditLog[]> {
        try {
            return await this.httpClient.get<AuditLog[]>(`${AUDITLOG_BASE_URL}/${AUDITLOG_ALL_ENDPOINT}`).toPromise();
        } catch (error) {
            this.notificationService.error(`Failed to load auditlog! Check logs for further info.`);
            console.log(`Failed to load auditlog: '${error}'`);
            return null;
        }
    }
}
