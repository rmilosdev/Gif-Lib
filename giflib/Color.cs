using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace giflib
{
    public class Color
    {
        public Dictionary<string, string> Colors;

        public Color()
        {
            Colors = new Dictionary<string, string>();
            Colors.Add("Aqua", "#59b3b3");
            Colors.Add("Blue", "#5976b3");
            Colors.Add("Purple", "#7e59b3");
            Colors.Add("Fucshia", "#b35986");
            Colors.Add("Orange", "#b36859");
            Colors.Add("Gold", "#b38f59");
        }
    }
}