using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace MuslimSuitePro.flyouts
{
    public partial class SettingsHelper
    {
        private Popup m_Popup = new Popup();
        private readonly List<ICommandInfo> Commands = new List<ICommandInfo>();
        public SettingsHelper()
        {
            SettingsPane.GetForCurrentView().CommandsRequested += (s, e) =>
            {
                foreach (var item in Commands)
                {
                    var _Command = new SettingsCommand(item.Key, item.Text,
                        c => { Show(item.Instance(), item.Width); });
                    e.Request.ApplicationCommands.Add(_Command);
                }
            };
        }

        public void AddCommand<T>(string text, string key = null, PanelWidths width = PanelWidths.Small) where T : UserControl, new()
        {
            CommandInfo<T> cmd =
                new CommandInfo<T>()
                {
                    Key = key ?? text,
                    Text = text,
                    Width = (int)width
                };

            if (null == this.Commands.FirstOrDefault(c => c.Key.Equals(cmd.Key)))
                this.Commands.Add(cmd);

        }

        private Popup Show(UserControl control, double width)
        {
            if (control == null)
                throw new Exception("Control is not defined");
            if (double.IsNaN(width))
                throw new Exception("Width is not defined");

            // layout
            m_Popup.Width = width;
            m_Popup.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;
            m_Popup.Height = Window.Current.Bounds.Height;
            m_Popup.SetValue(Canvas.LeftProperty, Window.Current.Bounds.Width - m_Popup.Width);

            // make content fit
            m_Popup.Child = control;
            control.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
            control.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            control.Height = m_Popup.Height;
            control.Width = m_Popup.Width;

            // add pretty animation(s)
            m_Popup.ChildTransitions = new Windows.UI.Xaml.Media.Animation.TransitionCollection 
        { 
            new Windows.UI.Xaml.Media.Animation.EntranceThemeTransition 
            { 
                FromHorizontalOffset = 20d, 
                FromVerticalOffset = 0d 
            }
        };

            // setup
            m_Popup.IsLightDismissEnabled = true;
            m_Popup.IsOpen = true;

            // handle when it closes
            m_Popup.Closed -= popup_Closed;
            m_Popup.Closed += popup_Closed;

            // handle making it close
            Window.Current.Activated -= Current_Activated;
            Window.Current.Activated += Current_Activated;

            // return
            return m_Popup;
        }

        private void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (m_Popup == null)
                return;
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
                m_Popup.IsOpen = false;
        }

        private void popup_Closed(object sender, object e)
        {
            Window.Current.Activated -= Current_Activated;
            if (m_Popup == null)
                return;
            m_Popup.IsOpen = false;
        }

        private interface ICommandInfo
        {
            string Key { get; set; }
            string Text { get; set; }
            double Width { get; set; }
            UserControl Instance();
        }

        private class CommandInfo<T> : ICommandInfo where T : UserControl, new()
        {
            public string Key { get; set; }
            public string Text { get; set; }
            public double Width { get; set; }
            public UserControl Instance() { return new T(); }
        }

        public enum PanelWidths : int
        {
            Small = 346,
            Large = 646
        }
    }

}
