﻿using System;
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
            this.Items = new List<string>();
        }

        #endregion

        #region Public Properties

        public string Name { get; set; }

        public string IconEmojiName { get; set; }

        public IEnumerable<string> Items { get; set; }

        #endregion
    }
}