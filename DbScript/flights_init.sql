CREATE TABLE IF NOT EXISTS Flights ( 
id              UUID                         NOT NULL,
destination     VARCHAR(255)                 NOT NULL,
origin          VARCHAR(255)                 NOT NULL,
status          INT                          NOT NULL,
flight_number    VARCHAR(255)                 NOT NULL
);