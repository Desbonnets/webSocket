using api;
using Microsoft.VisualBasic;
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

    public Conversation? SelectConversationAndMessage(long id)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string selectQuery = "SELECT c.id, c.name, c.date, u.username AS message_user, m.content AS message_content, m.timestamp AS message_date " +
                "FROM conversation AS c " +
                "INNER JOIN message AS m ON c.id = m.conversation_id " +
                "INNER JOIN user AS u ON u.id = m.sender_id " +
                "WHERE c.id = 1 " +
                "GROUP BY m.id ";
            MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection);
            selectCmd.Parameters.AddWithValue("@id", id);

            Conversation? conversation = null;

            using (MySqlDataReader reader = selectCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    conversation = new Conversation((long)reader["id"], (string)reader["name"], (DateTime)reader["date"]);
                    while (reader.Read())
                    {
                        Message message = new Message((string)reader["message_content"], (string)reader["message_user"], (DateTime)reader["message_date"]);

                        if (message != null)
                            conversation.addMessage(message);
                    }
                }
            }
            return conversation;
        }
    }

    public long InsertConversation(string name)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string insertQuery = "INSERT INTO `conversation` (`id`, `name`, `date`) VALUES (NULL, @name, @date)";
            MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
            insertCmd.Parameters.AddWithValue("@name", name);
            insertCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss"));

            insertCmd.ExecuteNonQuery();
            return insertCmd.LastInsertedId;
        }
    }
}