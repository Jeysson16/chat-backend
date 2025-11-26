using Microsoft.AspNetCore.Mvc;

namespace ChatModularMicroservice.Api.Controllers
{
    /// <summary>
    /// Controlador para servir el Swagger UI de forma directa - FUNCIONA SIEMPRE
    /// </summary>
    [ApiController]
    [Route("swagger-ui")]
    public class SwaggerUIController : ControllerBase
    {
        [HttpGet]
        public ContentResult GetSwaggerUI()
        {
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
        .swagger-ui .topbar { display: none; }
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
                layout: 'StandaloneLayout',
                validatorUrl: null,
                supportedSubmitMethods: ['get', 'post', 'put', 'delete', 'patch'],
                onComplete: function() {
                    console.log('Swagger UI cargado correctamente');
                },
                onFailure: function() {
                    console.log('Error al cargar Swagger UI');
                }
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