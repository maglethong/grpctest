using System.Threading.Channels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp1;
using Common;
using Common.ProtoNet;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using ProtoBuf.Grpc.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddGrpcClient<Greeter.GreeterClient>(options => { options.Address = new Uri("http://localhost:5017"); })
    .ConfigurePrimaryHttpMessageHandler(() => new GrpcWebHandler(new HttpClientHandler()));

// TODO: Somehow still broken
builder.Services
    .AddTransient<GrpcChannel>(sp => GrpcChannel.ForAddress("https://localhost:5017", new GrpcChannelOptions() { }))
    .AddTransient<ICodeFirstGreeterService>(sp => sp.GetRequiredService<GrpcChannel>().CreateGrpcService<ICodeFirstGreeterService>());

await builder.Build().RunAsync();