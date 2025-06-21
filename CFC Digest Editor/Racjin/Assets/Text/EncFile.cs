using System;
using System.Collections.Generic;

namespace CFC_Digest_Editor.Racjin.Assets.Text
{
    public class LetraseValores
    {
        public List<string> vals;
        public List<string> words;
        public LetraseValores(string val)
        {
            vals = new List<string>();
            words = new List<string>();
            string[] separator1 = { "\r\n" };
            string[] separator2 = { "|" };
            string[] vws = val.Split(separator1, StringSplitOptions.RemoveEmptyEntries);
            foreach(var s in vws)
            {
                string[] ss = s.Split(separator2, StringSplitOptions.RemoveEmptyEntries);
                vals.Add(ss[0]);
                words.Add(ss[1]);
               
            }
            

        }

    }
}
