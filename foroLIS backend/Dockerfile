FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY FilesUploaded /app/FilesUploaded
COPY . .

RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80

ENV CONNECTION_STRING="Server=db13696.public.databaseasp.net; Database=db13696; User Id=db13696; Password=a!9K2P@sT#o8; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;"
ENV TOKEN_SECRET="thisSecretKeyVerySecretOMGOMGOOO!@#$%"

ENTRYPOINT ["dotnet", "foroLIS backend.dll"]