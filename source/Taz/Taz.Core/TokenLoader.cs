using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace Taz.Core
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

        public static string GetTokenFor(User user)
        {
            return Load().First(x=>x.Name == user.ToString()).Token;
        }
    }
}
