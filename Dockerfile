FROM microsoft/dotnet:1.1-sdk AS build-env
WORKDIR /app

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

COPY src ./src
COPY test ./test
COPY *.sln .

WORKDIR /app/test/NCurses.Core.Tests
RUN dotnet msbuild /t:Restore /p:RuntimeIdentifier=debian.8-x64 /p:Configuration=Debug /p:Ncurses_Version=5 NCurses.Core.Tests.csproj

WORKDIR /app/test/NCurses.Core.Tests
RUN dotnet msbuild /t:Publish /p:RuntimeIdentifier=debian.8-x64 /p:Configuration=Debug /p:Ncurses_Version=5 NCurses.Core.Tests.csproj


FROM microsoft/dotnet:1.1-runtime-deps

WORKDIR /app
COPY --from=build-env /app/test/NCurses.Core.Tests/bin/Debug/netcoreapp1.0/debian.8-x64/publish ./

RUN apt-get update && apt-get install -y locales
RUN echo "en_US.UTF-8 UTF-8" > /etc/locale.gen && \
    locale-gen en_US.UTF-8 && \
    echo "en_US.UTF-8" > /etc/default/locale

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
ENV LC_ALL en_US.UTF-8
ENV TERM xterm

ENTRYPOINT ["./NCurses.Core.Tests"]
