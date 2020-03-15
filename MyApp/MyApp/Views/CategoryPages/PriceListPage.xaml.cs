using System;
using System.Threading.Tasks;
using MyApp.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.AudioRecorder;
using System.Collections;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;



namespace MyApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PriceListPage : ContentPage
    {
        public static string FilePathAudioRecorded { get; set; }
        public PriceListPage()
        {
            InitializeComponent();
      
        }
        private async Task CheckCommands(string command)
        {
            if (!TextDetector.AnyErrors)
            {
                switch (command)
                {
                    case "Open Camera.":
                        //await CameraActions.TakePhoto();
                        MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                        {
                            SaveToAlbum = false,
                            Directory = "iSeeApp",
                            Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg"
                        });

                        if (file == null)
                            return;
                        await TextSyntezer.SpeakResult("Photo was successfully uploaded");
                     //   await TextDetector.ReadPrintedText(file.Path);
                        await DisplayAlert("Recognition results", TextDetector.DetectedText, "OK");
                        await TextSyntezer.SpeakResult(TextDetector.DetectedText);
                        break;

                    case "Open Gallery.":
       
                        MediaFile photo = await CrossMedia.Current.PickPhotoAsync();
                        if (photo == null)
                            return;
                        await TextSyntezer.SpeakResult("Photo was successfully uploaded");
                     //   await TextDetector.ReadPrintedText(photo.Path);
                        await DisplayAlert("Recognition results", TextDetector.DetectedText, "OK");
                        await TextSyntezer.SpeakResult(TextDetector.DetectedText);
                        break;

                }
            }
        }

        private async void StartRecorder_Clicked(object sender, EventArgs e)
        {
            await RecordAudio2();
            await AnalyzeAudio();
        }
        private async Task AnalyzeAudio()
        {
            try
            {
                var filePath = MainPage.recorder.GetAudioFilePath();
                await TextSyntezer.SpeakResult("I'm a little bit slow. Wait please");

                if (filePath != null)
                {

                    RecordButton.IsEnabled = false;
                    MainPage.EngSpeechConf.SpeechRecognitionLanguage = "en-US";
                   
                    using (var audioInput = AudioConfig.FromWavFileInput(filePath))
                    {
                        using (var recognizer = new SpeechRecognizer(MainPage.EngSpeechConf, audioInput))
                        {

                            PhraseListGrammar phraseList = PhraseListGrammar.FromRecognizer(recognizer);
                            //phraseList.AddPhrase("Open pricelist.");

                            //await DisplayAlert("Results", "Recognizing first result...", "OK");
                            var result = await recognizer.RecognizeOnceAsync();

                            if (result.Reason == ResultReason.RecognizedSpeech)
                            {
                                await DisplayAlert("Results", $"We recognized: {result.Text}", "OK");

                                await CheckCommands(result.Text);
                            }
                            else if (result.Reason == ResultReason.NoMatch)
                            {
                                await DisplayAlert("Results", $"NOMATCH: Speech could not be recognized.", "OK");
                            }
                            else if (result.Reason == ResultReason.Canceled)
                            {
                                var cancellation = CancellationDetails.FromResult(result);
                                await DisplayAlert("Results", $"CANCELED: Reason={cancellation.Reason}", "OK");

                                if (cancellation.Reason == CancellationReason.Error)
                                {
                                    await DisplayAlert("Error", $"CANCELED: ErrorCode={cancellation.ErrorCode}", "OK");
                                    await DisplayAlert("Error", $"ErrorDetails ={cancellation.ErrorDetails}", "OK");

                                }
                            }

                            RecordButton.IsEnabled = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Occured", "OK");
            }
        }

        private async Task RecordAudio2()
        {

            try
            {
                if (!MainPage.recorder.IsRecording) //Record button clicked
                {
                    RecordButton.IsEnabled = false;

                    var audioRecordTask = await MainPage.recorder.StartRecording();
                    RecordButton.IsEnabled = true;
                    await audioRecordTask;
                    //await DisplayAlert("INFO", "Succsess recording", "OK");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Occured", "OK");
            }
        }
        private async void TakePhotoButton_Clicked(object sender, EventArgs eventArgs)
        {

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                SaveToAlbum = true,
                Directory = "MarinaApp",
                Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg"
            });

            if (file == null)
                return;
            if (SettingsPage.Language == "English")
            {
                await TextSyntezer.SpeakResult("Photo was successfully uploaded");
               // await TextDetector.ReadTextInEnglish(file.Path);
                
            }
            else
            {
                await TextSyntezer.SpeakResult("Фото успешно загружено");
              //  await TextDetector.ReadPrintedText(file.Path);
            }
            await DisplayAlert("Recognition results", TextDetector.DetectedText, "OK");
            await TextSyntezer.SpeakResult(TextDetector.DetectedText);
        }
        
        private async void GetPhotoButton_Clicked(object sender, EventArgs eventArgs)
        {
            // await CameraActions.GetPhoto();
            MediaFile photo = await CrossMedia.Current.PickPhotoAsync();
            if (photo == null)
                return;
       
            if (SettingsPage.Language == "English")
            {
                await TextSyntezer.SpeakResult("Photo was successfully uploaded");
                //await TextDetector.ReadTextInEnglish(photo.Path);

            }
            else
            {
                await TextSyntezer.SpeakResult("Фото успешно загружено");
               // await TextDetector.ReadPrintedText(photo.Path);
            }
            await DisplayAlert("Recognition results", TextDetector.DetectedText, "OK");
            await TextSyntezer.SpeakResult(TextDetector.DetectedText);
        }
    }
}