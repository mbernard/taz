using System;

namespace Taz.Core.Models
{
    public class Reaction
    {
        public string Name { get; set; }

        public string[] Users { get; set; }

        public int Count { get; set; }
    }
}