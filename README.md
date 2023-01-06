# Microservice Demo

### Requirements
- .NET 7.0
- PostgreSql
- RabbitMQ
- Redis

### Build and Publish

- Build the repository using the command below for resolve project references from sub solutions. (Must be run when sub
  solutions are updated)

```bash
cd build && ./build-all.sh
```

- For publishing the repository use the command below.

```bash
cd build && ./publish.sh
```

### Client Proxies

For generating the client proxies use the command below:

```bash
abp generate-proxy --type csharp --module contact --url https://localhost:44399/ --without-contracts -wd ./services/contact/src/MicroserviceDemo.ContactService.HttpApi.Client/
abp generate-proxy --type csharp --module report --url https://localhost:44307/ --without-contracts -wd ./services/report/src/MicroserviceDemo.ReportService.HttpApi.Client/
```