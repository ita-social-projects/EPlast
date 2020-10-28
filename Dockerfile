FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgdiplus libc6-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder
ARG Configuration=debug
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgdiplus libc6-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /EPlast

COPY ./EPlast/*.sln ./
COPY ./EPlast/EPlast/*.csproj ./EPlast/
COPY ./EPlast/EPlast.AutomatedTest/*.csproj ./EPlast.AutomatedTest/
COPY ./EPlast/EPlast.BLL/*.csproj ./EPlast.BLL/
COPY ./EPlast/EPlast.DataAccess/*.csproj ./EPlast.DataAccess/
COPY ./EPlast/EPlast.Resources/*.csproj ./EPlast.Resources/
COPY ./EPlast/EPlast.Tests/*.csproj ./EPlast.Tests/
COPY ./EPlast/EPlast.WebApi/*.csproj ./EPlast.WebApi/
COPY ./EPlast/EPlast.XUnitTest/*.csproj ./EPlast.XUnitTest/

RUN dotnet restore

COPY ./EPlast/ ./

WORKDIR /EPlast
RUN dotnet build -c $Configuration -o /app

FROM builder AS publish
ARG Configuration=debug
RUN dotnet publish -c $Configuration -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EPlast.WebApi.dll"]
