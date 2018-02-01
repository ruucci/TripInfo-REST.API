# TripInfo-REST.API
ASP.NET Core 2.0 API with some amazing plug-ins like Automapper, Sendgrid, Swagger, and Nlog
## List of all the endpoints and documentation on how to access them:
http://tripinfo-rest-api.azurewebsites.net/swagger
## Live demo of the project
http://tripinfo-rest-api.azurewebsites.net
## Instructions on how to use this project:
1. Make sure you have Visual Studio 2017 / Code and ASP.NET Core 2.0
2. Make sure you have a SQL Server database integration, I'm using Azure cloud SQL db. All you need is a connection String.
3. A SendGrid account would be nice, it's free and easy to use. You might wanna try. :)
4. Uncomment the line ```tripContext.EnsureSeedDataForContext();```from the Startup.cs class. This will make sure that you have some data on your first run. You may uncomment it later.
5. Add an appSettings.json file with the following fields:
```
{
  "connectionStrings": {
    "tripDBConnectionString": "Server=Your_SQL_Server_Connection_string;"
  },
  "mailSettings": {
    "mailToAddress": "youraddress@anydomain.com",
    "mailFromDeveloperAddress": "dev@yourwebsite.com",
    "mailFromProductionAddress": "prod@yourwebsite.com",
    "azure-username": "Azure_Username_OR_SENGRID_Username",
    "azure-password": "WhateverYouEnteredAbove'sPassword"
  },
  "AppSettings": {
    "Secret": "Any_Key_that_is_fairly_longggggggggggggggggggggg"
  }
}
```
