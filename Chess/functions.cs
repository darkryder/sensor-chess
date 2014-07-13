using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    public static class functions
    {
        public static bool liesBetween(int a, int b, int c)
        {
            if (((a >= b) && (a <= c)) == true)
            {
                return true;
            }
            return false;

        }
    }
}