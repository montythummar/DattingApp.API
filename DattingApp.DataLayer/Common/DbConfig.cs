using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DattingApp.API.Common
{
    public abstract class DbConfig
    {
        private static string ConnectionStringName = "Server=DESKTOP-C7SQ21L;Database=DattingApp;User Id=sa;Password=Gaurav@007;";
        public static string ConnectionString
        {
            get => ConnectionStringName;
        }
    }
}
