using System;
using System.Xml;
using System.Xml.Linq;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Collections.Generic;
using System.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MuslimSuitePro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Quran : Page
    {
        int bookmarkedChapter = 1;
        int bookmarkedVerse = 1;
        TextBlock previousLoadedChapter = new TextBlock();
        TextBlock bookmarkedChapterPaneItem = new TextBlock();

        StackPanel codeBehindReadingSP = new StackPanel();
        StackPanel codeBehindChapterSP = new StackPanel();

        bool showTranslation = false;
        bool fileFound = false;
        string translationLanguageName = App.translationLanguage.Split(':')[1].ToLower();
        XElement translationReader = null;
        IEnumerable<Ayas> myTranslation = null;

        XmlReaderSettings settings = new XmlReaderSettings();


        public Quran()
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

        public async void loadTranslation()
        {

            string translationFilePath = "";

            if (AppSettings.loadSettings("translationEnabled") == "true")
            {
                showTranslation = true;
                try
                {
                    StorageFolder translationsFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("translations");
                    StorageFile localFilePath = await translationsFolder.GetFileAsync(translationLanguageName + ".xml");
                    translationFilePath = localFilePath.Path.ToString();
                    fileFound = true;
                }
                catch (Exception e)
                {
                    fileFound = false;
                }

                if (fileFound)
                {
                    try
                    {

                        translationReader = XElement.Load(translationFilePath);
                        fileFound = true;
                    }
                    catch (Exception e)
                    {
                        fileFound = false;
                    }
                }
            }
        }

        public IEnumerable<Ayas> linqQuery(int chapterNo)
        {
            IEnumerable<Ayas> ElementsList = null;

            var name = from nm in translationReader.Descendants("sura")
                       where nm.Attribute("index").Value.ToString() == chapterNo.ToString()
                       select nm;
            foreach (XElement xEle in name)
            {
                ElementsList = from Elements in xEle.Descendants("aya")
                        select new Ayas
                        {
                            text = Elements.Attribute("text").Value,
                            ayaID = Convert.ToInt16(Elements.Attribute("index").Value)
                        };
            }

            return ElementsList;
        }

        public class Ayas
        {

            public string text { get; set; }
            public int ayaID { get; set; }

        }

        public void loadChapterPane(int chapterNo)
        {
            codeBehindChapterSP = chapterSP;

            XmlReader xmlReader1 = XmlReader.Create("quran/quran-data.xml");
            while (xmlReader1.Read())
            {
                if ((xmlReader1.NodeType == XmlNodeType.Element) && (xmlReader1.Name == "sura"))
                {
                    TextBlock abc = new TextBlock();
                    abc.Name = "chapterNamesLeftPane" + xmlReader1.GetAttribute("index");
                    abc.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                    abc.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                    abc.Margin = new Thickness(10, 2, 10, 2);
                    abc.FlowDirection = Windows.UI.Xaml.FlowDirection.RightToLeft;
                    abc.FontFamily = new FontFamily("/fonts/me_quran.ttf#me_quran");

                    if (Convert.ToInt16(xmlReader1.GetAttribute("index").ToString()) == chapterNo)
                    {
                        abc.Foreground = new SolidColorBrush(Colors.Gold);
                        abc.FontWeight = Windows.UI.Text.FontWeights.ExtraBold;
                        if(Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenPortrait)
                            abc.FontSize = 20;
                        else
                            abc.FontSize = 32;
                        bookmarkedChapterPaneItem = abc;
                    }
                    else
                    {
                        abc.Foreground = new SolidColorBrush(Colors.Black);
                        abc.FontWeight = Windows.UI.Text.FontWeights.Bold;
                        if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenPortrait)
                            abc.FontSize = 18;
                        else
                            abc.FontSize = 30;
                    }

                    abc.Tapped += leftPaneItem_Tapped;
                    codeBehindChapterSP.Children.Add(abc);

                    if (xmlReader1.HasAttributes)
                    {
                        switch (App.quranChapterNameLang)
                        {
                            case (1):
                                {
                                    abc.Text = xmlReader1.GetAttribute("tname");
                                    break;
                                }
                            default:
                                {
                                    abc.Text = " سورة "  + xmlReader1.GetAttribute("name");
                                    break;
                                }
                        }
                    }

                    abc.Tag = xmlReader1.GetAttribute("index").ToString();
                }
            }
        }

        public void loadQuranPane(int chapterNo, int verseNo)
        {
            codeBehindReadingSP = readingSP;

            XmlReader quranTextReader = XmlReader.Create("quran/sura"+chapterNo+".xml", settings);

            int tcount = 0;

            if (showTranslation == true && fileFound)
            {
                myTranslation = linqQuery(chapterNo);
            }
            
            while (quranTextReader.Read())
            {
                if ((quranTextReader.NodeType == XmlNodeType.Element) && (quranTextReader.Name == "sura"))
                {
                    TextBlock sura = new TextBlock();
                    sura.Name = "chapterNamesRightPane" + quranTextReader.GetAttribute("index");
                    sura.Foreground = new SolidColorBrush(Colors.AliceBlue);
                    sura.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                    sura.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                    sura.FontSize = 45;
                    sura.FontWeight = Windows.UI.Text.FontWeights.Bold;
                    sura.Margin = new Thickness(5, 10, 5, 10);
                    sura.FlowDirection = Windows.UI.Xaml.FlowDirection.RightToLeft;
                    sura.FontFamily = new FontFamily("/fonts/me_quran.ttf#me_quran");
                    
                    codeBehindReadingSP.Children.Add(sura);

                    if (quranTextReader.HasAttributes)
                        sura.Text = " سورة " + quranTextReader.GetAttribute("name");
                }

                if ((quranTextReader.NodeType == XmlNodeType.Element) && (quranTextReader.Name == "aya"))
                {
                    if (quranTextReader.HasAttributes)
                    {

                        tcount = Convert.ToInt16(quranTextReader.GetAttribute("index"));
                        TextBlock aya = new TextBlock();
                        aya.Name = "ayaRightPane" + tcount;
                        aya.Foreground = new SolidColorBrush(Colors.White);
                        aya.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
                        aya.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
                        aya.TextWrapping = TextWrapping.Wrap;
                        aya.FontSize = 25;
                        aya.FontWeight = Windows.UI.Text.FontWeights.Bold;
                        aya.Margin = new Thickness(5, 10, 5, 10);
                        aya.FlowDirection = Windows.UI.Xaml.FlowDirection.RightToLeft;
                        aya.Tag = chapterNo + ":" + quranTextReader.GetAttribute("index").ToString();
                        aya.FontFamily = new FontFamily("/fonts/me_quran.ttf#me_quran");

                        aya.Tapped += rightPaneItem_Tapped;
                        codeBehindReadingSP.Children.Add(aya);

                        aya.Text = quranTextReader.GetAttribute("text");


                        //translation
                        if (showTranslation == true && fileFound)
                        {
                            try
                            {
                                TextBlock ayaTranslation = new TextBlock();
                                ayaTranslation.Name = "ayaTranslationRightPane" + myTranslation.ElementAt(tcount-1).ayaID;
                                ayaTranslation.Foreground = new SolidColorBrush(Colors.Black);
                                ayaTranslation.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
                                ayaTranslation.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
                                ayaTranslation.TextWrapping = TextWrapping.Wrap;
                                ayaTranslation.TextAlignment = TextAlignment.Right;
                                ayaTranslation.FontSize = 20;
                                ayaTranslation.FontWeight = Windows.UI.Text.FontWeights.Bold;
                                ayaTranslation.Margin = new Thickness(5, 10, 5, 10);

                                codeBehindReadingSP.Children.Add(ayaTranslation);

                                ayaTranslation.Text = myTranslation.ElementAt(tcount-1).text;
                            }
                            catch (Exception e)
                            {
                                tbHeading.Text = e.Message.ToString();
                                //MessageDialog msg = new MessageDialog(e.Message.ToString(), "Muslim Suite Pro");
                                //msg.ShowAsync();   
                            }
                        }

                    }
                }
            }

            if (chapterNo == bookmarkedChapter)
            {
                TextBlock previousBookmark = new TextBlock();
                previousBookmark = (TextBlock)codeBehindReadingSP.FindName(("ayaRightPane" + bookmarkedVerse));
                previousBookmark.Foreground = new SolidColorBrush(Colors.Gold);
            }

        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Snapped)
            {
                this.Frame.Navigate(typeof(SnappedViews.QuranSnapped));
            }
            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Filled || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenLandscape || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenPortrait)
            {
                this.Frame.Navigate(typeof(Quran));
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

            loadTranslation();

            if (App.quranReadingBookmark != "" && App.quranReadingBookmark != null)
            {
                bookmarkedChapter = Convert.ToInt16(App.quranReadingBookmark.Split(':')[0]);
                bookmarkedVerse = Convert.ToInt16(App.quranReadingBookmark.Split(':')[1]);
            }

            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;

            loadChapterPane(bookmarkedChapter);
            loadQuranPane(bookmarkedChapter, bookmarkedVerse);

            DoScrolls(bookmarkedChapter, bookmarkedVerse);
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void leftPaneItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            readingSP.Children.Clear();

            if (previousLoadedChapter != bookmarkedChapterPaneItem)
            {
                previousLoadedChapter.Foreground = new SolidColorBrush(Colors.Black);
                previousLoadedChapter.FontWeight = Windows.UI.Text.FontWeights.Normal;

                bookmarkedChapterPaneItem.Foreground = new SolidColorBrush(Colors.Black);
                bookmarkedChapterPaneItem.FontWeight = Windows.UI.Text.FontWeights.Normal;
            }

            previousLoadedChapter = ((TextBlock)sender);

            if (((TextBlock)sender) != bookmarkedChapterPaneItem)
            {
                ((TextBlock)sender).Foreground = new SolidColorBrush(Colors.Gold);
                ((TextBlock)sender).FontWeight = Windows.UI.Text.FontWeights.ExtraBold;
            }
            
            loadQuranPane(Convert.ToInt32(((TextBlock)sender).Tag.ToString()), 1);

            SaveBookmark(Convert.ToInt32(((TextBlock)sender).Tag.ToString()) + ":1");

            //MessageDialog msg = new MessageDialog(((TextBlock)sender).Name + " location: " + ((TextBlock)sender).ActualHeight + "\r\nTotal Scrollable height: " + chaptersSV.ScrollableHeight.ToString() + "\r\nCurrent Location in Scroll: " + (chaptersSV.VerticalOffset).ToString(), "Muslim Suite Pro");
            //msg.ShowAsync();
        }

        private void rightPaneItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            { 
                TextBlock previousBookmark = new TextBlock();
                previousBookmark = (TextBlock)codeBehindReadingSP.FindName(("ayaRightPane" + bookmarkedVerse)); 
                previousBookmark.Foreground = new SolidColorBrush(Colors.White);
            }
            catch(Exception err)
            {
                //exception will occure if the chapter is changed.   
            }

            //MessageDialog msg = new MessageDialog(previousBookmark.Text.ToString(), "Muslim Suite Pro");
            //await msg.ShowAsync();


            ((TextBlock)sender).Foreground = new SolidColorBrush(Colors.Gold);
            SaveBookmark(((TextBlock)sender).Tag.ToString());


            /*
            ToastTemplateType toastType = ToastTemplateType.ToastImageAndText02;
            XmlDocument toastXML = ToastNotificationManager.GetTemplateContent(toastType);
            XmlNodeList toastText = toastXML.GetElementsByTagName("text");
            XmlNodeList toastImages = toastXML.GetElementsByTagName("image");
            toastText[0].InnerText = "Muslim Suite Pro";
            toastText[1].InnerText = "Bookmark set on " + App.quranReadingBookmark;
            ((XmlElement)toastImages[0]).SetAttribute("src", "ms-appx:///Assets/SmallLogo.png");
            ((XmlElement)toastImages[0]).SetAttribute("alt", "Muslim Suite Pro");

            ToastNotification toast = new ToastNotification(toastXML);
            
            ToastNotificationManager.CreateToastNotifier().Show(toast);
            */

            //MessageDialog msg = new MessageDialog(((TextBlock)sender).Tag.ToString(), "Muslim Suite Pro");
            //await msg.ShowAsync();
        }

        private void DoScrolls(int chapterNo, int verseNo)
        {

            chaptersSV.UpdateLayout();
            readingSV.UpdateLayout();
            chapterSP.UpdateLayout();
            readingSP.UpdateLayout();

            TextBlock previousBookmark = new TextBlock();

            previousBookmark = (TextBlock)codeBehindChapterSP.FindName(("chapterNamesLeftPane" + chapterNo));
            double svNewPosition = (previousBookmark.ActualHeight * chapterNo) - previousBookmark.ActualHeight;
            chaptersSV.ScrollToVerticalOffset(svNewPosition);
            chaptersSV.UpdateLayout();

            previousBookmark = (TextBlock)codeBehindReadingSP.FindName(("ayaRightPane" + bookmarkedVerse));
            svNewPosition = (previousBookmark.ActualHeight * verseNo) - previousBookmark.ActualHeight;
            readingSV.ScrollToVerticalOffset(svNewPosition);
            readingSV.UpdateLayout();

            //MessageDialog msg = new MessageDialog(previousBookmark.Text + " TextBlock Height " + previousBookmark.ActualHeight.ToString() + " \r\nDifference: " + (position).ToString(), "Muslim Suite Pro");
            //msg.ShowAsync();


            
        }

        private void readingSV_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            //tbHeading.Text = readingSV.VerticalOffset.ToString();

            //have to SaveBookmark by checking what is in the scroll view now...
        }

        private void SaveBookmark(string BookMarkDetails)
        {
            App.quranReadingBookmark = BookMarkDetails;
            AppSettings.saveSettings("quranReadingBookmark", App.quranReadingBookmark);

            bookmarkedChapter = Convert.ToInt16(App.quranReadingBookmark.Split(':')[0]);
            bookmarkedVerse = Convert.ToInt16(App.quranReadingBookmark.Split(':')[1]);
        


        }
    }
}
