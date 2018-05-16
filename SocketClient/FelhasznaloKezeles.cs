using System;
using System.Threading.Tasks;
using AsynchronousClient;
using Communication;

class FelhasznaloKezeles
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

        bool helyesAdat;
        do
        {
            helyesAdat = true;
            FelhasznaloiInterfesz.kiir("Jogosultsagok:\n");
            FelhasznaloiInterfesz.kiir("1. adminisztrator\n");
            FelhasznaloiInterfesz.kiir("2. diszpecser\n");
            FelhasznaloiInterfesz.kiir("3. muszakvezeto\n");
            FelhasznaloiInterfesz.kiir("4. raktaros\n");
            FelhasznaloiInterfesz.kiir("Kerem a jogosultsag sorszamat: ");

            FelhasznaloiInterfesz.kiir("Valasztas sorszama: ");
            jogosultsag = FelhasznaloiInterfesz.beker();
            switch (jogosultsag)
            {
                case "1":
                    jogosultsag = "adminisztrator";
                    break;
                case "2":
                    jogosultsag = "diszpecser";
                    break;
                case "3":
                    jogosultsag = "muszakvezeto";
                    break;
                case "4":
                    jogosultsag = "raktaros";
                    break;
                default:
                    FelhasznaloiInterfesz.kiir("Nem ertelmezheto a sorszam. Probald ujra!\n");
                    helyesAdat = false;
                    break;
            }
        } while (!helyesAdat);

        CommObject commObject = new CommObject("felhasznaloHozzaadasa");
        commObject.felhasznaloAdatok = new CommObject.felhasznaloAdatokStruct(azonosito, vonalkod, nev, jogosultsag);

        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
        CommObject dResponse = tsResponse.Result;
        if (dResponse.Message == "felhasznaloHozzaadva")
            FelhasznaloiInterfesz.kiir("Felhasznalo sikeresen hozzaadva!\n");
    }

    public void deleteFelhasznalo()
    {
        CommObject commObject = new CommObject("felhasznalokListazasa");
        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
        CommObject dResponse = tsResponse.Result;

        FelhasznaloiInterfesz.kiir("Azonosito\tNev\tJogosultsag\n\n");
        FelhasznaloiInterfesz.kiir("0. Kilepes\n");

        int i = 1;
        foreach(CommObject.felhasznaloAdatokStruct felhasznaloAdat in dResponse.felhasznalokLista)
        {
            FelhasznaloiInterfesz.kiir(i++ + ". " + felhasznaloAdat.azonosito + "\t" + felhasznaloAdat.nev + "\t" + felhasznaloAdat.jogosultsag + "\n");
        }

        string valasztas;
        bool helyesAdat;
        do
        {
            helyesAdat = true;
            FelhasznaloiInterfesz.kiir("Valasztas sorszama: ");
            valasztas = FelhasznaloiInterfesz.beker();

            int sorszam;
            if (Int32.TryParse(valasztas, out sorszam) && sorszam >= 0 && sorszam <= dResponse.felhasznalokLista.Count)
            {
                //kilepes
                if (sorszam == 0) return;

                FelhasznaloiInterfesz.kiir("Megerosit (i/h): ");
                string valasz = FelhasznaloiInterfesz.beker();
                if (valasz == "i")
                {
                    //Veglegesites kuldese
                    CommObject torloCommObject = new CommObject("felhasznaloTorlese");
                    torloCommObject.felhasznaloAdatok = dResponse.felhasznalokLista[sorszam - 1];

                    Task<CommObject> tsResponse2 = SocketClient.SendRequest(torloCommObject);
                    FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
                    CommObject dResponse2 = tsResponse2.Result;
                    if (dResponse2.Message == "felhasznaloTorolve")
                        FelhasznaloiInterfesz.kiir("Felhasznalo sikeresen torolve!\n");
                }
            }
            else
            {
                FelhasznaloiInterfesz.kiir("Nem megfelelo sorszam. Probald ujra!\n");
                helyesAdat = false;
            }
        } while (!helyesAdat);
    }

    public void modifyFelhasznalo()
    {
        CommObject commObject = new CommObject("felhasznalokListazasa");
        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
        CommObject dResponse = tsResponse.Result;

        FelhasznaloiInterfesz.kiir("Azonosito\tNev\tJogosultsag\n\n");
        FelhasznaloiInterfesz.kiir("0. Kilepes\n");

        int i = 1;
        foreach (CommObject.felhasznaloAdatokStruct felhasznaloAdat in dResponse.felhasznalokLista)
        {
            FelhasznaloiInterfesz.kiir(i++ + ". " + felhasznaloAdat.azonosito + "\t" + felhasznaloAdat.nev + "\t" + felhasznaloAdat.jogosultsag + "\n");
        }

        string valasztas;
        bool helyesAdat;
        do
        {
            helyesAdat = true;
            FelhasznaloiInterfesz.kiir("Valasztas sorszama: ");
            valasztas = FelhasznaloiInterfesz.beker();

            int sorszam;
            if (Int32.TryParse(valasztas, out sorszam) && sorszam >= 0 && sorszam <= dResponse.felhasznalokLista.Count)
            {
                //kilepes
                if (sorszam == 0) return;

                CommObject.felhasznaloAdatokStruct ujAdatok = dResponse.felhasznalokLista[sorszam - 1];
                FelhasznaloiInterfesz.kiir("Uj nev (Hagyja uresen, ha nem szeretne valtoztatni): ");
                string ujNev = FelhasznaloiInterfesz.beker();
                if (ujNev == "")
                    ujNev = ujAdatok.nev;
                else
                    ujAdatok.nev = ujNev;

                //Josogultsag bekerese
                string ujJogosultsag;
                do
                {
                    helyesAdat = true;
                    FelhasznaloiInterfesz.kiir("Jogosultsagok:\n");
                    FelhasznaloiInterfesz.kiir("1. adminisztrator\n");
                    FelhasznaloiInterfesz.kiir("2. diszpecser\n");
                    FelhasznaloiInterfesz.kiir("3. muszakvezeto\n");
                    FelhasznaloiInterfesz.kiir("4. raktaros\n");
                    FelhasznaloiInterfesz.kiir("Uj jogosultsag sorszama (Hagyja uresen, ha nem szeretne valtoztatni): ");

                    ujJogosultsag = FelhasznaloiInterfesz.beker();
                    switch (ujJogosultsag)
                    {
                        case "":
                            break;
                        case "1":
                            ujJogosultsag = "adminisztrator";
                            break;
                        case "2":
                            ujJogosultsag = "diszpecser";
                            break;
                        case "3":
                            ujJogosultsag = "muszakvezeto";
                            break;
                        case "4":
                            ujJogosultsag = "raktaros";
                            break;
                        default:
                            FelhasznaloiInterfesz.kiir("Nem ertelmezheto a sorszam. Probald ujra!\n");
                            helyesAdat = false;
                            break;
                    }
                } while (!helyesAdat);

                if (ujJogosultsag == "")
                    ujJogosultsag = ujAdatok.jogosultsag;
                else
                    ujAdatok.jogosultsag = ujJogosultsag;

                //megerosites
                FelhasznaloiInterfesz.kiir("Megerosit (i/h): ");
                string valasz = FelhasznaloiInterfesz.beker();
                if (valasz == "i")
                {
                    //Veglegesites kuldese
                    CommObject modositoCommObject = new CommObject("felhasznaloModositasa");
                    modositoCommObject.felhasznaloAdatok = ujAdatok;

                    Task<CommObject> tsResponse2 = SocketClient.SendRequest(modositoCommObject);
                    FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
                    CommObject dResponse2 = tsResponse2.Result;
                    if (dResponse2.Message == "felhasznaloModositva")
                        FelhasznaloiInterfesz.kiir("Felhasznalo sikeresen modositva!\n");
                }
            }
            else
            {
                FelhasznaloiInterfesz.kiir("Nem megfelelo sorszam. Probald ujra!\n");
                helyesAdat = false;
            }
        } while (!helyesAdat);
    }
}


