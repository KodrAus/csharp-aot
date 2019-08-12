using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [ApiController]
    [Route("/api/weather")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var rng = new Random();

            await using var writer = new Utf8JsonWriter(Response.BodyWriter);

            writer.WriteStartArray();
            foreach (var index in Enumerable.Range(1, 5))
            {
                writer.WriteStartObject();
                writer.WriteString("date", DateTime.Now.AddDays(index));
                writer.WriteNumber("temperature", rng.Next(-20, 55));
                writer.WriteString("summary", Summaries[rng.Next(Summaries.Length)]);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            return Ok();
        }
    }
}
