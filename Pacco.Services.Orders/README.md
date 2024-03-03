![Pacco](https://raw.githubusercontent.com/devmentors/Pacco/master/assets/pacco_logo.png)

**What is Pacco?**
----------------

Pacco is an open source project using microservices architecture written in .NET Core 3.1 and the domain tackles the exclusive parcels delivery which revolves around the general concept of limited resources availability. To read more about this project [click here](https://github.com/devmentors/Pacco).

**What is Pacco.Services.Orders?**
----------------

Pacco.Services.Orders is the microservice being part of [Pacco](https://github.com/devmentors/Pacco) solution.

|Branch             |Build status                                                  
|-------------------|-----------------------------------------------------
|master             |[![master branch build status](https://api.travis-ci.org/devmentors/Pacco.Services.Orders.svg?branch=master)](https://travis-ci.org/devmentors/Pacco.Services.Orders)
|develop            |[![develop branch build status](https://api.travis-ci.org/devmentors/Pacco.Services.Orders.svg?branch=develop)](https://travis-ci.org/devmentors/Pacco.Services.Orders/branches)

**How to start the application?**
----------------

Service can be started locally via `dotnet run` command (executed in the `/src/Pacco.Services.Orders` directory) or by running `./scripts/start.sh` shell script in the root folder of repository.

By default, the service will be available under http://localhost:5006.

You can also start the service via Docker, either by building a local Dockerfile: 

`docker build -t pacco.services.orders .` 

or using the official one: 

`docker pull devmentors/pacco.services.orders`

**What HTTP requests can be sent to the microservice API?**
----------------

You can find the list of all HTTP requests in [Pacco.Services.Orders.rest](https://github.com/devmentors/Pacco.Services.Orders/blob/master/Pacco.Services.Orders.rest) file placed in the root folder of the repository.
This file is compatible with [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) plugin for [Visual Studio Code](https://code.visualstudio.com). 