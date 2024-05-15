using MySql.Data.MySqlClient;
using System.Net;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:6001");

var app = builder.Build();
app.UseWebSockets();

var connections = new List<WebSocket>();

string server = "localhost"; // adresse du serveur MySQL
string database = "websocket"; // nom de votre base de donn�es
string uid = "root"; // nom d'utilisateur MySQL
string password = ""; // mot de passe MySQL

string connectionString = $"Server={server};Database={database};Uid={uid};Pwd={password};";

MySqlConnection connection = new MySqlConnection(connectionString);


app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var curName = context.Request.Query["name"];

        using var ws = await context.WebSockets.AcceptWebSocketAsync();

        connections.Add(ws);

        try
        {
            connection.Open();
            Console.WriteLine("Connexion à la base de données réussie!");

            // Exemple d'insertion de données
            string query = "INSERT INTO user (username) VALUES (@valeur1)";

            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@valeur1", curName);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Insertion réussie!");
            }
            else
            {
                Console.WriteLine("Aucune ligne insérée.");
            }

            connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur de connexion: " + ex.Message);
        }

        await Broadcast($"{curName} joined the room");
        await Broadcast($"{connections.Count} users connected");
        await ReceiveMessage(
            ws,
            async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await Broadcast(curName + ": " + message);
                }
                else if (result.MessageType == WebSocketMessageType.Close || ws.State == WebSocketState.Aborted)
                {
                    connections.Remove(ws);
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
        if (socket.State == WebSocketState.Open)
        {
            var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}

await app.RunAsync();
