var builder = DistributedApplication.CreateBuilder(args);

//Backing Services
var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDb = postgres.AddDatabase("catalogdb");

var cache = builder.AddRedis("cache")
    .WithRedisInsight()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

// add projects and cloud-native backing services here
builder.AddProject<Projects.Catalog>("catalog")
    .WithReference(catalogDb)
    .WaitFor(catalogDb);

// add projects and cloud-native backing services here

builder.AddProject<Projects.Basket>("basket")
    .WithReference(cache)
    .WaitFor(cache);

// add projects and cloud-native backing services here

builder.Build().Run();
