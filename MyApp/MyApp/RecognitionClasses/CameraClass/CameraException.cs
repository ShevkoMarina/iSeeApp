using System;

namespace MyApp.RecognitionClasses.CameraClass
{
    [Serializable]
    public class CameraException : Exception
    {
        public CameraException() { }
        public CameraException(string message) : base(message) { }
        public CameraException(string message, Exception inner) : base(message, inner) { }
        protected CameraException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

   