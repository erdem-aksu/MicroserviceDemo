FROM mcr.microsoft.com/dotnet/sdk:7.0-focal AS publish

ARG project
ARG project_dir
ARG authserver_authority
ARG remoteservices_default_baseurl

COPY . /src

WORKDIR /src

RUN apt-get update && apt-get install -y jq --no-install-recommends

RUN echo $(cat "$project_dir/$project/wwwroot/appsettings.json" | jq '.AuthServer.Authority=$newVal' --arg newVal "$authserver_authority") >"$project_dir/$project/wwwroot/appsettings.json"
RUN echo $(cat "$project_dir/$project/wwwroot/appsettings.json" | jq '.RemoteServices.Default.BaseUrl=$newVal' --arg newVal "$remoteservices_default_baseurl") >"$project_dir/$project/wwwroot/appsettings.json"

RUN dotnet restore "$project_dir/$project/$project.csproj" --configfile ./NuGet.Config -nowarn:msb3202,nu1503
RUN dotnet publish "$project_dir/$project/$project.csproj" --no-restore -nowarn:msb3202,nu1503 -c Release -p:BlazorCacheBootResources=true -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

ENV DEBIAN_FRONTEND noninteractive
ENV TERM xterm
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT Production
ENV DOTNET_ENVIRONMENT Production
ENV DOTNET_RUNNING_IN_CONTAINER=true

# install apache
RUN apt-get update \
    && apt-get install -y \
        apache2 curl unzip supervisor nano git jq --no-install-recommends

RUN apt-get update \
    && apt-get install -y \
        libc6-dev libgdiplus

RUN mkdir /var/run/apache2
RUN a2enmod ssl
RUN a2enmod proxy
RUN a2enmod proxy_balancer
RUN a2enmod proxy_http
RUN a2enmod headers

# Copy configs
COPY --from=publish /src/docker/blazor/conf/ /opt/docker/

RUN chmod -R 775 /opt/docker
RUN ln -sf /opt/docker/etc/supervisord/apache2.conf /etc/supervisor/conf.d/apache2.conf

# Clean web apps.
RUN rm -fr /etc/apache2/sites-enabled/* \
    && rm -fr /etc/apache2/sites-available/* \
    && rm -fr /var/www/html/*

RUN ln -sf /opt/docker/etc/httpd/default.conf /etc/apache2/sites-enabled/000-default.conf

# Clean-up
RUN rm -rf /var/lib/apt/lists/*

WORKDIR /var/www/app

COPY --from=publish /app/publish/wwwroot .

EXPOSE 80

ENTRYPOINT ["bash", "/opt/docker/bin/entrypoint.sh"]

CMD ["supervisord", "-n"]
