var builder = DistributedApplication.CreateBuilder(args);

var wallerService = builder.AddProject<Projects.EWallet_Wallet>("wallet");
builder.AddProject<Projects.EWallet_Identity>("identity").WaitFor(wallerService);
builder.AddProject<Projects.EWallet_Gateways_WebAPI>("gateway").WaitFor(wallerService);

builder.Build().Run();
