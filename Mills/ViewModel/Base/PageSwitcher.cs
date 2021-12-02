using System.ComponentModel;

namespace Mills.ViewModel.Base
{
    public delegate void PageChangeRequestedEventHandler(object sender, PageChangeRequestedEventArgs e);

    public class PageSwitcher
    {
        public event PageChangeRequestedEventHandler PageChangeRequested;

        /// <summary>
        /// Löst das PageChangeRequested-Event aus.
        /// </summary>
        private void OnPageChangeRequested(INotifyPropertyChanged sender, string pageName)
        {
            PageChangeRequested.Invoke(sender, new PageChangeRequestedEventArgs
            {
                Pagename = pageName
            });
        }

        private static PageSwitcher instance;

        public static PageSwitcher Instance => instance ??= new PageSwitcher();

        public void SwitchPage(INotifyPropertyChanged sender, string pageName)
        {
            OnPageChangeRequested(sender, pageName);
        }
    }
}
