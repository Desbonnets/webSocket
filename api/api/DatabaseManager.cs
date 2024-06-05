using MySql.Data.MySqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class DatabaseManager
{
    private string connectionString;

    public DatabaseManager(string server, string database, string uid, string password)
    {
        connectionString = $"Server={server};Database={database};Uid={uid};Pwd={password};";
    }

    public long InsertUser(string username)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string insertQuery = "INSERT INTO user (username) VALUES (@username)";
            MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
            insertCmd.Parameters.AddWithValue("@username", username);

            insertCmd.ExecuteNonQuery();
            return insertCmd.LastInsertedId;
        }
    }

    public long InsertMessage(long userId, string message)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string insertQuery = "INSERT INTO message (id, conversation_id, sender_id, content, timestamp) VALUES (NULL, 1, @userId , @message, @date)"; ;
            MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
            insertCmd.Parameters.AddWithValue("@userId", userId);
            insertCmd.Parameters.AddWithValue("@message", message);
            insertCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss"));

            insertCmd.ExecuteNonQuery();
            return insertCmd.LastInsertedId;
        }
    }

    public List<Message> SelectMessageByConversation(long id)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string selectQuery = "SELECT m.content, u.username, m.timestamp FROM message as m INNER JOIN user as u ON m.sender_id = u.id WHERE conversation_id = @id;";
            MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection);
            selectCmd.Parameters.AddWithValue("@id", id);

            List<Message> allMessages;

            using (MySqlDataReader reader = selectCmd.ExecuteReader())
            {
                var list = new List<Message>();
                while (reader.Read())
                    list.Add(new Message((string)reader["content"], (string)reader["username"], (DateTime)reader["timestamp"]));
                allMessages = list;
            }
            return allMessages;
        }
    }
}