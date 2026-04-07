# BlogAPI
Minimal API with authentication and blog posting
Change default jwt secret key: [appsettings.json](https://github.com/joaostack/BlogAPI/blob/main/back-end/appsettings.json)

Start migrations:
```
dotnet-ef migrations add Initial
```

Update
```
dotnet-ef database update
```

Doc in `/swagger` (run in development mode)
