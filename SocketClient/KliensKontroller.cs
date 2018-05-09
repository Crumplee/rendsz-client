using System;
using System.Threading.Tasks;
using AsynchronousClient;
using Communication;
using UI;

namespace KliensKontroller
{
    class Process
    {
        bool kijelentkezett;
        bool kilepett = false;
        public Process() { }
        public void ReadAndWrite()
        {
            do
            {
                kijelentkezett = false;
                CommObject dResponse = null;
                string azon = null;
                do
                {
                    FelhasznaloiInterfesz.kiir("\nJelentkezz be te fereg\n");
                    FelhasznaloiInterfesz.kiir("Azonosito (kilepeshez 'bye'): ");
                    azon = FelhasznaloiInterfesz.beker();
                    // Kilepes
                    if (azon == "bye") return;

                    FelhasznaloiInterfesz.kiir("Vonalkod: ");
                    string kod = FelhasznaloiInterfesz.beker();

                    CommObject dataaa = new CommObject();
                    dataaa.Message = "bejelentkezes";
                    dataaa.bejelentkezesadatok = new CommObject.bejelentkezesAdatok(azon, kod);
                    Task<CommObject> tsResponse = SocketClient.SendRequest(dataaa);

                    FelhasznaloiInterfesz.kiir("Sent request, waiting for response...\n");
                    dResponse = tsResponse.Result;
                    if (dResponse.Message == "hiba")
                    {
                        FelhasznaloiInterfesz.kiir("Sikertelen bejelentkezes! Probald ujra...te fereg!\n");
                    }

                } while (dResponse.Message == "hiba");

                switch (dResponse.Message)
                {
                    case "adminisztrator":
                        MenuAdminisztratorhoz();
                        break;
                    case "diszpecser":
                        MenuDiszpecserhez();
                        break;
                    case "muszakvezeto":
                        MenuMuszakvezetohoz();
                        break;
                    case "raktaros":
                        MenuRaktaroshoz();
                        break;
                }
            } while (kijelentkezett);
        }

        public void MenuAdminisztratorhoz()
        {
            string data = null;
            do
            {
                FelhasznaloiInterfesz.kiir("\n1. Felhasznalo hozzaadasa\n");
                FelhasznaloiInterfesz.kiir("2. Felhasznalo modositasa\n");
                FelhasznaloiInterfesz.kiir("3. Felhasznalo torlese\n");
                FelhasznaloiInterfesz.kiir("4. Kijelentkezes\n");
                FelhasznaloiInterfesz.kiir("5. Kilepes\n");

                FelhasznaloiInterfesz.kiir("Valasztas sorszama: ");
                data = FelhasznaloiInterfesz.beker();
                switch (data)
                {
                    case "1":
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    case "4":
                        kijelentkezes();
                        break;
                    case "5":
                        kilepes();
                        break;
                    default:
                        FelhasznaloiInterfesz.kiir("Nem ertelmezheto a sorszam. Probald ujra!\n");
                        break;
                }
            } while (!kilepett && !kijelentkezett);
        }

        public void MenuDiszpecserhez()
        {
            string data = null;
            do
            {
                FelhasznaloiInterfesz.kiir("\n1. Behozando termek regisztralas\n");
                FelhasznaloiInterfesz.kiir("2. Termekek listazasa\n");
                FelhasznaloiInterfesz.kiir("3. Termekek szurt listazasa\n");
                FelhasznaloiInterfesz.kiir("4. Terminal beosztas letrehozasa\n");
                FelhasznaloiInterfesz.kiir("5. Terminal beosztas lekerdezese\n");
                FelhasznaloiInterfesz.kiir("6. Beosztas lekerdezese\n");
                FelhasznaloiInterfesz.kiir("7. Kijelentkezes\n");
                FelhasznaloiInterfesz.kiir("8. Kilepes\n");

                FelhasznaloiInterfesz.kiir("Valasztas sorszama: ");
                data = FelhasznaloiInterfesz.beker();
                switch (data)
                {
                    case "1":
                        BehozandoTermekRegisztralas beTermekReg = new BehozandoTermekRegisztralas();
                        beTermekReg.adatokBekerese();
                        break;
                    case "2":
                        termekekListazasa();
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                    case "5":
                        break;
                    case "6":
                        munkarendLekerdezes();
                        break;
                    case "7":
                        kijelentkezes();
                        break;
                    case "8":
                        kilepes();
                        break;
                    default:
                        FelhasznaloiInterfesz.kiir("Nem ertelmezheto a sorszam. Probald ujra!\n\n");
                        break;
                }
            } while (!kilepett && !kijelentkezett);
        }

        public void MenuMuszakvezetohoz()
        {
            string data = null;
            do
            {
                FelhasznaloiInterfesz.kiir("\n1. Termekek listazasa\n");
                FelhasznaloiInterfesz.kiir("2. Termekek szurt listazasa\n");
                FelhasznaloiInterfesz.kiir("3. Munkarend hozzaadasa\n");
                FelhasznaloiInterfesz.kiir("4. Munkarend lekerdezese\n");
                FelhasznaloiInterfesz.kiir("5. Terminal beosztas lekerdezese\n");
                FelhasznaloiInterfesz.kiir("6. Termek behozatal\n");
                FelhasznaloiInterfesz.kiir("7. Termek kivitel\n");
                FelhasznaloiInterfesz.kiir("8. Kijelentkezes\n");
                FelhasznaloiInterfesz.kiir("9. Kilepes\n");

                FelhasznaloiInterfesz.kiir("Valasztas sorszama: ");
                data = FelhasznaloiInterfesz.beker();
                switch (data)
                {
                    case "1":
                        termekekListazasa();
                        break;
                    case "2":
                        break;
                    case "3":
                        munkarendHozzaadas("bela01");
                        break;
                    case "4":
                        munkarendLekerdezes();
                        break;
                    case "5":
                        break;
                    case "6":
                        break;
                    case "7":
                        break;
                    case "8":
                        kijelentkezes();
                        break;
                    case "9":
                        kilepes();
                        break;
                    default:
                        FelhasznaloiInterfesz.kiir("Nem ertelmezheto a sorszam. Probald ujra!\n\n");
                        break;
                }
            } while (!kilepett && !kijelentkezett);
        }

        public void MenuRaktaroshoz()
        {
            string data = null;
            do
            {
                FelhasznaloiInterfesz.kiir("\n1. Munkarend lekerdezese\n");
                FelhasznaloiInterfesz.kiir("2. Kijelentkezes\n");
                FelhasznaloiInterfesz.kiir("3. Kilepes\n");

                FelhasznaloiInterfesz.kiir("Valasztas sorszama: ");
                data = FelhasznaloiInterfesz.beker();
                switch (data)
                {
                    case "1":
                        munkarendLekerdezes();
                        break;
                    case "2":
                        kijelentkezes();
                        break;
                    case "3":
                        kilepes();
                        break;
                    default:
                        FelhasznaloiInterfesz.kiir("Nem ertelmezheto a sorszam. Probald ujra!\n");
                        break;
                }
            } while (!kilepett && !kijelentkezett);
        }

        public void kilepes()
        {
            kilepett = true;
        }
        public void kijelentkezes()
        {
            CommObject commObject = new CommObject("kijelentkezes");
            Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
            FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
            CommObject dResponse = tsResponse.Result;

            if (dResponse.Message == "kijelentkezes_sikeres") kijelentkezett = true;
            FelhasznaloiInterfesz.kiir("Sikeres kijelentkezes!\n");
        }

        public void termekekListazasa()
        {
            CommObject commObject = new CommObject("termekekListazasa");

            Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
            FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
            CommObject dResponse = tsResponse.Result;

            int i = 1;
            foreach(CommObject.termekAdatokStruct termek in dResponse.termekAdatokLista)
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
                foreach(string raklap in termek.raklapAdatok)
                {
                    FelhasznaloiInterfesz.kiir(j++ + ". raklap azonositoja: " + raklap + "\n");
                }
                FelhasznaloiInterfesz.kiir("________________________________\n");
            }
        }

        public void munkarendHozzaadas(string azonosito)
        {
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
            foreach(CommObject.beosztasAdatokStruct beosztasAdatok in dResponse.beosztasokAdatokLista)
            {
                FelhasznaloiInterfesz.kiir("\n" + i++ + ". beosztas: \n");
                FelhasznaloiInterfesz.kiir("Dolgozo azonositoja: " + beosztasAdatok.dolgozoAzonosito + "\n");
                FelhasznaloiInterfesz.kiir("Datum: " + beosztasAdatok.datum.ToString() + "\n");
                FelhasznaloiInterfesz.kiir("Muszak sorszama: " + beosztasAdatok.muszakSorszam.ToString() + "\n");
                FelhasznaloiInterfesz.kiir("_________________________\n");
            }
        }
    }
}
