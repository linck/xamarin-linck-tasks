using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinckTasks.Util
{
    public class SettingsUtils
    {
        private static ISettings AppSetings => CrossSettings.Current;

        const string UserIdKey = "userid";
        static readonly string UserIdDefault = string.Empty;

        const string AuthTokenKey = "authtoken";
        static readonly string AuthTokenDefault = string.Empty;

        public static string AuthToken
        {
            get { return AppSetings.GetValueOrDefault<string>(AuthTokenKey, AuthTokenDefault); }
            set { AppSetings.AddOrUpdateValue<string>(AuthTokenKey, value); }
        }

        public static string UserId
        {
            get { return AppSetings.GetValueOrDefault<string>(UserIdKey, UserIdDefault); }
            set { AppSetings.AddOrUpdateValue<string>(UserIdKey, value); }
        }

        public static bool IsLoggedIn => !string.IsNullOrWhiteSpace(UserId);
    }
}
