// ref: https://devblogs.microsoft.com/dotnet/new-aspire-app-with-react/
// ref: https://www.nuget.org/packages/Aspire.Hosting.JavaScript

var builder = DistributedApplication.CreateBuilder(args);

var wallerService = builder.AddProject<Projects.EWallet_Wallet>("wallet-service");
builder.AddProject<Projects.EWallet_Identity>("identity-service").WaitFor(wallerService);
builder.AddProject<Projects.EWallet_Gateways_WebAPI>("api-gateway").WaitFor(wallerService);
builder.AddJavaScriptApp("frontend", "../../Clients/customer").WaitFor(wallerService);

builder.Build().Run();
