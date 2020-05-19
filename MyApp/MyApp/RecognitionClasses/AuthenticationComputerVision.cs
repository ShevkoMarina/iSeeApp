using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using MyApp.RecognitionClasses.ModelsRecognition;
using Newtonsoft.Json;
using System;
using System.IO;

namespace MyApp.RecognitionClasses
{
    public static class AuthenticationComputerVision
    {
        public static ComputerVisionClient client = AuthenticateMicrosoftComputerVisionClient(Constants.ComputerVisionEndpoint, Constants.ComputerVisionKey);

        #region Methods

        /// <summary>
        /// Аутентификация клиента компьютерного зрения Microsoft
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        static private ComputerVisionClient AuthenticateMicrosoftComputerVisionClient(string endpoint, string key)
        {
            ComputerVisionClient client =
                new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
                { Endpoint = endpoint };

            return client;
        }

        /// <summary>
        ///  Аутентификация клиента компьютерного зрения Google
        /// </summary>
        public static void AuthenticateGoogleVision()
        {
            GoogleAuth googleAuth = CreateGoogleAuthenticationJson();
            string jsonstring = JsonConvert.SerializeObject(googleAuth);
            SaveJson(jsonstring);
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",
               Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/AccessKGoogle.json");
        }

        /// <summary>
        /// Генерация ключа для аутентификации
        /// </summary>
        /// <returns></returns>
        private static GoogleAuth CreateGoogleAuthenticationJson()
        {
            return new GoogleAuth
            {
                type = Constants.GAuthType,
                project_id = Constants.GAuthProjectId,
                private_key_id = Constants.GAuthPrivateKeyId,
                private_key = Constants.GAuthPrivateKey,
                client_email = Constants.GAuthClientEmail,
                client_id = Constants.GAuthClientId,
                auth_uri = Constants.GAuthAuthUri,
                token_uri = Constants.GAuthTokenUri,
                auth_provider_x509_cert_url = Constants.GAuthauthProviderUrl,
                client_x509_cert_url = Constants.GAuthClientUrl
            };
        }

        /// <summary>
        /// Сохранение ключа аутентификации в папку приложения
        /// </summary>
        /// <param name="jsonstring"></param>
        private static void SaveJson(string jsonstring)
        {
            var backingFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/AccessKGoogle.json");
            using (var writer = File.CreateText(backingFile))
            {
                writer.Write(jsonstring);
            }
        }
        #endregion
    }
}
