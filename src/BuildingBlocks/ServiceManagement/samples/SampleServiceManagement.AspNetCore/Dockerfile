FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY samples/SampleServiceManagement.AspNetCore/SampleServiceManagement.AspNetCore.csproj samples/SampleServiceManagement.AspNetCore/
RUN dotnet restore samples/SampleServiceManagement.AspNetCore/SampleServiceManagement.AspNetCore.csproj
COPY . .
WORKDIR /src/samples/SampleServiceManagement.AspNetCore
RUN dotnet build SampleServiceManagement.AspNetCore.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SampleServiceManagement.AspNetCore.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SampleServiceManagement.AspNetCore.dll"]
