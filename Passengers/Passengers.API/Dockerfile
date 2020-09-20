FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS base

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

WORKDIR /src

### Copy Passengers csproj
COPY ./Passengers/Passengers.API/Passengers.API.csproj ./Passengers/Passengers.API/Passengers.API.csproj 
COPY ./Passengers/Passengers.Infrastructure/Passengers.Infrastructure.csproj ./Passengers/Passengers.Infrastructure/Passengers.Infrastructure.csproj 
COPY ./Passengers/Passengers.Core/Passengers.Core.csproj ./Passengers/Passengers.Core/Passengers.Core.csproj 
COPY ./Passengers/Passengers.Application/Passengers.Application.csproj ./Passengers/Passengers.Application/Passengers.Application.csproj 

### Copy shared csproj
COPY ./Shared/Shared.Core/Shared.Core.csproj ./Shared/Shared.Core/Shared.Core.csproj
COPY ./Shared/Shared.Infrastructure/Shared.Infrastructure.csproj ./Shared/Shared.Infrastructure/Shared.Infrastructure.csproj

RUN dotnet restore ./Passengers/Passengers.API/Passengers.API.csproj

### Copy Passengers files
COPY ./Passengers/Passengers.API ./Passengers/Passengers.API
COPY ./Passengers/Passengers.Infrastructure ./Passengers/Passengers.Infrastructure 
COPY ./Passengers/Passengers.Core ./Passengers/Passengers.Core
COPY ./Passengers/Passengers.Application ./Passengers/Passengers.Application 

### Copy shared files
COPY ./Shared/Shared.Core ./Shared/Shared.Core
COPY ./Shared/Shared.Infrastructure ./Shared/Shared.Infrastructure

RUN dotnet publish ./Passengers/Passengers.API/Passengers.API.csproj --runtime linux-musl-x64 -c $BUILDCONFIG -o out /p:Version=$VERSION -p:PublishTrimmed=true

FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1-alpine
WORKDIR /app
COPY --from=base /src/out ./

EXPOSE 8050
ENTRYPOINT ["./Passengers.API"]