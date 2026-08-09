FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Curriculum.Api/Curriculum.Api.csproj Curriculum.Api/
COPY Curriculum.Services/Curriculum.Services.csproj Curriculum.Services/
COPY Curriculum.Infrastructure/Curriculum.Infrastructure.csproj Curriculum.Infrastructure/
COPY Curriculum.Core/Curriculum.Core.csproj Curriculum.Core/

RUN dotnet restore Curriculum.Api/Curriculum.Api.csproj

COPY Curriculum.Api/ Curriculum.Api/
COPY Curriculum.Services/ Curriculum.Services/
COPY Curriculum.Infrastructure/ Curriculum.Infrastructure/
COPY Curriculum.Core/ Curriculum.Core/

RUN dotnet publish Curriculum.Api/Curriculum.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

EXPOSE 8080
EXPOSE 8443

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Curriculum.Api.dll"]

