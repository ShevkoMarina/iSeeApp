using Microsoft.CognitiveServices.Speech;

namespace MyApp.RecognitionClasses
{
    public static class SpeechAnalyzer
    {
        private static SpeechConfig speechConfiguration = SpeechConfig.FromSubscription(Constants.SpeechKey, "eastus");

        public static SpeechConfig SpeechConfiguration { get => speechConfiguration; set => speechConfiguration = value; }

        static SpeechAnalyzer()
        {
            SpeechConfiguration.SpeechRecognitionLanguage = "ru-RU";
        }

        /// <summary>
        /// Предобработка голосовых команд
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string PreprocessingCommands(string result)
        {
            string command = result.ToLower();
            command = command.Replace(".", "").Replace("?", "").Replace("!", "").Replace(",", "");
            return command;
        }
    }
}
