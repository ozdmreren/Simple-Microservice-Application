Technologies Used
This project leverages a modern microservice architecture with a combination of the following technologies:

Entity Framework Core (EF Core) with Microsoft SQL Server
→ Used to persist and manage order data efficiently in a relational database.

MongoDB
→ Utilized for storing document-based or non-relational data in services that require flexible schemas.

MassTransit with RabbitMQ
→ Implements asynchronous communication between microservices via event-driven messaging patterns, ensuring loose coupling and scalability.

Serilog with Seq
→ Provides structured logging and real-time log visualization, enabling easier debugging and monitoring of services.

Redis
→ Used as an in-memory cache store, primarily to cache product data for improved performance and faster access.
