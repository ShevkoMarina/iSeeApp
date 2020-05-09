using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyApp.RecognitionClasses
{
    class CommandsChecker
    {
        public static SpeechConfig SpeechConfig;
        private static List<string> commandList = new List<string>()
        {
            "деньги", "печатный", "рукописный", "камера", "галерея", "помощь", "настройки"
        };

        public static List<string> CommandList { get => commandList; set => commandList = value; }

        public static async Task AnalizeCommand(string filePath)
        {
            try
            {   
                if (!String.IsNullOrEmpty(filePath))
                {
                    SpeechConfig.SpeechRecognitionLanguage = "ru-RU";
                    using (var audioInput = AudioConfig.FromWavFileInput(filePath))
                    {
                        using (var recognizer = new SpeechRecognizer(SpeechConfig, audioInput))
                        {
                            /*
                            PhraseListGrammar phraseList = PhraseListGrammar.FromRecognizer(recognizer);
                            phraseList.AddPhrase("Pricelist.");
                            phraseList.AddPhrase("Camera.");
                            phraseList.AddPhrase("Gallery.");
                            */
                            //await DisplayAlert("Results", "Recognizing first result...", "OK");
                            var result = await recognizer.RecognizeOnceAsync();

                            if (result.Reason == ResultReason.RecognizedSpeech)
                            {
                                //await DisplayAlert("Results", $"We recognized: {result.Text}", "OK");

                               // await CheckCommands(result.Text);
                            }
                            else if (result.Reason == ResultReason.NoMatch)
                            {
                                //await DisplayAlert("Results", $"NOMATCH: Speech could not be recognized.", "OK");
                            }
                            else if (result.Reason == ResultReason.Canceled)
                            {
                                var cancellation = CancellationDetails.FromResult(result);
                                //await DisplayAlert("Results", $"CANCELED: Reason={cancellation.Reason}", "OK");

                                if (cancellation.Reason == CancellationReason.Error)
                                {
                                   //await DisplayAlert("Error", $"CANCELED: ErrorCode={cancellation.ErrorCode}", "OK");
                                    //await DisplayAlert("Error", $"ErrorDetails ={cancellation.ErrorDetails}", "OK");

                                }
                            }

                           // RecordButton.IsEnabled = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
               // await DisplayAlert("Error", "Occured", "OK");
            }
        }
    }
}
