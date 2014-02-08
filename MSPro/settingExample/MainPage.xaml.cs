using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using PrayerTimesCalculator;
using System.Collections.Generic;
using NotificationsExtensions.TileContent;
using NotificationsExtensions.ToastContent;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MuslimSuitePro
{
    public sealed partial class MainPage : Page
    {

        int argument = 0;
        public static int[] notificationFlag = { 0, 0, 0, 0, 0 };

        public MainPage()
        {
            this.InitializeComponent(); //init all the controls in xaml

            Loaded += MainPage_Loaded;

            
            
            //MessageDialog msg = new MessageDialog("Current Date from App: " + App.scheduleFactorySetupDate.ToString() + " \r\nDifference: " + App.scheduleFactorySetupDate.Date.CompareTo(DateTime.Now.Date).ToString(), "Muslim Suite Pro");
            //msg.ShowAsync();

        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            Window.Current.SizeChanged += Current_SizeChanged;
        }

        public void loadSettings()
        {
            if (App.shareSourceFlag ==false)
            {
                ShareSourceLoad();
                App.shareSourceFlag = true;
            }

            if (AppSettings.loadSettings("FirstRun") == null)
            {
                this.Frame.Navigate(typeof(MuslimSuitePro.ChangeLocation),1);
               
            }
            else
            {
                notificationFlag[0] = App.fajrNotification;
                notificationFlag[1] = App.dhuhrNotification;
                notificationFlag[2] = App.asrNotification;
                notificationFlag[3] = App.maghribNotification;
                notificationFlag[4] = App.ishaNotification;
                try
                {
                    ImageBrush myBrush = new ImageBrush();
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri("ms-appx:///Images/" + App.backgroundImage));
                    myBrush.ImageSource = image.Source;
                    landScapeMode.Background = myBrush;
                    
                }
                catch (Exception e)
                {
                    this.Frame.Navigate(typeof(MuslimSuitePro.ChangeLocation), 1);
                }
            }
        }

        private void ShareSourceLoad()
        {
            {
                try
                {
                    DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
                    dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.DataRequested);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            
        }

        private void DataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            string dataPackageText = "";
            dataPackageText = "<html><head>Sent at: " + dateTime.ToString() + "</head><body><table align='left' width='40%' cellspacing='10'>";
            dataPackageText += "<tr><td align='right' width='40%'>For Location </td><td align='left' width='60%'>" + tbLocation.Text + "</td></tr>";
            dataPackageText += "<tr><td align='right' width='40%'>Farj </td><td align='left' width='60%'>" + tbTimeFajr1.Text + "</td></tr>";
            dataPackageText += "<td align='right' width='40%'>Sunrise</td><td align='left' width='60%'>" + tbTimeSunrise1.Text + "</td></tr>";
            dataPackageText += "<td align='right' width='40%'>Dhuhr</td><td align='left' width='60%'>" + tbTimeDhur1.Text + "</td></tr>";
            dataPackageText += "<td align='right' width='40%'>Asr</td><td align='left' width='60%'>"+ tbTimeAsr1.Text + "</td></tr>";
            dataPackageText += "<td align='right' width='40%'>Maghrib</td><td align='left' width='60%'>"  + tbTimeAsr1.Text + "</td></tr>";
            dataPackageText += "<td align='right' width='40%'>Isha</td><td align='left' width='60%'>" + tbTimeIsha1.Text + "</td></tr><p></br>Sent using Muslim Suite Pro</p>";
            dataPackageText += "</table></body><br><br><br><br></html>";

            DataRequest request = e.Request;

            request.Data.Properties.Title = "Prayer Time Schedule";
            request.Data.Properties.Description = "Today's time table for prayers.";
            request.Data.SetHtmlFormat(Windows.ApplicationModel.DataTransfer.HtmlFormatHelper.CreateHtmlFormat(dataPackageText));
        }

        public void myLandScapeInit()
        {
            string[] s;
            string dayName;
            int dayIncrementCounter = 0;
            DateTime myDate = DateTime.Today;

            TextBlock[,] infoTabs = new TextBlock[7,7];

            infoTabs[0, 0] = tbTimeFajr1;
            infoTabs[0, 1] = tbTimeSunrise1;
            infoTabs[0, 2] = tbTimeDhur1;
            infoTabs[0, 3] = tbTimeAsr1;
            infoTabs[0, 4] = tbTimeMaghrib1;
            infoTabs[0, 5] = tbTimeIsha1;
            infoTabs[0, 6] = tbDate1;

            infoTabs[1, 0] = tbTimeFajr2;
            infoTabs[1, 1] = tbTimeSunrise2;
            infoTabs[1, 2] = tbTimeDhur2;
            infoTabs[1, 3] = tbTimeAsr2;
            infoTabs[1, 4] = tbTimeMaghrib2;
            infoTabs[1, 5] = tbTimeIsha2;
            infoTabs[1, 6] = tbDate2;

            infoTabs[2, 0] = tbTimeFajr3;
            infoTabs[2, 1] = tbTimeSunrise3;
            infoTabs[2, 2] = tbTimeDhur3;
            infoTabs[2, 3] = tbTimeAsr3;
            infoTabs[2, 4] = tbTimeMaghrib3;
            infoTabs[2, 5] = tbTimeIsha3;
            infoTabs[2, 6] = tbDate3;

            infoTabs[3, 0] = tbTimeFajr4;
            infoTabs[3, 1] = tbTimeSunrise4;
            infoTabs[3, 2] = tbTimeDhur4;
            infoTabs[3, 3] = tbTimeAsr4;
            infoTabs[3, 4] = tbTimeMaghrib4;
            infoTabs[3, 5] = tbTimeIsha4;
            infoTabs[3, 6] = tbDate4;

            infoTabs[4, 0] = tbTimeFajr5;
            infoTabs[4, 1] = tbTimeSunrise5;
            infoTabs[4, 2] = tbTimeDhur5;
            infoTabs[4, 3] = tbTimeAsr5;
            infoTabs[4, 4] = tbTimeMaghrib5;
            infoTabs[4, 5] = tbTimeIsha5;
            infoTabs[4, 6] = tbDate5;

            infoTabs[5, 0] = tbTimeFajr6;
            infoTabs[5, 1] = tbTimeSunrise6;
            infoTabs[5, 2] = tbTimeDhur6;
            infoTabs[5, 3] = tbTimeAsr6;
            infoTabs[5, 4] = tbTimeMaghrib6;
            infoTabs[5, 5] = tbTimeIsha6;
            infoTabs[5, 6] = tbDate6;

            infoTabs[6, 0] = tbTimeFajr7;
            infoTabs[6, 1] = tbTimeSunrise7;
            infoTabs[6, 2] = tbTimeDhur7;
            infoTabs[6, 3] = tbTimeAsr7;
            infoTabs[6, 4] = tbTimeMaghrib7;
            infoTabs[6, 5] = tbTimeIsha7;
            infoTabs[6, 6] = tbDate7;


            
            for (int i = 0; i < 7; i++)
            {
                dayName = myDate.AddDays(dayIncrementCounter).DayOfWeek.ToString();
                s = getPrayerTimes(App.ln, App.lt, myDate.AddDays(dayIncrementCounter), 0);

                infoTabs[i, 6].Text = dayName + "\r\n" + myDate.AddDays(dayIncrementCounter).Day + "/" + myDate.AddDays(dayIncrementCounter).Month + "/" + myDate.AddDays(dayIncrementCounter).Year;
                infoTabs[i, 0].Text = s[1];
                infoTabs[i, 1].Text = s[2];
                infoTabs[i, 2].Text = s[3];
                infoTabs[i, 3].Text = s[4];
                infoTabs[i, 4].Text = s[6];
                infoTabs[i, 5].Text = s[7];

                dayIncrementCounter++;
            }

            tbLocation.Text = App.countryFullName.ToString();
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

        public string[] getPrayerTimes(double ln, double lt, DateTime workableDate, int overRideMethod)
        {
            int tz = 0;
            if(App.timeZone==99)
                tz = TimeZoneInfo.Utc.GetUtcOffset(new DateTime(workableDate.Year, workableDate.Month, workableDate.Month)).Hours;
            else
                tz = App.timeZone;

            var calculator = new Calculator(App.cm, App.aj, App.hlm);
            calculator.setOffSets(App.fajrAdjustment, App.dhuhrAdjustment, App.asrAdjustment, App.maghribAdjustment, App.ishaAdjustment);
            PrayerTimeDto times;
            if (overRideMethod == 1) // time for tiles
            {
                if (App.displayTimeFormat == 0)
                {
                    times = calculator.GetPrayerTimes(workableDate, lt, ln, 0, tz, App.isDST, App.displayTimeFormat);
                }
                else
                {
                    times = calculator.GetPrayerTimes(workableDate, lt, ln, 0, tz, App.isDST, TimeFormats.Hour24);
                }
            }
            else
                times = calculator.GetPrayerTimes(workableDate, lt, ln, 0, tz, App.isDST, App.displayTimeFormat);

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

            return s;
        }

        public string[] getPrayerTimesForNotification(double ln, double lt, DateTime workableDate, int overRideMethod)
        {
            int tz = 0;
            if (App.timeZone == 99)
                tz = TimeZoneInfo.Utc.GetUtcOffset(new DateTime(workableDate.Year, workableDate.Month, workableDate.Month)).Hours;
            else
                tz = App.timeZone;

            var calculator = new Calculator(App.cm, App.aj, App.hlm);
            calculator.setOffSets(App.fajrAdjustment, App.dhuhrAdjustment, App.asrAdjustment, App.maghribAdjustment, App.ishaAdjustment);
            PrayerTimeDto times;
            if (overRideMethod == 1) // time for tiles
            {
                if (App.displayTimeFormat == 0)
                {
                    times = calculator.GetPrayerTimes(workableDate, lt, ln, 0, tz, App.isDST, App.displayTimeFormat);
                }
                else
                {
                    times = calculator.GetPrayerTimes(workableDate, lt, ln, 0, tz, App.isDST, TimeFormats.Hour24);
                }
            }
            else
                times = calculator.GetPrayerTimes(workableDate, lt, ln, 0, tz, App.isDST, App.displayTimeFormat);
            String[] s = new String[5];
            s[0] = times.Fajr;
            s[1] = times.Dhuhur;
            s[2] = times.Asr;
            s[3] = times.Magrib;
            s[4] = times.Isha;
            return s;
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
            myLandScapeInit(); //load landscape ui
            ScheduleNotification();
            //StartTile("sadfsdaf", "dsfsfdfd")
        }

        private void changePhoto_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ChangeBackground),0);
        }

        private void changeLocation_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ChangeLocation),0);
        }

        private void holidays_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Holidays),0);
        }

        private void changeSettings_click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PrayerSettings),0);
        }

        private void readQuran_click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Quran), "");
        }

        public void ScheduleNotification()
        {

            string[] salatTimesForNotification;
            double timeNowHor = Convert.ToDouble(DateTime.Now.Hour);
            double timeNowMin = Convert.ToDouble(DateTime.Now.Minute);
            double[] prayerTimesHor = new double[5];
            double[] prayerTimesMin = new double[5];
            salatTimesForNotification = getPrayerTimesForNotification(App.ln, App.lt, DateTime.Today, 1);
            string[] prayerNames = {"Fajr","Dhuhr", "Asr", "Maghrib",  "Isha"};
            for (int i = 0; i < salatTimesForNotification.Length; i++)
            {
                    if (DateTime.Now.CompareTo(Convert.ToDateTime(salatTimesForNotification[i])) < 0)
                    { 
                        if (i < 4)
                        {
                            ScheduleTile(prayerNames[i+1], salatTimesForNotification[i + 1], Convert.ToDateTime(salatTimesForNotification[i]), i);
                        }
                        else
                        {
                            string fajrTomorrow = getPrayerTimesForNotification(App.ln, App.lt, DateTime.Today.AddDays(1), 1)[0];
                            ScheduleTile(prayerNames[0], fajrTomorrow+"Tomorrow", Convert.ToDateTime(salatTimesForNotification[i]), i);
                        }
                    }
              
            }

            for(int j=0;j<salatTimesForNotification.Length;j++)
            {

                if (DateTime.Now.CompareTo(Convert.ToDateTime(salatTimesForNotification[j])) < 0)
                {
                    if (notificationFlag[j] != 0)
                    {
                        //var newTime = Convert.ToDateTime(salatTimesForNotification[j]).AddMinutes(-15);
                        //ScheduleToast(prayerNames[j], newTime, j);
                        ScheduleToast(prayerNames[j], Convert.ToDateTime(salatTimesForNotification[j]), j + 5);
                        
                    }
                }
            }


            for (int i = 0; i < 5; i++)
            {
                prayerTimesHor[i] = Convert.ToDouble(salatTimesForNotification[i].Split(':')[0]);
                prayerTimesMin[i] = Convert.ToDouble(salatTimesForNotification[i].Split(':')[1]);
            }

            if (timeNowHor <= prayerTimesHor[0])
            {
                if (timeNowHor == prayerTimesHor[0])
                {
                    if (timeNowMin >= prayerTimesMin[0])
                    {
                        if (timeNowMin == prayerTimesMin[0])
                        {
                            StartToast("Fajr", 1);
                        }
                        StartTile("Dhuhr", salatTimesForNotification[1]);
                    }
                    else
                    {
                        if ((prayerTimesMin[0] - timeNowMin) < 15)
                        {
                            StartToast("Fajr", 0);
                        }
                        StartTile("Fajr", salatTimesForNotification[0]);
                    }
                }
                else
                {
                    StartTile("Fajr", salatTimesForNotification[0]);
                }
            }

            else if (timeNowHor <= prayerTimesHor[1])
            {
                if (timeNowHor == prayerTimesHor[1])
                {
                    if (timeNowMin >= prayerTimesMin[1])
                    {
                        if (timeNowMin == prayerTimesMin[1])
                        {
                            StartToast("Dhuhr", 1);
                        }
                        StartTile("Asr", salatTimesForNotification[2]);
                    }
                    else
                    {
                        if ((prayerTimesMin[1] - timeNowMin) < 15)
                        {
                            StartToast("Dhuhr", 0);
                        }
                        StartTile("Dhuhr", salatTimesForNotification[1]);
                    }
                }
                else
                {
                    StartTile("Dhuhr", salatTimesForNotification[1]);
                }
            }
            else if (timeNowHor <= prayerTimesHor[2])
            {
                if (timeNowHor == prayerTimesHor[2])
                {
                    if (timeNowMin >= prayerTimesMin[2])
                    {
                        if (timeNowMin == prayerTimesMin[2])
                        {
                            StartToast("Asr", 1);
                        }

                        StartTile("Maghrib", salatTimesForNotification[3]);

                    }
                    else
                    {
                        if ((prayerTimesMin[2] - timeNowMin) < 15)
                        {
                            StartToast("Asr", 0);
                        }
                        StartTile("Asr", salatTimesForNotification[2]);
                    }
                }
                else
                {
                    StartTile("Asr", salatTimesForNotification[2]);
                }

            }
            else if (timeNowHor <= prayerTimesHor[3])
            {
                if (timeNowHor == prayerTimesHor[3])
                {
                    if (timeNowMin >= prayerTimesMin[3])
                    {
                        if (timeNowMin == prayerTimesMin[3])
                        {
                            StartToast("Maghrib", 1);
                        }

                        StartTile("Isha", salatTimesForNotification[4]);

                    }
                    else
                    {
                        if ((prayerTimesMin[3] - timeNowMin) < 15)
                        {
                            StartToast("Maghrib", 0);
                        }
                        StartTile("Maghrib", salatTimesForNotification[3]);
                    }
                }
                else
                {
                    StartTile("Maghrib", salatTimesForNotification[3]);
                }
            }
            else if (timeNowHor <= prayerTimesHor[4])
            {
                if (timeNowHor == prayerTimesHor[4])
                {
                    if (timeNowMin >= prayerTimesMin[4])
                    {
                        if (timeNowMin == prayerTimesMin[4])
                        {
                            StartToast("Isha", 1);
                        }
                        string fajrTomorrow = getPrayerTimesForNotification(App.ln, App.lt, DateTime.Today.AddDays(1), 1)[0];
                        StartTile("Fajr", fajrTomorrow + "Tomorrow");
                    }
                    else
                    {
                        if ((prayerTimesMin[4] - timeNowMin) < 15)
                        {
                            StartToast("Isha", 0);
                        }
                        StartTile("Isha", salatTimesForNotification[4]);
                    }
                }
                else
                {
                    StartTile("Isha", salatTimesForNotification[4]);
                }
            }
            else
            {
                string fajrTomorrow = getPrayerTimesForNotification(App.ln, App.lt, DateTime.Today.AddDays(1), 1)[0];
                StartTile("Fajr", fajrTomorrow + "Tomorrow");
            }
        
        AppSettings.saveSettings("scheduleFactorySetupDate", DateTime.Now.Date.ToString());
        App.scheduleFactorySetupDate = Convert.ToDateTime(AppSettings.loadSettings("scheduleFactorySetupDate"));
        }

        public void ScheduleToast(string prayerName, DateTime dueTime, int idNumber)
        {


            try
            {
                ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();
                IReadOnlyList<ScheduledToastNotification> scheduled = notifier.GetScheduledToastNotifications();
                for (int i = 0; i < scheduled.Count; i++)
                {
                    if (scheduled[i].Id == "Toast" + idNumber)
                    {
                        notifier.RemoveFromSchedule(scheduled[i]);
                    }
                }
                IToastText02 toastContent = ToastContentFactory.CreateToastText02();
                if (idNumber < 5)
                {
                    toastContent.TextHeading.Text = "Next prayer is: " + prayerName;
                    toastContent.TextBodyWrap.Text = "Only 15 Minutes Left!";
                }
                else
                {
                    toastContent.TextHeading.Text = prayerName + "  Begins";
                }
                ScheduledToastNotification toast;
                toast = new ScheduledToastNotification(toastContent.GetXml(), dueTime);
                toast.Id = "Toast" + idNumber;
                ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ScheduleTile(string prayerName,string prayerTime,DateTime dueTime, int idNumber)
        {

            try
            {
                TileUpdater updater = TileUpdateManager.CreateTileUpdaterForApplication();
                IReadOnlyList<ScheduledTileNotification> scheduled = updater.GetScheduledTileNotifications();
                for (int i = 0; i < scheduled.Count; i++)
                {
                    if (scheduled[i].Id == "Tile" + idNumber)
                    {
                        updater.RemoveFromSchedule(scheduled[i]);
                    }
                }
                ITileWideText09 tileContent = TileContentFactory.CreateTileWideText09();
                tileContent.TextHeading.Text = "Next Prayer: " + prayerName;
                tileContent.TextBodyWrap.Text = "At: " + prayerTime;
                ITileSquareText01 squareContent = TileContentFactory.CreateTileSquareText01();
                squareContent.TextHeading.Text = prayerName;
                squareContent.TextBody1.Text = "At:  " + prayerTime.Substring(0, 5) ;
                tileContent.SquareContent = squareContent;
                ScheduledTileNotification futureTile = new ScheduledTileNotification(tileContent.GetXml(), dueTime);
                futureTile.Id = "Tile" + idNumber;
                TileUpdateManager.CreateTileUpdaterForApplication().AddToSchedule(futureTile);
                
            }
            catch (Exception e)
            {
                throw e;
            }
     
        }
    
        public void StartToast(string prayerName, int z)
        {
            
            {
                
                if (z != 1)
                {
                    if (AppSettings.loadSettings("toastCheck_NotDue") != prayerName)
                    {
                        IToastText02 toastContent = ToastContentFactory.CreateToastText02();
                        toastContent.TextHeading.Text = "Next prayer is: " + prayerName;
                        toastContent.TextBodyWrap.Text = "Only 15 Minutes Left!";
                        ToastNotification toastNotification = toastContent.CreateNotification();
                        ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
                        AppSettings.saveSettings("toastCheck_NotDue", prayerName);
                    }
                }
                else
                { 
                    if (AppSettings.loadSettings("toastCheck_Due") != prayerName)
                    {
                        IToastText02 toastContent = ToastContentFactory.CreateToastText02();
                        toastContent.TextHeading.Text = prayerName + "  Begins";
                        ToastNotification toastNotification = toastContent.CreateNotification();
                        ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
                        AppSettings.saveSettings("toastCheck_Due", prayerName);
                    }
                }
                
            }

        }

        public void StartTile(string prayerName, string prayerTime)
        {
            try
            {
                ITileWideText09 tileContent = TileContentFactory.CreateTileWideText09();
                tileContent.TextHeading.Text = "Next Prayer: " + prayerName;
                tileContent.TextBodyWrap.Text = "At: " + prayerTime;
                ITileSquareText01 squareContent = TileContentFactory.CreateTileSquareText01();
                squareContent.TextHeading.Text = prayerName;
                squareContent.TextBody1.Text = "At:  " + prayerTime.Substring(0, 5);
                tileContent.SquareContent = squareContent;
                TileNotification tileNotification = tileContent.CreateNotification();
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}