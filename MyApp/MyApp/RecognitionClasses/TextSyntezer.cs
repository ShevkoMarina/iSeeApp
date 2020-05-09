using System.Linq;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Threading;

namespace MyApp.RecognitionClasses
{
    public class TextSyntezer
    {
        #region Methods
        public static async Task VoiceResult(string text)
        {
            
            var locales = await TextToSpeech.GetLocalesAsync();
            //var locale = locales.ElementAtOrDefault(8);
            var settings = new SpeechOptions()
            {
                Volume = 1,
               // Locale = locale,
                Pitch = 1,
            };
            await TextToSpeech.SpeakAsync(text, settings);
        }

        public static async Task<string> VoiceResultInEnglish(string text)
        {
            var locales = await TextToSpeech.GetLocalesAsync();
          
            var settings = new SpeechOptions()
            {
                Volume = 1,
                Locale = locales.Where(x => x.Language == "en").First(),
                Pitch = 1,             
            };
            await TextToSpeech.SpeakAsync(text, settings);
            return locales.ToString();
        }
        #endregion
    }
}
