using System;
using System.Threading.Tasks;
using AsynchronousClient;
using Communication;

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

    }

    public void szabadRaklaphelyekListazasa(bool hutott)
    {
        CommObject commObject = new CommObject("szabadRaklaphelyekListazasa");
        commObject.setHutott(hutott);
        Console.Write("asdasdasdasdasdasdasd");

        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        Console.WriteLine("Sent request, waiting for response");
        CommObject dResponse = tsResponse.Result;
        Console.WriteLine("Received response: " + dResponse);
    }
}