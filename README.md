# Curriculum

GraphQL CV API (with seeded data matching my own CV). Built as a take-home in C# / ASP.NET Core.

**Live:** [https://api.greboh.dev/graphql](https://api.greboh.dev/graphql)  
**GraphiQL:** [https://api.greboh.dev/ui/graphiql](https://api.greboh.dev/ui/graphiql)

## Stack

- .NET 10, ASP.NET Core
- [graphql-dotnet](https://graphql-dotnet.github.io/)
-  Uses PostgresSQL with EFCore (If there is no data in the database, it seeds default data).
- .NET Aspire for local orchestration
- Docker + GitHub Actions with GitVersion building images to GHCR
- Deployed to my k3s cluster using Argo CD. Manifests can be found in this repo. (`deploy/`)
- Tracked with [GitHub Projects](https://github.com/users/Greboh/projects/3/views/1) 

## Local
**Requirements:** .NET 10 SDK
1. Clone repo.
2. Open the solution (`Curriculum.slnx`) and run **Curriculum.AppHost** (Aspire). 
   - Locally Aspire ensures the database is configured and migrations are run.
3. Open GraphiQL at `http://localhost:5076/ui/graphiql`.