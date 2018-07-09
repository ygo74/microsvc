FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR .
COPY samples/SampleServiceManagement.AspNetCore/SampleServiceManagement.AspNetCore.csproj samples/SampleServiceManagement.AspNetCore/
COPY src/MESF.Core.ServiceManagement/MESF.Core.ServiceManagement.csproj src/MESF.Core.ServiceManagement/
RUN dotnet restore samples/SampleServiceManagement.AspNetCore/SampleServiceManagement.AspNetCore.csproj
COPY . .
WORKDIR samples/SampleServiceManagement.AspNetCore
RUN dotnet build SampleServiceManagement.AspNetCore.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SampleServiceManagement.AspNetCore.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SampleServiceManagement.AspNetCore.dll"]
