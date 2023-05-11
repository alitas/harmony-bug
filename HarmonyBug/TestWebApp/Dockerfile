FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy AS base

# install dotnet diagnostic tools
RUN wget -q https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb
RUN dotnet tool install --tool-path /opt/bin dotnet-counters  && \
    dotnet tool install --tool-path /opt/bin dotnet-dump  && \
    dotnet tool install --tool-path /opt/bin dotnet-gcdump  && \
    dotnet tool install --tool-path /opt/bin dotnet-sos  && \
    dotnet tool install --tool-path /opt/bin dotnet-stack  && \
    dotnet tool install --tool-path /opt/bin dotnet-trace

ENV PATH=/opt/bin:$PATH
RUN dotnet-sos install

# Dotnet GC Settings
ENV COMPlus_DbgEnableMiniDump=1 COMPlus_gcServer=1 COMPlus_DbgMiniDumpName=/tmp/core.dump

# Dotnet PGO Settings
ENV DOTNET_TieredPGO=1 DOTNET_ReadyToRun=0 DOTNET_TC_QuickJitForLoops=1

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy AS build
WORKDIR /src
COPY ["TestWebApp/TestWebApp.csproj", "TestWebApp/"]
RUN dotnet restore "TestWebApp/TestWebApp.csproj"
COPY . .
WORKDIR "/src/TestWebApp"
RUN dotnet build "TestWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestWebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestWebApp.dll"]
