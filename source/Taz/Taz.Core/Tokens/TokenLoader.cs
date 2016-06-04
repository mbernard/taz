using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Newtonsoft.Json;

namespace Taz.Core.Tokens
{
    public static class TokenLoader
    {

        public static IList<TokenEntry> Load()
        {
            List<TokenEntry> tokens;

            var assembly = Assembly.GetAssembly(typeof(TokenLoader));
            using (var reader = new StreamReader(assembly.GetManifestResourceStream("Taz.Core.Tokens.secrets.json")))
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
