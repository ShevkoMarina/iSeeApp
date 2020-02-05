using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.SfPicker.XForms;

namespace MyApp
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {

        public static string Language { get => language; set => language = value; }

        private static int selectedItem = 1;
        private static string language = "English";

        public static int SelectedItem { get => selectedItem; private set => selectedItem = value; }
        public SettingsPage()
        {
            InitializeComponent();

            volume.Text = "Volume";
            languageLabel.Text = "Language";
            settings.Text = "Settings";
            voice.Text = "Voice Control";

            LanguageSettings language = new LanguageSettings();
            picker.ItemsSource = language.Languages;
            picker.ColumnHeaderText = "Language";
            picker.ShowColumnHeader = true;
            picker.ShowFooter = true;
            picker.IsOpen = true;
           
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            try
            {
                var message = new SmsMessage("Эта херня опять не работает!!!", "89266440212");
                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Sms is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        private void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (picker.Items[selectedIndex] == "Russian")
            {
                //monkeyNameLabel.Text = picker.Items[selectedIndex];
                volume.Text = "Громкость";
                voice.Text = "Управление голосом";
                languageLabel.Text = "Язык";
                settings.Text = "Настройки";
            }
            else
            {
                volume.Text = "Volume";
                voice.Text = "Voice Control";
                languageLabel.Text = "Language";
                settings.Text = "Settings";
            }
        }
    }
}