using System;
using System.Threading.Tasks;
using AsynchronousClient;
using Communication;
using System.Collections.Generic;

class BehozandoTermekRegisztralas
{
    public BehozandoTermekRegisztralas()
    {

    }

    public void behozandoTermekRegisztralasa()
    {
        string termekNev, kulsoVonalkod, megrendeloAzonosito, tipus;
        DateTime beIdopont, kiIdopont;
        int mennyiseg;
        FelhasznaloiInterfesz.kiir("Megrendelo azonosito: ");
        megrendeloAzonosito = FelhasznaloiInterfesz.beker();
        FelhasznaloiInterfesz.kiir("Termek neve: ");
        termekNev = FelhasznaloiInterfesz.beker();
        FelhasznaloiInterfesz.kiir("Kulso vonalkod: ");
        kulsoVonalkod = FelhasznaloiInterfesz.beker();
        FelhasznaloiInterfesz.kiir("Termek tipusa: "); //nem hutott NH, hutott minden HH, csak hely hutott H
        tipus = FelhasznaloiInterfesz.beker();
        FelhasznaloiInterfesz.kiir("Behozatal idopontja: ");
        DateTime.TryParse(FelhasznaloiInterfesz.beker(), out beIdopont);
        FelhasznaloiInterfesz.kiir("Kivitel idopontja: ");
        DateTime.TryParse(FelhasznaloiInterfesz.beker(), out kiIdopont);
        FelhasznaloiInterfesz.kiir("Mennyiseg: ");
        int.TryParse(FelhasznaloiInterfesz.beker(), out mennyiseg);
        if (tipus == "NH")
            szabadRaklaphelyekListazasa(false);
        else szabadRaklaphelyekListazasa(true);

        List<string> raklaphelyek = new List<string>();
        for (int i = 0; i < mennyiseg; ++i)
        {
            FelhasznaloiInterfesz.kiir(i + ". raklap: ");
            raklaphelyek.Add(FelhasznaloiInterfesz.beker());
        }
        FelhasznaloiInterfesz.kiir("Megerosit (i/h): ");
        string valasz = FelhasznaloiInterfesz.beker();
        if (valasz == "i")
            adatokRogzitese(megrendeloAzonosito, termekNev, kulsoVonalkod, tipus, beIdopont, kiIdopont, mennyiseg, raklaphelyek);
    }

    public void szabadRaklaphelyekListazasa(bool hutott)
    {
        CommObject commObject = new CommObject("szabadRaklaphelyekListazasa");
        commObject.hutott = hutott;

        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
        CommObject dResponse = tsResponse.Result;

        foreach (string elem in dResponse.lista)
        {
            FelhasznaloiInterfesz.kiir(elem + "\n");
        }
        
    }

    public void adatokRogzitese(string megrendeloAzonosito,
                                string termekNev,
                                string kulsoVonalkod,
                                string tipus, 
                                DateTime beIdopont, 
                                DateTime kiIdopont, 
                                int mennyiseg, 
                                List<string> raklaphelyek)
    {
        CommObject commObject = new CommObject("behozandoTermekRogzitese");
        commObject.termekAdatok = new CommObject.termekAdatokStruct(megrendeloAzonosito, termekNev, kulsoVonalkod, tipus, beIdopont.ToString(), kiIdopont.ToString(), mennyiseg, raklaphelyek);
        
        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
        CommObject dResponse = tsResponse.Result;
        FelhasznaloiInterfesz.kiir(dResponse.Message + "\n");
    }
}