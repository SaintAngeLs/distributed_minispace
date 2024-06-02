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
- **Framework**: Backend services are built using [Convey](https://github.com/snatch-dev/Convey), a set of libraries optimized for building .NET microservices.
- **Architecture**: Implements a microservice architecture with an emphasis on the CQRS pattern, separating read operations from update operations to enhance performance and scalability.
- **Services**:
  - Identity Service
  - User Service
  - Event Service
  - Reports Service
  - Notifications Service
  - Friend Service
  - Post Service
  - Organizer Service
  - Media Files Service
  - Comments Service
  - Reactions Service
  - Students Service
  - Organizations Service
- **API Gateway**: Utilizes [Ntrada](https://github.com/snatch-dev/Ntrada) for routing and managing requests across multiple services through a single entry point.


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


## Code Coverage
Stay informed about the code quality with our integrated Codecov badge that displays the current code coverage percentage:
[![codecov](https://codecov.io/gh/SaintAngeLs/distributed_minispace/graph/badge.svg?token=SW3T9CN2QS)](https://codecov.io/gh/SaintAngeLs/distributed_minispace)

## Contributing
Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

Please refer to [CONTRIBUTING.md](./CONTRIBUTING.md) for more details.

## License
Distributed under the MIT License. See [LICENSE](./LICENSE) for more information.

## Contact
- Project Link: [https://github.com/SaintAngeLs/distributed_minispace](https://github.com/SaintAngeLs/distributed_minispace)
- Live Demo: [minispace.itsharppro.com](http://minispace.itsharppro.com)

Thank you for considering Mini Space for your academic community engagement needs!