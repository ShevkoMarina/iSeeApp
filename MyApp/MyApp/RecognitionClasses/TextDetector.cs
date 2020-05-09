using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Generic;
using Google.Cloud.Vision.V1;

namespace MyApp.RecognitionClasses
{
    public static partial class TextDetector
    {
        #region Recognition Methods

        /// <summary>
        /// Распознать печатный текст
        /// </summary>
        /// <param name="localImagePath"></param>
        /// <returns></returns>
        public static async Task<string> ReadPrintedText(string localImagePath)
        {
            string detectedText; 
            ImageAnnotatorClient client = await ImageAnnotatorClient.CreateAsync(); 
            IReadOnlyList<EntityAnnotation> textAnnotations = await client.DetectTextAsync(await Google.Cloud.Vision.V1.Image.FromFileAsync(localImagePath));
            detectedText = textAnnotations[0].Description;
            if (detectedText != null) return detectedText;
            else throw new TextDetectorException();

        }

        /// <summary>
        /// Распознать рукописный текст
        /// </summary>
        /// <param name="localImagePath"></param>
        /// <returns></returns>
        public static async Task<string> ReadHandwrittenText(string localImagePath)
        {  
            const int numberOfCharsInOperationId = 36;
            using (Stream imageStream = File.OpenRead(localImagePath))
            {
                BatchReadFileInStreamHeaders localFileTextHeaders = await AuthenticationComputerVision.client.BatchReadFileInStreamAsync(imageStream);            
                string operationLocation = localFileTextHeaders.OperationLocation;              
                string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);
               
                int i = 0;
                int maxRetries = 10;
                ReadOperationResult results;
                do
                {
                    results = await AuthenticationComputerVision.client.GetReadOperationResultAsync(operationId);                
                    await Task.Delay(1000);
                    if (maxRetries == 9)
                    {
                        throw new TextDetectorException("Превышено время связи с сервером");
                    }
                }
                while ((results.Status == TextOperationStatusCodes.Running ||
                        results.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries);

                string textResult = null;
                foreach (TextRecognitionResult recResult in results.RecognitionResults)
                {                 
                    foreach (var line in recResult.Lines)
                    {
                        textResult += line.Text + " ";                      
                    }                
                }

                if (textResult == null)
                {
                    throw new TextDetectorException("Ничего не распознано");
                }
                else
                {
                    return textResult;
                }
            };
        }
        #endregion
    }
}
