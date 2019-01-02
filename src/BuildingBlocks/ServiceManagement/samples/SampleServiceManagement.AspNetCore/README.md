## Dependencies
* NSwag.AspNetCore :
Source : (docs microsoft)[https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-nswag?view=aspnetcore-2.2&tabs=visual-studio%2Cvisual-studio-xml]	


## Execute docker compose manually
```cmd
cd D:\devel\github\microsvc\src\BuildingBlocks\ServiceManagement
docker-compose  -f "D:\devel\github\microsvc\src\BuildingBlocks\ServiceManagement\docker-compose.yml" -f "D:\devel\github\microsvc\src\BuildingBlocks\ServiceManagement\docker-compose.override.yml" -p dockercompose15096720486911126714 --no-ansi up -d
```