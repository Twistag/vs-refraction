using Newtonsoft.Json;
using Refracion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using static System.Windows.Forms.LinkLabel;
using EnvDTE;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Windows.Controls;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.LanguageServices;
using Community.VisualStudio.Toolkit;

namespace Refraction
{
    internal class RefractionBackendService
    {
        private static HttpClient httpClient = new HttpClient();
        private static string baseUrl = "https://app.refraction.dev";

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

        private static Task<bool> callGenerate(CodeAndLanguage codeAndLanguage, UserCredentials userCredentials, string utility)
        {

            string requestBody = JsonConvert.SerializeObject(codeAndLanguage);
            byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);

            Uri uri = new Uri(baseUrl + "/api/generate/" + utility);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.Headers.Add("X-Refraction-Source", "VS");
            request.Headers.Add("X-Refraction-User", userCredentials.UserId);
            request.Headers.Add("X-Refraction-Team", userCredentials.TeamId);
            request.ContentLength = byteArray.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        TextSelection selection = VSServices.GetTextSelection();

                        selection.Insert(codeAndLanguage.code);
                        selection.Insert("\n\n");

                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            line += "\n";
                            selection.Insert(line);
                        }
                    }
                }

            }

            return Task.FromResult<bool>(true);
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

            await callGenerate(codeAndLanguage, userCredentials, utility);
            return true;
        }

        private static bool isEmpty(string str)
        {
            return str == null || str.Length == 0;
        }
    }
}
