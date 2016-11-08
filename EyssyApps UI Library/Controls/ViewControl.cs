namespace EyssyApps.UI.Library.Controls
{
    using System;
    using System.Windows.Controls;
    using Core.Library.Events;

    public class ViewControl : UserControl, IViewControl
    {
        public ViewControl()
        {
        }

        public event EventHandler<EventArgs<string>> ChangeView;
    }
}