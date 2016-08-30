using System;

namespace Dashboard.API.Domain
{
    public class Response : DataEntity
    {
        public ResponseType ResponseType { get; set; }
        public string ResponseId { get; set; }
        public long? InputId { get; set; }
        public string Email { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Answer { get; set; }
        public Question Question { get; set; }
    }

    [Flags]
    public enum ResponseType
    {
        text = 1,
        yesno = 2,
    }
}
