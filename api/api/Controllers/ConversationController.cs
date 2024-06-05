using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConversationController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ConversationController> _logger;

        private string connectionString = $"Server=localhost;Database=websocket;Uid=root;Pwd=;";

        DatabaseManager dbManager = new DatabaseManager("localhost", "websocket", "root", "");

        public ConversationController(ILogger<ConversationController> logger)
        {
            _logger = logger;
        }

        // Mise à jour de la route pour inclure l'ID dans l'URL
        [HttpGet("{id}", Name = "GetConversation")]
        public ActionResult<Conversation> Get(long id)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            Conversation? conversation = dbManager.SelectConversationAndMessage(id);
            if (conversation != null)
            {
                return Ok(conversation);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<Conversation> Post([FromBody] Conversation newConversation)
        {
            long IdConversation = dbManager.InsertConversation(newConversation.Name);

            return CreatedAtRoute("GetConversation", new { id = IdConversation }, newConversation);
        }

    }
}
