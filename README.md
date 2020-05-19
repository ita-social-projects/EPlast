![ASP.NET Core CI](https://github.com/IrynaZavushchak/EPlast/workflows/ASP.NET%20Core%20CI/badge.svg)

# EPlast
<img src="https://github.com/IrynaZavushchak/EPlast/blob/master/EPlast/EPlast/wwwroot/images/homepage/ePlastLogotype.png"  width="150" height="150" />

# 1. About the project
**EPlast** is project to provide web-based multi-user system solution for digitize and automate Plast organization. The system makes it easy to access Plast's internal data, such as events, reports and document, etc. Information and location of their affiliates and clubs throughout Ukraine. Any interested person can join this system or become a full member of Plast.

# 2. Main parts of the project and technologies.
This project contains  4 main parts: client frontend, backend, and database. All parts of the project are at **https://github.com/IrynaZavushchak/EPlast**

Let's take a closer look at the technologies of each part.

**Client Frontend** - using React, Bootstrap 4

**Backend** - using ASP.NET Core.

**Database** -  Microsoft SQL Server and hosted on Microsoft Azure.

# 3. How to contribute
You're encouraged to contribute to our project if you've found any issues or missing functionality that you would want to see.  [**Here**](https://github.com/IrynaZavushchak/EPlast/issues) you can see the list of issues and [**here**](https://github.com/IrynaZavushchak/EPlast/issues/new) you can create a new issue.

Before sending any pull request, please discuss requirements/changes to be implemented using an existing issue or by creating a new one. All pull requests should be done into the development branch.

# 4. How to start the project locally.
4.1. Clone or download the project from **https://github.com/IrynaZavushchak/EPlast.git**

4.2 Install [ASP.NET Core Runtime 2.2.0](https://dotnet.microsoft.com/download/dotnet-core/2.2) and [ASP.NET Core SDK 2.2.100](https://dotnet.microsoft.com/download/dotnet-core/2.2)

4.3 Install [Microsoft SQL Server 2017+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

4.4 Install [Node.js v10.19.0](https://nodejs.org/en/blog/release/v10.19.0/)

4.5 Create local database from EPlast.DataAccess migrations.

(Open Package Manager Console, change default project to "EPlast.DataAccess" and write "Update-Database")

# 5. Project Deploy

Our project deployed at **http://eplast.azurewebsites.net/**
