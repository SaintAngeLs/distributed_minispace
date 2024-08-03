from diagrams import Diagram
from diagrams.c4 import Person, Container, Database, System, SystemBoundary, Relationship

graph_attr = {
    "splines": "spline",
    "nodesep": "4.0",  # Increase space between nodes
    "ranksep": "4.0"   # Increase space between ranks
}

with Diagram("Container Diagram for MiniSpace System", direction="TB", graph_attr=graph_attr):
    user = Person(name="MiniSpace User", description="User interacting with the MiniSpace system.")
    
    with SystemBoundary("MiniSpace System"):
        api_gateway = Container(
            name="API Gateway Interface",
            technology="Spring Boot",
            description="Serves as an entry point for API requests."
        )
        
        fabio_load_balancer = Container(
            name="Fabio Load Balancer",
            technology="HAProxy",
            description="Balances the load among HTTP clients using Consul."
        )
        
        event_bus = Container(
            name="Event Bus",
            technology="RabbitMQ",
            description="Handles the events within the system."
        )
        
        consul = Container(
            name="Consul",
            technology="HashiCorp Consul",
            description="Service discovery and configuration management."
        )
        
        jaeger = Container(
            name="Jaeger",
            technology="Jaeger",
            description="Distributed tracing for monitoring and troubleshooting."
        )
        
        seq = Container(
            name="Seq",
            technology="Seq",
            description="Centralized logging for monitoring and alerting."
        )
        
        # Define Services
        comments_service = Container(
            name="Comments Service",
            technology=".NET",
            description="Manages comments on posts."
        )
        
        email_service = Container(
            name="Email Service",
            technology=".NET",
            description="Handles email notifications and communication."
        )
        
        events_service = Container(
            name="Events Service",
            technology=".NET",
            description="Manages event-related data and services."
        )
        
        friends_service = Container(
            name="Friends Service",
            technology=".NET",
            description="Manages friend relationships and connections."
        )
        
        identity_service = Container(
            name="Identity Service",
            technology=".NET",
            description="Handles user identity and authentication."
        )
        
        media_files_service = Container(
            name="Media Files Service",
            technology=".NET",
            description="Manages media files uploaded by users."
        )
        
        notifications_service = Container(
            name="Notifications Service",
            technology=".NET",
            description="Manages notifications to users."
        )
        
        organizations_service = Container(
            name="Organizations Service",
            technology=".NET",
            description="Manages organization-related data."
        )
        
        posts_service = Container(
            name="Posts Service",
            technology=".NET",
            description="Manages user posts and related data."
        )
        
        reactions_service = Container(
            name="Reactions Service",
            technology=".NET",
            description="Handles reactions (likes, dislikes) on posts."
        )
        
        reports_service = Container(
            name="Reports Service",
            technology=".NET",
            description="Manages user reports on content or behavior."
        )
        
        students_service = Container(
            name="Students Service",
            technology=".NET",
            description="Handles student-specific data and operations."
        )
        
        # Define Databases
        comments_db = Database(
            name="Comments Database",
            technology="MongoDB",
            description="Stores comment-related data."
        )
        
        email_db = Database(
            name="Email Database",
            technology="MongoDB",
            description="Stores email-related data."
        )
        
        events_db = Database(
            name="Events Database",
            technology="MongoDB",
            description="Stores event-related data."
        )
        
        friends_db = Database(
            name="Friends Database",
            technology="MongoDB",
            description="Stores friend relationship data."
        )
        
        identity_db = Database(
            name="Identity Database",
            technology="MongoDB",
            description="Stores identity and authentication data."
        )
        
        media_files_db = Database(
            name="Media Files Database",
            technology="MongoDB",
            description="Stores media file data."
        )
        
        notifications_db = Database(
            name="Notifications Database",
            technology="MongoDB",
            description="Stores notification data."
        )
        
        organizations_db = Database(
            name="Organizations Database",
            technology="MongoDB",
            description="Stores organization-related data."
        )
        
        posts_db = Database(
            name="Posts Database",
            technology="MongoDB",
            description="Stores post-related data."
        )
        
        reactions_db = Database(
            name="Reactions Database",
            technology="MongoDB",
            description="Stores reaction-related data."
        )
        
        reports_db = Database(
            name="Reports Database",
            technology="MongoDB",
            description="Stores report-related data."
        )
        
        students_db = Database(
            name="Students Database",
            technology="MongoDB",
            description="Stores student-related data."
        )
        
    # Define Relationships
    user >> Relationship("Uses") >> api_gateway
    api_gateway >> Relationship("Routes requests through") >> fabio_load_balancer
    fabio_load_balancer >> Relationship("Forwards requests to") >> [
        comments_service,
        email_service,
        events_service,
        friends_service,
        identity_service,
        media_files_service,
        notifications_service,
        organizations_service,
        posts_service,
        reactions_service,
        reports_service,
        students_service
    ]
    
    comments_service >> Relationship("Stores data in") >> comments_db
    email_service >> Relationship("Stores data in") >> email_db
    events_service >> Relationship("Stores data in") >> events_db
    friends_service >> Relationship("Stores data in") >> friends_db
    identity_service >> Relationship("Stores data in") >> identity_db
    media_files_service >> Relationship("Stores data in") >> media_files_db
    notifications_service >> Relationship("Stores data in") >> notifications_db
    organizations_service >> Relationship("Stores data in") >> organizations_db
    posts_service >> Relationship("Stores data in") >> posts_db
    reactions_service >> Relationship("Stores data in") >> reactions_db
    reports_service >> Relationship("Stores data in") >> reports_db
    students_service >> Relationship("Stores data in") >> students_db
    
    # Service Discovery, Tracing, and Monitoring
    fabio_load_balancer >> Relationship("Registers with") >> consul
    [comments_service, email_service, events_service, friends_service, identity_service, media_files_service,
     notifications_service, organizations_service, posts_service, reactions_service, reports_service, students_service] >> Relationship("Registers with") >> consul
    
    [comments_service, email_service, events_service, friends_service, identity_service, media_files_service,
     notifications_service, organizations_service, posts_service, reactions_service, reports_service, students_service] >> Relationship("Sends tracing data to") >> jaeger
    
    [comments_service, email_service, events_service, friends_service, identity_service, media_files_service,
     notifications_service, organizations_service, posts_service, reactions_service, reports_service, students_service] >> Relationship("Sends logs to") >> seq
    
    # Services sending events to Event Bus
    comments_service >> Relationship("Sends: CommentAddedEvent, CommentUpdatedEvent") >> event_bus
    email_service >> Relationship("Sends: EmailSentEvent, EmailFailedEvent") >> event_bus
    events_service >> Relationship("Sends: EventCreatedEvent, EventUpdatedEvent") >> event_bus
    friends_service >> Relationship("Sends: FriendAddedEvent, FriendRemovedEvent") >> event_bus
    identity_service >> Relationship("Sends: UserAuthenticatedEvent, UserAuthorizationUpdatedEvent") >> event_bus
    media_files_service >> Relationship("Sends: MediaFileUploadedEvent, MediaFileDeletedEvent") >> event_bus
    notifications_service >> Relationship("Sends: NotificationSentEvent, NotificationFailedEvent") >> event_bus
    organizations_service >> Relationship("Sends: OrganizationCreatedEvent, OrganizationUpdatedEvent") >> event_bus
    posts_service >> Relationship("Sends: PostCreatedEvent, PostUpdatedEvent") >> event_bus
    reactions_service >> Relationship("Sends: ReactionAddedEvent, ReactionRemovedEvent") >> event_bus
    reports_service >> Relationship("Sends: ReportFiledEvent, ReportReviewedEvent") >> event_bus
    students_service >> Relationship("Sends: StudentEnrolledEvent, StudentUpdatedEvent") >> event_bus
    
    # Services consuming events from Event Bus
    comments_service << Relationship("Consumes: CommentAddedEvent, CommentUpdatedEvent") << event_bus
    email_service << Relationship("Consumes: EmailSentEvent, EmailFailedEvent") << event_bus
    events_service << Relationship("Consumes: EventCreatedEvent, EventUpdatedEvent") << event_bus
    friends_service << Relationship("Consumes: FriendAddedEvent, FriendRemovedEvent") << event_bus
    identity_service << Relationship("Consumes: UserAuthenticatedEvent, UserAuthorizationUpdatedEvent") << event_bus
    media_files_service << Relationship("Consumes: MediaFileUploadedEvent, MediaFileDeletedEvent") << event_bus
    notifications_service << Relationship("Consumes: NotificationSentEvent, NotificationFailedEvent") << event_bus
    organizations_service << Relationship("Consumes: OrganizationCreatedEvent, OrganizationUpdatedEvent") << event_bus
    posts_service << Relationship("Consumes: PostCreatedEvent, PostUpdatedEvent") << event_bus
    reactions_service << Relationship("Consumes: ReactionAddedEvent, ReactionRemovedEvent") << event_bus
    reports_service << Relationship("Consumes: ReportFiledEvent, ReportReviewedEvent") << event_bus
    students_service << Relationship("Consumes: StudentEnrolledEvent, StudentUpdatedEvent") << event_bus
