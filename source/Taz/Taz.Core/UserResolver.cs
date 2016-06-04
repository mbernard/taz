using System;
using System.Collections.Generic;

namespace Taz.Core
{
    public static class UserResolver
    {
        #region Fields

        private static readonly IDictionary<string, User> Mappings = new Dictionary<string, User>()
        {
            { "miguelbernard", User.Miguel },
            { "yohan.belval", User.Yohan },
            { "elodie-taz", User.Elodie },
            { "phil", User.Phil }
        };

        #endregion

        #region Methods

        public static User GetByUserId(string userId)
        {
            return Mappings[userId];
        }

        #endregion
    }
}