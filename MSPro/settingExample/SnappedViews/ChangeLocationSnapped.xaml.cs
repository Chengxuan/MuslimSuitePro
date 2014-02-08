using Bing.Maps;
using System;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
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
    public sealed partial class ChangeLocationSnapped : Page
    {

        double ln, lt;
        string countryFullName;
        int argument = 0, locationTry = 0;

        Pushpin pin = new Pushpin();
        Location pinLocation;
        ImageBrush ib = new ImageBrush();

        public ChangeLocationSnapped()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
            
            //MessageDialog msg = new MessageDialog("Current Date from App: " + App.scheduleFactorySetupDate.ToString() + " \r\nDifference: " + App.scheduleFactorySetupDate.Date.CompareTo(DateTime.Now.Date).ToString(), "Muslim Suite Pro");
            //msg.ShowAsync();

        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;
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


            // check to see if
            // application is running for the first time
            // if yes then
            // going to take through steps to configure
            // 0) display welcome and configuration alert
            // 1) background
            // 2) location, if not determined

            if (argument == 1 && AppSettings.loadSettings("FirstRun") == null)
            {
                //    //MessageDialog msg = new MessageDialog("Welcome to Muslim Suite Pro.\r\nAs it is your first time running this app.\r\nWizard will take you through few configuration steps.", "Muslim Suite Pro");
                //    //msg.ShowAsync();
                //    showWelcome();
                showWelcome.IsOpen = true;
            }

            if (App.ln == 0.00 || App.lt == 0.00)
            {
                if (AppSettings.loadSettings("ln") == null || AppSettings.loadSettings("lt") == null)
                {
                    getPositionUsingGPS();
                }
                else
                {
                    ln = Convert.ToDouble(AppSettings.loadSettings("ln"));
                    lt = Convert.ToDouble(AppSettings.loadSettings("lt"));
                }
            }
            else
            {
                ln = App.ln;
                lt = App.lt;
            }

            BottomAppBar.IsOpen = true;
            myMap.UpdateLayout();
            myMap.Children.Add(pin);
            ib.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/pushpin.png"));
            pin.Background = ib;
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Snapped || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenPortrait)
            {
                this.Frame.Navigate(typeof(SnappedViews.ChangeLocationSnapped),argument);
            }
            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Filled || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenLandscape)
            {
                this.Frame.Navigate(typeof(ChangeLocation), argument);
                //Window.Current.Activate();
            }
        }

        private void saveSelection_Click(object sender, RoutedEventArgs e)
        {
            AppSettings.saveSettings("ln", ln.ToString()); // save the longtitude in the settings
            AppSettings.saveSettings("lt", lt.ToString()); // save the latitude in the settings
            AppSettings.saveSettings("countryFullName", countryFullName.ToString()); // save the country in the settings

            App.ln = ln;
            App.lt = lt;
            App.countryFullName = countryFullName;


            if (argument == 1)
            {
                if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Snapped || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenPortrait)
                {
                    this.Frame.Navigate(typeof(SnappedViews.ChangeBGSnapped), argument);
                }
                if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Filled || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenLandscape)
                {
                    this.Frame.Navigate(typeof(ChangeBackground), argument);
                }
            }
            else
            {
                this.Frame.Navigate(typeof(MainPage), argument);
            }

        }

        private async void getPositionUsingGPS()
        {

            Geolocator locator = null;
            locator = new Geolocator();
            locator.DesiredAccuracy = PositionAccuracy.High;

            Geoposition position = null;

            try
            {
                position = await locator.GetGeopositionAsync(TimeSpan.Zero, TimeSpan.FromSeconds(10));
            }
            catch (Exception e) { }

            if (position == null)
            {
                lt = 53.279137;
                ln = -9.011222;
                countryFullName = "Galway, Ireland";
                txtLocation.Text = "Location not found. Using the default location.\r\nPlease permit app to access location services and hit refresh in App bar.\r\n \r\n";
                txtLocation.Text += countryFullName + " (" + lt + ", " + ln + ")";

                if (locationTry == 0)
                {
                    getPositionUsingGPS();
                    locationTry++;
                }

            }
            else
            {
                countryFullName = position.CivicAddress.PostalCode.ToString() + " " + getCountry(position.CivicAddress.Country.ToString());
                lt = position.Coordinate.Latitude;
                ln = position.Coordinate.Longitude;
                txtLocation.Text = countryFullName + " (" + lt + ", " + ln + ")";
            }

            myMap.Center.Latitude = lt;
            myMap.Center.Longitude = ln;
            myMap.SetView(new Location(Convert.ToDouble(lt), Convert.ToDouble(ln)));
            
            pinLocation = new Location(Convert.ToDouble(lt), Convert.ToDouble(ln));

            MapLayer.SetPosition(pin, pinLocation);

            BottomAppBar1.IsOpen = true;

        }

        public string getCountry(string cSmallName)
        {
            string countryFullName = "";
            switch (cSmallName)
            {
                case "AF":
                    countryFullName = "Afghanistan";
                    break;
                case "AL":
                    countryFullName = "Albania";
                    break;
                case "DZ":
                    countryFullName = "Algeria";
                    break;
                case "AS":
                    countryFullName = "American Samoa";
                    break;
                case "AD":
                    countryFullName = "Andorra";
                    break;
                case "AO":
                    countryFullName = "Angola";
                    break;
                case "AI":
                    countryFullName = "Anguilla";
                    break;
                case "AQ":
                    countryFullName = "Antarctica";
                    break;
                case "AG":
                    countryFullName = "Antiqua and Barbuda";
                    break;
                case "AR":
                    countryFullName = "Argentina";
                    break;
                case "AM":
                    countryFullName = "Armenia";
                    break;
                case "AW":
                    countryFullName = "Aruba";
                    break;
                case "AU":
                    countryFullName = "Australia";
                    break;
                case "AT":
                    countryFullName = "Austria";
                    break;
                case "AZ":
                    countryFullName = "Azerbaijan";
                    break;
                case "BS":
                    countryFullName = "Bahamas";
                    break;
                case "BH":
                    countryFullName = "Bahrain";
                    break;
                case "BD":
                    countryFullName = "Bangladesh";
                    break;
                case "BB":
                    countryFullName = "Barbados";
                    break;
                case "BY":
                    countryFullName = "Belarus";
                    break;
                case "BE":
                    countryFullName = "Belgium";
                    break;
                case "BZ":
                    countryFullName = "Belize";
                    break;
                case "BJ":
                    countryFullName = "Benin";
                    break;
                case "BM":
                    countryFullName = "Bermuda";
                    break;
                case "BT":
                    countryFullName = "Bhutan";
                    break;
                case "BO":
                    countryFullName = "Bolivia";
                    break;
                case "BA":
                    countryFullName = "Bosnia and Herzegovina";
                    break;
                case "BW":
                    countryFullName = "Botswana";
                    break;
                case "BR":
                    countryFullName = "Brazil";
                    break;
                case "IO":
                    countryFullName = "British Indian Ocean Territory";
                    break;
                case "VG":
                    countryFullName = "British Virgin Islands";
                    break;
                case "BN":
                    countryFullName = "Brunei";
                    break;
                case "BG":
                    countryFullName = "Bulgaria";
                    break;
                case "BF":
                    countryFullName = "Burkina Faso";
                    break;
                case "MM":
                    countryFullName = "Burma(Mayanmar)";
                    break;
                case "BI":
                    countryFullName = "Burundi";
                    break;
                case "KH":
                    countryFullName = "Cambodia";
                    break;
                case "CM":
                    countryFullName = "Cameroon";
                    break;
                case "CA":
                    countryFullName = "Canada";
                    break;
                case "CV":
                    countryFullName = "Cape Verde";
                    break;
                case "KY":
                    countryFullName = "Cayman Islands";
                    break;
                case "CF":
                    countryFullName = "Central African Republic";
                    break;
                case "TD":
                    countryFullName = "Chad";
                    break;
                case "CL":
                    countryFullName = "Chile";
                    break;
                case "CN":
                    countryFullName = "China";
                    break;
                case "CX":
                    countryFullName = "Christmas Island";
                    break;
                case "CC":
                    countryFullName = "Cocos(Keeling) Islands";
                    break;
                case "CO":
                    countryFullName = "Colombia";
                    break;
                case "KM":
                    countryFullName = "Comoros";
                    break;
                case "CG":
                    countryFullName = "Republic of the Congo";
                    break;
                case "CD":
                    countryFullName = "Democratic Republic of the Congo";
                    break;
                case "CK":
                    countryFullName = "Cook Islands";
                    break;
                case "CR":
                    countryFullName = "Costa Rica";
                    break;
                case "HR":
                    countryFullName = "Croatia";
                    break;
                case "CU":
                    countryFullName = "Cuba";
                    break;
                case "CY":
                    countryFullName = "Cyprus";
                    break;
                case "CZ":
                    countryFullName = "Czech Republic";
                    break;
                case "DK":
                    countryFullName = "Denmark";
                    break;
                case "DJ":
                    countryFullName = "Djibouti";
                    break;
                case "DM":
                    countryFullName = "Dominica";
                    break;
                case "DO":
                    countryFullName = "Dominican Republic";
                    break;
                case "TL":
                    countryFullName = "Timor-Leste";
                    break;
                case "EC":
                    countryFullName = "Ecuador";
                    break;
                case "EG":
                    countryFullName = "Egypt";
                    break;
                case "SV":
                    countryFullName = "EI Salvador";
                    break;
                case "GQ":
                    countryFullName = "Equatorial Guinea";
                    break;
                case "ER":
                    countryFullName = "Eritrea";
                    break;
                case "EE":
                    countryFullName = "Estonia";
                    break;
                case "ET":
                    countryFullName = "Ethiopia";
                    break;
                case "FK":
                    countryFullName = "Falkland Islands";
                    break;
                case "FO":
                    countryFullName = "Faroe Islands";
                    break;
                case "FJ":
                    countryFullName = "Fiji";
                    break;
                case "FI":
                    countryFullName = "Finland";
                    break;
                case "FR":
                    countryFullName = "France";
                    break;
                case "PF":
                    countryFullName = "French Polynesia";
                    break;
                case "GA":
                    countryFullName = "Gabon";
                    break;
                case "GM":
                    countryFullName = "Gambia";
                    break;
                case "GE":
                    countryFullName = "Georgia";
                    break;
                case "DE":
                    countryFullName = "Germany";
                    break;
                case "GH":
                    countryFullName = "Ghana";
                    break;
                case "GI":
                    countryFullName = "GIbraltar";
                    break;
                case "GR":
                    countryFullName = "Greece";
                    break;
                case "GL":
                    countryFullName = "Greenland";
                    break;
                case "GD":
                    countryFullName = "Grenada";
                    break;
                case "GU":
                    countryFullName = "Guam";
                    break;
                case "GT":
                    countryFullName = "Guatemala";
                    break;
                case "GN":
                    countryFullName = "Guinea";
                    break;
                case "GW":
                    countryFullName = "Guinea-Bissau";
                    break;
                case "GY":
                    countryFullName = "Guyana";
                    break;
                case "HT":
                    countryFullName = "Haiti";
                    break;
                case "HN":
                    countryFullName = "Honduras";
                    break;
                case "HK":
                    countryFullName = "Hong Kong";
                    break;
                case "HU":
                    countryFullName = "Hungary";
                    break;
                case "IS":
                    countryFullName = "Iceland";
                    break;
                case "IN":
                    countryFullName = "India";
                    break;
                case "ID":
                    countryFullName = "Indonesia";
                    break;
                case "IR":
                    countryFullName = "Iran";
                    break;
                case "IQ":
                    countryFullName = "Iraq";
                    break;
                case "IE":
                    countryFullName = "Ireland";
                    break;
                case "IM":
                    countryFullName = "Isle of Man";
                    break;
                case "IL":
                    countryFullName = "Israel";
                    break;
                case "IT":
                    countryFullName = "Italy";
                    break;
                case "CI":
                    countryFullName = "Ivory Coast";
                    break;
                case "JM":
                    countryFullName = "Jamaica";
                    break;
                case "JP":
                    countryFullName = "Japan";
                    break;
                case "JE":
                    countryFullName = "Jersey";
                    break;
                case "JO":
                    countryFullName = "Jordan";
                    break;
                case "KZ":
                    countryFullName = "Kazahstan";
                    break;
                case "KE":
                    countryFullName = "Kenya";
                    break;
                case "KI":
                    countryFullName = "Kosovo";
                    break;
                case "KW":
                    countryFullName = "Kuwait";
                    break;
                case "KG":
                    countryFullName = "Kyrgyzstan";
                    break;
                case "LA":
                    countryFullName = "Laos";
                    break;
                case "LV":
                    countryFullName = "Latvia";
                    break;
                case "LB":
                    countryFullName = "Lebanon";
                    break;
                case "LS":
                    countryFullName = "Lesotho";
                    break;
                case "LR":
                    countryFullName = "Liberia";
                    break;
                case "LY":
                    countryFullName = "Libya";
                    break;
                case "LI":
                    countryFullName = "Liechtenstein";
                    break;
                case "LT":
                    countryFullName = "Lithuania";
                    break;
                case "LU":
                    countryFullName = "Luxembourg";
                    break;
                case "MO":
                    countryFullName = "Macau";
                    break;
                case "MK":
                    countryFullName = "Macedonia";
                    break;
                case "MG":
                    countryFullName = "Madaquscar";
                    break;
                case "MW":
                    countryFullName = "Malawi";
                    break;
                case "MY":
                    countryFullName = "Malaysia";
                    break;
                case "MV":
                    countryFullName = "Maldives";
                    break;
                case "ML":
                    countryFullName = "Mali";
                    break;
                case "MT":
                    countryFullName = "Malta";
                    break;
                case "MH":
                    countryFullName = "Marshall Islands";
                    break;
                case "MR":
                    countryFullName = "Mauritania";
                    break;
                case "MU":
                    countryFullName = "Mauritius";
                    break;
                case "YT":
                    countryFullName = "Mayotte";
                    break;
                case "MX":
                    countryFullName = "Mexico";
                    break;
                case "FM":
                    countryFullName = "Micronesia";
                    break;
                case "MD":
                    countryFullName = "Moldova";
                    break;
                case "MC":
                    countryFullName = "Monaco";
                    break;
                case "MN":
                    countryFullName = "Mongolia";
                    break;
                case "ME":
                    countryFullName = "Montenegro";
                    break;
                case "MS":
                    countryFullName = "Montserrat";
                    break;
                case "MA":
                    countryFullName = "Morocco";
                    break;
                case "MZ":
                    countryFullName = "Mozambique";
                    break;
                case "NA":
                    countryFullName = "Namibia";
                    break;
                case "NR":
                    countryFullName = "Nauru";
                    break;
                case "NP":
                    countryFullName = "Nepal";
                    break;
                case "NL":
                    countryFullName = "Netherlands";
                    break;
                case "AN":
                    countryFullName = "Netherlands Antilles";
                    break;
                case "NC":
                    countryFullName = "New Caledonia";
                    break;
                case "NZ":
                    countryFullName = "New Zealand";
                    break;
                case "NI":
                    countryFullName = "Nicaragua";
                    break;
                case "NE":
                    countryFullName = "Niger";
                    break;
                case "NG":
                    countryFullName = "Nigeria";
                    break;
                case "NU":
                    countryFullName = "Niue";
                    break;
                case "NFK":
                    countryFullName = "Norfolk Island";
                    break;
                case "MP":
                    countryFullName = "Northern Mariana";
                    break;
                case "KP":
                    countryFullName = "North Korea";
                    break;
                case "NO":
                    countryFullName = "Norway";
                    break;
                case "OM":
                    countryFullName = "Oman";
                    break;
                case "PK":
                    countryFullName = "Pakistan";
                    break;
                case "PW":
                    countryFullName = "Palau";
                    break;
                case "PA":
                    countryFullName = "Panama";
                    break;
                case "PG":
                    countryFullName = "Papua New Guinea";
                    break;
                case "PY":
                    countryFullName = "Paraguay";
                    break;
                case "PE":
                    countryFullName = "Peru";
                    break;
                case "PH":
                    countryFullName = "Philippines";
                    break;
                case "PN":
                    countryFullName = "Pitcairn Islands";
                    break;
                case "PL":
                    countryFullName = "Poland";
                    break;
                case "PT":
                    countryFullName = "Portugal";
                    break;
                case "PR":
                    countryFullName = "Puerto Rico";
                    break;
                case "QA":
                    countryFullName = "Qatar";
                    break;
                case "RO":
                    countryFullName = "Romania";
                    break;
                case "RU":
                    countryFullName = "Russia";
                    break;
                case "RW":
                    countryFullName = "Rwanda";
                    break;
                case "BL":
                    countryFullName = "Saint Barthelemy";
                    break;
                case "WS":
                    countryFullName = "Samoa";
                    break;
                case "ST":
                    countryFullName = "Sao Tome and Principe";
                    break;
                case "SA":
                    countryFullName = "Saudi Arabia";
                    break;
                case "SN":
                    countryFullName = "Senegal";
                    break;
                case "RS":
                    countryFullName = "Serbia";
                    break;
                case "SC":
                    countryFullName = "Seychelles";
                    break;
                case "SL":
                    countryFullName = "Sierra Leone";
                    break;
                case "SG":
                    countryFullName = "Singapore";
                    break;
                case "SK":
                    countryFullName = "Slovakia";
                    break;
                case "SI":
                    countryFullName = "Slovenia";
                    break;
                case "SB":
                    countryFullName = "Solomon Islands";
                    break;
                case "SO":
                    countryFullName = "Somalia";
                    break;
                case "ZA":
                    countryFullName = "South Africa";
                    break;
                case "KR":
                    countryFullName = "South Korea";
                    break;
                case "ES":
                    countryFullName = "Spain";
                    break;
                case "LK":
                    countryFullName = "Sri Lanka";
                    break;
                case "SH":
                    countryFullName = "Saint Helena";
                    break;
                case "KN":
                    countryFullName = "Saint Kitts and Nevis";
                    break;
                case "LC":
                    countryFullName = "Saint Lucia";
                    break;
                case "MF":
                    countryFullName = "Saint Martin";
                    break;
                case "PM":
                    countryFullName = "Saint Pierre and Miguelon";
                    break;
                case "VC":
                    countryFullName = "Saint Vincent and the Grenadines";
                    break;
                case "SD":
                    countryFullName = "Sudan";
                    break;
                case "SR":
                    countryFullName = "Suriname";
                    break;
                case "SJ":
                    countryFullName = "Svalbard";
                    break;
                case "SZ":
                    countryFullName = "Swaziland";
                    break;
                case "SE":
                    countryFullName = "Sweden";
                    break;
                case "CH":
                    countryFullName = "Switzerland";
                    break;
                case "SY":
                    countryFullName = "Syria";
                    break;
                case "TW":
                    countryFullName = "Taiwan";
                    break;
                case "TJ":
                    countryFullName = "Tajikistan";
                    break;
                case "TZ":
                    countryFullName = "Tanzania";
                    break;
                case "TH":
                    countryFullName = "Thailand";
                    break;
                case "TG":
                    countryFullName = "Togo";
                    break;
                case "TK":
                    countryFullName = "Tokelau";
                    break;
                case "TO":
                    countryFullName = "Tonga";
                    break;
                case "TT":
                    countryFullName = "Trinidad and Tobago";
                    break;
                case "TN":
                    countryFullName = "Tunisia";
                    break;
                case "TR":
                    countryFullName = "Turkey";
                    break;
                case "TM":
                    countryFullName = "Turkmenistan";
                    break;
                case "TC":
                    countryFullName = "Turks and Caicos Islands";
                    break;
                case "TV":
                    countryFullName = "Tuvalu";
                    break;
                case "AE":
                    countryFullName = "United Arab Emirates";
                    break;
                case "UG":
                    countryFullName = "Uganda";
                    break;
                case "GB":
                    countryFullName = "United Kingdom";
                    break;
                case "UA":
                    countryFullName = "Ukraine";
                    break;
                case "UY":
                    countryFullName = "Uruguay";
                    break;
                case "US":
                    countryFullName = "United States";
                    break;
                case "UZ":
                    countryFullName = "Uzbekistan";
                    break;
                case "VU":
                    countryFullName = "Vanuatu";
                    break;
                case "VA":
                    countryFullName = "Holy See (Vatican City)";
                    break;
                case "VE":
                    countryFullName = "Venezuela";
                    break;
                case "VN":
                    countryFullName = "Vietnam";
                    break;
                case "VI":
                    countryFullName = "US Virgin Islands";
                    break;
                case "WF":
                    countryFullName = "Wallis and Futuna";
                    break;
                case "EH":
                    countryFullName = "Western Sahara";
                    break;
                case "YE":
                    countryFullName = "Yemen";
                    break;
                case "ZM":
                    countryFullName = "Zambia";
                    break;
                case "ZW":
                    countryFullName = "Zimbabwe";
                    break;
                default:
                    countryFullName = cSmallName;
                    break;
            }

            return countryFullName;
        }

        private void refreshLocation_Click(object sender, RoutedEventArgs e)
        {
            txtLocation.Text = "";
            getPositionUsingGPS();
        }

        private void closeShowWelcome_Click(object sender, RoutedEventArgs e)
        {
            showWelcome.IsOpen = false;
            BottomAppBar1.IsOpen = true;
        }
    }
}
