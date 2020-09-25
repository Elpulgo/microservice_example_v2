FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS base

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

WORKDIR /src

### Copy flight csproj
COPY ./Flights/Flights.API/Flights.API.csproj ./Flights/Flights.API/Flights.API.csproj 
COPY ./Flights/Flights.Infrastructure/Flights.Infrastructure.csproj ./Flights/Flights.Infrastructure/Flights.Infrastructure.csproj 
COPY ./Flights/Flights.Core/Flights.Core.csproj ./Flights/Flights.Core/Flights.Core.csproj 
COPY ./Flights/Flights.Application/Flights.Application.csproj ./Flights/Flights.Application/Flights.Application.csproj 

### Copy shared csproj
COPY ./Shared/Shared.Core/Shared.Core.csproj ./Shared/Shared.Core/Shared.Core.csproj
COPY ./Shared/Shared.Infrastructure/Shared.Infrastructure.csproj ./Shared/Shared.Infrastructure/Shared.Infrastructure.csproj

RUN dotnet restore ./Flights/Flights.API/Flights.API.csproj

### Copy flight files
COPY ./Flights/Flights.API ./Flights/Flights.API
COPY ./Flights/Flights.Infrastructure ./Flights/Flights.Infrastructure 
COPY ./Flights/Flights.Core ./Flights/Flights.Core
COPY ./Flights/Flights.Application ./Flights/Flights.Application 

### Copy shared files
COPY ./Shared/Shared.Core ./Shared/Shared.Core
COPY ./Shared/Shared.Infrastructure ./Shared/Shared.Infrastructure

RUN dotnet publish ./Flights/Flights.API/Flights.API.csproj --runtime linux-musl-x64 -c $BUILDCONFIG -o out /p:Version=$VERSION -p:PublishTrimmed=true

FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1-alpine
WORKDIR /app
COPY --from=base /src/out ./

EXPOSE 8040
EXPOSE 8041
ENTRYPOINT ["./Flights.API"]