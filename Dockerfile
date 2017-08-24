FROM microsoft/dotnet:2.0-sdk AS build-env
WORKDIR /app

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

COPY src ./src
COPY test ./test
COPY *.sln .

WORKDIR /app/test/NCurses.Core.Tests
RUN dotnet msbuild /t:Restore /p:RuntimeIdentifier=linux-x64 /p:Configuration=Debug /p:Ncurses_Version=5 NCurses.Core.Tests.csproj

WORKDIR /app/test/NCurses.Core.Tests
RUN dotnet msbuild /t:Publish /p:RuntimeIdentifier=linux-x64 /p:Configuration=Debug /p:Ncurses_Version=5 NCurses.Core.Tests.csproj


FROM ncurses:2.0-sdk

WORKDIR /app
COPY --from=build-env /app/test/NCurses.Core.Tests/bin/Debug/netcoreapp2.0/linux-x64/publish ./

ENTRYPOINT ["dotnet", "./NCurses.Core.Tests.dll"]