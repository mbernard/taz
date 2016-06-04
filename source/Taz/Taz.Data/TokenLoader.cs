using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taz.Data
{
    public static class TokenLoader
    {

        public static IList<TokenEntry> Load(string path = "../../../../../secrets.json")
        {
            List<TokenEntry> tokens;

            using (var reader = new StreamReader(path))
            { 
                tokens = JsonConvert.DeserializeObject<List<TokenEntry>>(reader.ReadToEnd());
            }

            return tokens;
        }
    }
}
