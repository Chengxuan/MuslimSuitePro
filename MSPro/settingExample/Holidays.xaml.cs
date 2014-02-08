using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MuslimSuitePro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Holidays : Page
    {
        public Holidays()
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
        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Snapped || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenPortrait)
            {
                this.Frame.Navigate(typeof(SnappedViews.HolidaysSnapped));
            }
            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Filled || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenLandscape)
            {
                this.Frame.Navigate(typeof(Holidays));
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            /// <summary>
            /// Invoked when this page is about to be displayed in a Frame.
            /// </summary>
            /// <param name="e">Event data that describes how this page was reached.  The Parameter
            /// property is typically used to configure the page.</param>
            /// 
            loadBackground();
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
