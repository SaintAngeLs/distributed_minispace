# MiniSpace - Student Life Activity App

[![codecov](https://codecov.io/gh/SaintAngeLs/distributed_minispace/graph/badge.svg?token=SW3T9CN2QS)](https://codecov.io/gh/SaintAngeLs/distributed_minispace)

MiniSpace is a multifaceted application designed to enhance student life by providing a robust platform where students can explore, register, and participate in various events. These events range from cultural to educational and social, aiming to enrich the academic community's vibrant life.

![Home](images/minispace-home.png)
![Events Dashboard View](images/minispace-events.png)

## Table of Contents
- [Overview](#overview)
- [Features and Functionalities](#features-and-functionalities)
  - [For Students](#for-students)
  - [For Event Organizers](#for-event-organizers)
  - [For Friends and Social Connections](#for-friends-and-social-connections)
  - [For System Administrators](#for-system-administrators)
- [Use Cases](#use-cases)
- [Frontend](#frontend)
- [Backend](#backend)
  - [Identity Service](#identity-service)
  - [Students Service](#students-service)
  - [Events Service](#events-service)
  - [Posts Service](#posts-service)
  - [Friends Service](#friends-service)
  - [Comments Service](#comments-service)
  - [Reactions Service](#reactions-service)
  - [Organizations Service](#organizations-service)
  - [MediaFiles Service](#mediafiles-service)
  - [Reports Service](#reports-service)
  - [Notifications Service](#notifications-service)
  - [Email Service](#email-service)
  - [API Gateway](#api-gateway)
- [Infrastructure](#infrastructure)
- [Code Coverage](#code-coverage)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)
- [Contributors](#contributors)

## Overview
MiniSpace is transitioning from a university-focused application to a comprehensive real-life social application. Initially built to serve student services at a university, MiniSpace is currently undergoing a major refactor to broaden its scope and enhance its functionalities. The system now includes a comprehensive authorization system featuring 2FA email verification, JWT token generation, and password reset capabilities.

## Features and Functionalities

### For Students
- **Event Interaction**: Students can browse through available events, register to participate, and receive updates and notifications about upcoming activities.
- **Social Interaction**: The platform allows students to connect with friends, share event experiences, and interact through posts and discussions.
- **Administration Interaction**: Students can report issues directly through the app, ensuring a seamless and user-friendly experience.

### For Event Organizers
- **Event Management**: Organizers can create and manage events, including setting details like location, time, and description.
- **Participant Engagement**: Tools to communicate with participants, manage attendance, and gather feedback post-event to improve future events.
- **Data Analysis**: Access to real-time data analytics to monitor event success and participant engagement.

### For Friends and Social Connections
- **Enhanced Event Filters**: Friends can see which events others are attending, making the event choice more social.
- **Invitations**: Ability to send and receive invitations to events, enhancing the social experience.

### For System Administrators
- **User and System Oversight**: Admins have the tools to manage user issues and system functionality, ensuring smooth operation.
- **Content Moderation**: Capabilities to monitor and manage the content to maintain a respectful and constructive community environment.

## Use Cases
- **Events**: From browsing to attending and reviewing events.
- **Social Interactions**: Managing friends lists, sending invitations, and sharing experiences.
- **Administration**: Handling user reports, system updates, and data analysis for continuous improvement.

## Frontend
- **Framework**: [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) - A single-page web application framework within the .NET ecosystem.
- **Components**: [MudBlazor](https://mudblazor.com/) - Enhance the aesthetics and responsiveness of the web application.

## Backend
- **Framework**: Built using [Convey](https://github.com/snatch-dev/Convey), optimized for .NET microservices.
- **Architecture**: Microservice architecture with a focus on CQRS pattern for performance and scalability.
- **Services**:
  ### Identity Service
  Manages user authentication and authorization, including functionalities like user registration, login, 2FA email verification, JWT token generation, and password reset.

  ### Students Service
  Handles user information and settings preferences, providing a centralized service for managing student profiles and related data.

  ### Events Service
  Manages the creation, updating, and deletion of events. This includes setting event details such as location, time, and description.

  ### Posts Service
  Allows users and organizations to create and manage posts. This service supports the sharing of information, announcements, and updates within the community.

  ### Friends Service
  Manages connections between users, allowing them to add friends, view friend lists, and interact socially within the platform.

  ### Comments Service
  Enables users to comment on events, posts, and organizational activities, fostering interaction and engagement.

  ### Reactions Service
  Allows users to react to posts, comments, and events, providing a way to express opinions and feedback.

  ### Organizations Service
  Manages the creation and administration of organizations and sub-organizations within the platform, enabling structured group activities and management.

  ### MediaFiles Service
  Handles the management and storage of media files uploaded by users and organizations, ensuring efficient and secure media operations.

  ### Reports Service
  Manages the justification inappropriate cases and generation with administration of various reports, providing insights and analytics to users and administrators.

  ### Notifications Service
  Provides real-time notifications to users using SignalR, ensuring timely updates about events, posts, and other activities.

  ### Email Service
  Configured to handle email communications, including notifications, event updates, and user communications through the event bus of the notification service.

  ### API Gateway
  Uses [Ntrada](https://github.com/snatch-dev/Ntrada) for routing and managing requests across multiple services through a single entry point.

## Infrastructure
- [**MongoDB**](https://www.mongodb.com/products/platform/cloud) - Document-oriented database.
- [**Consul**](https://www.consul.io) - Microservices discovery.
- [**RabbitMQ**](https://www.rabbitmq.com) - Message broker.
- [**Fabio**](https://github.com/fabiolb/fabio) - Load balancing.
- [**Jaeger**](https://www.jaegertracing.io) - Distributed tracing.
- [**Grafana**](https://grafana.com) - Metrics extension.
- [**Prometheus**](https://prometheus.io) - Metrics extension.
- [**Seq**](https://datalust.co/seq) - Logging extension.
- [**Vault**](https://www.vaultproject.io) - Secrets management.

## Code Coverage
Stay informed about the code quality with our integrated Codecov badge that displays the current code coverage percentage:
[![codecov](https://codecov.io/gh/SaintAngeLs/distributed_minispace/graph/badge.svg?token=SW3T9CN2QS)](https://codecov.io/gh/SaintAngeLs/distributed_minispace)

## Contributing
Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

Please refer to [CONTRIBUTING.md](./CONTRIBUTING.md) for more details.

## License
Distributed under the Apache License. See [LICENSE](./LICENSE) for more information.

## Contact
- **Project Link**: [MiniSpace GitHub Repository](https://github.com/SaintAngeLs/distributed_minispace)
- **Live Demo**: [MiniSpace Live Demo](http://minispace.itsharppro.com)

## Contributors
MiniSpace is made possible thanks to the contributions of several individuals. Here is a list of the remarkable people who have contributed to this project:
- **@eggwhat**
- **@an2508374**
- **@olegkiprik**
- **@zniwiarzxxx**

Thank you for considering MiniSpace for your academic community engagement needs!