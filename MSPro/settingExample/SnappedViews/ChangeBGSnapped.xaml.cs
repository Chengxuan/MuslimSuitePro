using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MuslimSuitePro.SnappedViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChangeBGSnapped : Page
    {

        Border previouslySelect = null; // to store the brush of the image which was previously in selected queue
        Image newSelectedImage = null; // to store the reference of the newely seleted image

        int argument = 0;

        public ChangeBGSnapped()
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

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {

            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Snapped || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenPortrait)
            {
                this.Frame.Navigate(typeof(SnappedViews.ChangeBGSnapped),argument);
            }
            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Filled || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenLandscape)
            {
                this.Frame.Navigate(typeof(ChangeBackground), argument);
            }
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            /// <summary>
            /// Invoked when this page is about to be displayed in a Frame.
            /// </summary>
            /// <param name="e">Event data that describes how this page was reached.  The Parameter
            /// property is typically used to configure the page.</param>
            /// 

            try
            {
                argument = (int)e.Parameter;
            }
            catch (Exception e1)
            { }

            string savedBackgroundImageName = App.backgroundImage;

            if (savedBackgroundImageName != null)
            {
                savedBackgroundImageName = savedBackgroundImageName.Split('.')[0];
                if (savedBackgroundImageName != "")
                {
                    Image savedBackgroundImageInstance = (Image)FindName(savedBackgroundImageName);
                    savedBackgroundImageInstance.Opacity = 0.6;
                    newSelectedImage = savedBackgroundImageInstance;

                    Border bb = (Border)FindName("bb" + savedBackgroundImageName);
                    bb.BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Gray);
                    previouslySelect = bb;
                    //BottomAppBar.IsOpen = true;
                }
            }
        }

        private void Image_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            newSelectedImage = (Image)sender;
            newSelectedImage.Opacity = 0.6;

            if (previouslySelect != null)
                previouslySelect.BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.White);

            Border bb = (Border)newSelectedImage.Parent;
            bb.BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Gray);
            previouslySelect = bb;
            BottomAppBar.IsOpen = true;
        }

        private async void saveSelection_Click(object sender, RoutedEventArgs e)
        {
            string bgImage;

            if (newSelectedImage == null)
            {
                MessageDialog msg = new MessageDialog("Please choose an image for the background", "Muslim Suite Pro");
                await msg.ShowAsync();
            }
            else
            {
                bgImage = newSelectedImage.Tag.ToString(); // get the name of the selected image which is stored in the tag
                AppSettings.saveSettings("backgroundImage", bgImage); // save the image name in the settings
                App.backgroundImage = bgImage;


                if (argument == 1)
                {
                    if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Snapped || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenPortrait)
                    {
                        this.Frame.Navigate(typeof(SnappedViews.PrayerSettingsSnapped), argument);
                    }
                    if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Filled || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenLandscape)
                    {
                        this.Frame.Navigate(typeof(PrayerSettings), argument);
                    }
                }
                else
                {
                    this.Frame.Navigate(typeof(MainPageSnapped), argument);
                }
            }
        }
    }
}
