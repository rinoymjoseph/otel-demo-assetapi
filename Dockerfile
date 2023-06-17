#START 

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# WorkDir
WORKDIR /app

# copy csproj and restore as distinct layers
COPY src/Otel.Demo.AssetApi/Otel.Demo.AssetApi.csproj ./src/Otel.Demo.AssetApi/

#restore files
RUN dotnet restore ./src/Otel.Demo.AssetApi/Otel.Demo.AssetApi.csproj -r linux-x64

# copy files
COPY . .

# publish app
RUN dotnet publish ./src/Otel.Demo.AssetApi/ -c release -o build -r linux-x64 -p:PublishTrimmed=true --self-contained true --no-restore

# Stage - Run
FROM registry.access.redhat.com/ubi8/ubi-minimal:8.7

# Set ENV variables
ENV ASPNETCORE_URLS=http://+:8080

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

# Expose port 8080
EXPOSE 8080

# Create a work directory
WORKDIR /app

# Copy build to work directory
COPY --from=build /app/build .

ENTRYPOINT ["./Otel.Demo.AssetApi"]

#END