## Dependencies
* NSwag.AspNetCore :
Source : (docs microsoft)[https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-nswag?view=aspnetcore-2.2&tabs=visual-studio%2Cvisual-studio-xml]	


## Execute docker compose manually
* Start :
```cmd
cd D:\devel\github\microsvc\src\BuildingBlocks\ServiceManagement
docker-compose  -f "D:\devel\github\microsvc\src\BuildingBlocks\ServiceManagement\docker-compose.yml" -f "D:\devel\github\microsvc\src\BuildingBlocks\ServiceManagement\docker-compose.override.yml" -p dockercompose15096720486911126714 --no-ansi up -d

docker-compose logs -f --tail="all" dockercompose15096720486911126714
```

* Stop :
```cmd
cd D:\devel\github\microsvc\src\BuildingBlocks\ServiceManagement
docker-compose  -f "D:\devel\github\microsvc\src\BuildingBlocks\ServiceManagement\docker-compose.yml" -f "D:\devel\github\microsvc\src\BuildingBlocks\ServiceManagement\docker-compose.override.yml" -p dockercompose15096720486911126714 down
```

docker volume create --driver local --opt device=d:/devel/temp/sql_db_docker/service_management sql_data_service_management
docker volume inspect sql_data_service_management

docker run --rm -e "ACCEPT_EULA=Y" -e "Pass@word.1" -p 1433:1433 --name sqllinux01 -v D:\devel\temp\sql_db_docker\service_management:/var/opt/mssql -d mcr.microsoft.com/mssql/server:2017-latest