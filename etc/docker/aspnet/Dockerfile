FROM mcr.microsoft.com/dotnet/aspnet:7.0-jammy AS runtime

ARG project
ARG signalr

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

RUN apt-get update \
    && apt-get install -y locales \
    && echo "en_US.UTF-8 UTF-8" > /etc/locale.gen \
    && echo 'LANG="en_US.UTF-8"' > /etc/default/locale \
    && locale-gen \
    && update-locale LANG=en_US.UTF-8

ENV LANG en_US.UTF-8
ENV LANGUAGE en_US:en
ENV LC_ALL en_US.UTF-8

RUN mkdir /var/run/apache2
RUN a2enmod rewrite
RUN a2enmod ssl
RUN a2enmod proxy
RUN a2enmod proxy_balancer
RUN a2enmod proxy_http
RUN a2enmod headers
RUN if [ "$signalr" ]; then a2enmod proxy_wstunnel ; fi

# Copy configs
COPY etc/docker/aspnet/conf/ /opt/docker/

RUN chmod -R 775 /opt/docker
RUN ln -sf /opt/docker/etc/supervisord/apache2.conf /etc/supervisor/conf.d/apache2.conf
RUN ln -sf /opt/docker/etc/supervisord/dotnet.conf /etc/supervisor/conf.d/dotnet.conf

# Clean web apps.
RUN rm -fr /etc/apache2/sites-enabled/* \
    && rm -fr /etc/apache2/sites-available/* \
    && rm -fr /var/www/html/*

RUN ln -sf /opt/docker/etc/httpd/default.conf /etc/apache2/sites-enabled/000-default.conf
RUN if [ "$signalr" ]; then ln -sf /opt/docker/etc/httpd/signalr.conf /etc/apache2/sites-enabled/000-default.conf ; fi

# Clean-up
RUN rm -rf /var/lib/apt/lists/*

WORKDIR /app

ARG CACHEBUST=1
COPY "publish/$project/" .

EXPOSE 80

ENTRYPOINT ["bash", "/opt/docker/bin/entrypoint.sh"]

CMD ["supervisord", "-n"]
