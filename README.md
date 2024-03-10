## Student life activity app - Mini Space

## General description
- [ ] Multitier application built with [.NET](https://dotnet.microsoft.com/en-us)
- [ ] Targets to create web interface for students to browse events and sign up for them, create own activities and interact with other students
- [ ] Focuses on cultural, educational and social events available for academic community

## Frontend
- [ ] Created with [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) - single-page web application framework being a part of the .NET ecosystem
- [ ] Written in C#, can run in the browser by using [WebAssembly](https://webassembly.org)
- [ ] Runs in the same security sandbox as JavaScript frameworks like Angular, React or Vue

## Backend
- [ ] Written in C#, created with [Convey](https://github.com/snatch-dev/Convey) - lightweight set of libraries for building .NET microservices
- [ ] Based on concept of microservice architecture
- [ ] CQRS pattern - using a different model to update data than the model to read data
- [ ] Will be composed of the following services:
    - [ ] Identity Service
    - [ ] User Service
    - [ ] Event Service
    - [ ] Events Service
    - [ ] Reports Service
    - [ ] Notifications Service
    - [ ] Friend Service
    - [ ] Post Service
    - [ ] Organizer Service
- [ ] API.Gateway created with [Ntrada](https://github.com/snatch-dev/Ntrada) to clip endpoints from microservices to be accessible by one port

## Infrastructure
- [ ] [MongoDB](https://www.mongodb.com/products/platform/cloud) - document-oriented database
- [ ] [Consul](https://www.consul.io) - microservices discovery
- [ ] [RabbitMQ](https://www.rabbitmq.com) - message broker
- [ ] [Fabio](https://github.com/fabiolb/fabio) - load balancing
- [ ] [Jaeger](https://www.jaegertracing.io) - distributed tracing
- [ ] [Grafana](https://grafana.com) - metrics extension
- [ ] [Prometheus](https://prometheus.io) - metrics extension
- [ ] [Seq](https://datalust.co/seq) - logging extension
- [ ] [Vault](https://www.vaultproject.io) - secrets extension