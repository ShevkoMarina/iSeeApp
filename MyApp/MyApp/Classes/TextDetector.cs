using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Drawing;
using System.Collections.Generic;

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
        public static bool AnyErrors { get; private set; }

        static string subscriptionKey = Constants.ComputerVisionKey;

        static string endpoint = Constants.ComputerVisionEndpoint;
        static string uriBase = endpoint + "vision/v2.1/ocr";

        public static async Task ReadPrintedText(string localImagePath)
        {
            AnyErrors = false;
            try
            {
                string detectedtext = "";
                HttpClient client = new HttpClient();
                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);
                string requestParameters = "language=ru&detectOrientation=true";
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;
               
                Bitmap image = Preprocessing(localImagePath);

                var byteData = GetImageAsByteArray(localImagePath);
                
     
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
                string json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);
                JObject jObj = JObject.Parse(json);
                PrintedTextFromPhoto r = new PrintedTextFromPhoto();
                r = jObj.ToObject<PrintedTextFromPhoto>();
                foreach (var region in r.regions)
                    foreach (var line in region.lines)
                        foreach (var word in line.words)
                            detectedtext += word.text + " ";
                // Display the JSON response.
                DetectedText = detectedtext;
            }
            catch (Exception)
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
                    foreach (var line in recResult.Lines)

                    {
                        textResult += line.Text + " ";
                        //await DisplayAlert("Резы", line.Text, "OK");
                    }
                    DetectedText = textResult;


                }
            };
        }
        public static byte[] Monochrome(Bitmap image, int level)
        {
            byte[] imageInBytes = new byte[image.Width * image.Height];
            int k = 0;
            for (int j = 0; j < image.Height; j++)
            {
                for (int i = 0; i < image.Width; i++)
                {
                    var color = image.GetPixel(i, j);
                    byte gray = (byte)((color.R + color.G + color.B) / 3);
                    imageInBytes[k++] = gray;
                //    imageInBytes[i]
                }
            }
            return imageInBytes;
        }

        private static Bitmap Preprocessing(string imageFilePath)
        {
            Bitmap image = GetImageAsBitmap(imageFilePath);
           // MedianFiltering(image);
            Monochrome(image, 80);
            return image;
        }

        public static void MedianFiltering(Bitmap bm)
        {
            List<byte> termsList = new List<byte>();

            byte[,] image = new byte[bm.Width, bm.Height];

            //Convert to Grayscale 
            for (int i = 0; i < bm.Width; i++)
            {
                for (int j = 0; j < bm.Height; j++)
                {
                    var c = bm.GetPixel(i, j);
                    byte gray = (byte)(.333 * c.R + .333 * c.G + .333 * c.B);
                    image[i, j] = gray;
                }
            }

            //applying Median Filtering 
            for (int i = 0; i <= bm.Width - 3; i++)
                for (int j = 0; j <= bm.Height - 3; j++)
                {
                    for (int x = i; x <= i + 2; x++)
                        for (int y = j; y <= j + 2; y++)
                        {
                            termsList.Add(image[x, y]);
                        }
                    byte[] terms = termsList.ToArray();
                    termsList.Clear();
                    Array.Sort<byte>(terms);
                    Array.Reverse(terms);
                    byte color = terms[4];
                    bm.SetPixel(i + 1, j + 1, Color.FromArgb(color, color, color));
                }
        }
        static Bitmap GetImageAsBitmap(string imageFilePath)
        {
            Bitmap imageInBytes = new Bitmap(imageFilePath);
            return imageInBytes;
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {          
            using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {              
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

    }
}
