using posWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace resWebApp.Models
{
    public class Global
    {
        public static string APIUri = "http://localhost:7474/api/";
        //public static string APIUri = "http://192.168.1.5:4437/api/";

        #region pagination settings
        public static int rowsInPage = 5;
        #endregion
        #region folders pathes
        public const string TMPFolder = "C:/Temp/Thumb";
        public const string TMPUsersFolder = "C:/Temp/Thumb/users";
        public const string TMPAgentsFolder = "C:/Temp/Thumb/agents";
        #endregion

        public static BasicSettings basicSettings ;
    }
}