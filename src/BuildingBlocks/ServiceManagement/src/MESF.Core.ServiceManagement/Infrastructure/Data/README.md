```powershell
add-migration InitDB_v1 -project MESF.Core.ServiceManagement -StartupProject SampleServiceManagement.AspNetCore
update-database InitDB_v1 -project MESF.Core.ServiceManagement -StartupProject SampleServiceManagement.AspNetCore
```
