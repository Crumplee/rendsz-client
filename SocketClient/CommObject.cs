using System;

namespace Communication
{
    [Serializable]
    public class CommObject
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public CommObject() { }
        public CommObject(string msg)
        {
            this.Message = msg;
            this.Date = DateTime.Now;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
