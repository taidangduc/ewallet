var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.EWallet_Wallet>("wallet");
builder.AddProject<Projects.EWallet_Identity>("identity");

builder.Build().Run();
