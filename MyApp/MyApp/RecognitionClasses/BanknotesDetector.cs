using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace MyApp.RecognitionClasses
{
    class BanknotesDetector
    {
        static BanknotesDetector()
        {
            client = new HttpClient();
        }

        #region FieldsAndProperties
        private static HttpClient client;
        #endregion

        #region Methods
        public static async Task<string> MakeBanknotesDetectionRequest(string imageFilePath)
        {
            string detectedBanknote = null;
            client.DefaultRequestHeaders.Add("Prediction-Key", Constants.BanknotesKey);

            HttpResponseMessage response;
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(Constants.BanknotesURL, content);
                var json = await response.Content.ReadAsStringAsync();

                JObject jsonObj = JObject.Parse(json);
                BanknotesDetectorResponse banknotesDetectorResponse = new BanknotesDetectorResponse();
                banknotesDetectorResponse = jsonObj.ToObject<BanknotesDetectorResponse>();

                foreach (var prediction in banknotesDetectorResponse.predictions)
                {
                    if (prediction.probability > 0.5)
                    {
                        detectedBanknote = prediction.tagName;
                        break;
                    }                
                }       
            }

            if (detectedBanknote != null)
            {
                return detectedBanknote;
            }
            else
            {
                throw new BanknotesDetectionException();
            }
        }
        
        private static byte[] GetImageAsByteArray(string imageFilePath)
        {

            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
        #endregion
    }
}
