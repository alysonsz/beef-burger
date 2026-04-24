FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["BeefBurger.FrontEnd.csproj", "./"]
RUN dotnet restore "BeefBurger.FrontEnd.csproj"

COPY . .
RUN dotnet build "BeefBurger.FrontEnd.csproj" -c Release -o /app/build

RUN dotnet publish "BeefBurger.FrontEnd.csproj" -c Release -o /app/publish

FROM nginx:alpine
WORKDIR /usr/share/nginx/html

COPY --from=build /app/publish/wwwroot .

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
