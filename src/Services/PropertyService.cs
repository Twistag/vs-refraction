using Community.VisualStudio.Toolkit;
using EnvDTE;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Refraction
{
    internal sealed class PropertyService
    {

        private const string userIdProperty = "Refraction.userId";
        private const string teamIdProperty = "Refraction.teamId";


        public static void saveUserCredentials(UserCredentials userCredentials)
        {

            SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            WritableSettingsStore userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            saveProperty(userSettingsStore, userIdProperty, userCredentials.UserId);
            saveProperty(userSettingsStore, teamIdProperty, userCredentials.TeamId);
        }

        public static UserCredentials GetUserCredentials()
        {

            SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            WritableSettingsStore userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            return new UserCredentials(getProperty(userSettingsStore, userIdProperty), getProperty(userSettingsStore, teamIdProperty));
        }

        private static void saveProperty(WritableSettingsStore userSettingsStore, string key, string value)
        {
            if (userSettingsStore.CollectionExists(key))
            {
                userSettingsStore.DeleteCollection(key);
            }
            if (key != null)
            {
                userSettingsStore.CreateCollection(key);
                userSettingsStore.SetString(key, "Value", value);
            }
        }

        private static string getProperty(WritableSettingsStore userSettingsStore, string key)
        {
            if (userSettingsStore.CollectionExists(key))
            {
                return userSettingsStore.GetString(key, "Value");
            }
            return null;
        }
    }
}
