
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace WebApplication1.Controllers
{
    public class IntegrationController : Controller
    {
        private readonly ILogger<IntegrationController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _modelId;

        public IntegrationController(
            ILogger<IntegrationController> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("GoogleAI");
            _apiKey = configuration["GoogleAI:ApiKey"];
            _modelId = configuration["GoogleAI:ModelId"] ?? "gemini-1.5-flash";
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AnalyzeImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No se ha seleccionado ninguna imagen.");
            }

            try
            {
                // Convertir la imagen a base64
                string base64Image;
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();
                    base64Image = Convert.ToBase64String(imageBytes);
                }

                // Determinar el tipo MIME de la imagen
                string mimeType = image.ContentType;

                // Crear el payload para la API de Gemini con el formato actualizado
                var payload = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new object[]
                            {
                                 new { text = "Analiza esta imagen y descríbeme en detalle lo que ves. Luego, según lo que observes, indícame a qué servicio debería acudir en el taller de reparaciones Marimon (por ejemplo: pintura, mecánica general, mantenimiento preventivo, cambio de neumáticos, alineación, balanceo y eléctirco, entre otros)." },

                                new
                                {
                                    inline_data = new
                                    {
                                        mime_type = mimeType,
                                        data = base64Image
                                    }
                                }
                            }
                        }
                    },
                    generation_config = new
                    {
                        temperature = 0.4,
                        topK = 32,
                        topP = 1,
                        maxOutputTokens = 2048
                    }
                };

                // Serializar el payload
                var jsonContent = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Hacer la solicitud a la API de Google AI
                var endpoint = $"v1/models/{_modelId}:generateContent?key={_apiKey}";

                // Para depuración: registrar el endpoint y payload
                _logger.LogInformation($"Endpoint: {endpoint}");
                _logger.LogInformation($"Payload: {jsonContent}");

                var response = await _httpClient.PostAsync(endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error en Google AI API: {errorContent}");
                    return StatusCode((int)response.StatusCode, $"Error en Google AI API: {errorContent}");
                }

                // Leer y procesar la respuesta
                var responseContent = await response.Content.ReadFromJsonAsync<JsonElement>();

                // Extraer el texto de la respuesta
                string resultText = "";
                if (responseContent.TryGetProperty("candidates", out var candidates) &&
                    candidates.GetArrayLength() > 0 &&
                    candidates[0].TryGetProperty("content", out var content1) &&
                    content1.TryGetProperty("parts", out var parts) &&
                    parts.GetArrayLength() > 0 &&
                    parts[0].TryGetProperty("text", out var text))
                {
                    resultText = text.GetString();
                }
                else
                {
                    // Si la estructura esperada no se encuentra, registra la respuesta completa
                    resultText = "No se pudo extraer el texto de la respuesta.";
                    _logger.LogWarning($"Estructura de respuesta inesperada: {responseContent}");
                }

                return Json(new { success = true, result = resultText });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al analizar la imagen");
                return BadRequest($"Error al analizar la imagen: {ex.Message}");
            }
        }
    }
}