using System;
using System.Threading.Tasks;
using AsynchronousClient;
using Communication;
using System.Collections.Generic;

class TermekKezeles
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
            FelhasznaloiInterfesz.kiir((i+1) + ". raklap azonositoja: ");
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

        FelhasznaloiInterfesz.kiir("Raklap azonositok: \n");
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

    public void termekBehozatal()
    {
        FelhasznaloiInterfesz.kiir("Kerem a termek azonositojat: ");
        string termekAzonosito = FelhasznaloiInterfesz.beker();

        DateTime datum;
        bool helyesFormatum;
        do
        {
            helyesFormatum = true;
            FelhasznaloiInterfesz.kiir("Kerem a datumot: ");
            if (DateTime.TryParse(FelhasznaloiInterfesz.beker(), out datum))
            {
                FelhasznaloiInterfesz.kiir("Kerem a terminal azonositot: ");
                string terminalAzonosito = FelhasznaloiInterfesz.beker();

                //kuldes
                CommObject commObject = new CommObject("termekMozgatasLekerdezes");
                commObject.termekMozgatasLekerdezes = new CommObject.termekMozgatasLekerdezesStruct(termekAzonosito, datum.ToString(), terminalAzonosito);

                Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
                FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
                CommObject dResponse = tsResponse.Result;

                //uj lista feltoltese
                List<CommObject.mozgoRaklapAdatokStruct> tmpList = new List<CommObject.mozgoRaklapAdatokStruct>();
                foreach (CommObject.mozgoRaklapAdatokStruct raklap in dResponse.mozgoRaklapAdatok)
                {
                    FelhasznaloiInterfesz.kiir(raklap.raklap + " allapota: ");
                    string epseg = FelhasznaloiInterfesz.beker();
                    FelhasznaloiInterfesz.kiir("Raklap beerkezett? (i/h): ");
                    bool bejott = (FelhasznaloiInterfesz.beker() == "i" ? true : false);
                    tmpList.Add(new CommObject.mozgoRaklapAdatokStruct(raklap.raklap, bejott, epseg));
                }

                //kuldes
                CommObject commObject2 = new CommObject("termekBehozatal");
                commObject2.mozgoRaklapAdatok = tmpList;
                commObject2.termekAzonosito = dResponse.termekAzonosito;

                Task<CommObject> tsResponse2 = SocketClient.SendRequest(commObject2);
                FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
                CommObject dResponse2 = tsResponse.Result;
                FelhasznaloiInterfesz.kiir(dResponse2.Message);

            }
            else
            {
                FelhasznaloiInterfesz.kiir("Hibas datum formatum! Probald ujra!\n");
                helyesFormatum = false;
            }
        } while (!helyesFormatum);
    }
}
