using Google.Apis.Auth.OAuth2;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using MyApp.Classes;
using Google.Cloud.Vision.V1;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MyApp
{
    public class AuthenticationComputerVision
    {
        public static ComputerVisionClient client = Authenticate(Constants.ComputerVisionEndpoint, Constants.ComputerVisionKey);
        static private ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
                new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
                { Endpoint = endpoint };

            return client;
        }
        public static void AuthenticationGoogleVision()
        {
           
            // var directoryname = Path.Combine(Environment.ExternalStoragePublicDirectory), "XXX");
            //Directory.CreateDirectory(directoryname);
            JSONDetectedText.GoogleAuth googleAuth = new JSONDetectedText.GoogleAuth
            {
                type = "service_account",
                project_id = "elite-contact-269918",
                private_key_id = "5d92934e4187f38df0ca58bc8679164da28f3e23",
                private_key = "-----BEGIN PRIVATE KEY-----\nMIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQDISnpA/RBNiM8a\nZxR/LdBTIXKsPqIND/TZiFet4k6OVCf3WK8Y/Ou4npW/8yWG++w1zeR0QdQuD8KN\nKaCJMwYa6DiRWehuzDlPI4vFWWUq5rxSKZGbQeLO1Rct7+KJv866F7q1tVpS45Xg\n5bRa0Uuy1NHFOfXjRb0LAujKc7TPtNz3CSdOjOvH7TuHJ1YQwFiNinfpSNk+6g7g\n5p73RGOFXW3SbOJgExsgJcKWxPHjqFvLpB+3Ht7nICLqThXeVsbEi7WZ69BP1QcU\nTkCMt9X9lQZ0zkGhKB/PYA+mSunU7F75Zt1PDYMHOhD9DyqPlhEvNR1YK1Siv4+2\neHeCXUiHAgMBAAECggEAP/Mvt6JVWzwsTBTPgY/iLTjHwWE6IouOtQoIXLeeN2l9\nXUdOmN7gcJEJ5UDAIao8EC3OHKSMF6zmhGXfIQ9OirbMoPQg94dzYOZLkVZqq9kC\nFgW5vOW7giidfwhRv4OjgN1kuvHNhhuuViFhhYG/9rhmxd8suEUjB+oE4+cvuwVG\nJj1awrm6dcsaEIDqzU1YyastsH6w/7lxt0Nj5Dn1zxI4I9v4g+SQ2TeGDYH+BEFF\nePm3LkoxsgNxbxJcplj3LXspBOf5DuJdgIbixKfAGm4rGblZ70QG3qMjcxmvpmIz\n6Bzh9pZyf30yohJKvr8jXmPhz4LHs7P6K+0Ht8t6iQKBgQD5XtGHoRRqLQ9DTYhz\nG4unzdMCXLJuhtPwE3gOOWxmANI1jhqqhG6e+NhwLRTwqsyi5wcqG+LpPaG3J4mR\nJ4a+imhLaPBjduA5A18Ts/2YUIcP34AAV4rJEcOjzI4biPAvL6mVazgvGlSzvU6N\n4LfL+Eo6oq/5JyUTvqdta6KvuwKBgQDNnaF/jLniJo/q2IGLl0hDlU0aaJoqriva\n5Ohwt5spXVhfO1PjwxAmGd1HMZfiguVduJFcB+GcRtX2k1zcmIBFar2VbROTbG8a\n70w8pYJI552+sugnH5QWkHeCfjlp9qpyVouGBYVs6Ig2N/8xKzF8XBT02O3kG63j\noRxL0Bk/pQKBgGG4iH+P3gIlmj+TmI8TEk012SSp0rqBV3hTCM76LcJjO/0ErOp5\nbygY+CgtYnFVXU/RHPuhZfBk2IR/l+csCB+O+YPjoaA4q3FsIswD0rLni6Xqvaxk\nodxde78qn92mcgjrspb9J00+EkdFPg1XLG9f4ybLLg6DCoKMSewVK/GVAoGAaQPZ\nOAvn5FlNp+RsRZ8+iUX320UReDr8qw++p9MerEHAoOLsNaeq18rd+T6bFFVGUFez\n5g0gbXDAVu9svj4lRU7/BRJwrRr0pA0UIuVlZVbmej9l0pWOTPf8Eenl6PjSlRMe\nr5SiUL1y1D5FWBd3bsyY53TkC10XsurWSOJAAwECgYBdXs2UxWrXnzddSPwF94Eh\npTsaqtzLo7hYdv9zWdvKUa0E4YNcd7XLoX1vBEBOuKoMSpQMXC5Er2JHBcs8RM5w\n2nHXLS0wUGIkiHBYpZpIctIefmgRCo9W8lV9kYKcKpZ6je0atabS3dBKRNR1pxah\nO6HKYCah5/Bqxj4EgC4Z0Q==\n-----END PRIVATE KEY-----\n",
                client_email = "iseeapp@elite-contact-269918.iam.gserviceaccount.com",
                client_id = "110715543981285850074",
                auth_uri = "https://accounts.google.com/o/oauth2/auth",
                token_uri = "https://oauth2.googleapis.com/token",
                auth_provider_x509_cert_url = "https://www.googleapis.com/oauth2/v1/certs",
                client_x509_cert_url = "https://www.googleapis.com/robot/v1/"
            };
            string jsonstring = JsonConvert.SerializeObject(googleAuth);
            SaveJsonAsync(jsonstring);
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",
               Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/AccessKGoogle.json");
        }
        public static void SaveJsonAsync(string jsonstring)
        {
            var backingFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/AccessKGoogle.json");
            using (var writer = File.CreateText(backingFile))
            {
                writer.Write(jsonstring);
            }
        }
    }
}
