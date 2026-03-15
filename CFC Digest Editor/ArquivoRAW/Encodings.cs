using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;

namespace NUC_Raw_Tools
{
    public class Encodings
    {
        public class Naruto
        {
            public class UzumakiChronicles2
            {

                public static byte[] GetBytes(string s)
                {
                    //5080 |% 8
                    //5280 |% 4
                    //5180 |% 2
                    //5380 |% 6
                    //5480 |% l1 (6A80)
                    //5580 |% l2 (6B80)
                    //5680 |% r1
                    //5780 |% r2
                    //6880 |% r1
                    //6980 |% r2
                    var llv = new LetraseValores(File.ReadAllText("Encoding.enc", Encoding.Default));
                    var seq = new List<byte>();
                    for(int i =0;i<s.Length;i++)
                    {
                        int j = 0;
                        if (s[i].ToString() == "\n")
                        {
                            byte[] str = BitConverter.GetBytes(Convert.ToInt16("0180", 16));
                            Array.Reverse(str);
                            seq.AddRange(str);
                        }
                        foreach (var w in llv.words)
                        {
                            
                                if (s[i].ToString() == w&&s[i].ToString()!= "/")
                                {
                                    byte[] str = BitConverter.GetBytes(Convert.ToInt16(llv.vals[j], 16));
                                    Array.Reverse(str);
                                    seq.AddRange(str);
                                }
                                else if(i<s.Length-1&&s[i].ToString() + s[i + 1].ToString()==w)
                                {
                                byte[] str = BitConverter.GetBytes(Convert.ToInt16(llv.vals[j], 16));
                                Array.Reverse(str);
                                seq.AddRange(str);
                                i++;
                                }
                                else if (i < s.Length - 2 && s[i].ToString() + s[i + 1].ToString() + s[i + 2].ToString() == w)
                                {
                                byte[] str = BitConverter.GetBytes(Convert.ToInt16(llv.vals[j], 16));
                                Array.Reverse(str);
                                seq.AddRange(str);
                                i+=2;
                                }


                            j++;
                        }
                    }
                    return seq.ToArray();
                }
                    
                    
                
                public static string GetString(byte[] bytes)
                {
                    var llv = new LetraseValores(File.ReadAllText("Encoding.enc", Encoding.Default));
                    string seq = "";
                    for (int i =0;i<bytes.Length;i+=2)
                    {
                        for (int j = 0; j < llv.vals.Count; j++)
                        {

                            if (bytes[i].ToString("X2") + bytes[i + 1].ToString("X2") == llv.vals[j])
                            {

                                seq += llv.words[j];

                            }

                        }
                         if (bytes[i].ToString("X2") + bytes[i + 1].ToString("X2") == "0180")
                        {
                            seq += "\n";
                        }
                    }
                    return seq;
                }

            }

        }

    }
}
