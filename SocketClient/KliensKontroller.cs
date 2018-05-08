using System;
using System.Threading.Tasks;
using AsynchronousClient;
using Communication;
using UI;

namespace KliensKontroller
{
    class Process
    {
        public Process() { }
        public void ReadAndWrite()
        {
            CommObject dResponse = null;
            do
            {
                FelhasznaloiInterfesz.kiir("Jelentkezz be te fereg\n");
                FelhasznaloiInterfesz.kiir("Azonosito: ");
                string azon = FelhasznaloiInterfesz.beker();
                FelhasznaloiInterfesz.kiir("Vonalkod: ");
                string kod = FelhasznaloiInterfesz.beker();

                CommObject dataaa = new CommObject();
                dataaa.Message = "bejelentkezes";
                dataaa.bejelentkezesadatok = new CommObject.bejelentkezesAdatok(azon, kod);
                Task<CommObject> tsResponse = SocketClient.SendRequest(dataaa);
                
                FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
                dResponse = tsResponse.Result;
                if(dResponse.Message == "hiba")
                {
                    FelhasznaloiInterfesz.kiir("Sikertelen bejelentkezes! Probald ujra...te fereg!\n");
                }

            } while (dResponse.Message == "hiba");

            switch(dResponse.Message)
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
        }

        public void MenuAdminisztratorhoz()
        {
            string data = null;
            do
            {
                FelhasznaloiInterfesz.kiir("1. Felhasznalo hozzaadasa\n");
                FelhasznaloiInterfesz.kiir("2. Felhasznalo modositasa\n");
                FelhasznaloiInterfesz.kiir("3. Felhasznalo torlese\n");

                data = FelhasznaloiInterfesz.beker();
                switch (data)
                {
                    case "1":
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    default:
                        break;
                }
            } while (data != "bye");
        }

        public void MenuDiszpecserhez()
        {
            string data = null;
            do
            {
                FelhasznaloiInterfesz.kiir("1. Behozando termek regisztralas\n");
                FelhasznaloiInterfesz.kiir("2. Termekek listazasa\n");
                FelhasznaloiInterfesz.kiir("3. Termekek szurt listazasa\n");
                FelhasznaloiInterfesz.kiir("4. Terminal beosztas letrehozasa\n");
                FelhasznaloiInterfesz.kiir("5. Terminal beosztas lekerdezese\n");
                FelhasznaloiInterfesz.kiir("6. Beosztas lekerdezese\n");

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
                    default:
                        break;
                }
            } while (data != "bye");
        }

        public void MenuMuszakvezetohoz()
        {
            string data = null;
            do
            {
                FelhasznaloiInterfesz.kiir("1. Termekek listazasa\n");
                FelhasznaloiInterfesz.kiir("2. Termekek szurt listazasa\n");
                FelhasznaloiInterfesz.kiir("3. Munkarend hozzaadasa\n");
                FelhasznaloiInterfesz.kiir("4. Munkarend lekerdezese\n");
                FelhasznaloiInterfesz.kiir("5. Terminal beosztas lekerdezese\n");
                FelhasznaloiInterfesz.kiir("6. Termek behozatal\n");
                FelhasznaloiInterfesz.kiir("7. Termek kivitel\n");

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
                    default:
                        break;
                }
            } while (data != "bye");
        }

        public void MenuRaktaroshoz()
        {
            string data = null;
            do
            {
                FelhasznaloiInterfesz.kiir("1. Munkarend lekerdezese\n");

                data = FelhasznaloiInterfesz.beker();
                switch (data)
                {
                    case "1":
                        munkarendLekerdezes();
                        break;
                    default:
                        break;
                }
            } while (data != "bye");
        }

        public void termekekListazasa()
        {
            CommObject commObject = new CommObject("termekekListazasa");

            Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
            FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
            CommObject dResponse = tsResponse.Result;

            foreach(CommObject.termekAdatokStruct termek in dResponse.termekAdatokLista)
            {
                FelhasznaloiInterfesz.kiir(termek.megrendeloAzonosito + "\n");
                FelhasznaloiInterfesz.kiir(termek.termekNev + "\n");
                FelhasznaloiInterfesz.kiir(termek.kulsoVonalkod + "\n");
                FelhasznaloiInterfesz.kiir(termek.tipus + "\n");
                FelhasznaloiInterfesz.kiir(termek.beIdopont.ToString() + "\n");
                FelhasznaloiInterfesz.kiir(termek.kiIdopont.ToString() + "\n");
                FelhasznaloiInterfesz.kiir(termek.mennyiseg.ToString() + "\n");
                foreach(string raklap in termek.raklapAdatok)
                {
                    FelhasznaloiInterfesz.kiir(raklap + "\n");
                }
                FelhasznaloiInterfesz.kiir("________________________________\n");
            }
        }

        public void munkarendHozzaadas(string azonosito)
        {
            FelhasznaloiInterfesz.kiir("Datum: ");
            DateTime datum;
            DateTime.TryParse(FelhasznaloiInterfesz.beker(), out datum);
            FelhasznaloiInterfesz.kiir("Muszak sorszam: ");
            int muszakSorszam;
            int.TryParse(FelhasznaloiInterfesz.beker(), out muszakSorszam);


            CommObject commObject = new CommObject("munkarendHozzaadas");
            commObject.beosztasAdatok = new CommObject.beosztasAdatokStruct(azonosito, datum, muszakSorszam);

            Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
            FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
            CommObject dResponse = tsResponse.Result;
            FelhasznaloiInterfesz.kiir(dResponse.Message + "\n");
        }

        public void munkarendLekerdezes()
        {
            CommObject commObject = new CommObject("munkarendLekerdezes");

            Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
            FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
            CommObject dResponse = tsResponse.Result;
            foreach(CommObject.beosztasAdatokStruct beosztasAdatok in dResponse.beosztasokAdatokLista)
            {
                FelhasznaloiInterfesz.kiir(beosztasAdatok.dolgozoAzonosito + "\n");
                FelhasznaloiInterfesz.kiir(beosztasAdatok.datum.ToString() + "\n");
                FelhasznaloiInterfesz.kiir(beosztasAdatok.muszakSorszam.ToString() + "\n");
                FelhasznaloiInterfesz.kiir("_________________________\n");
            }
        }
    }
}
