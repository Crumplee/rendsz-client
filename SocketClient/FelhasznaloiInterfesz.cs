using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
    public static class FelhasznaloiInterfesz
    {
        public static void kiir(string data)
        {
            Console.Write(data);
        }

        public static string beker()
        {
            return Console.ReadLine();
        }
    }
}
