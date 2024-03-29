#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
RUN apt-get update \ 
    && apt-get install -y --no-install-recommends libgdiplus libc6-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*
RUN cd /usr/lib && ln -s libgdiplus.so gdiplus.dll
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["OSSA-Website/OSSA-Website.csproj", "OSSA-Website/"]
RUN dotnet restore "OSSA-Website/OSSA-Website.csproj"
COPY . .
WORKDIR "/src/OSSA-Website"
RUN dotnet build "OSSA-Website.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OSSA-Website.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OSSA-Website.dll"]