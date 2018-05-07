﻿using System;
using System.Collections.Generic;

namespace Communication
{
    [Serializable]
    public class CommObject
    {
        public struct bejelentkezesAdatok
        {
            public string azonosito, vonalkod;

            public bejelentkezesAdatok(string _azonosito, string _vonalkod)
            {
                azonosito = _azonosito;
                vonalkod = _vonalkod;
            }
        }

        public bejelentkezesAdatok bejelentkezesadatok;

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

        public struct beosztasAdatokStruct
        {
            public string dolgozoAzonosito;
            public DateTime datum;
            public int muszakSorszam;

            public beosztasAdatokStruct(string _dolgozoAzonosito, DateTime _datum, int _muszakSorszam)
            {
                dolgozoAzonosito = _dolgozoAzonosito;
                datum = _datum;
                muszakSorszam = _muszakSorszam;
            }
        }

        public List<termekAdatokStruct> termekAdatokLista = new List<termekAdatokStruct>();
        public List<beosztasAdatokStruct> beosztasokAdatokLista = new List<beosztasAdatokStruct>();
        public termekAdatokStruct termekAdatok;
        public beosztasAdatokStruct beosztasAdatok;

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
