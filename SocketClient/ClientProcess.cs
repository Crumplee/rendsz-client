using System;
using System.Threading.Tasks;
using AsynchronousClient;
using Communication;

namespace ClientProcess
{
    class Process
    {
        public Process() { }
        public void ReadAndWrite()
        {
            Console.WriteLine("Jelentkezz be te fereg");
            Console.WriteLine("azon: ");
            string azon = Console.ReadLine();
            Console.WriteLine("vonalkod: ");
            string kod = Console.ReadLine();

            CommObject dataaa = new CommObject();
            dataaa.Message = "bejelentkezes";
            dataaa.bejelentkezesadatok = new CommObject.bejelentkezesAdatok(azon, kod);
            Task<CommObject> tsResponse = SocketClient.SendRequest(dataaa);
            Console.WriteLine("Sent request, waiting for response");
            CommObject dResponse = tsResponse.Result;
            Console.WriteLine(dResponse.Message);


            Menu();
            string data = Console.ReadLine();
            while (data != "bye")
            {
                switch(data)
                {
                    case "1":
                        //termekregisztralas
                        BehozandoTermekRegisztralas beTermekReg = new BehozandoTermekRegisztralas();
                        beTermekReg.adatokBekerese();
                        break;
                    case "2":
                        //termekekkilistazasa
                        termekekListazasa();
                        break;
                    case "3":
                        //munkarendfelvitele
                        munkarendHozzaadas("bela01");
                        break;
                    case "4":
                        //munkarendlekerdezes
                        munkarendLekerdezes();
                        break;
                    default:
                        break;
                }
                Menu();
                data = Console.ReadLine();
            }
        }

        public void Menu()
        {
            Console.WriteLine("1. Behozando termek regisztralas");
            Console.WriteLine("2. Termekek adatainak kiirasa");
            Console.WriteLine("3. Munkarend felvitele");
            Console.WriteLine("4. Munkarend lekerdezese");
        }

        public void termekekListazasa()
        {
            CommObject commObject = new CommObject("termekekListazasa");

            Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
            Console.WriteLine("Sent request, waiting for response");
            CommObject dResponse = tsResponse.Result;

            foreach(CommObject.termekAdatokStruct termek in dResponse.termekAdatokLista)
            {
                Console.WriteLine(termek.megrendeloAzonosito);
                Console.WriteLine(termek.termekNev);
                Console.WriteLine(termek.kulsoVonalkod);
                Console.WriteLine(termek.tipus);
                Console.WriteLine(termek.beIdopont);
                Console.WriteLine(termek.kiIdopont);
                Console.WriteLine(termek.mennyiseg);
                foreach(string raklap in termek.raklapAdatok)
                {
                    Console.WriteLine(raklap);
                }
                Console.WriteLine("________________________________");
            }
        }

        public void munkarendHozzaadas(string azonosito)
        {
            Console.Write("Datum: ");
            DateTime datum;
            DateTime.TryParse(Console.ReadLine(), out datum);
            Console.Write("Muszak sorszam: ");
            int muszakSorszam;
            int.TryParse(Console.ReadLine(), out muszakSorszam);


            CommObject commObject = new CommObject("munkarendHozzaadas");
            commObject.beosztasAdatok = new CommObject.beosztasAdatokStruct(azonosito, datum, muszakSorszam);

            Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
            Console.WriteLine("Sent request, waiting for response");
            CommObject dResponse = tsResponse.Result;
            Console.WriteLine(dResponse.Message);
        }

        public void munkarendLekerdezes()
        {
            CommObject commObject = new CommObject("munkarendLekerdezes");

            Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
            Console.WriteLine("Sent request, waiting for response");
            CommObject dResponse = tsResponse.Result;
            foreach(CommObject.beosztasAdatokStruct beosztasAdatok in dResponse.beosztasokAdatokLista)
            {
                Console.WriteLine(beosztasAdatok.dolgozoAzonosito);
                Console.WriteLine(beosztasAdatok.datum);
                Console.WriteLine(beosztasAdatok.muszakSorszam);
                Console.WriteLine("_________________________");
            }
        }
    }
}
