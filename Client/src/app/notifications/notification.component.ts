import { ClassField } from '@angular/compiler';
// Courtesy of https://angular-10-alerts.stackblitz.io

import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Subscription } from 'rxjs';

import { Notification, NotificationType } from './notification.model';
import { NotificationService } from './notification.service';

@Component({
    selector: 'notification',
    templateUrl: 'notification.component.html',
  styleUrls: ['./notification.component.scss']

})
export class NotificationComponent implements OnInit, OnDestroy {
    @Input() id = 'default-notification';
    @Input() fade = true;

    public notifications: Notification[] = [];
    private _notificationSubscription: Subscription;

    constructor(
        private notificationService: NotificationService) { }

    public ngOnInit() {
        this._notificationSubscription = this.notificationService
            .onNotification(this.id)
            .subscribe(notification => {
                if (!notification.message) {
                    this.notifications = this.notifications.filter(x => x.keepAfterRouteChange);
                    this.notifications.forEach(x => delete x.keepAfterRouteChange);
                    return;
                }

                this.notifications.push(notification);

                if (notification.autoClose) {
                    setTimeout(() => this.removeNotification(notification), 3000);
                }
            });
    }

    public ngOnDestroy() {
        this._notificationSubscription.unsubscribe();
    }

    public removeNotification(notification: Notification): void {
        if (!this.notifications.includes(notification))
            return;

        if (this.fade) {
            this.notifications.find(x => x === notification).fade = true;
            setTimeout(() => {
                this.notifications = this.notifications.filter(x => x !== notification);
            }, 250);
        } else {
            this.notifications = this.notifications.filter(x => x !== notification);
        }
    }

    public cssClass(notification: Notification): string {
        if (!notification)
            return;

        const classes = ['alert', 'alert-dismissable'];

        const notificationTypeClass = {
            [NotificationType.Success]: 'alert alert-success',
            [NotificationType.Error]: 'alert alert-danger',
            [NotificationType.Info]: 'alert alert-info',
            [NotificationType.Warning]: 'alert alert-warning'
        }

        classes.push(notificationTypeClass[notification.type]);

        if (notification.fade) {
            classes.push('fade');
        }

        return classes.join(' ');
    }
}