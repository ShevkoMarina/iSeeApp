using System;
using System.Collections.Generic;


namespace MyApp.RecognitionClasses
{   
    public class BanknotesDetectorResponse
    {
        public string id { get; set; }
        public string project { get; set; }
        public string iteration { get; set; }
        public DateTime created { get; set; }
        public IList<Prediction> predictions { get; set; }
    }
    public class Prediction
    {
        public double probability { get; set; }
        public string tagId { get; set; }
        public string tagName { get; set; }
    }
}
