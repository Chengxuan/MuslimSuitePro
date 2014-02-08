using System;
using Windows.Storage;

namespace MuslimSuitePro
{
    class AppSettings
    {
        public AppSettings()
        { 
            //empty contstructor
        }

        public static void saveSettings(string key, string value)
        {
            try
            {
                ApplicationData.Current.LocalSettings.Values[key] = value;
            }
            catch (Exception e)
            { 
                //couldn't write settings
            }
        }

        public static string loadSettings(string key)
        {
            try
            {
                return (string)ApplicationData.Current.LocalSettings.Values[key];
            }
            catch (Exception e)
            {
                return null;            
            }
        }
    }
}
