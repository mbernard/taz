using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taz.Core.Models
{
    public class Digest
    {
        #region Constructors and Destructors

        public Digest()
        {
            this.Sections = new List<Section>();
        }

        #endregion

        #region Public Properties

        public List<Section> Sections { get; set; }

        #endregion
    }

    public class Section
    {
        #region Constructors and Destructors

        public Section()
        {
            this.Items = new List<Message>();
        }

        #endregion

        #region Public Properties

        public string Name { get; set; }

        public IEnumerable<Message> Items { get; set; }
        public string IconEmoji { get; set; }
        public string Color { get; set; }
        public string TitleImageUrl { get; set; }

        #endregion
    }
}