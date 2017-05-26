using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaHouseManage.Common.Common
{
    public class CopterBool
    {
        public CopterBool()
        { }

        static CopterBool()
        {
            False = false;
            True = true;
        }

        public static bool False { get; }

        public static bool True { get; }

    }
}
