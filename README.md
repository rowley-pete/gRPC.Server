# gRPC.Server Tutorial

## Run
```
dotnet run
```

## Docker build
```
docker build -t grpcserverapi:latest . -f .\docker\Dockerfile
```

## Docker run
```
docker run -p 5241:5241 -p 5242:5242 grpcserverapi
```