---
topic: Cloud-Based Online Blood Bank System
languages:
  - aspx-csharp
products:
  - Azure App Service
  - Azure Web Apps
  - Azure Application registration(Unverified and Registered as SPA)
  - Azure SQL Database
---

Group ID (GID): 9ejjQqps_
Group Lead : Prithviraj K

Pre-released website : https://bloodbankloc.azurewebsites.net/

#The app is registered but yet to be verified applies the Client id is mentioned as per registeration

#The app consists of two authentication -Microsoft identity platform (or) -Default(Guest) 
	Note : The type of authentication defines the access of the user

#As the app registation is SPA which eventually means the authentication is available only to those with personal microsoft accounts.

#The project can be locally hosted can be accessed at https://localhost:44368/
	Note : Change <add key="redirectUri" value="https://bloodbankloc.azurewebsites.net/" /> in web.config to the mentioned localhost
