using System;


namespace MyApp.RecognitionClasses
{
    [Serializable]
    public class BanknotesDetectionException : Exception
    {
        public BanknotesDetectionException() { }
        public BanknotesDetectionException(string message) : base(message) { }
        public BanknotesDetectionException(string message, Exception inner) : base(message, inner) { }
        protected BanknotesDetectionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
