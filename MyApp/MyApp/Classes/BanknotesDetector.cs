using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace MyApp.Classes
{
   
    class BanknotesDetector
    {
        public static string DetectedBanknote { get; set; }

        public static async Task MakePredictionRequest(string imageFilePath)
        {
  

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Prediction-Key", Constants.BanknotesKey);

            HttpResponseMessage response;
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(Constants.BanknotesURL, content);
                var json = await response.Content.ReadAsStringAsync();

                JObject jObj = JObject.Parse(json);
                BanknotesDetectorResponse example = new BanknotesDetectorResponse();
                example = jObj.ToObject<BanknotesDetectorResponse>();
                foreach (var r in example.predictions)
                {
                    if (r.probability > 0.5)
                    {
                        DetectedBanknote = r.tagName;
                        break;
                    }
                }
                
            }        
        }
        
        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
        public class Prediction
        {
            public double probability { get; set; }
            public string tagId { get; set; }
            public string tagName { get; set; }
        }

        public class BanknotesDetectorResponse
        {
            public string id { get; set; }
            public string project { get; set; }
            public string iteration { get; set; }
            public DateTime created { get; set; }
            public IList<Prediction> predictions { get; set; }
        }

    }
}
