﻿using System;
using System.Collections.Generic;

namespace Taz.Core.User
{
    public static class UserResolver
    {
        #region Fields

        private static readonly IDictionary<string, User> Mappings = new Dictionary<string, User>()
        {
            { "miguelbernard", User.Miguel },
            { "yohan.belval", User.Yohan },
            { "elodie-taz", User.Elodie },
            { "phil", User.Phil },
            { "bot", User.Bot }
        };

        #endregion

        #region Methods

        public static User GetByUserName(string userId)
        {
            return Mappings[userId];
        }

        #endregion
    }
}