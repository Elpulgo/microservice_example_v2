namespace Flights.Core
{
    public enum FlightStatus
    {
        None = 0,
        WaitingForBoarding = 1,
        Boarded = 2,
        OnRoute = 3,
        Landed = 4,
        PassengersDisembarked = 5
    }
}