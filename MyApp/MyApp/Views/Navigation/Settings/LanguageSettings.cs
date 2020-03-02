using System.Collections.ObjectModel;

namespace MyApp
{
    public partial class SettingsPage
    {
        public class LanguageSettings
        {
            private ObservableCollection<string> language;

            public ObservableCollection<string> Languages
            {
                get { return language; }
                set { language = value; }
            }
            public LanguageSettings()
            {
                Languages = new ObservableCollection<string>();
                Languages.Add("Russian");
                Languages.Add("English");
            }
        }
    }
}