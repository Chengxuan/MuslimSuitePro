using PrayerTimesCalculator;
using System;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MuslimSuitePro.SnappedViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PrayerSettingsSnapped : Page
    {

        int argument = 0;

        DownloadOperation _activeDownload;

        public PrayerSettingsSnapped()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
            BottomAppBar1.IsOpen = true;

            //MessageDialog msg = new MessageDialog("Current Date from App: " + App.scheduleFactorySetupDate.ToString() + " \r\nDifference: " + App.scheduleFactorySetupDate.Date.CompareTo(DateTime.Now.Date).ToString(), "Muslim Suite Pro");
            //msg.ShowAsync();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Snapped || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenPortrait)
            {
                this.Frame.Navigate(typeof(PrayerSettingsSnapped), argument);
            }
            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Filled || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenLandscape)
            {
                saveSettings();
                this.Frame.Navigate(typeof(MuslimSuitePro.PrayerSettings), argument);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                argument = (int)e.Parameter;
            }
            catch (Exception e1)
            {

            }
            /// <summary>
            /// Invoked when this page is about to be displayed in a Frame.
            /// </summary>
            /// <param name="e">Event data that describes how this page was reached.  The Parameter
            /// property is typically used to configure the page.</param>
            /// 
            loadSettings();
            loadBackground();
        }

        public void loadSettings()
        {
            try
            {
                //MessageDialog msg = new MessageDialog("index is " + abc.ToString(), "Muslim Suite Pro");
                //await msg.ShowAsync();


                dayLightSaving.SelectedIndex = Convert.ToInt16(App.isDST);
                timeformat.SelectedIndex = Convert.ToInt16(App.displayTimeFormat);
                calcMethod.SelectedIndex = Convert.ToInt16(App.cm);
                hlMethod.SelectedIndex = Convert.ToInt16(App.hlm);
                asrMethod.SelectedIndex = Convert.ToInt16(App.aj);

                fajrNotification.SelectedIndex = Convert.ToInt16(App.fajrNotification);
                dhuhrNotification.SelectedIndex = Convert.ToInt16(App.dhuhrNotification);
                asrNotification.SelectedIndex = Convert.ToInt16(App.asrNotification);
                maghribNotification.SelectedIndex = Convert.ToInt16(App.maghribNotification);
                ishaNotification.SelectedIndex = Convert.ToInt16(App.ishaNotification);

                int tempIndex = 10;
                tempIndex += Convert.ToInt16(App.fajrAdjustment);
                fajrAdjustment.SelectedIndex = tempIndex;

                tempIndex = 10;
                tempIndex += Convert.ToInt16(App.dhuhrAdjustment);
                dhuhrAdjustment.SelectedIndex = tempIndex;

                tempIndex = 10;
                tempIndex += Convert.ToInt16(App.asrAdjustment);
                asrAdjustment.SelectedIndex = tempIndex;

                tempIndex = 10;
                tempIndex += Convert.ToInt16(App.maghribAdjustment);
                maghribAdjustment.SelectedIndex = tempIndex;

                tempIndex = 10;
                tempIndex += Convert.ToInt16(App.ishaAdjustment);
                ishaAdjustment.SelectedIndex = tempIndex;

                tempIndex = 0;
                tempIndex += Convert.ToInt32(App.timeZoneSelectedIndex);
                timeZone.SelectedIndex = tempIndex;

                translationLanguage.SelectedIndex = Convert.ToInt16(App.translationLanguage.Split(':')[0]);

                suraLanguage.SelectedIndex = Convert.ToInt16(App.quranChapterNameLang);

            }
            catch (Exception e)
            {

            }
        }

        public void loadBackground()
        {

            //make a image brush using backgroundImage and assign it to the background of the Grid
            ImageBrush myBrush = new ImageBrush();
            Image image = new Image();
            image.Source = new BitmapImage(new Uri("ms-appx:///Images/" + App.backgroundImage));
            myBrush.ImageSource = image.Source;
            landScapeMode.Background = myBrush;
        }

        public void saveSettings()
        {

            //save all new settings
            AppSettings.saveSettings("isDST", dayLightSaving.SelectedIndex.ToString());

            if (timeZone.SelectionBoxItem.ToString() == "Auto")
                AppSettings.saveSettings("timeZone", "99"); //99 to represent Auto
            else
                AppSettings.saveSettings("timeZone", timeZone.SelectionBoxItem.ToString());

            AppSettings.saveSettings("timeZoneSelectedIndex", timeZone.SelectedIndex.ToString());
            AppSettings.saveSettings("displayTimeFormat", timeformat.SelectedIndex.ToString());
            AppSettings.saveSettings("cm", calcMethod.SelectedIndex.ToString());
            AppSettings.saveSettings("hlm", hlMethod.SelectedIndex.ToString());
            AppSettings.saveSettings("aj", asrMethod.SelectedIndex.ToString());

            AppSettings.saveSettings("fajrNotification", fajrNotification.SelectedIndex.ToString());
            AppSettings.saveSettings("dhuhrNotification", dhuhrNotification.SelectedIndex.ToString());
            AppSettings.saveSettings("asrNotification", asrNotification.SelectedIndex.ToString());
            AppSettings.saveSettings("maghribNotification", maghribNotification.SelectedIndex.ToString());
            AppSettings.saveSettings("ishaNotification", ishaNotification.SelectedIndex.ToString());

            AppSettings.saveSettings("fajrAdjustment", fajrAdjustment.SelectionBoxItem.ToString());
            AppSettings.saveSettings("dhuhrAdjustment", dhuhrAdjustment.SelectionBoxItem.ToString());
            AppSettings.saveSettings("asrAdjustment", asrAdjustment.SelectionBoxItem.ToString());

            AppSettings.saveSettings("maghribAdjustment", maghribAdjustment.SelectionBoxItem.ToString());
            AppSettings.saveSettings("ishaAdjustment", ishaAdjustment.SelectionBoxItem.ToString());

            AppSettings.saveSettings("translationLanguage", translationLanguage.SelectedIndex.ToString() + ":" + translationLanguage.SelectionBoxItem.ToString().Replace(" ", ""));

            AppSettings.saveSettings("quranChapterNameLang", suraLanguage.SelectedIndex.ToString());

            AppSettings.saveSettings("translationEnabled", "true");

            AppSettings.saveSettings("FirstRun", "No");

            //load new settings on application level

            App.fajrNotification = Convert.ToInt16(AppSettings.loadSettings("fajrNotification"));
            App.dhuhrNotification = Convert.ToInt16(AppSettings.loadSettings("dhuhrNotification"));
            App.asrNotification = Convert.ToInt16(AppSettings.loadSettings("asrNotification"));
            App.maghribNotification = Convert.ToInt16(AppSettings.loadSettings("maghribNotification"));
            App.ishaNotification = Convert.ToInt16(AppSettings.loadSettings("ishaNotification"));

            App.fajrAdjustment = Convert.ToDouble(AppSettings.loadSettings("fajrAdjustment"));
            App.dhuhrAdjustment = Convert.ToDouble(AppSettings.loadSettings("dhuhrAdjustment"));
            App.asrAdjustment = Convert.ToDouble(AppSettings.loadSettings("asrAdjustment"));
            App.maghribAdjustment = Convert.ToDouble(AppSettings.loadSettings("maghribAdjustment"));
            App.ishaAdjustment = Convert.ToDouble(AppSettings.loadSettings("ishaAdjustment"));

            App.isDST = Convert.ToBoolean(Convert.ToInt16(AppSettings.loadSettings("isDST")));
            App.timeZone = Convert.ToInt32(AppSettings.loadSettings("timeZone"));
            App.timeZoneSelectedIndex = Convert.ToInt32(AppSettings.loadSettings("timeZoneSelectedIndex"));
            App.displayTimeFormat = (TimeFormats)Enum.Parse(typeof(TimeFormats), AppSettings.loadSettings("displayTimeFormat"));
            App.cm = (CalculationMethods)Enum.Parse(typeof(CalculationMethods), AppSettings.loadSettings("cm"));
            App.hlm = (HighLatitudeMethods)Enum.Parse(typeof(HighLatitudeMethods), AppSettings.loadSettings("hlm"));
            App.aj = (AsrJuristics)Enum.Parse(typeof(AsrJuristics), AppSettings.loadSettings("aj"));

            App.translationLanguage = AppSettings.loadSettings("translationLanguage");
            App.quranChapterNameLang = Convert.ToInt16(AppSettings.loadSettings("quranChapterNameLang"));
            App.quranReadingBookmark = AppSettings.loadSettings("quranReadingBookmark");
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {

            saveSettings();

            string translationLanguageName = AppSettings.loadSettings("translationLanguage").Split(':')[1].ToLower();

            if (translationLanguageName != "none")
            {
                if (AppSettings.loadSettings("cachedLanguage") != null)
                {
                    string cachedLanguages = AppSettings.loadSettings("cachedLanguage");

                    if (!AppSettings.loadSettings("cachedLanguage").Contains(translationLanguageName))
                    {

                        //language was not cached before
                        downloadProgressGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        home.IsEnabled = false;
                        save.IsEnabled = false;

                        DownloadFile(translationLanguageName);
                    }
                    else
                    {
                        //language was cached before
                        AppSettings.saveSettings("translationEnabled", "true");
                        home.IsEnabled = true;
                        save.IsEnabled = true;
                        navigate_home();
                    }
                }
                else
                {
                    //no language has been cached before
                    downloadProgressGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    home.IsEnabled = false;
                    save.IsEnabled = false;

                    DownloadFile(translationLanguageName);
                }

            }
            else
            {
                AppSettings.saveSettings("translationEnabled", "false");
                navigate_home();
            }

        }

        private void navigate_home()
        {
            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Snapped)
            {
                this.Frame.Navigate(typeof(SnappedViews.MainPageSnapped), argument);
            }
            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Filled || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenLandscape || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenPortrait)
            {
                this.Frame.Navigate(typeof(MainPage), argument);
            }
        }

        private void resetBookmark_Click(object sender, RoutedEventArgs e)
        {
            App.quranReadingBookmark = "";
            AppSettings.saveSettings("quranReadingBookmark", "");
        }

        public static bool CheckInternetConnection()
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile != null && profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
                return true;
            else
                return false;
        }

        private async void DownloadFile(string languageName)
        {
            string resAdd = "http://www.agilani.me/msp/translations/" + languageName + ".xml";

            bool internetAccess = CheckInternetConnection();

            if (internetAccess == true)
            {
                try
                {

                    Uri source = new Uri(resAdd);
                    string fileName = languageName + ".xml";


                    StorageFolder allTranslationsFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("translations", CreationCollisionOption.OpenIfExists);
                    StorageFile destinationFile = await allTranslationsFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                    BackgroundDownloader downloader = new BackgroundDownloader();
                    DownloadOperation download = downloader.CreateDownload(source, destinationFile);

                    await StartDownloadAsync(download);
                }
                catch (Exception ex)
                {
                    messageBox.Text = "Error saving file";
                    AppSettings.saveSettings("translationEnabled", "false");
                    home.IsEnabled = true;
                    save.IsEnabled = true;

                }
            }
            else
            {
                //internet is not on
                downloadProgressGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                messageBox.Text = "Internet is disabled.\r\nTranslation not enabled";
                AppSettings.saveSettings("translationEnabled", "false");
                home.IsEnabled = true;
                save.IsEnabled = true;
            }
        }

        private void ProgressCallback(DownloadOperation obj)
        {
            double progress = ((double)obj.Progress.BytesReceived / obj.Progress.TotalBytesToReceive);
            DownloadProgress.Value = progress * 100;
            if (progress >= 1.0)
            {
                _activeDownload = null;
            }

            if (obj.Progress.BytesReceived == obj.Progress.TotalBytesToReceive)
            {
                messageBox.Text = "Download Complete";

                AppSettings.saveSettings("translationEnabled", "true");

                string cachedLanguages = AppSettings.loadSettings("cachedLanguage");

                if (cachedLanguages != null)
                {
                    AppSettings.saveSettings("cachedLanguage", AppSettings.loadSettings("cachedLanguage") + ":" + AppSettings.loadSettings("translationLanguage").Split(':')[1].ToLower());
                }
                else
                {
                    AppSettings.saveSettings("cachedLanguage", AppSettings.loadSettings("translationLanguage").Split(':')[1].ToLower());
                }

                home.IsEnabled = true;
                save.IsEnabled = true;

            }
            else
            {
                AppSettings.saveSettings("translationEnabled", "false");
                messageBox.Text = "File download error";

                home.IsEnabled = true;
                save.IsEnabled = true;

            }
        }

        private async Task StartDownloadAsync(DownloadOperation downloadOperation)
        {
            _activeDownload = downloadOperation;
            var progress = new Progress<DownloadOperation>(ProgressCallback);
            await downloadOperation.StartAsync().AsTask(progress);
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            navigate_home();
        }
    }
}
