using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AzureADConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Configurar los parámetros de autenticación
            string tenantId = "fe920e13-933e-4199-9b6b-43f16efa2c5c"; // Reemplazar con el ID de inquilino de Azure AD
            string clientId = "b5366365-e816-4999-9cb4-c2af55e865d0"; // Reemplazar con el ID de cliente de la aplicación registrada en Azure AD
            string clientSecret = "8LQ8Q~Iq4HtHkDdacx1NiFq1ATgV2FrD9pM~fbqW"; // Reemplazar con el secreto de cliente de la aplicación registrada en Azure AD
            string resourceUrl = "https://graph.microsoft.com"; // Reemplazar con el recurso al que se quiere acceder
            string username = "earg@citlali.mx"; // Reemplazar con el nombre de usuario del usuario de Azure AD
            string password = "PASSWORD"; // Reemplazar con la contraseña del usuario de Azure AD

            // Crear el cliente HTTP
            HttpClient client = new HttpClient();

            // Construir la solicitud de token de acceso
            var tokenRequest = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("resource", resourceUrl),
                new KeyValuePair<string, string>("scope", "openid"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            // Enviar la solicitud de token de acceso a Azure AD
            var tokenResponse = await client.PostAsync($"https://login.microsoftonline.com/{tenantId}/oauth2/token", tokenRequest);
            var tokenResponseContent = await tokenResponse.Content.ReadAsStringAsync();

            // Analizar la respuesta de token de acceso
            if (tokenResponse.IsSuccessStatusCode)
            {
                // Extraer el token de acceso del cuerpo de la respuesta
                string accessToken = JsonConvert.DeserializeObject<dynamic>(tokenResponseContent).access_token;

                // Usar el token de acceso para realizar llamadas al servicio REST
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync("https://graph.microsoft.com/v1.0/me");
                var responseContent = await response.Content.ReadAsStringAsync();

                // Procesar la respuesta del servicio REST
                if (response.IsSuccessStatusCode)
                {
                    // Manejar la respuesta exitosa del servicio REST
                    Console.WriteLine("Respuesta del servicio REST:");
                    Console.WriteLine(responseContent);
                }
                else
                {
                    // Manejar errores de respuesta del servicio REST
                    Console.WriteLine("Error en la respuesta del servicio REST:");
                    Console.WriteLine(responseContent);
                }
            }
            else
            {
                // Manejar errores de respuesta de token de acceso
                Console.WriteLine("Error en la respuesta del token de acceso:");
                Console.WriteLine(tokenResponseContent);
                Console.ReadKey();
            }






        }

      


    }
}
