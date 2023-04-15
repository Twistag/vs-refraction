using Newtonsoft.Json;
using Refracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Refraction
{
    internal class RefractionBackendService
    {
        private static HttpClient httpClient = new HttpClient();
        private static string baseUrl = "https://www.refraction.dev";

        private static async Task<bool> authenticateAsync(UserCredentials userCredentials)
        {
            var requestData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("userId", userCredentials.UserId),
                    new KeyValuePair<string, string>("teamId", userCredentials.TeamId)
                });

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, baseUrl + "/api/vscode/authenticate");
            request.Headers.Add("X-Refraction-Source", "VS");
            request.Headers.Add("X-Refraction-User", userCredentials.UserId);
            request.Headers.Add("X-Refraction-Team", userCredentials.TeamId);
            request.Content = requestData;

            HttpResponseMessage response = await httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        private static async Task<bool> callGenerateAsync(CodeAndLanguage codeAndLanguage, UserCredentials userCredentials, string utility)
        {

            string requestBody = JsonConvert.SerializeObject(codeAndLanguage);

            HttpContent httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(HttpMethod.Post, baseUrl + "/api/generate/" + utility);
            request.Headers.Add("X-Refraction-Source", "VS");
            request.Headers.Add("X-Refraction-User", userCredentials.UserId);
            request.Headers.Add("X-Refraction-Team", userCredentials.TeamId);
            request.Content = httpContent;

            HttpResponseMessage response = await httpClient.SendAsync(request);


            if (response.IsSuccessStatusCode)
            {
                VSServices.InsertText("\n\n");

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                using (var streamReader = new System.IO.StreamReader(responseStream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string line = await streamReader.ReadLineAsync();
                        line += "\n";
                        VSServices.InsertText(line);
                    }
                }
            }

            return response.IsSuccessStatusCode;  
        }

        public static async Task<List<LanguageProps>> getLanguages()
        {
            CodeAndLanguage codeAndLanguage = VSServices.getCodeAndLanguage();
            UserCredentials userCredentials = PropertyService.GetUserCredentials();
            string requestBody = JsonConvert.SerializeObject(codeAndLanguage);

            HttpContent httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(HttpMethod.Post, baseUrl + "/api/vscode/languages");
            request.Headers.Add("X-Refraction-Source", "VS");
            request.Headers.Add("X-Refraction-User", userCredentials.UserId);
            request.Headers.Add("X-Refraction-Team", userCredentials.TeamId);
            request.Content = httpContent;

            HttpResponseMessage response = await httpClient.SendAsync(request);

            string textContent = await response.Content.ReadAsStringAsync();

            List<LanguageProps> languages = JsonConvert.DeserializeObject<List<LanguageProps>>(textContent);
            return languages;
        }

        public static async Task<bool> GenerateAsync(string utility)
        {
            try
            {
                return await internalGenerateAsync(utility, null);
            } 
            catch(Exception ex)
            {
                NotificationService.ShowErrorMessage(ex.Message);
                return false;
            }
        }

        public static async Task<bool> GenerateAsync(string utility, string framework)
        {
            try
            {
                return await internalGenerateAsync(utility, framework);
            }
            catch (Exception ex)
            {
                NotificationService.ShowErrorMessage(ex.Message);
                return false;
            }
        }

        private static async Task<bool> internalGenerateAsync(string utility, string framework)
        {
            UserCredentials userCredentials = PropertyService.GetUserCredentials();
            CodeAndLanguage codeAndLanguage = VSServices.getCodeAndLanguage();
            codeAndLanguage.framework = framework;
            if (isEmpty(userCredentials.UserId))
            {
                NotificationService.ShowErrorMessage("Please set your Refraction User ID from tools menu");
                return false;
            }
            if (isEmpty(codeAndLanguage.code))
            {
                NotificationService.ShowErrorMessage("No code selected");
                return false;
            }
            if (isEmpty(codeAndLanguage.language))
            {
                NotificationService.ShowErrorMessage("No language selected");
                return false;
            }
            if (codeAndLanguage.code.Length > 3000)
            {
                NotificationService.ShowErrorMessage("Sorry, but the code you've selected is too long (" + codeAndLanguage.code.Length + " tokens). Please reduce the length of your code to 3000 tokens or less.");
                return false;
            }


            bool authenticated = await authenticateAsync(userCredentials);
            if (!authenticated)
            {
                NotificationService.ShowErrorMessage("User is not authenticated");
                return false;
            }

            await callGenerateAsync(codeAndLanguage, userCredentials, utility);
            return true;
        }

        private static bool isEmpty(string str)
        {
            return str == null || str.Length == 0;
        }
    }
}
