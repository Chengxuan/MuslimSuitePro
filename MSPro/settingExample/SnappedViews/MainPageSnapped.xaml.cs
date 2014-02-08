using PrayerTimesCalculator;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MuslimSuitePro.SnappedViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPageSnapped : Page
    {

        int argument = 0;
        public MainPageSnapped()
        {
            this.InitializeComponent();
            //Window.Current.SizeChanged += Current_SizeChanged; //add event to the handle snapping and landscaping

            string[] s = getPrayerTimes(App.ln, App.lt);

            tbSnappedDate1.Text = DateTime.Now.DayOfWeek.ToString() + "\r\n" + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            tbSnappedTimeImsak1.Text = s[0];
            tbSnappedTimeFajr1.Text = s[1];
            tbSnappedTimeSunrise1.Text = s[2];
            tbSnappedTimeDhur1.Text = s[3];
            tbSnappedTimeAsr1.Text = s[4];
            tbSnappedTimeSunset1.Text = s[5];
            tbSnappedTimeMaghrib1.Text = s[6];
            tbSnappedTimeIsha1.Text = s[7];
            tbSnappedTimeMidnight1.Text = s[8];

            tbLocationSnapped.Text = App.countryFullName;


            Loaded += MainPage_Loaded;



            //MessageDialog msg = new MessageDialog("Current Date from App: " + App.scheduleFactorySetupDate.ToString() + " \r\nDifference: " + App.scheduleFactorySetupDate.Date.CompareTo(DateTime.Now.Date).ToString(), "Muslim Suite Pro");
            //msg.ShowAsync();

        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            Window.Current.SizeChanged += Current_SizeChanged;
        }

        public string[] getPrayerTimes(double ln, double lt)
        {
            int tz = 0;
            if (App.timeZone == 99)
                tz = TimeZoneInfo.Utc.GetUtcOffset(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Month)).Hours;
            else
                tz = App.timeZone;

            var calculator = new Calculator(App.cm, App.aj, App.hlm);

            calculator.setOffSets(App.fajrAdjustment, App.dhuhrAdjustment, App.asrAdjustment, App.maghribAdjustment, App.ishaAdjustment);

            var times = calculator.GetPrayerTimes(DateTime.Today, lt, ln, 0, tz, App.isDST, App.displayTimeFormat);
            
            String[] s = new String[9];
            s[0] = times.Imsak;
            s[1] = times.Fajr;
            s[2] = times.Sunrise;
            s[3] = times.Dhuhur;
            s[4] = times.Asr;
            s[5] = times.Sunset;
            s[6] = times.Magrib;
            s[7] = times.Isha;
            s[8] = times.Midnight;

            return s ;
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
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
        }
    }
}
