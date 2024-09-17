## 🚧 Project Update: Intensive Development and Large-Scale Refactoring in Progress 🚧


# MiniSpace - ✨ Student Life Activity App

[![codecov](https://codecov.io/gh/SaintAngeLs/distributed_minispace/graph/badge.svg?token=SW3T9CN2QS)](https://codecov.io/gh/SaintAngeLs/distributed_minispace)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/SaintAngeLs/distributed_minispace?color=blue&label=Version)
![GitHub](https://img.shields.io/github/license/SaintAngeLs/distributed_minispace?color=green)
![Build Status](https://img.shields.io/github/actions/workflow/status/SaintAngeLs/distributed_minispace/build_microservices.yml?branch=main)
![Containers Deploy Status](https://img.shields.io/github/actions/workflow/status/SaintAngeLs/distributed_minispace/cloud_deploy.yml?branch=main)
![Safety Status](https://img.shields.io/github/actions/workflow/status/SaintAngeLs/distributed_minispace/codeql.yml?branch=main)
![Ossar analyser Status](https://img.shields.io/github/actions/workflow/status/SaintAngeLs/distributed_minispace/ossaranalyzer.yml?branch=main)
![Gitlab Sync Status](https://img.shields.io/github/actions/workflow/status/SaintAngeLs/distributed_minispace/gitlab_sync.yml?branch=main)
![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blueviolet)

**MiniSpace** is a powerful and dynamic application designed to enhance student life by offering a comprehensive platform where students can explore, register, and actively participate in a variety of events. Whether it’s cultural, educational, or social, MiniSpace is your gateway to enriching the academic community’s vibrant life.

### General Complex System Diagram
![General Diagram for MiniSpace System](docs/complex_system_diagram.png)

## 📋 Table of Contents
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

## 🌍 Overview
**MiniSpace** is evolving from a university-centric application into a fully-fledged, real-life social networking platform. Initially designed to serve student services within a university setting, MiniSpace is undergoing a major transformation to broaden its scope and enhance its capabilities. Our system now includes a robust authorization mechanism featuring 2FA email verification, JWT token generation, and secure password reset functionalities.

## 🚀 Features and Functionalities

### 🎓 For Students
- ⏩ **Event Interaction**: Discover and register for exciting events. Stay updated with real-time notifications on upcoming activities.
- ⏩ **Social Interaction**: Connect with friends, share experiences, and engage in meaningful discussions through posts.
- ⏩ **Easy Reporting**: Seamlessly report issues through the app, ensuring a smooth and user-friendly experience.

### 🎉 For Event Organizers
- ⏩ **Event Management**: Effortlessly create and manage events with customizable details like location, time, and descriptions.
- ⏩ **Engage Participants**: Communicate with attendees, manage participation, and collect valuable feedback to enhance future events.
- ⏩ **Data-Driven Insights**: Access real-time analytics to monitor event success and participant engagement.

### 🤝 For Friends and Social Connections
- ⏩ **Social Event Filters**: See which events your friends are attending, making your choices more social and fun.
- ⏩ **Send Invitations**: Invite your friends to events and strengthen your social bonds through shared experiences.

### 🛠️ For System Administrators
- ⏩ **Comprehensive Oversight**: Manage users and system functionality with advanced tools that ensure smooth operations.
- ⏩ **Content Moderation**: Monitor and manage content to maintain a respectful and positive community environment.

## 🛠️ Container Diagram
The C4 Container Diagram below provides a high-level overview of the MiniSpace system architecture:

![Container Diagram for MiniSpace System](docs/container_diagram_for_minispace_system.png)

This diagram showcases the critical containers (e.g., microservices, databases) that make up the MiniSpace system. The `API Gateway` serves as the primary entry point for users, routing requests through the `Fabio Load Balancer` to various specialized services such as the `Comments Service`, `Events Service`, `Friends Service`, and more. Each service interacts with its dedicated database, ensuring efficient data management. Additionally, monitoring and logging services like `Jaeger`, `Seq`, `Prometheus`, and `Grafana` are integrated to ensure system health and performance.

## 🌐 Complex System Diagram
The Complex System Diagram offers a detailed view of MiniSpace's infrastructure and the interactions between various components:

![General Diagram for MiniSpace System](docs/complex_system_diagram.png)

### 🔑 Key Concepts Illustrated:
- ⏩ **Event Sourcing**: All changes within the system are recorded as a sequence of immutable events (e.g., `CommentAddedEvent`, `EmailSentEvent`). This allows the system to rebuild its state from these events, ensuring data integrity and providing a complete historical record.
  
- ⏩ **Inbox/Outbox Pattern**: This pattern ensures reliable communication between microservices. The Outbox temporarily stores events before sending them to the message broker (`RabbitMQ`), ensuring that no events are lost. The Inbox ensures that messages are processed exactly once, preventing duplication.

- ⏩ **Choreographical Saga Pattern**: To handle long-running, complex transactions across multiple microservices, MiniSpace uses the Choreographical Saga Pattern. Each service in the transaction executes its local operation and then publishes an event to trigger the next service in the sequence. This decentralized approach ensures that transactions are robust, even in the event of partial failures.

### 🛠️ Infrastructure Components:
- ☑️ **API Gateway**: The central hub for handling incoming API requests, routing them to the appropriate microservices.
- ☑️ **NGINX**: Acts as a reverse proxy, providing load balancing and enhanced security for incoming traffic.
- ☑️ **RabbitMQ (Event Bus)**: Facilitates asynchronous communication between services, enabling decoupled and efficient interactions.
- ☑️**Prometheus and Grafana**: Work together to monitor system metrics and visualize performance, ensuring reliability and uptime.
- ☑️ **Jaeger**: Manages distributed tracing, allowing for end-to-end monitoring of requests across multiple services.
- ☑️ **Seq**: Centralized logging system that simplifies monitoring and debugging of the entire infrastructure.
- ☑️ **Consul**: Provides service discovery and configuration management, ensuring that services can dynamically find and communicate with each other.
- ☑️ **AWS S3**: Secure and scalable storage solution used by the `Media Files Service` for managing user-uploaded media.

### 💡 Example Use Cases:
- ☑️ **Media Upload**: When a user uploads a media file, the `Media Files Service` processes and stores it in an S3 bucket. An event is then published to notify other services of the new media file.
- ☑️ **Event Notification**: Upon the creation or update of an event, the `Notifications Service` sends real-time updates to users using SignalR, keeping them informed about upcoming activities.

- ☑️ **Event Registration and Attendance**: A student discovers an upcoming coding workshop through MiniSpace. They register for the event via the platform, receiving instant confirmation and reminders through the `Notifications Service`. On the day of the event, the student checks in using the app, and the `Events Service` tracks their attendance, updating the organizer with real-time participant data.

- ☑️ **Social Networking and Interaction**: A group of students interested in forming a study group uses MiniSpace to create a new event. The `Posts Service` allows them to share updates and schedules within the group, while the `Friends Service` helps them invite other students with similar academic interests. Reactions and comments foster engagement, making the study group dynamic and interactive.

- ☑️ **Content Reporting and Moderation**: Suppose a student comes across inappropriate content on a public event post. Using the `Reports Service`, they can flag the content for review. The system logs the report and notifies the `Notifications Service`, which then alerts the moderators. Moderators can review the report and take necessary actions to ensure the platform remains safe and respectful.

- ☑️ **Organizational Announcements**: A student organization wants to announce an upcoming charity drive. They create a post using the `Posts Service`, and it automatically notifies all members through the `Notifications Service`. The organization can track the post's engagement, such as likes and comments, via the `Reactions Service`, and gather insights into member participation.

- ☑️ **Collaborative Media Sharing**: During a university festival, students upload photos and videos to the MiniSpace app. The `Media Files Service` stores these files securely in an AWS S3 bucket, while the `Notifications Service` alerts event participants about new media uploads. The event organizer can later download these files for use in promotional materials.

- ☑️ **Custom Event Analytics**: After hosting a large seminar, the organizer uses MiniSpace’s analytics tools to review event success. By accessing data provided by the `Events Service` and `Prometheus`, they can see how many students attended, track peak engagement times, and review feedback collected via post-event surveys. This helps them plan even more successful events in the future.

- ☑️ **Real-Time Event Feedback**: During a live event, students are encouraged to provide feedback using MiniSpace. As students submit their responses, the `Comments Service` captures their input in real-time, while the `Event Bus` ensures that feedback is stored and made available for immediate review by the event organizers.

- ☑️ **Secure File Storage for Student Projects**: A class is using MiniSpace to manage group projects. Students upload their documents and project files through the `Media Files Service`, which are securely stored in an S3 bucket. The system ensures that only authorized group members have access to these files, maintaining the privacy and integrity of their work.

- ☑️ **Dynamic Event Invitations**: A student receives a notification about an upcoming event that matches their interests. Using the `Friends Service`, they can instantly invite friends to join them. The system tracks RSVPs and sends reminders, ensuring that everyone is informed and ready to participate.

- ☑️ **Automated Email Communication**: An event organizer schedules a series of workshops and uses the `Email Service` to automate communication. The system sends out confirmation emails to registrants, reminder emails before each workshop, and follow-up emails with links to workshop recordings and additional resources.


## 🎨 Frontend
- **Framework**: [Blazor Server](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) - A powerful single-page application framework within the .NET ecosystem.
- **Components**: [MudBlazor](https://mudblazor.com/) - Enhances the user interface with modern and responsive design elements.

## 💻 Backend
- **Framework**: Built with [Convey](https://github.com/snatch-dev/Convey), a framework optimized for .NET microservices.
- **Architecture**: Microservice architecture, orchestrated-choreographical hybrid Saga pattern, utilizing the CQRS pattern with complex event sourcing to enhance performance and scalability.
- **Services**:
  ### ✅ Identity Service
  Manages authentication and authorization, including user registration, login, 2FA email verification, JWT token generation, and password reset.

  ### ✅ Students Service
  Manages student profiles and preferences, providing centralized control over student-related data.

  ### ✅ Events Service
  Facilitates the creation, updating, and deletion of events, managing details like location, time, and descriptions.

  ### ✅ Posts Service
  Enables users and organizations to create and manage posts, fostering information sharing and community engagement.

  ### ✅ Friends Service
  Manages user connections, enabling them to add friends, view friend lists, and interact socially within the platform.

  ### ✅ Comments Service
  Allows users to comment on events, posts, and organizational activities, promoting interaction and engagement.

  ### ✅ Reactions Service
  Provides users with the ability to react to posts, comments, and events, enabling feedback and expression.

  ### ✅ Organizations Service
  Manages the creation and administration of organizations and sub-organizations within the platform, facilitating structured group activities.

  ### ✅ MediaFiles Service
  Handles the storage and management of media files uploaded by users and organizations, ensuring secure and efficient operations.

  ### ✅ Reports Service
  Manages the generation and administration of various reports, providing valuable insights and analytics for users and administrators.

  ### ✅ Notifications Service
  Delivers real-time notifications to users using SignalR, ensuring timely updates about events, posts, and other activities.

  ### ✅ Email Service
  Configured to handle email communications, including notifications, event updates, and user communications via the event bus of the notification service.

  ### ✅ API Gateway
  Uses [Ntrada](https://github.com/snatch-dev/Ntrada) for efficient routing and management of requests across multiple services through a unified entry point.

## 🛠️ Infrastructure
- [**MongoDB**](https://www.mongodb.com/products/platform/cloud) - NoSQL, document-oriented database for scalable storage.
- [**Consul**](https://www.consul.io) - Microservices discovery and configuration management.
- [**RabbitMQ**](https://www.rabbitmq.com) - Message broker for asynchronous service communication.
- [**Fabio**](https://github.com/fabiolb/fabio) - Load balancing solution for distributing traffic across services.
- [**Jaeger**](https://www.jaegertracing.io) - Distributed tracing to monitor service interactions.
- [**Grafana**](https://grafana.com) - Metrics visualization platform.
- [**Prometheus**](https://prometheus.io) - Monitoring tool for capturing real-time metrics.
- [**Seq**](https://datalust.co/seq) - Centralized logging platform for tracking application logs.
- [**Vault**](https://www.vaultproject.io) - Secure management of secrets and sensitive information.

## 📊 Code Coverage
Stay informed about code quality with our integrated Codecov badge displaying the current code coverage percentage:
[![codecov](https://codecov.io/gh/SaintAngeLs/distributed_minispace/graph/badge.svg?token=SW3T9CN2QS)](https://codecov.io/gh/SaintAngeLs/distributed_minispace)

## 🙌 Contributing
Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

Please refer to [CONTRIBUTING.md](./CONTRIBUTING.md) for more details.

## ⚖️ License
Distributed under the Apache License. See [LICENSE](./LICENSE) for more information.

## 📞 Contact
- **Project Link**: [MiniSpace GitHub Repository](https://github.com/SaintAngeLs/distributed_minispace)
- **Live Demo**: [MiniSpace Live Demo](http://minispace.itsharppro.com)

## 👥 Contributors
MiniSpace is made possible thanks to the contributions of several dedicated individuals. Here are the remarkable people who have contributed to this project:
- **@eggwhat**
- **@an2508374**
- **@olegkiprik**
- **@zniwiarzxxx**

Thank you for considering MiniSpace for your academic and social networking needs!

---

This README is now more engaging, user-friendly, and optimized for search engines. It effectively communicates the benefits of MiniSpace, the architecture behind it, and how users can contribute to its development.