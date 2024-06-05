
using api;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:6001");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseWebSockets();

var connections = new List<KeyValuePair<long, WebSocket>>();

string server = "localhost"; // adresse du serveur MySQL
string database = "websocket"; // nom de votre base de donn�es
string uid = "root"; // nom d'utilisateur MySQL
string password = ""; // mot de passe MySQL

string connectionString = $"Server={server};Database={database};Uid={uid};Pwd={password};";

DatabaseManager dbManager = new DatabaseManager(server, database, uid, password);

MySqlConnection connection = new MySqlConnection(connectionString);

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        string curName = context.Request.Query["name"].ToString();

        using var ws = await context.WebSockets.AcceptWebSocketAsync();

        // requete bdd qui créer l'utilisateur
        long userId = dbManager.InsertUser(curName);

        // ajoute le websocket a la list
        connections.Add(new KeyValuePair<long, WebSocket>(userId, ws));

        List<Message> Messages = dbManager.SelectMessageByConversation(1);

        // envoie des messages
        foreach (var message in Messages)
        {
            await Broadcast(message.getSender()+ ": " + message.getMessage());
        }
        await Broadcast($"{curName} joined the room");
        await Broadcast($"{connections.Count} users connected");
        
        // reçoie les messages
        await ReceiveMessage(
            ws,
            async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    // requete bdd qui ajoute le message a la conversation
                    dbManager.InsertMessage(userId, message);

                    await Broadcast(curName + ": " + message);
                }
                else if (result.MessageType == WebSocketMessageType.Close || ws.State == WebSocketState.Aborted)
                {
                    connections.Remove(new KeyValuePair<long, WebSocket>(userId, ws));
                    await Broadcast($"{curName} left the room");
                    await Broadcast($"{connections.Count} users connected");
                    if (result.CloseStatus != null)
                    {
                        await ws.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                    }
                }
            });
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

async Task ReceiveMessage(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
{
    var buffer = new byte[1024 * 4];

    while (socket.State == WebSocketState.Open)
    {
        var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        handleMessage(result, buffer);
    }
}

async Task Broadcast(string message)
{
    var bytes = Encoding.UTF8.GetBytes(message);

    foreach (var socket in connections)
    {
        if (socket.Value.State == WebSocketState.Open)
        {
            var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await socket.Value.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
