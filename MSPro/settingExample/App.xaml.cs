using PrayerTimesCalculator;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace MuslimSuitePro
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        /// 

        public static double ln = 0.00, lt = 0.00;
        public static string countryFullName = "", backgroundImage = "", translationLanguage = "", quranReadingBookmark = "1:1";
        public static int quranChapterNameLang = 0;
        public static bool isDST = false;
        public static int timeZone = 0;
        public static int timeZoneSelectedIndex = 0;

        public static TimeFormats displayTimeFormat = new TimeFormats();
        public static CalculationMethods cm = new CalculationMethods();
        public static HighLatitudeMethods hlm = new HighLatitudeMethods();
        public static AsrJuristics aj = new AsrJuristics();
        public static bool shareSourceFlag = false;
        public static int fajrNotification = 0, dhuhrNotification = 0, asrNotification = 0, maghribNotification = 0, ishaNotification = 0;

        public static double fajrAdjustment = 0, dhuhrAdjustment = 0, asrAdjustment = 0, maghribAdjustment = 0, ishaAdjustment = 0;

        public static DateTime scheduleFactorySetupDate;


        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            /// <summary>
            /// Invoked when the application is launched normally by the end user.  Other entry points
            /// will be used when the application is launched to open a specific file, to display
            /// search results, and so forth.
            /// </summary>
            /// <param name="args">Details about the launch request and process.</param>
            /// 


            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter

                Type goTo = null;

                if (AppSettings.loadSettings("FirstRun") == null)
                {
                    goTo = typeof(ChangeLocation);

                    if (!rootFrame.Navigate(goTo, 1))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }
                else
                {
                    loadSettings();

                    goTo = typeof(MainPage);

                    if (!rootFrame.Navigate(goTo, args.Arguments))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }
                var helper = new flyouts.SettingsHelper();
                helper.AddCommand<flyouts.About>("About Muslim Suite Pro");
                helper.AddCommand<flyouts.PrivacyPolicy>("Privacy Policy");
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        private void loadSettings()
        {
            if (AppSettings.loadSettings("countryFullName") != null)
                App.countryFullName = Convert.ToString(AppSettings.loadSettings("countryFullName"));

            if (AppSettings.loadSettings("ln") != null)
                App.ln = Convert.ToDouble(AppSettings.loadSettings("ln"));

            if (AppSettings.loadSettings("lt") != null)
                App.lt = Convert.ToDouble(AppSettings.loadSettings("lt"));

            if (AppSettings.loadSettings("backgroundImage") != null)
                App.backgroundImage = Convert.ToString(AppSettings.loadSettings("backgroundImage"));

            if (AppSettings.loadSettings("fajrNotification") != null)
                App.fajrNotification = Convert.ToInt32(AppSettings.loadSettings("fajrNotification"));

            if (AppSettings.loadSettings("dhuhrNotification") != null)
                App.dhuhrNotification = Convert.ToInt16(AppSettings.loadSettings("dhuhrNotification"));

            if (AppSettings.loadSettings("asrNotification") != null)
                App.asrNotification = Convert.ToInt16(AppSettings.loadSettings("asrNotification"));

            if (AppSettings.loadSettings("maghribNotification") != null)
                App.maghribNotification = Convert.ToInt16(AppSettings.loadSettings("maghribNotification"));

            if (AppSettings.loadSettings("ishaNotification") != null)
                App.ishaNotification = Convert.ToInt16(AppSettings.loadSettings("ishaNotification"));

            if (AppSettings.loadSettings("fajrAdjustment") != null)
                App.fajrAdjustment = Convert.ToDouble(AppSettings.loadSettings("fajrAdjustment"));

            if (AppSettings.loadSettings("dhuhrAdjustment") != null)
                App.dhuhrAdjustment = Convert.ToDouble(AppSettings.loadSettings("dhuhrAdjustment"));

            if (AppSettings.loadSettings("asrAdjustment") != null)
                App.asrAdjustment = Convert.ToDouble(AppSettings.loadSettings("asrAdjustment"));

            if (AppSettings.loadSettings("maghribAdjustment") != null)
                App.maghribAdjustment = Convert.ToDouble(AppSettings.loadSettings("maghribAdjustment"));

            if (AppSettings.loadSettings("ishaAdjustment") != null)
                App.ishaAdjustment = Convert.ToDouble(AppSettings.loadSettings("ishaAdjustment"));

            if (AppSettings.loadSettings("isDST") != null)
                App.isDST = Convert.ToBoolean(Convert.ToInt16(AppSettings.loadSettings("isDST")));

            if (AppSettings.loadSettings("timeZone") != null)
                App.timeZone = Convert.ToInt32(Convert.ToInt16(AppSettings.loadSettings("timeZone")));

            if (AppSettings.loadSettings("timeZoneSelectedIndex") != null)
                App.timeZone = Convert.ToInt32(Convert.ToInt16(AppSettings.loadSettings("timeZoneSelectedIndex")));

            if (AppSettings.loadSettings("displayTimeFormat") != null)
                App.displayTimeFormat = (TimeFormats)Enum.Parse(typeof(TimeFormats), AppSettings.loadSettings("displayTimeFormat"));

            if (AppSettings.loadSettings("cm") != null)
                App.cm = (CalculationMethods)Enum.Parse(typeof(CalculationMethods), AppSettings.loadSettings("cm"));

            if (AppSettings.loadSettings("hlm") != null)
                App.hlm = (HighLatitudeMethods)Enum.Parse(typeof(HighLatitudeMethods), AppSettings.loadSettings("hlm"));

            if (AppSettings.loadSettings("aj") != null)
                App.aj = (AsrJuristics)Enum.Parse(typeof(AsrJuristics), AppSettings.loadSettings("aj"));

            if (AppSettings.loadSettings("translationLanguage") != null)
                App.translationLanguage = AppSettings.loadSettings("translationLanguage");

            if (AppSettings.loadSettings("quranChapterNameLang") != null)
                App.quranChapterNameLang = Convert.ToInt16(AppSettings.loadSettings("quranChapterNameLang"));

            if (AppSettings.loadSettings("quranReadingBookmark") != null)
                App.quranReadingBookmark = AppSettings.loadSettings("quranReadingBookmark");

            if (AppSettings.loadSettings("scheduleFactorySetupDate") == null)
                App.scheduleFactorySetupDate = new DateTime().Date.AddYears(-1);
            else
                App.scheduleFactorySetupDate = Convert.ToDateTime(AppSettings.loadSettings("scheduleFactorySetupDate"));
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            /// <summary>
            /// Invoked when application execution is being suspended.  Application state is saved
            /// without knowing whether the application will be terminated or resumed with the contents
            /// of memory still intact.
            /// </summary>
            /// <param name="sender">The source of the suspend request.</param>
            /// <param name="e">Details about the suspend request.</param>

            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
