using System;
using System.Threading.Tasks;
using Plugin.AudioRecorder;

namespace MyApp.RecognitionClasses
{
    public class AudioRec
    {
        public static string FilePathAudioRecorded { get; set; }      
        public static AudioRecorderService recorder = new AudioRecorderService
        {
            StopRecordingAfterTimeout = true,
            TotalAudioTimeout = TimeSpan.FromSeconds(15),
            AudioSilenceTimeout = TimeSpan.FromSeconds(5)
        };
        public static async Task RecordAudio()
        {
            if (!recorder.IsRecording) //Record button clicked
            {
                //recorder.StopRecordingOnSilence = TimeoutSwitch.IsToggled;
                //RecordButton.IsEnabled = false;
                //PlayButton.IsEnabled = false;
                //start recording audio
                var audioRecordTask = await recorder.StartRecording();
              
                // RecordButton.IsEnabled = true;
                await audioRecordTask;
                // RecordButton.Text = "Record";
                //  PlayButton.IsEnabled = true;
            }
            else //Stop button clicked
            {
                //RecordButton.IsEnabled = false;
                //stop the recording...
                await recorder.StopRecording();
                // RecordButton.IsEnabled = true;
            }
            FilePathAudioRecorded = recorder.GetAudioFilePath();
        }
    }
}

