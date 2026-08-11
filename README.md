# Curriculum

GraphQL CV API (with seeded data matching my own CV). Built as a take-home in C# / ASP.NET Core.

**Live:** [https://api.greboh.dev/graphql](https://api.greboh.dev/graphql)  
**GraphiQL:** [https://api.greboh.dev/ui/graphiql](https://api.greboh.dev/ui/graphiql)

## Stack

- .NET 10, ASP.NET Core
- [graphql-dotnet](https://graphql-dotnet.github.io/)
- In-memory data seeded from my CV (skills are mutable)
- .NET Aspire for local orchestration
- Docker + GitHub Actions with GitVersion building images to GHCR
- Deployed to my k3s cluster using Argo CD. Manifests can be found in this repo. (`deploy/`)
- Tracked with [GitHub Projects](https://github.com/users/Greboh/projects/3/views/1) 

## Local
**Requirements:** .NET 10 SDK
1. Clone repo.
2. Open the solution (`Curriculum.slnx`) and run either:
    - **Curriculum.AppHost** (Aspire), or
    - **Curriculum.Api** directly
3. Open GraphiQL at `http://localhost:5076/ui/graphiql`.