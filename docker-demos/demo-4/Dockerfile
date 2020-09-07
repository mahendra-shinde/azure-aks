FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as buildStage

WORKDIR /app

COPY mywebapp/ /app

RUN ["dotnet","restore"]

RUN ["dotnet","publish","-o","dist"]

## final stage
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

COPY --from=buildStage /app/dist/ /app

CMD ["dotnet","/app/mywebapp.dll"]