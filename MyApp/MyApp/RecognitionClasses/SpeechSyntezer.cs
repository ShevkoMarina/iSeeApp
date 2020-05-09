using System.Linq;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Threading;

namespace MyApp.RecognitionClasses
{
    public class SpeechSyntezer
    {
        private static CancellationTokenSource ctsRus;
        private static CancellationTokenSource ctsEng;

        #region Methods

        /// <summary>
        /// Озвучить результат на языке устройства
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Task VoiceResult(string text)
        {
            ctsRus = new CancellationTokenSource();
            var settings = new SpeechOptions()
            {
                Volume = 1,
                Pitch = 1,
            };
            return TextToSpeech.SpeakAsync(text, settings, cancelToken: ctsRus.Token);
        }

        /// <summary>
        /// Озвучить результат на английском
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static async Task<string> VoiceResultInEnglish(string text)
        {
            ctsEng = new CancellationTokenSource();
            var locales = await TextToSpeech.GetLocalesAsync();
          
            var settings = new SpeechOptions()
            {
                Volume = 1,
                Locale = locales.Where(x => x.Language == "en").First(),
                Pitch = 1,             
            };
            await TextToSpeech.SpeakAsync(text, settings, cancelToken: ctsEng.Token);
            return locales.ToString();
        }

        /// <summary>
        /// Отменяет озвучку текста
        /// </summary>
        public static void CancelSpeech()
        {
            if (ctsRus?.IsCancellationRequested ?? true)
                return;

            ctsRus.Cancel();
        }

        public static void CancelEnglishSpeech()
        {
            if (ctsEng?.IsCancellationRequested ?? true)
                return;

            ctsEng.Cancel();
        }
        #endregion
    }
}
