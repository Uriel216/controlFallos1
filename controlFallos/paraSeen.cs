using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlFallos
{
    class paraSeen
    {
    private static int res { set; get; }
        public static void asignarres(int resul)
        {
          res += resul;

        }
        public static void borrarres()
        {
            res = 0;
        }
        public static int getRes()
        {
            return res;

        }
    }
}
