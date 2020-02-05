using System.Linq;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace MyApp.Classes
{
    public class TextSyntezer
    {
        public static async Task SpeakResult(string text)
        {
            var locales = await TextToSpeech.GetLocalesAsync();
            var locale = locales.ElementAtOrDefault(1);
            int n = locales.Count();
            var settings = new SpeechOptions()
            {
                Volume = 1,
                Locale = locale,
                Pitch = 1,
            };
            await TextToSpeech.SpeakAsync(text, settings);
        }
    }
}
