using System;

namespace Communication
{
    [Serializable]
    public class CommObject
    {
        public string Message { get; set; }
        public bool hutott;




        public CommObject() { }
        public CommObject(string msg)
        {
            this.Message = msg;
        }
        
        public void setHutott(bool value)
        {
            hutott = value;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
