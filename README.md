![ASP.NET Core CI](https://github.com/ITA-Social-Projects/EPlast/workflows/ASP.NET%20Core%20CI/badge.svg)[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ita-social-projects-eplast&metric=alert_status)](https://sonarcloud.io/dashboard?id=ita-social-projects-eplast) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ita-social-projects-eplast&metric=coverage)](https://sonarcloud.io/dashboard?id=ita-social-projects-eplast)[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=ita-social-projects-eplast&metric=bugs)](https://sonarcloud.io/dashboard?id=ita-social-projects-eplast)[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=ita-social-projects-eplast&metric=code_smells)](https://sonarcloud.io/dashboard?id=ita-social-projects-eplast)[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=ita-social-projects-eplast&metric=security_rating)](https://sonarcloud.io/dashboard?id=ita-social-projects-eplast)

# EPlast
<img src="https://github.com/ITA-Social-Projects/EPlast/blob/master/ePlastLogotype.png"  width="150" height="150" />

# 1. About the project
**EPlast** is project to provide web-based multi-user system solution for digitize and automate Plast organization. The system makes it easy to access Plast's internal data, such as events, reports and document, etc. Information and location of their affiliates and clubs throughout Ukraine. Any interested person can join this system or become a full member of Plast.

# 2. Main parts of the project and technologies.
This project contains  4 main parts: client frontend, backend, and database. All parts of the project are at **https://github.com/ITA-Social-Projects/EPlast**

Let's take a closer look at the technologies of each part.

**Client Frontend** - You can see [here](https://github.com/ITA-Social-Projects/EPlast-Client/). We using React, Ant Design

**Backend** - ASP.NET Core 3.1.0

**Database** -  Microsoft SQL Server and hosted on Microsoft Azure.

**Code quality** - [SonarCloud](https://sonarcloud.io/dashboard?id=ita-social-projects-eplast)   

**Testing** - xUnit, NUnit, Selenium, Moq

**SDLC** - Scrum/Kanban  

# 3. How to contribute
You're encouraged to contribute to our project if you've found any issues or missing functionality that you would want to see.  [**Here**](https://github.com/ITA-Social-Projects/EPlast/issues) you can see the list of issues and [**here**](https://github.com/ITA-Social-Projects/EPlast/issues/new) you can create a new issue.

Before sending any pull request, please discuss requirements/changes to be implemented using an existing issue or by creating a new one. All pull requests should be done into the development branch.

# 4. How to start the project locally.
4.1. Clone or download the project from **https://github.com/ITA-Social-Projects/EPlast.git**

4.2 Install [ASP.NET Core Runtime 3.1.0](https://dotnet.microsoft.com/download/dotnet-core/3.1) and [ASP.NET Core SDK 3.1.0](https://dotnet.microsoft.com/download/dotnet-core/3.1)

4.3 Install [Microsoft SQL Server 2017+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

4.4 Install [Node.js v10.19.0](https://nodejs.org/en/blog/release/v10.19.0/)+

4.5 Setup user secrets (IT Academy students can get it from our Google Drive)

(Right click on Eplast.WebApi project, select "Manage User Secrets", copy and paste data from User Secrets file. Don't forget to write your local database connection string into User Secrets file)

4.6 Create local database from EPlast.DataAccess migrations.

(Open Package Manager Console, change default project to "EPlast.DataAccess"(make sure that Solution Explorer default project EPlast.WebApi) and write "Update-Database")

4.7 Run project and make sure that data is written to the table [AspNetRoles]

# 5. How to run the project with docker-compose locally.

The instructions below will allow you to run application locally in the containers for developing and testing purpose. 

5.1. Installation Prerequisites:<br/>
     -  [Docker](https://www.docker.com) version 17.05 or higher, [Docker Compose] (https://docs.docker.com/compose). If you are running Microsoft Windows family OS, it is better to use [docker-desktop](https://www.docker.com/products/docker-desktop). How to [set up](https://drive.google.com/file/d/1K55JtMZ_--rkW1Xk9easkz8RUW3AZCUp/view) a docker.
     - [Microsoft SQL Server 2017+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or access to cloud MSSQL server with deployed database.     
     - Access to the Azure Storage.
     
5.2. Clone repository from GitHub with:<br/>
     - git clone https://github.com/ITA-Social-Projects/EPlast.git     
     - git clone https://github.com/ITA-Social-Projects/EPlast-Client.git
     
5.3. Go to the EPlast directory and create the environment file, named .env with following parameters:<br/>
       ConnectionStrings__EPlastDBConnection={Full connection string to the MSSQL}       
       StorageConnectionString={Full connection string to the Azure storage}
       
5.4. Edit file EPlast-Client/src/config.ts with the next line: const BASE_URL = "http://localhost:5000/api/";

5.5. Run the FronEnd and BackEnd of the application by executing the "docker-compose up" command.

5.6. Now you can access your application at http://localhost.

# 6. Project Deploy

Our project deployed at **https://eplast.westeurope.cloudapp.azure.com/**
