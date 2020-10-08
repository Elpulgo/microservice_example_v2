import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { filter } from 'rxjs/operators';

import { Notification, NotificationType } from './notification.model';

@Injectable({ providedIn: 'root' })
export class NotificationService {

    private _subject = new Subject<Notification>();
    private _defaultId = 'default-notification';

    public onNotification(id = this._defaultId): Observable<Notification> {
        return this._subject.asObservable().pipe(filter(x => x && x.id === id));
    }

    public success(message: string, options?: any) {
        this.notify(new Notification({ ...options, type: NotificationType.Success, message }));
    }

    public error(message: string, options?: any) {
        this.notify(new Notification({ ...options, type: NotificationType.Error, message }));
    }

    public info(message: string, options?: any) {
        this.notify(new Notification({ ...options, type: NotificationType.Info, message }));
    }

    public warn(message: string, options?: any) {
        this.notify(new Notification({ ...options, type: NotificationType.Warning, message }));
    }

    public clear(id = this._defaultId) {
        this._subject.next(new Notification({ id }));
    }

    private notify(notification: Notification) {
        notification.autoClose = true;
        notification.id = notification.id || this._defaultId;
        this._subject.next(notification);
    }
}