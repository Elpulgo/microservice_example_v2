FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS base

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

WORKDIR /src

### Copy passengers csproj
COPY ./Passengers/Passengers.Processor/Passengers.Processor.csproj ./Passengers/Passengers.Processor/Passengers.Processor.csproj 
COPY ./Passengers/Passengers.Infrastructure/Passengers.Infrastructure.csproj ./Passengers/Passengers.Infrastructure/Passengers.Infrastructure.csproj 
COPY ./Passengers/Passengers.Core/Passengers.Core.csproj ./Passengers/Passengers.Core/Passengers.Core.csproj 

### Copy shared csproj
COPY ./Shared/Shared.Core/Shared.Core.csproj ./Shared/Shared.Core/Shared.Core.csproj
COPY ./Shared/Shared.Infrastructure/Shared.Infrastructure.csproj ./Shared/Shared.Infrastructure/Shared.Infrastructure.csproj

RUN dotnet restore ./Passengers/Passengers.Processor/Passengers.Processor.csproj

### Copy passengers files
COPY ./Passengers/Passengers.Processor ./Passengers/Passengers.Processor
COPY ./Passengers/Passengers.Infrastructure ./Passengers/Passengers.Infrastructure 
COPY ./Passengers/Passengers.Core ./Passengers/Passengers.Core

### Copy shared files
COPY ./Shared/Shared.Core ./Shared/Shared.Core
COPY ./Shared/Shared.Infrastructure ./Shared/Shared.Infrastructure

RUN dotnet publish ./Passengers/Passengers.Processor/Passengers.Processor.csproj --runtime linux-musl-x64 -c $BUILDCONFIG -o out /p:Version=$VERSION -p:PublishTrimmed=true

FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1-alpine
WORKDIR /app
COPY --from=base /src/out ./

EXPOSE 8020
ENTRYPOINT ["./Passengers.Processor"]