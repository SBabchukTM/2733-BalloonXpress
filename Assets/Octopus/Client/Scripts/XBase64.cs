using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Octopus.Client
{
    public class XBase64
    {
        public static string Encode(string data = "123")
        {
            var base64Encoded = Base64Encode(data);
            
            var outStr = "";
            
            var rand = new Random();
            
            foreach (var c in base64Encoded)
            {
                outStr += c;
                
                var randomNum = rand.Next(98, 122);
                
                var randomChar = (char)randomNum;
                
                RandomUpperChar(randomNum, ref randomChar);
                
                outStr += randomChar;
            }

            return AddSpaces(outStr);
        }

        public static string Decode(string data = "123") 
        {
            var dataBytes = Encoding.UTF8.GetBytes(RemoveSpaces(data));

            var outBytes = new List<byte>();

            for (var k = 0; k < dataBytes.Length / 2; k++) {
                outBytes.Add((byte)Encoding.UTF8.GetString(new byte[] { dataBytes[2 * k] }).ToCharArray()[0]);
            }

            while (outBytes.Count % 4 != 0) {
                outBytes.Add((byte)'=');
            }

            var outStr = Encoding.UTF8.GetString(outBytes.ToArray());
            
            var decoded = Convert.FromBase64String(outStr);
            
            return Encoding.UTF8.GetString(decoded);
        }
        
        private static string Base64Encode(string data)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);

            var base64Encoded = Convert.ToBase64String(dataBytes);

            base64Encoded = base64Encoded.TrimEnd('=');
            
            return base64Encoded;
        }
        
        private static void RandomUpperChar(int randomNum, ref char randomChar)
        {
            randomChar = randomNum % 2 == 0 ? Char.ToUpper(randomChar) : randomChar;
        }
        
        private static Random random = new Random();

        private static string AddSpaces(string input)
        {
            var output = new StringBuilder();

            for (var i = 0; i < input.Length; i++)
            {
                output.Append(input[i]);

                if (i < input.Length - 1 && random.NextDouble() < 0.2)
                {
                    output.Append(' ');
                }
            }

            return output.ToString();
        }

        private static string RemoveSpaces(string input)
        {
            var output = input.Replace(" ", "");
            
            return output;
        }
    }
}
