using System;
using System.Threading.Tasks;
using AsynchronousClient;
using Communication;

class TermekekListazasa
{
    public void termekekListazasa()
    {
        CommObject commObject = new CommObject("termekekListazasa");

        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
        CommObject dResponse = tsResponse.Result;

        int i = 1;
        foreach (CommObject.termekAdatokStruct termek in dResponse.termekAdatokLista)
        {
            FelhasznaloiInterfesz.kiir("\n" + i++ + ". termek adatai: \n");
            FelhasznaloiInterfesz.kiir("Megrendelo azonositoja: " + termek.megrendeloAzonosito + "\n");
            FelhasznaloiInterfesz.kiir("Nev: " + termek.termekNev + "\n");
            FelhasznaloiInterfesz.kiir("Kulso vonalkod: " + termek.kulsoVonalkod + "\n");
            FelhasznaloiInterfesz.kiir("Tipus: " + (termek.tipus == "H" ? "hutott" : "nem hutott") + "\n");
            FelhasznaloiInterfesz.kiir("Behozatal idopontja: " + termek.beIdopont.ToString() + "\n");
            FelhasznaloiInterfesz.kiir("Kivitel idopontja: " + termek.kiIdopont.ToString() + "\n");
            FelhasznaloiInterfesz.kiir("Mennyiseg: " + termek.mennyiseg.ToString() + "\n");

            int j = 1;
            foreach (string raklap in termek.raklapAdatok)
            {
                FelhasznaloiInterfesz.kiir(j++ + ". raklap azonositoja: " + raklap + "\n");
            }
            FelhasznaloiInterfesz.kiir("________________________________\n");
        }
    }

}
