﻿using System;
using System.Threading.Tasks;
using AsynchronousClient;
using Communication;

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
                    FelhasznaloiInterfesz.kiir("\nJelentkezz be!\n");
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
                        FelhasznaloiInterfesz.kiir("Sikertelen bejelentkezes. Probald ujra!\n");
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
                FelhasznaloiInterfesz.kiir("4. Naplo bejegyzesek listazasa\n");
                FelhasznaloiInterfesz.kiir("5. Kijelentkezes\n");
                FelhasznaloiInterfesz.kiir("6. Kilepes\n");

                FelhasznaloiInterfesz.kiir("Valasztas sorszama: ");
                data = FelhasznaloiInterfesz.beker();
                switch (data)
                {
                    case "1":
                        addFelhasznalo();
                        break;
                    case "2":
                        modifyFelhasznalo();
                        break;
                    case "3":
                        deleteFelhasznalo();
                        break;
                    case "4":
                        logokListazasa();
                        break;
                    case "5":
                        kijelentkezes();
                        break;
                    case "6":
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
                FelhasznaloiInterfesz.kiir("4. Termek adatok modositasa\n");
                FelhasznaloiInterfesz.kiir("5. Termek torlese\n");
                FelhasznaloiInterfesz.kiir("6. Terminal beosztas letrehozasa\n");
                FelhasznaloiInterfesz.kiir("7. Terminal beosztas lekerdezese\n");
                FelhasznaloiInterfesz.kiir("8. Beosztas lekerdezese\n");
                FelhasznaloiInterfesz.kiir("9. Kijelentkezes\n");
                FelhasznaloiInterfesz.kiir("10. Kilepes\n");

                FelhasznaloiInterfesz.kiir("Valasztas sorszama: ");
                data = FelhasznaloiInterfesz.beker();
                switch (data)
                {
                    case "1":
                        behozandoTermekRegisztralasa();
                        break;
                    case "2":
                        termekekListazasa();
                        break;
                    case "3":
                        termekekSzurtListazasa();
                        break;
                    case "4":
                        termekModositas();
                        break;
                    case "5":
                        termekTorles();
                        break;
                    case "6":
                        terminalBeosztasLetrehozasa();
                        break;
                    case "7":
                        terminalBeosztasLekerdezese();
                        break;
                    case "8":
                        munkarendLekerdezes();
                        break;
                    case "9":
                        kijelentkezes();
                        break;
                    case "10":
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
                FelhasznaloiInterfesz.kiir("5. Munkarendek lekerdezese\n");
                FelhasznaloiInterfesz.kiir("6. Terminal beosztas lekerdezese\n");
                FelhasznaloiInterfesz.kiir("7. Termek behozatal\n");
                FelhasznaloiInterfesz.kiir("8. Termek kivitel\n");
                FelhasznaloiInterfesz.kiir("9. Kijelentkezes\n");
                FelhasznaloiInterfesz.kiir("10. Kilepes\n");

                FelhasznaloiInterfesz.kiir("Valasztas sorszama: ");
                data = FelhasznaloiInterfesz.beker();
                switch (data)
                {
                    case "1":
                        termekekListazasa();
                        break;
                    case "2":
                        termekekSzurtListazasa();
                        break;
                    case "3":
                        munkarendHozzaadas("bela01");
                        break;
                    case "4":
                        munkarendLekerdezes();
                        break;
                    case "5":
                        munkarendekLekerdezes();
                        break;
                    case "6":
                        terminalBeosztasLekerdezese();
                        break;
                    case "7":
                        termekBehozatal();
                        break;
                    case "8":
                        termekKivitel();
                        break;
                    case "9":
                        kijelentkezes();
                        break;
                    case "10":
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

        public void behozandoTermekRegisztralasa()
        {
            new TermekKezeles().behozandoTermekRegisztralasa();
        }

        public void termekekListazasa()
        {
            new TermekKezeles().termekekListazasa();
        }

        public void munkarendHozzaadas(string azonosito)
        {
            new MunkarendKezeles().munkarendHozzaadas();
        }

        public void munkarendLekerdezes()
        {
            new MunkarendKezeles().munkarendLekerdezes();
        }

        public void munkarendekLekerdezes()
        {
            new MunkarendKezeles().munkarendekLekerdezes();
        }

        public void addFelhasznalo()
        {
            new FelhasznaloKezeles().addFelhasznalo();
        }

        public void deleteFelhasznalo()
        {
            new FelhasznaloKezeles().deleteFelhasznalo();
        }

        public void modifyFelhasznalo()
        {
            new FelhasznaloKezeles().modifyFelhasznalo();
        }

        public void terminalBeosztasLetrehozasa()
        {
            new TerminalBeosztasKezeles().terminalBeosztasLetrehozasa();
        }

        public void terminalBeosztasLekerdezese()
        {
            new TerminalBeosztasKezeles().terminalBeosztasLekerdezese();
        }

        public void termekBehozatal()
        {
            new TermekKezeles().termekBehozatal();
        }

        public void termekKivitel()
        {
            new TermekKezeles().termekKivitel();
        }

        public void termekModositas()
        {
            new TermekKezeles().termekModositas();
        }

        public void termekTorles()
        {
            new TermekKezeles().termekTorles();
        }

        public void termekekSzurtListazasa()
        {
            new TermekKezeles().termekekSzurtListazasa();
        }

        public void logokListazasa()
        {
            new LogMegtekintes().logokListazasa();
        }
    }
}
