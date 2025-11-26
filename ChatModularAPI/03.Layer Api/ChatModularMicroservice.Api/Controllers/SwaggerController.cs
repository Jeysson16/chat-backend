using Microsoft.AspNetCore.Mvc;

namespace ChatModularMicroservice.Api.Controllers
{
    /// <summary>
    /// Controlador para servir el Swagger UI de forma directa
    /// </summary>
    [ApiController]
    [Route("api-docs")]
    public class SwaggerController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public SwaggerController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// Sirve una página HTML simple con el Swagger UI - AHORA SIEMPRE DISPONIBLE
        /// </summary>
        [HttpGet]
        public ContentResult GetSwaggerUI()
        {
            // AHORA SIEMPRE DISPONIBLE - Quité la restricción de desarrollo

            var html = @"<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <title>ChatModular API - Swagger</title>
    <link rel='stylesheet' href='https://unpkg.com/swagger-ui-dist@4.5.0/swagger-ui.css' />
    <style>
        html { box-sizing: border-box; overflow: -moz-scrollbars-vertical; overflow-y: scroll; }
        *, *:before, *:after { box-sizing: inherit; }
        body { margin: 0; background: #fafafa; }
    </style>
</head>
<body>
    <div id='swagger-ui'></div>
    <script src='https://unpkg.com/swagger-ui-dist@4.5.0/swagger-ui-bundle.js'></script>
    <script src='https://unpkg.com/swagger-ui-dist@4.5.0/swagger-ui-standalone-preset.js'></script>
    <script>
        window.onload = function() {
            const ui = SwaggerUIBundle({
                url: '/swagger/v1/swagger.json',
                dom_id: '#swagger-ui',
                deepLinking: true,
                presets: [
                    SwaggerUIBundle.presets.apis,
                    SwaggerUIStandalonePreset
                ],
                plugins: [
                    SwaggerUIBundle.plugins.DownloadUrl
                ],
                layout: 'StandaloneLayout'
            });
            window.ui = ui;
        };
    </script>
</body>
</html>";

            return Content(html, "text/html");
        }
    }
}