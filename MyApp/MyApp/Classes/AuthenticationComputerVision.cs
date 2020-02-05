using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using MyApp.Classes;

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
    }
}
