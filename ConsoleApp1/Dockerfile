FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY *.sln .
COPY KBIPMobileBackend.ConsoleApp1/*.csproj ./KBIPMobileBackend.ConsoleApp1/
RUN dotnet restore

COPY KBIPMobileBackend.ConsoleApp1/. ./KBIPMobileBackend.ConsoleApp1/
RUN dotnet publish -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "KBIPMobileBackend.ConsoleApp1.dll"]
