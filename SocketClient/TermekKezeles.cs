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
        string beTerminalAzonosito = "";
        int beIdotartamEgyseg = 0;
        string kiTerminalAzonosito = "";
        int kiIdotartamEgyseg = 0;

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
        //Behozatali terminal
        if (beIdopont.ToString() != new DateTime().ToString())
        {
            CommObject beCommObject = new CommObject("terminalBeosztasokLekerdezes");
            beCommObject.terminalBeosztasLekerdezes = new CommObject.terminalBeosztasLekerdezesStruct("datumEsHutottseg", beIdopont.ToString(), "", (tipus == "HH" || tipus == "H"));

            Task<CommObject> tsResponse2 = SocketClient.SendRequest(beCommObject);
            FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
            CommObject beResponse = tsResponse2.Result;

            int i = 1;
            foreach (CommObject.terminalBeosztasAdatokStruct terminalBeosztas in beResponse.terminalBeosztasAdatokLista)
            {
                FelhasznaloiInterfesz.kiir(i++ + ". terminal beosztas adatai:\n");
                FelhasznaloiInterfesz.kiir(terminalBeosztas.terminalAzonosito + " " +
                    (terminalBeosztas.hutott ? "Hutott" : "Nem hutott") + " " +
                    terminalBeosztas.idopont + " " + terminalBeosztas.idotartamEgyseg + "\n");
            }

            //uj bekeres
            FelhasznaloiInterfesz.kiir("Kerem a terminal azonositojat: ");
            beTerminalAzonosito = FelhasznaloiInterfesz.beker();
            FelhasznaloiInterfesz.kiir("Kerem a beosztas idotartam egyseg szamat: ");
            Int32.TryParse(FelhasznaloiInterfesz.beker(), out beIdotartamEgyseg);
        }


        FelhasznaloiInterfesz.kiir("Kivitel idopontja: ");
        DateTime.TryParse(FelhasznaloiInterfesz.beker(), out kiIdopont);

        //Kiviteli terminal
        if (kiIdopont.ToString() != new DateTime().ToString())
        {
            CommObject kiCommObject = new CommObject("terminalBeosztasokLekerdezes");
            kiCommObject.terminalBeosztasLekerdezes = new CommObject.terminalBeosztasLekerdezesStruct("datumEsHutottseg", kiIdopont.ToString(), "", (tipus == "HH" || tipus == "H"));

            Task<CommObject> tsResponse2 = SocketClient.SendRequest(kiCommObject);
            FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
            CommObject kiResponse = tsResponse2.Result;

            int i = 1;
            foreach (CommObject.terminalBeosztasAdatokStruct terminalBeosztas in kiResponse.terminalBeosztasAdatokLista)
            {
                FelhasznaloiInterfesz.kiir(i++ + ". terminal beosztas adatai:\n");
                FelhasznaloiInterfesz.kiir(terminalBeosztas.terminalAzonosito + " " +
                    (terminalBeosztas.hutott ? "Hutott" : "Nem hutott") + " " +
                    terminalBeosztas.idopont + " " + terminalBeosztas.idotartamEgyseg + "\n");
            }

            //uj bekeres
            FelhasznaloiInterfesz.kiir("Kerem a terminal azonositojat: ");
            kiTerminalAzonosito = FelhasznaloiInterfesz.beker();
            FelhasznaloiInterfesz.kiir("Kerem a beosztas idotartam egyseg szamat: ");
            Int32.TryParse(FelhasznaloiInterfesz.beker(), out kiIdotartamEgyseg);
        }

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
        else return;
        //be kuldes
        if (beIdopont.ToString() != new DateTime().ToString())
        {
            CommObject terminalBeosztasCommObject = new CommObject("terminalBeosztasTermekhez");
            terminalBeosztasCommObject.terminalBeosztasAdatok = new CommObject.terminalBeosztasAdatokStruct(beIdopont.ToString(), beIdotartamEgyseg, (tipus == "HH" || tipus == "H"), "be", kulsoVonalkod, beTerminalAzonosito);

            Task<CommObject> tsResponse3 = SocketClient.SendRequest(terminalBeosztasCommObject);
            FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
            CommObject dResponse3 = tsResponse3.Result;
            FelhasznaloiInterfesz.kiir(dResponse3.Message + "\n");
        }

        //ki kuldes
        if (kiIdopont.ToString() != new DateTime().ToString())
        {
            CommObject terminalBeosztasCommObject = new CommObject("terminalBeosztasTermekhez");
            terminalBeosztasCommObject.terminalBeosztasAdatok = new CommObject.terminalBeosztasAdatokStruct(kiIdopont.ToString(), kiIdotartamEgyseg, (tipus == "HH" || tipus == "H"), "be", kulsoVonalkod, kiTerminalAzonosito);

            Task<CommObject> tsResponse3 = SocketClient.SendRequest(terminalBeosztasCommObject);
            FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
            CommObject dResponse3 = tsResponse3.Result;
            FelhasznaloiInterfesz.kiir(dResponse3.Message + "\n");
        }
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

    public void termekKivitel()
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
                    tmpList.Add(new CommObject.mozgoRaklapAdatokStruct(raklap.raklap, false, epseg));
                }

                //kuldes
                CommObject commObject2 = new CommObject("termekKivitel");
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

    public void termekModositas()
    {
        CommObject commObject = new CommObject("termekekListazasa");

        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
        CommObject dResponse = tsResponse.Result;

        FelhasznaloiInterfesz.kiir("\n0. kilepes\n");
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
        bool helyesSorszam;

        do
        {
            helyesSorszam = true;
            FelhasznaloiInterfesz.kiir("Kerem a termek sorszamat: ");
            string valasztas = FelhasznaloiInterfesz.beker();
            int sorszam;

            if (Int32.TryParse(valasztas, out sorszam) && sorszam >= 0 && sorszam <= dResponse.termekAdatokLista.Count)
            {
                if (sorszam == 0) return;

                CommObject.termekAdatokStruct ujAdatok = dResponse.termekAdatokLista[sorszam - 1];

                FelhasznaloiInterfesz.kiir("Uj megrendelo azonosito (Hagyja uresen, ha nem szeretne valtoztatni): ");
                string ujMegrendeloAzonosito = FelhasznaloiInterfesz.beker();
                if (ujMegrendeloAzonosito == "")
                    ujMegrendeloAzonosito = ujAdatok.megrendeloAzonosito;
                else
                    ujAdatok.megrendeloAzonosito = ujMegrendeloAzonosito;

                FelhasznaloiInterfesz.kiir("Uj termek nev (Hagyja uresen, ha nem szeretne valtoztatni): ");
                string ujTermekNev = FelhasznaloiInterfesz.beker();
                if (ujTermekNev == "")
                    ujTermekNev = ujAdatok.termekNev;
                else
                    ujAdatok.termekNev = ujTermekNev;

                FelhasznaloiInterfesz.kiir("Uj kulso vonalkod (Hagyja uresen, ha nem szeretne valtoztatni): ");
                string ujKulsoVonalkod = FelhasznaloiInterfesz.beker();
                if (ujKulsoVonalkod == "")
                    ujKulsoVonalkod = ujAdatok.kulsoVonalkod;
                else
                    ujAdatok.kulsoVonalkod = ujKulsoVonalkod;

                FelhasznaloiInterfesz.kiir("Uj behozatal idopont (Hagyja uresen, ha nem szeretne valtoztatni): ");
                string ujBeIdopont = FelhasznaloiInterfesz.beker();
                string beTerminalAzonosito = null;
                int beIdotartamEgyseg = 0;

                DateTime ujBeIdopontDateTime = new DateTime();
                if (ujBeIdopont == "" || DateTime.Parse(ujBeIdopont).ToString() == ujAdatok.beIdopont)
                    ujBeIdopont = ujAdatok.beIdopont;
                else
                {
                    ujAdatok.beIdopont = ujBeIdopont;
                    //Terminalok
                    DateTime.TryParse(ujBeIdopont, out ujBeIdopontDateTime);
                    //Behozatali terminal
                    if (ujBeIdopontDateTime.ToString() != new DateTime().ToString())
                    {
                        CommObject beCommObject = new CommObject("terminalBeosztasokLekerdezes");
                        beCommObject.terminalBeosztasLekerdezes = new CommObject.terminalBeosztasLekerdezesStruct("datumEsHutottseg", ujBeIdopontDateTime.ToString(), "", (ujAdatok.tipus == "HH" || ujAdatok.tipus == "H"));

                        Task<CommObject> tsResponse2 = SocketClient.SendRequest(beCommObject);
                        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
                        CommObject beResponse = tsResponse2.Result;

                        i = 1;
                        foreach (CommObject.terminalBeosztasAdatokStruct terminalBeosztas in beResponse.terminalBeosztasAdatokLista)
                        {
                            FelhasznaloiInterfesz.kiir(i++ + ". terminal beosztas adatai:\n");
                            FelhasznaloiInterfesz.kiir(terminalBeosztas.terminalAzonosito + " " +
                                (terminalBeosztas.hutott ? "Hutott" : "Nem hutott") + " " +
                                terminalBeosztas.idopont + " " + terminalBeosztas.idotartamEgyseg + "\n");
                        }

                        //uj bekeres
                        FelhasznaloiInterfesz.kiir("Kerem a terminal azonositojat: ");
                        beTerminalAzonosito = FelhasznaloiInterfesz.beker();
                        FelhasznaloiInterfesz.kiir("Kerem a beosztas idotartam egyseg szamat: ");
                        Int32.TryParse(FelhasznaloiInterfesz.beker(), out beIdotartamEgyseg);
                    }
                }

                FelhasznaloiInterfesz.kiir("Uj kiviteli idopont (Hagyja uresen, ha nem szeretne valtoztatni): ");
                string ujKiIdopont = FelhasznaloiInterfesz.beker();
                string kiTerminalAzonosito = null;
                int kiIdotartamEgyseg = 0;

                DateTime ujKiIdopontDateTime = new DateTime();
                if (ujKiIdopont == "" || DateTime.Parse(ujKiIdopont).ToString() == ujAdatok.kiIdopont)
                    ujKiIdopont = ujAdatok.kiIdopont;
                else
                {
                    ujAdatok.kiIdopont = ujKiIdopont;
                    DateTime.TryParse(ujKiIdopont, out ujKiIdopontDateTime);
                    //Kihozatali terminal
                    if (ujKiIdopontDateTime.ToString() != new DateTime().ToString())
                    {
                        CommObject beCommObject = new CommObject("terminalBeosztasokLekerdezes");
                        beCommObject.terminalBeosztasLekerdezes = new CommObject.terminalBeosztasLekerdezesStruct("datumEsHutottseg", ujKiIdopontDateTime.ToString(), "", (ujAdatok.tipus == "HH" || ujAdatok.tipus == "H"));

                        Task<CommObject> tsResponse2 = SocketClient.SendRequest(beCommObject);
                        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
                        CommObject beResponse = tsResponse2.Result;

                        i = 1;
                        foreach (CommObject.terminalBeosztasAdatokStruct terminalBeosztas in beResponse.terminalBeosztasAdatokLista)
                        {
                            FelhasznaloiInterfesz.kiir(i++ + ". terminal beosztas adatai:\n");
                            FelhasznaloiInterfesz.kiir(terminalBeosztas.terminalAzonosito + " " +
                                (terminalBeosztas.hutott ? "Hutott" : "Nem hutott") + " " +
                                terminalBeosztas.idopont + " " + terminalBeosztas.idotartamEgyseg + "\n");
                        }

                        //uj bekeres
                        FelhasznaloiInterfesz.kiir("Kerem a terminal azonositojat: ");
                        kiTerminalAzonosito = FelhasznaloiInterfesz.beker();
                        FelhasznaloiInterfesz.kiir("Kerem a beosztas idotartam egyseg szamat: ");
                        Int32.TryParse(FelhasznaloiInterfesz.beker(), out kiIdotartamEgyseg);
                    }
                }

                //megerosites
                FelhasznaloiInterfesz.kiir("Megerosit (i/h): ");
                string valasz = FelhasznaloiInterfesz.beker();
                if (valasz == "i")
                {
                    //modositas kuldese

                    CommObject modifyingCommObject = new CommObject("termekModositas");
                    modifyingCommObject.termekAdatok = ujAdatok;
                    modifyingCommObject.termekAzonosito = dResponse.termekAdatokLista[sorszam - 1].kulsoVonalkod;

                    Task<CommObject> modifyingtsResponse = SocketClient.SendRequest(modifyingCommObject);
                    FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
                    CommObject modifyingResponse = modifyingtsResponse.Result;
                    FelhasznaloiInterfesz.kiir(modifyingResponse.Message + "\n");
                }
                else return;
                //be kuldes
                if (beTerminalAzonosito != null)
                {
                    CommObject terminalBeosztasCommObject = new CommObject("terminalBeosztasTermekhez");
                    terminalBeosztasCommObject.terminalBeosztasAdatok = new CommObject.terminalBeosztasAdatokStruct(ujBeIdopontDateTime.ToString(), beIdotartamEgyseg, (ujAdatok.tipus == "HH" || ujAdatok.tipus == "H"), "be", ujKulsoVonalkod, beTerminalAzonosito);

                    Task<CommObject> tsResponse3 = SocketClient.SendRequest(terminalBeosztasCommObject);
                    FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
                    CommObject dResponse3 = tsResponse3.Result;
                    FelhasznaloiInterfesz.kiir(dResponse3.Message + "\n");
                }

                //ki kuldes
                if (kiTerminalAzonosito != null)
                {
                    CommObject terminalBeosztasCommObject = new CommObject("terminalBeosztasTermekhez");
                    terminalBeosztasCommObject.terminalBeosztasAdatok = new CommObject.terminalBeosztasAdatokStruct(ujKiIdopontDateTime.ToString(), kiIdotartamEgyseg, (ujAdatok.tipus == "HH" || ujAdatok.tipus == "H"), "be", ujKulsoVonalkod, kiTerminalAzonosito);

                    Task<CommObject> tsResponse3 = SocketClient.SendRequest(terminalBeosztasCommObject);
                    FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
                    CommObject dResponse3 = tsResponse3.Result;
                    FelhasznaloiInterfesz.kiir(dResponse3.Message + "\n");
                }

            }
            else
            {
                FelhasznaloiInterfesz.kiir("Nem megfelelo sorszam. Probald ujra!\n");
                helyesSorszam = false;
            }
        } while (!helyesSorszam);
    }

    public void termekTorles()
    {
        CommObject commObject = new CommObject("termekekListazasa");

        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
        CommObject dResponse = tsResponse.Result;

        FelhasznaloiInterfesz.kiir("\n0. kilepes\n");
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
        bool helyesSorszam;

        do
        {
            helyesSorszam = true;
            FelhasznaloiInterfesz.kiir("Kerem a termek sorszamat: ");
            string valasztas = FelhasznaloiInterfesz.beker();
            int sorszam;

            if (Int32.TryParse(valasztas, out sorszam) && sorszam >= 0 && sorszam <= dResponse.termekAdatokLista.Count)
            {
                if (sorszam == 0) return;

                FelhasznaloiInterfesz.kiir("Megerosit (i/h): ");
                string valasz = FelhasznaloiInterfesz.beker();
                if (valasz == "i")
                {
                    CommObject torlesCommObject = new CommObject("termekTorlese");
                    torlesCommObject.termekAzonosito = dResponse.termekAdatokLista[sorszam - 1].kulsoVonalkod;

                    Task<CommObject> torlestsResponse = SocketClient.SendRequest(torlesCommObject);
                    FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
                    CommObject torlesdResponse = torlestsResponse.Result;
                }
                else return;
            }
            else
            {
                FelhasznaloiInterfesz.kiir("Nem megfelelo sorszam. Probald ujra!\n");
                helyesSorszam = false;
            }
        } while (!helyesSorszam);
    }

    public void termekekSzurtListazasa()
    {
        CommObject.termekAdatokStruct szurok = new CommObject.termekAdatokStruct();

        FelhasznaloiInterfesz.kiir("Nev szuro (Hagyja uresen, ha nem szeretne megadni): ");
        string nev = FelhasznaloiInterfesz.beker();
        if (nev == "")
            szurok.termekNev = null;
        else
            szurok.termekNev = nev;

        FelhasznaloiInterfesz.kiir("Kulso vonalkod szuro (Hagyja uresen, ha nem szeretne megadni): ");
        string kulsoVonalkod = FelhasznaloiInterfesz.beker();
        if (kulsoVonalkod == "")
            szurok.kulsoVonalkod = null;
        else
            szurok.kulsoVonalkod = nev;

        FelhasznaloiInterfesz.kiir("Behozatal datum szuro (Hagyja uresen, ha nem szeretne megadni): ");
        string beDatum = FelhasznaloiInterfesz.beker();
        if (beDatum == "")
            szurok.beIdopont = new DateTime().ToString();
        else
            szurok.beIdopont = DateTime.Parse(beDatum).ToString();

        FelhasznaloiInterfesz.kiir("Kivitel datum szuro (Hagyja uresen, ha nem szeretne megadni): ");
        string kiDatum = FelhasznaloiInterfesz.beker();
        if (kiDatum == "")
            szurok.kiIdopont = new DateTime().ToString();
        else
            szurok.kiIdopont = DateTime.Parse(kiDatum).ToString();

        FelhasznaloiInterfesz.kiir("Tipus szuro (Hagyja uresen, ha nem szeretne megadni): ");
        string tipus = FelhasznaloiInterfesz.beker();
        if (tipus == "")
            szurok.tipus = null;
        else
            szurok.tipus = tipus;

        FelhasznaloiInterfesz.kiir("Raklap azonosito szuro (Hagyja uresen, ha nem szeretne megadni): ");
        string raklapAzonosito = FelhasznaloiInterfesz.beker();
        if (raklapAzonosito == "")
        {
            szurok.raklapAdatok = new List<string>();
            szurok.raklapAdatok.Add(null);
        }
        else
        {
            szurok.raklapAdatok = new List<string>();
            szurok.raklapAdatok.Add(raklapAzonosito);
        }

        FelhasznaloiInterfesz.kiir("Megrendelo azonosito szuro (Hagyja uresen, ha nem szeretne megadni): ");
        string megrendeloAzonosito = FelhasznaloiInterfesz.beker();
        if (megrendeloAzonosito == "")
            szurok.megrendeloAzonosito = null;
        else
            szurok.megrendeloAzonosito = megrendeloAzonosito;

        CommObject szuroCommObject = new CommObject("termekSzurese");
        szuroCommObject.termekAdatok = szurok;

        Task<CommObject> szurotsResponse = SocketClient.SendRequest(szuroCommObject);
        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
        CommObject szurodResponse = szurotsResponse.Result;

        FelhasznaloiInterfesz.kiir(szurodResponse.termekAdatokLista.Count.ToString());
        int i = 1;
        foreach (CommObject.termekAdatokStruct termek in szurodResponse.termekAdatokLista)
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
