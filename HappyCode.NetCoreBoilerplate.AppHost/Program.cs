var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis");
builder.AddProject<Projects.HappyCode.NetCoreBoilerplate.AppHost>("HappyCode.NetCoreBoilerplate.AppHost")
    .WithReference(redis)
    .WaitFor(redis);

builder.Build().Run();
