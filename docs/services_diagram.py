from diagrams import Diagram
from diagrams.aws.storage import S3
from diagrams.onprem.network import Nginx
from diagrams.onprem.monitoring import Prometheus, Grafana
from diagrams.onprem.queue import Rabbitmq
from diagrams.onprem.network import Consul
from diagrams.c4 import Person, Container, Database, System, SystemBoundary, Relationship

graph_attr = {
    "splines": "spline",
    "nodesep": "1.0",
    "ranksep": "2.0",
    "ratio": "auto"
}

with Diagram("Complex System Diagram", direction="TB", graph_attr=graph_attr):
    user = Person(name="User", description="Interacts with the system")
    
    with SystemBoundary("MiniSpace System"):
        # API and Gateway Services
        nginx = Nginx("NGINX")
        api_gateway = Container(
            name="API Gateway (Entrance) ",
            technology=".NET",
            description="Entry point for API requests"
        )
        nginx >> api_gateway

        # Load Balancer
        fabio_load_balancer = Container(
            name="Fabio Load Balancer",
            technology="HAProxy",
            description="Balances load among services"
        )
        api_gateway >> fabio_load_balancer

        # Event Handling
        event_bus = Rabbitmq("RabbitMQ")

        # Monitoring and Metrics
        prometheus = Prometheus("Prometheus")
        grafana = Grafana("Grafana")

        # Tracing and Logging
        jaeger = Container(
            name="Jaeger",
            technology="Jaeger",
            description="Distributed tracing"
        )
        seq = Container(
            name="Seq",
            technology="Seq",
            description="Centralized logging"
        )

        # Service Discovery
        consul = Consul("Consul")

        # Services
        comments_service = Container(
            name="Comments Service",
            technology=".NET",
            description="Handles comments"
        )
        email_service = Container(
            name="Email Service",
            technology=".NET",
            description="Handles emails"
        )
        events_service = Container(
            name="Events Service",
            technology=".NET",
            description="Handles events"
        )
        friends_service = Container(
            name="Friends Service",
            technology=".NET",
            description="Handles friend connections"
        )
        identity_service = Container(
            name="Identity Service",
            technology=".NET",
            description="Handles user identity"
        )
        media_files_service = Container(
            name="Media Files Service",
            technology=".NET",
            description="Handles media files"
        )
        notifications_service = Container(
            name="Notifications Service",
            technology=".NET",
            description="Handles notifications"
        )
        organizations_service = Container(
            name="Organizations Service",
            technology=".NET",
            description="Handles organizations"
        )
        posts_service = Container(
            name="Posts Service",
            technology=".NET",
            description="Handles posts"
        )
        reactions_service = Container(
            name="Reactions Service",
            technology=".NET",
            description="Handles reactions"
        )
        reports_service = Container(
            name="Reports Service",
            technology=".NET",
            description="Handles reports"
        )
        students_service = Container(
            name="Students Service",
            technology=".NET",
            description="Handles students"
        )

        # Databases
        comments_db = Database(
            name="Comments DB",
            technology="MongoDB",
            description="Stores comment data"
        )
        email_db = Database(
            name="Email DB",
            technology="MongoDB",
            description="Stores email data"
        )
        events_db = Database(
            name="Events DB",
            technology="MongoDB",
            description="Stores event data"
        )
        friends_db = Database(
            name="Friends DB",
            technology="MongoDB",
            description="Stores friend data"
        )
        identity_db = Database(
            name="Identity DB",
            technology="MongoDB",
            description="Stores identity data"
        )
        media_files_db = Database(
            name="Media Files DB",
            technology="MongoDB",
            description="Stores media files data"
        )
        notifications_db = Database(
            name="Notifications DB",
            technology="MongoDB",
            description="Stores notification data"
        )
        organizations_db = Database(
            name="Organizations DB",
            technology="MongoDB",
            description="Stores organization data"
        )
        posts_db = Database(
            name="Posts DB",
            technology="MongoDB",
            description="Stores post data"
        )
        reactions_db = Database(
            name="Reactions DB",
            technology="MongoDB",
            description="Stores reaction data"
        )
        reports_db = Database(
            name="Reports DB",
            technology="MongoDB",
            description="Stores report data"
        )
        students_db = Database(
            name="Students DB",
            technology="MongoDB",
            description="Stores student data"
        )

        # AWS S3 Bucket connected to Media Files Service
        s3_bucket = S3("AWS S3 Bucket")

        # Define Relationships
        user >> nginx >> api_gateway
        api_gateway >> fabio_load_balancer
        fabio_load_balancer >> [
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
        
        # Service to Database relationships
        comments_service >> comments_db
        email_service >> email_db
        events_service >> events_db
        friends_service >> friends_db
        identity_service >> identity_db
        media_files_service >> media_files_db
        media_files_service >> s3_bucket
        notifications_service >> notifications_db
        organizations_service >> organizations_db
        posts_service >> posts_db
        reactions_service >> reactions_db
        reports_service >> reports_db
        students_service >> students_db

        # Monitoring, Tracing, and Event Handling
        fabio_load_balancer >> Relationship("Registers with") >> consul
        api_gateway >> Relationship("Sends metrics to") >> prometheus
        prometheus >> grafana
        api_gateway >> jaeger
        api_gateway >> seq
        [comments_service, email_service, events_service, friends_service, identity_service, media_files_service,
        notifications_service, organizations_service, posts_service, reactions_service, reports_service, students_service] >> Relationship("Sends events to") >> event_bus
        
        # Direct connection from Notifications Service to the User application
        notifications_service >> Relationship("Pushes notifications to") >> user
