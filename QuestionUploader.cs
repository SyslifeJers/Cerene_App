using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cerene_App
{
    public class QuestionUploader
    {
        private readonly HttpClient _http = new();
        public string Link { get; set; }

        public QuestionUploader(string link)
        {
            Link = link;
        }

        public async Task EnviarPreguntasAsync(int idExamen, IEnumerable<Pregunta> preguntas)
        {
            var payload = new { id_examen = idExamen, preguntas };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _http.PostAsync(Link, content);
        }
    }
}
