using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Drawing;
using System.Collections.Generic;
using Google.Cloud.Vision.V1;

namespace MyApp.RecognitionClasses
{
    public static partial class TextDetector
    {
        #region Recognition Methods
        public static async Task<string> ReadPrintedText(string localImagePath)
        {
            string detectedText; 
            ImageAnnotatorClient client = await ImageAnnotatorClient.CreateAsync(); 
            IReadOnlyList<EntityAnnotation> textAnnotations = await client.DetectTextAsync(await Google.Cloud.Vision.V1.Image.FromFileAsync(localImagePath));
            detectedText = textAnnotations[0].Description;
            if (detectedText != null) return detectedText;
            else throw new TextDetectorException();

        }

        public static async Task<string> ReadHandwrittenText(string localImagePath)
        {  
            const int numberOfCharsInOperationId = 36;
            using (Stream imageStream = File.OpenRead(localImagePath))
            {
                // 
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
                        throw new TextDetectorException("Server time is out");
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

                if (textResult == null) throw new TextDetectorException("Nothing was recognised");
                else return textResult;
            };
        }
        #endregion

        #region Preprocessing Methods
        private static void Monochrome(ref Bitmap image, int level)
        {
            for (int j = 0; j < image.Height; j++)
            {
                for (int i = 0; i < image.Width; i++)
                {
                    var color = image.GetPixel(i, j);
                    int sr = (color.R + color.G + color.B) / 3;
                    image.SetPixel(i, j, (sr < level ? Color.Black : Color.White));
                }
            }
        }

        private static void MedianFiltering(Bitmap bm)
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
            {
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
        }

        static Bitmap GetImageAsBitmap(string imageFilePath)
        {
            Bitmap imageInBytes = new Bitmap(imageFilePath);
            return imageInBytes;
        }
   
        static byte[] GetImageAsByteArray(string imageFilePath)
        {          
            using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {              
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
        #endregion
    }
}
