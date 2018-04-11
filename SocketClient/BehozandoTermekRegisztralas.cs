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

    public void adatokBekerese()
    {
        string termekNev, kulsoVonalkod, megrendeloAzonosito, tipus;
        DateTime beIdopont, kiIdopont;
        int mennyiseg;
        Console.Write("Megrendelo azonosito: ");
        megrendeloAzonosito = Console.ReadLine();
        Console.Write("Termek neve: ");
        termekNev = Console.ReadLine();
        Console.Write("Kulso vonalkod: ");
        kulsoVonalkod = Console.ReadLine();
        Console.Write("Termek tipusa: "); //nem hutott NH, hutott minden HH, csak hely hutott H
        tipus = Console.ReadLine();
        Console.Write("Behozatal idopontja: ");
        DateTime.TryParse(Console.ReadLine(), out beIdopont);
        Console.Write("Kivitel idopontja: ");
        DateTime.TryParse(Console.ReadLine(), out kiIdopont);
        Console.Write("Mennyiseg: ");
        int.TryParse(Console.ReadLine(), out mennyiseg);
        if (tipus == "NH")
            szabadRaklaphelyekListazasa(false);
        else szabadRaklaphelyekListazasa(true);

        List<string> raklaphelyek = new List<string>();
        for (int i = 0; i < mennyiseg; ++i)
        {
            Console.Write(i + ". raklap: ");
            raklaphelyek.Add(Console.ReadLine());
        }
        Console.Write("Megerosit (i/h): ");
        string valasz = Console.ReadLine();
        if (valasz == "i")
            adatokRogzitese(megrendeloAzonosito, termekNev, kulsoVonalkod, tipus, beIdopont, kiIdopont, mennyiseg, raklaphelyek);
    }

    public void szabadRaklaphelyekListazasa(bool hutott)
    {
        CommObject commObject = new CommObject("szabadRaklaphelyekListazasa");
        commObject.setHutott(hutott);

        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        Console.WriteLine("Sent request, waiting for response");
        CommObject dResponse = tsResponse.Result;
        foreach (string elem in dResponse.lista)
        {
            Console.WriteLine(elem);
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
        commObject.termekAdatok = new CommObject.termekAdatokStruct(megrendeloAzonosito, termekNev, kulsoVonalkod, tipus, beIdopont, kiIdopont, mennyiseg, raklaphelyek);


        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        Console.WriteLine("Sent request, waiting for response");
        CommObject dResponse = tsResponse.Result;
    }
}