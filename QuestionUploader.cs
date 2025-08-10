using System.Collections.Generic;
using System.Linq;
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
            var listaPreguntas = preguntas.ToList();

            var opciones = listaPreguntas
                .SelectMany(p => p.Opciones)
                .DistinctBy(o => o.Id)
                .Select(o => new { id = o.Id, texto = o.Texto })
                .ToList();

            var secciones = listaPreguntas
                .Select(p => p.Seccion?.Nombre ?? "General")
                .Distinct()
                .Select((nombre, index) => new { id = index + 1, nombre })
                .ToList();

            var seccionLookup = secciones.ToDictionary(s => s.nombre, s => s.id);

            var preguntasPayload = listaPreguntas.Select(p => new
            {
                numero = p.Numero,
                texto = p.Texto,
                tipo = p.Tipo.ToString(),
                multiple = p.Multiple,
                opciones = string.Join(",", p.Opciones.Select(o => o.Id)),
                id_seccion = seccionLookup[p.Seccion?.Nombre ?? "General"],
                respuesta_correcta = p.RespuestaCorrecta?.Id
            });

            var payload = new
            {
                id_examen = idExamen,
                opciones,
                secciones,
                preguntas = preguntasPayload
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            Link = ApiConfig.InsertPreguntas;
                await _http.PostAsync(Link, content);
        }
    }
}
