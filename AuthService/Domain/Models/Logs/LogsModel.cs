namespace AuthService.Domain.Models.Logs
{
    public class LogsModel
    {
        public int id { get; set; }
        public string date { get; set; }
        public string level { get; set; }
        public string logger { get; set; }
        public string responsetime { get; set; }
        public string message { get; set; }
        public string username { get; set; }
        public string exception { get; set; }
        public string statuscode { get; set; }
        public string url { get; set; }
        public string action { get; set; }
    }
}
