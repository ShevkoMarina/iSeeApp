using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Plugin.Media.Abstractions;
using System;


namespace MyApp.Classes
{
    // сделать распохнование печатного на всех яхыках и как доп опция сделать рукописный на англ
    // что делать с исключением
    // сделать экран загрузки
    // сделать настройки
    // нал референс эксепшн если результаты распознованич нулевые
    public static class TextDetector
    {
        public static string DetectedText { get; private set; }
        public static string[] DetectedLines { get; private set; }
        public static bool AnyErrors { get; private set; }


        static string subscriptionKey = Constants.ComputerVisionKey;

        static string endpoint = Constants.ComputerVisionEndpoint;
        static string uriBase = endpoint + "vision/v2.1/ocr";

        public static async Task ReadPrintedText(string localImagePath)
        {
            AnyErrors = false;
            try
            {
               
                
                HttpClient client = new HttpClient();
                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);
                string requestParameters = "language=ru&detectOrientation=true";
                string uri = uriBase + "?" + requestParameters;
               
                HttpResponseMessage response;

                byte[] byteData = GetImageAsByteArray(localImagePath);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses the "application/octet-stream" content type.
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Asynchronously call the REST API method.
                    response = await client.PostAsync(uri, content);
                }

                // Asynchronously get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();
                
                
                // Display the JSON response.

                DetectedText = JToken.Parse(contentString).ToString();
            


            }
            catch(Exception)
            {
                AnyErrors = true;
            }         
        }

        public static async Task ReadTextInEnglish(string localImagePath)
        {          
          
                AnyErrors = false;
                const int numberOfCharsInOperationId = 36;
                using (Stream imageStream = File.OpenRead(localImagePath))
                {
                    
                    BatchReadFileInStreamHeaders localFileTextHeaders = await AuthenticationComputerVision.client.BatchReadFileInStreamAsync(imageStream);
                    // Get the operation location (operation ID)
                    string operationLocation = localFileTextHeaders.OperationLocation;
                    // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
                    string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);
                    // Extract text, wait for it to complete.
                    int i = 0;
                    int maxRetries = 10;
                    ReadOperationResult results;
                    do
                    {
                        results = await AuthenticationComputerVision.client.GetReadOperationResultAsync(operationId);
                        //await DisplayAlert("PR", $"Server status {results.Status}, waiting {i} sec", "OK");
                        await Task.Delay(1000);
                        if (maxRetries == 9)
                        {
                            //await DisplayAlert("Error", "Server timed out.", "OK");
                            AnyErrors = true;
                        }
                    }
                    while ((results.Status == TextOperationStatusCodes.Running ||
                            results.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries);
                    // Display the found text.

                    var textRecognitionLocalFileResults = results.RecognitionResults;

                    foreach (TextRecognitionResult recResult in textRecognitionLocalFileResults)
                    {
                        string textResult = "";
                        foreach (Line line in recResult.Lines)

                        {
                            textResult += line.Text + " ";                         
                            //await DisplayAlert("Резы", line.Text, "OK");
                        }
                        DetectedText = textResult;
                    

                   
                    }
                };
        }
        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            // Open a read-only file stream for the specified file.
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

    }
}
