# See https://aka.ms/containerimagehelp for information on how to use Windows Server 1709 containers with Service Fabric.
# FROM microsoft/dotnet:2.2-runtime-nanoserver-1709
FROM microsoft/dotnet:2.2-runtime-nanoserver-sac2016
ARG source
WORKDIR /app
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "MESF.Core.ServiceManagement.Tests.dll"]
