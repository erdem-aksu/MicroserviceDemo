FROM mcr.microsoft.com/dotnet/aspnet:7.0-jammy AS runtime

ARG project

ENV DEBIAN_FRONTEND noninteractive
ENV TERM xterm
ENV ASPNETCORE_ENVIRONMENT Production
ENV DOTNET_ENVIRONMENT Production
ENV DOTNET_RUNNING_IN_CONTAINER=true

# install apache
RUN apt-get update \
    && apt-get install -y \
        curl unzip nano git jq --no-install-recommends

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

COPY etc/docker/console/entrypoint.sh/ /opt/docker/
RUN chmod +x /opt/docker/entrypoint.sh

WORKDIR /app

ARG CACHEBUST=1
COPY "publish/$project/" .

ENTRYPOINT ["bash", "/opt/docker/entrypoint.sh"]

