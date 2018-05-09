using System;
using System.Threading.Tasks;
using AsynchronousClient;
using Communication;

class FelhasznaloKezelese
{
    public void addFelhasznalo()
    {
        string azonosito, vonalkod, nev, jogosultsag;
        FelhasznaloiInterfesz.kiir("\nFelhasznalo Hozzaadasa:\n");
        FelhasznaloiInterfesz.kiir("Azonosito: ");
        azonosito = FelhasznaloiInterfesz.beker();
        FelhasznaloiInterfesz.kiir("Vonalkod: ");
        vonalkod = FelhasznaloiInterfesz.beker();
        FelhasznaloiInterfesz.kiir("Nev: ");
        nev = FelhasznaloiInterfesz.beker();
        FelhasznaloiInterfesz.kiir("Jogosultsag: ");
        jogosultsag = FelhasznaloiInterfesz.beker();

        CommObject commObject = new CommObject("felhasznaloHozzaadasa");
        commObject.felhasznaloAdatok = new CommObject.felhasznaloAdatokStruct(azonosito, vonalkod, nev, jogosultsag);

        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
        CommObject dResponse = tsResponse.Result;
        if (dResponse.Message == "felhasznaloHozzaadva")
            FelhasznaloiInterfesz.kiir("Felhasznalo sikeresen hozzaadva!\n");
    }
}


