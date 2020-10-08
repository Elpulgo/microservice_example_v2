export interface AuditLog {
    created: string;
    eventType: string;
    eventNumber: number;
    eventId: string;
    data: any;
    metaData: MetaData;
}

export interface MetaData {
    eventTypeOperation: EventTypeOperation;
    eventName: string;
    timestamp: string;
}

export enum EventTypeOperation {
    None = 0,
    Create = 1,
    Update = 2,
    Query = 3,
    Delete = 4
}