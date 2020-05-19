using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace MyApp.RecognitionClasses
{
    public static class BanknotesDetector
    {
        static BanknotesDetector()
        {
            client = new HttpClient();
        }


        #region FieldsAndProperties
        private static HttpClient client;
        #endregion

        #region Methods

        /// <summary>
        /// Распознать номинал банкноты
        /// </summary>
        /// <param name="imageFilePath"></param>
        /// <returns></returns>
        public static async Task<string> MakeBanknotesDetectionRequest(string imageFilePath)
        {
            try
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
                    detectedBanknote = banknotesDetectorResponse.predictions[0].tagName;

                    if (detectedBanknote == "Negative")
                    {
                        return "Не удалось распознать банкноту";
                    }
                    else
                    {
                        return detectedBanknote + "рублей";
                    }
                }
            }
            catch (Exception)
            {
                throw new BanknotesDetectionException("Не удалось распознать банкноту");
            }
        }
        
        /// <summary>
        /// Представить картинку в виде байтов
        /// </summary>
        /// <param name="imageFilePath"></param>
        /// <returns></returns>
        private static byte[] GetImageAsByteArray(string imageFilePath)
        {

            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
        #endregion
    }
}
