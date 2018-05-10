using System;
using System.Threading.Tasks;
using AsynchronousClient;
using Communication;
using System.Collections.Generic;

class TerminalBeosztasKezeles
{
    public void terminalBeosztasLetrehozasa()
    {
        //Termekek listazasa
        CommObject commObject = new CommObject("termekekListazasa");

        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
        CommObject dResponse = tsResponse.Result;

        //kilepes ha nincs termek
        if (dResponse.termekAdatokLista.Count == 0)
        {
            FelhasznaloiInterfesz.kiir("Nincs felvitt termek!");
            return;
        }

        FelhasznaloiInterfesz.kiir("\n0. Kilepes: \n");
        int i = 1;
        foreach (CommObject.termekAdatokStruct termek in dResponse.termekAdatokLista)
        {
            FelhasznaloiInterfesz.kiir(i++ + ". termek adatai: \n");
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

        //valasztas
        string valasztas;
        bool helyesAdat;
        do
        {
            helyesAdat = true;
            FelhasznaloiInterfesz.kiir("Valasztott termek sorszama: ");
            valasztas = FelhasznaloiInterfesz.beker();

            int sorszam;
            if (Int32.TryParse(valasztas, out sorszam) && sorszam >= 0 && sorszam <= dResponse.termekAdatokLista.Count)
            {
                //kilepes
                if (sorszam == 0) return;


                //Adatok bekérése
                CommObject.termekAdatokStruct termek = dResponse.termekAdatokLista[sorszam - 1];

                bool hutott = (termek.tipus == "HH");

                FelhasznaloiInterfesz.kiir("Irany? (behozatal/kivitel): ");
                string irany = FelhasznaloiInterfesz.beker();

                string date = null;

                //Irany es termek datumok szerint datum
                if (irany == "ki" && DateTime.Parse(termek.kiIdopont).ToString() != new DateTime(0).ToString())
                {
                    date = termek.kiIdopont;
                }
                else if (irany == "ki")
                {
                    FelhasznaloiInterfesz.kiir("Kerem a kiviteli datumot: ");
                    date = FelhasznaloiInterfesz.beker();
                }

                if (irany == "be" && DateTime.Parse(termek.beIdopont).ToString() != new DateTime().ToString())
                {
                    date = termek.beIdopont;
                }
                else if (irany == "be")
                {
                    FelhasznaloiInterfesz.kiir("Kerem a behozatal datumot: ");
                    date = FelhasznaloiInterfesz.beker();
                }

                //Veglegesites kuldese
                CommObject lekerdezoCommObject = new CommObject("terminalBeosztasLekerdezes");
                lekerdezoCommObject.terminalBeosztasLekerdezes = new CommObject.terminalBeosztasLekerdezesStruct("datumEsHutottseg", date, "", hutott);

                Task<CommObject> tsResponse2 = SocketClient.SendRequest(lekerdezoCommObject);
                FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
                CommObject dResponse2 = tsResponse2.Result;

                i = 1;
                foreach( CommObject.terminalBeosztasAdatokStruct terminalBeosztas in dResponse2.terminalBeosztasAdatokLista)
                {
                    FelhasznaloiInterfesz.kiir(i++ + ". terminal beosztas adatai:\n");
                    FelhasznaloiInterfesz.kiir(terminalBeosztas.terminalAzonosito + " " +
                        (terminalBeosztas.hutott ? "Hutott" : "Nem hutott") + " " +
                        terminalBeosztas.idopont + " " + terminalBeosztas.idotartamEgyseg + "\n");
                }

                //uj bekeres
                FelhasznaloiInterfesz.kiir("Kerem a terminal azonositojat: ");
                string terminalAzonosito = FelhasznaloiInterfesz.beker();
                FelhasznaloiInterfesz.kiir("Kerem a beosztas idotartam egyseg szamat: ");
                int idotartamEgyseg;
                Int32.TryParse(FelhasznaloiInterfesz.beker(), out idotartamEgyseg);

                CommObject terminalBeosztasCommObject = new CommObject("terminalBeosztasTermekhez");
                terminalBeosztasCommObject.terminalBeosztasAdatok = new CommObject.terminalBeosztasAdatokStruct(date, idotartamEgyseg, hutott, irany, termek.kulsoVonalkod, terminalAzonosito);

                Task<CommObject> tsResponse3 = SocketClient.SendRequest(terminalBeosztasCommObject);
                FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
                CommObject dResponse3 = tsResponse3.Result;
                FelhasznaloiInterfesz.kiir(dResponse3.Message + "\n");
            }
            else
            {
                Console.WriteLine("Nem megfelelo sorszam. Probald ujra!");
                helyesAdat = false;
            }
        } while (!helyesAdat);
    }

    public void terminalBeosztasLekerdezese()
    {
        bool helyesSorszam;
        do
        {
            helyesSorszam = true;
            string sorszam;
            FelhasznaloiInterfesz.kiir("\n0. Kilepes\n");
            FelhasznaloiInterfesz.kiir("1. Terminal beosztasok datum szerint\n");
            FelhasznaloiInterfesz.kiir("2. Terminal beosztasok terminal azonosito szerint\n");
            FelhasznaloiInterfesz.kiir("3. Terminal beosztasok datum es hutottseg szerint\n");

            FelhasznaloiInterfesz.kiir("Valasztas sorszama: ");
            sorszam = FelhasznaloiInterfesz.beker();
            switch (sorszam)
            {
                case "0":
                    return;
                case "1":
                    terminalBeosztasLekerdezesDatumSzerint();
                    break;
                case "2":
                    terminalBeosztasLekerdezesTerminalSzerint();
                    break;
                case "3":
                    terminalBeosztasLekerdezesDatumEsHutottsegSzerint();
                    break;
                default:
                    FelhasznaloiInterfesz.kiir("Nem ertelmezheto a sorszam. Probald ujra!\n\n");
                    helyesSorszam = false;
                    break;
            }
        } while (!helyesSorszam);
    }

    public void terminalBeosztasLekerdezesDatumSzerint()
    {
        DateTime datum;
        FelhasznaloiInterfesz.kiir("Kerem a datumot: ");
        if (DateTime.TryParse(FelhasznaloiInterfesz.beker(), out datum)) {
            //kuldes
            CommObject commObject = new CommObject("terminalBeosztasLekerdezes");
            commObject.terminalBeosztasLekerdezes = new CommObject.terminalBeosztasLekerdezesStruct("datum", datum.ToString(), "", true);

            Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
            FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
            CommObject dResponse = tsResponse.Result;

            int i = 1;
            foreach (CommObject.terminalBeosztasAdatokStruct terminalBeosztas in dResponse.terminalBeosztasAdatokLista)
            {
                FelhasznaloiInterfesz.kiir(i++ + ". terminal beosztas adatai:\n");
                FelhasznaloiInterfesz.kiir(terminalBeosztas.terminalAzonosito + " " +
                    (terminalBeosztas.hutott ? "Hutott" : "Nem hutott") + " " +
                    terminalBeosztas.idopont + " " + terminalBeosztas.idotartamEgyseg + "\n");
            }
        }
        else
        {
            FelhasznaloiInterfesz.kiir("Hibas datum formatum!");
        }
    }

    public void terminalBeosztasLekerdezesTerminalSzerint()
    {
        FelhasznaloiInterfesz.kiir("Kerem a terminal azonositot: ");
        string azonosito = FelhasznaloiInterfesz.beker();
        
        //kuldes
        CommObject commObject = new CommObject("terminalBeosztasLekerdezes");
        commObject.terminalBeosztasLekerdezes = new CommObject.terminalBeosztasLekerdezesStruct("terminal", null, azonosito, true);

        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
        CommObject dResponse = tsResponse.Result;

        int i = 1;
        foreach (CommObject.terminalBeosztasAdatokStruct terminalBeosztas in dResponse.terminalBeosztasAdatokLista)
        {
            FelhasznaloiInterfesz.kiir(i++ + ". terminal beosztas adatai:\n");
            FelhasznaloiInterfesz.kiir(terminalBeosztas.terminalAzonosito + " " +
                (terminalBeosztas.hutott ? "Hutott" : "Nem hutott") + " " +
                terminalBeosztas.idopont + " " + terminalBeosztas.idotartamEgyseg + "\n");
        }
    }

    public void terminalBeosztasLekerdezesDatumEsHutottsegSzerint()
    {
        DateTime datum;
        FelhasznaloiInterfesz.kiir("Kerem a datumot: ");
        if (DateTime.TryParse(FelhasznaloiInterfesz.beker(), out datum))
        {
            //hutott-e
            FelhasznaloiInterfesz.kiir("Hutott? (i/h): ");
            bool hutott = FelhasznaloiInterfesz.beker() == "i" ? true : false;

            //kuldes
            CommObject commObject = new CommObject("terminalBeosztasLekerdezes");
            commObject.terminalBeosztasLekerdezes = new CommObject.terminalBeosztasLekerdezesStruct("datumEsHutottseg", datum.ToString(), "", hutott);

            Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
            FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
            CommObject dResponse = tsResponse.Result;

            int i = 1;
            foreach (CommObject.terminalBeosztasAdatokStruct terminalBeosztas in dResponse.terminalBeosztasAdatokLista)
            {
                FelhasznaloiInterfesz.kiir(i++ + ". terminal beosztas adatai:\n");
                FelhasznaloiInterfesz.kiir(terminalBeosztas.terminalAzonosito + " " +
                    (terminalBeosztas.hutott ? "Hutott" : "Nem hutott") + " " +
                    terminalBeosztas.idopont + " " + terminalBeosztas.idotartamEgyseg + "\n");
            }
        }
        else
        {
            FelhasznaloiInterfesz.kiir("Hibas datum formatum!");
        }
    }
}
