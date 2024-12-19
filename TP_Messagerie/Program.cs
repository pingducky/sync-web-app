using Cassandra;
using MongoDB.Driver;
using TP_Messagerie.Components;

var builder = WebApplication.CreateBuilder(args);

// Cassandra configuration
builder.Services.AddSingleton<Cassandra.ISession>(sp =>
{
    var cluster = Cluster.Builder()
        .AddContactPoints("https://92500a2b-01c5-4475-9181-41120b08f93e-westus3.apps.astra.datastax.com")
        .WithPort(9042)                
        .Build();
    return cluster.Connect("default_keyspace"); // Remplacez par votre keyspace Cassandra
});

// MongoDB configuration
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = MongoClientSettings.FromConnectionString("mongodb+srv://admin:MP1hoE2p39IGakLd@sync-app-db.nkles.mongodb.net/?retryWrites=true&w=majority&appName=sync-app-db");
    return new MongoClient(settings);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase("your_database");
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.MapControllerRoute(
    name: "test",
    pattern: "test",
    defaults: new { controller = "Test", action = "Index" });

app.Run();
