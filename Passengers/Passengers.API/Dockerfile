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
COPY ./Shared/Shared.Proto ./Shared/Shared.Proto

# RUN apk update \
#     && apk --no-cache add libc6-compat \
#     && apk --no-cache add protobuf \
#     && cd /root/.nuget/packages/grpc.tools/1.19.0/tools/linux_x64 \
#     && rm protoc \
#     && ln -s /usr/bin/protoc protoc \
#     && ln -s /lib/libc.musl-x86_64.so.1 /lib/ld-linux-x86-64.so.2 \
#     && chmod +x grpc_csharp_plugin

ENV GLIBC_REPO=https://github.com/sgerrand/alpine-pkg-glibc
ENV GLIBC_VERSION=2.28-r0

RUN set -ex && \
    apk --update add libstdc++ curl ca-certificates && \
    for pkg in glibc-${GLIBC_VERSION} glibc-bin-${GLIBC_VERSION}; \
        do curl -sSL ${GLIBC_REPO}/releases/download/${GLIBC_VERSION}/${pkg}.apk -o /tmp/${pkg}.apk; done && \
    apk add --allow-untrusted /tmp/*.apk && \
    rm -v /tmp/*.apk && \
    /usr/glibc-compat/sbin/ldconfig /lib /usr/glibc-compat/lib

RUN dotnet publish ./Passengers/Passengers.API/Passengers.API.csproj --runtime linux-musl-x64 -c $BUILDCONFIG -o out /p:Version=$VERSION -p:PublishTrimmed=true

FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1-alpine
WORKDIR /app
COPY --from=base /src/out ./

EXPOSE 8050
ENTRYPOINT ["./Passengers.API"]