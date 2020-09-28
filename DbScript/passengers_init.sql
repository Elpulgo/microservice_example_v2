CREATE TABLE IF NOT EXISTS Passengers ( 
id              UUID                  UNIQUE NOT NULL,
flight_id       UUID                         NOT NULL,
name            VARCHAR(255)                 NOT NULL,
status          INT                          NOT NULL
);