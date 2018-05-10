using System;
using System.Threading.Tasks;
using AsynchronousClient;
using Communication;

class MunkarendKezeles
{
    public void munkarendHozzaadas()
    {
        FelhasznaloiInterfesz.kiir("Kerem az azonositot: ");
        string azonosito = FelhasznaloiInterfesz.beker();
        FelhasznaloiInterfesz.kiir("Datum: ");
        DateTime datum;
        DateTime.TryParse(FelhasznaloiInterfesz.beker(), out datum);
        FelhasznaloiInterfesz.kiir("Muszak sorszama: ");
        int muszakSorszam;
        int.TryParse(FelhasznaloiInterfesz.beker(), out muszakSorszam);


        CommObject commObject = new CommObject("munkarendHozzaadas");
        commObject.beosztasAdatok = new CommObject.beosztasAdatokStruct(azonosito, datum, muszakSorszam);

        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
        CommObject dResponse = tsResponse.Result;
        if (dResponse.Message == "munkarendHozzaadva")
            FelhasznaloiInterfesz.kiir("Munkarend sikeresen hozzaadva!\n");
    }

    public void munkarendLekerdezes()
    {
        CommObject commObject = new CommObject("munkarendLekerdezes");

        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
        CommObject dResponse = tsResponse.Result;

        int i = 1;
        foreach (CommObject.beosztasAdatokStruct beosztasAdatok in dResponse.beosztasokAdatokLista)
        {
            FelhasznaloiInterfesz.kiir("\n" + i++ + ". beosztas: \n");
            FelhasznaloiInterfesz.kiir("Dolgozo azonositoja: " + beosztasAdatok.dolgozoAzonosito + "\n");
            FelhasznaloiInterfesz.kiir("Datum: " + beosztasAdatok.datum.ToString() + "\n");
            FelhasznaloiInterfesz.kiir("Muszak sorszama: " + beosztasAdatok.muszakSorszam.ToString() + "\n");
            FelhasznaloiInterfesz.kiir("_________________________\n");
        }
    }
}
