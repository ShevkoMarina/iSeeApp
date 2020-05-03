using System;
using System.Threading.Tasks;
using Plugin.AudioRecorder;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace MyApp.RecognitionClasses
{
    public static class AudioRecording
    {
        public static AudioRecorderService recorder = new AudioRecorderService
        {
            StopRecordingAfterTimeout = true,
            TotalAudioTimeout = TimeSpan.FromSeconds(15),
            AudioSilenceTimeout = TimeSpan.FromSeconds(3)
        };

        public static async Task<string> RecordAudio()
        {
            recorder.FilePath = "/data/user/0/com.companyname.myapp/cache/ARS_recording.wav";
            if (!recorder.IsRecording) //Record button clicked
            {
                //recorder.StopRecordingOnSilence = TimeoutSwitch.IsToggled;
                //RecordButton.IsEnabled = false;
                //PlayButton.IsEnabled = false;
                //start recording audio
     
                var audioRecordTask = recorder.StartRecording();
                // RecordButton.IsEnabled = true;
                await audioRecordTask.Unwrap();
                // RecordButton.Text = "Record";
                //  PlayButton.IsEnabled = true;
                await recorder.StopRecording();
             
            }

            return recorder.FilePath;
        }

        public static async Task<bool> CheckAudioPermissions()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone);
            if (status != PermissionStatus.Granted)
            {
                await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Microphone);             
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Microphone);
                if (results.ContainsKey(Permission.Microphone))
                {
                    status = results[Permission.Microphone];
                }
            }
            return status == PermissionStatus.Granted;
        }
    }
}

