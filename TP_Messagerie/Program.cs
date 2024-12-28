using Cassandra;
using MongoDB.Driver;
using TP_Messagerie.Components;
using MudBlazor.Services;
using TP_Messagerie.Services;
using TP_Messagerie.Data;

var builder = WebApplication.CreateBuilder(args);

// Ajouter le service UserSession
builder.Services.AddScoped<UserSession>();


builder.Services.AddScoped<ConversationService>();

// Cassandra configuration
builder.Services.AddSingleton<Cassandra.ISession>(sp =>
{
    var secureConnectPath = Path.Combine(AppContext.BaseDirectory, "Properties", "secure-connect-tp-messagerie.zip");
    var cluster = Cluster.Builder()
        .WithCloudSecureConnectionBundle(secureConnectPath)
        .WithCredentials("nMkNQtqMOSKhwRUUHXdQYfEK", "r0j,fZrULlTtsQX17_SQU0iEmSHYP2ANwZKgy+ZmQ38lHSPzGLezGRmyY3b2WyHc+NqrXhQKf+wye2NbHkzmRJpJjj2OWU87aTmDeGzg9s.foxDCRFtpJFmZRich4aD7")
        .Build();
    return cluster.Connect("messagerie");
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

app.Run();
