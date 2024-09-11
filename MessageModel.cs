namespace ServerlessFunction
{
    public class MessageModel
    {
        public string type { get; set; }
        public double value1 { get; set; }
        public double value2 { get; set; }
    }

    public class ResultModel
    {
        public string type { get; set; }
        public double value1 { get; set; }
        public double value2 { get; set; }
        public double result { get; set; }
    }
}
