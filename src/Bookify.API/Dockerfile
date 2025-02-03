##See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /src
#COPY ["src/Bookify.API/Bookify.API.csproj", "src/Bookify.API/"]
#RUN dotnet restore "src/Bookify.API/Bookify.API.csproj"
#COPY . .
#WORKDIR "/src/src/Bookify.API"
#RUN dotnet build "Bookify.API.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Bookify.API.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Bookify.API.dll"]
# Use the full SDK image so we have dotnet watch and build tools available
FROM mcr.microsoft.com/dotnet/sdk:8.0

# Set the working directory to where your solution is copied
WORKDIR /app

COPY src/Bookify.API/Bookify.API.csproj .

WORKDIR /src/Bookify.API
RUN dotnet restore  
COPY . .
EXPOSE 80
EXPOSE 443

CMD ["dotnet", "watch", "run", "--project" , "src/Bookify.API/Bookify.API.csproj",  "--urls", "https://*:443;http://*:80"]
