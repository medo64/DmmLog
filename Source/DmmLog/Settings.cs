using System;
using System.Collections.Generic;
using System.Text;

namespace DmmLog {
    internal static class Settings {

        public static String SidebarFontName {
            get { return "Arial"; }
        }

        public static Int32 SidebarDigitCount {
            get { return 4; }
        }

        public static Boolean SidebarSlidingDecimalPoint {
            get { return true; }
        }

        public static Boolean SidebarSlidingMinusSign {
            get { return true; }
        }

    }
}
