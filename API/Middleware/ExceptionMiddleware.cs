using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {

        public RequestDelegate _next { get; }
        public ILogger<ExceptionMiddleware> _logger { get; }
        public IHostEnvironment _env { get; }

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Si no hay excepciones, se ejecuta la siguiente request
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message); // Se registra el error en el log
                context.Response.ContentType = "application/json"; // Se establece el tipo de contenido de la respuesta
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError; // Se establece el código de estado de la respuesta
                var response = _env.IsDevelopment() // Si estamos en desarrollo, se devuelve el mensaje de error y los detalles
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode, ex.Message ,"Error interno del servidor"); // Si estamos en producción, se devuelve el mensaje de error
                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase}; // Se establece la política de nombramiento de las propiedades
                var json = JsonSerializer.Serialize(response, options); // Se serializa el objeto response
                await context.Response.WriteAsync(json); // Se escribe el json en la respuesta
            }
        }
    }
}