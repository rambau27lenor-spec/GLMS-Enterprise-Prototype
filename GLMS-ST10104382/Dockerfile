FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["GLMS-ST10104382.csproj", "."]
RUN dotnet restore "GLMS-ST10104382.csproj"

COPY . .
RUN dotnet publish "GLMS-ST10104382.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "GLMS-ST10104382.dll"]