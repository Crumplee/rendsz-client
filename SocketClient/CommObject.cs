using System;
using System.Collections.Generic;

namespace Communication
{
    [Serializable]
    public class CommObject
    {
        public string Message { get; set; }
        public bool hutott;
        public List<string> lista;
        public struct termekAdatokStruct
        {
            public string megrendeloAzonosito;
            public string termekNev;
            public string kulsoVonalkod;
            public string tipus;
            public DateTime beIdopont;
            public DateTime kiIdopont;
            public int mennyiseg;
            public List<string> raklaphelyek;
            public List<string> raklapAdatok;

            public termekAdatokStruct(string _megrendeloAzonosito,
                                string _termekNev,
                                string _kulsoVonalkod,
                                string _tipus,
                                DateTime _beIdopont,
                                DateTime _kiIdopont,
                                int _mennyiseg,
                                List<string> _raklaphelyek)
            {
                megrendeloAzonosito = _megrendeloAzonosito;
                termekNev = _termekNev;
                kulsoVonalkod = _kulsoVonalkod;
                tipus = _tipus;
                beIdopont = _beIdopont;
                kiIdopont = _kiIdopont;
                mennyiseg = _mennyiseg;
                raklaphelyek = _raklaphelyek;
                raklapAdatok = new List<string>();
            }
        }


        public List<termekAdatokStruct> termekAdatokLista = new List<termekAdatokStruct>();
        public termekAdatokStruct termekAdatok;

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
