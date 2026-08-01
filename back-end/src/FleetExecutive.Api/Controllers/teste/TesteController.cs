using Microsoft.AspNetCore.Mvc;
using Npgsql;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FleetExecutive.Api.Controllers.teste
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesteController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TesteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/teste
        // Retorna o resultado de: SELECT * FROM public."teste" ORDER BY id ASC
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dictionary<string, object?>>>> Get()
        {
            // Mesma resolução de connection string usada no AddInfrastructure:
            // prioriza a variável de ambiente "StrCon_UserPost18" e cai para
            // ConnectionStrings:Master do appsettings.
            var connectionString = _configuration["StrCon_UserPost18"]
                ?? _configuration.GetConnectionString("Master")
                ?? throw new InvalidOperationException(
                    "Connection string não configurada: defina 'StrCon_UserPost18' ou 'ConnectionStrings:Master'.");

            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(
                "SELECT * FROM public.\"teste\" ORDER BY id ASC", connection);
            await using var reader = await command.ExecuteReaderAsync();

            var rows = new List<Dictionary<string, object?>>();
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object?>(reader.FieldCount);
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = await reader.IsDBNullAsync(i)
                        ? null
                        : reader.GetValue(i);
                }
                rows.Add(row);
            }

            return Ok(rows);
        }

        // GET api/<TesteController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TesteController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TesteController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TesteController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
