﻿using System;
using System.IO;
using System.Threading.Tasks;
using Plugin.AudioRecorder;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace MyApp.RecognitionClasses
{
    public static class AudioRecording
    {
        private static string recorderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/iSee_recording.wav");

        public static AudioRecorderService recorder = new AudioRecorderService
        {
            StopRecordingAfterTimeout = true,
            TotalAudioTimeout = TimeSpan.FromSeconds(15),
            AudioSilenceTimeout = TimeSpan.FromSeconds(3)
        };

        public static string RecorderPath { get => recorderPath; set => recorderPath = value; }

        #region Methods
        /// <summary>
        /// Записать аудио с микрофона устройства
        /// </summary>
        /// <returns></returns>
        public static async Task RecordAudio()
        {
            recorder.FilePath = recorderPath;
            if (!recorder.IsRecording) //Record button clicked
            {
                var audioRecordTask = recorder.StartRecording();
                await audioRecordTask.Unwrap();
                await recorder.StopRecording();
            }
        }

        /// <summary>
        /// Проверить разрешение на использование микрофона
        /// </summary>
        /// <returns></returns>
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
        #endregion
    }
}

