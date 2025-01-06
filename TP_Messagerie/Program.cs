using Cassandra;
using MongoDB.Driver;
using TP_Messagerie.Components;
using MudBlazor.Services;
using TP_Messagerie.Services;
using TP_Messagerie.Data;
using Blazored.LocalStorage;
using TP_Messagerie.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<UserSession>();
builder.Services.AddScoped<ConversationService>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddSingleton<Cassandra.ISession>(sp =>
{
    // Connexion à Cassandra local (conteneur Docker)
    var cluster = Cluster.Builder()
        .AddContactPoint("127.0.0.1") // Adresse IP du conteneur Cassandra (localhost exposé sur 9042)
        .WithPort(9042) // Port par défaut de Cassandra
        .Build();

    var session = cluster.Connect();

    // Création du keyspace si nécessaire
    session.Execute(@"
        CREATE KEYSPACE IF NOT EXISTS messagerie
        WITH replication = { 'class': 'SimpleStrategy', 'replication_factor': 1 }
    ");

    // Utiliser le keyspace "messagerie"
    session.ChangeKeyspace("messagerie");

    session.Execute(@"
    CREATE TABLE IF NOT EXISTS user_actions (
        username TEXT,
        action TEXT,
        timestamp TIMESTAMP,
        details TEXT,
        PRIMARY KEY (username, timestamp)
    )
");

    return session;
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
    return client.GetDatabase("sync-message-app");
});

// Add services to the container.
builder.Services.AddMudServices();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();

builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<MessageService>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<LoggerService>();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHub<MessageHub>("/messagehub");

app.Run();
