FROM microsoft/dotnet:2.1-sdk as build
WORKDIR /src
COPY . .
RUN dotnet publish -c Debug MetricsDemo.csproj -o /out

FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
COPY --from=build /out .

EXPOSE 5656

ENTRYPOINT ["dotnet", "MetricsDemo.dll"]

HEALTHCHECK --interval=3s --timeout=5s \
  CMD curl -f http://localhost:5656/metrics || exit 1
