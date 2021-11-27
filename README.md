## Certify SSL

1. Clean cert if has before

   > `dotnet dev-certs https --clean`

2. Trust cert
   > `dotnet dev-certs https --trust`

---

## Setup Host

> ### Program.cs

Host (IHost) object:

- Dependency Injection (ID): IServiceProvider (ServiceCollection)
- Logging (ILogging)
- Configuration
- IHostedService => StartAsync : Run HTTP Server (Kestrel Http)

Steps:

1.  Create IHostBuilder
2.  Configure, Register Services (ConfigureWebHostDefaults)
3.  IHostBuilder.Build() => Host(IHost)
4.  Host.Run();

> ### Startup.cs

1. Register Services (using Dependency Injection)
2. Build pipeline (middlewares)
