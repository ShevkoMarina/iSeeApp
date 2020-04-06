using System;

namespace MyApp.RecognitionClasses
{
    [Serializable]
    public class TextDetectorException : Exception
    {
        public TextDetectorException() { }
        public TextDetectorException(string message) : base(message) { }
        public TextDetectorException(string message, Exception inner) : base(message, inner) { }
        protected TextDetectorException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }  
}
