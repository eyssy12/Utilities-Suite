using System;
using System.Windows;
using System.Windows.Controls;
using EyssyApps.Core.Library.Events;
using EyssyApps.UI.Library.Controls;

namespace File.Organiser.UI.Views
{
    /// <summary>
    /// Interaction logic for IndividualTask.xaml
    /// </summary>
    public partial class IndividualTask : UserControl, IViewControl
    {
        public IndividualTask()
        {
            InitializeComponent();
        }

        public event EventHandler<EventArgs<string>> ChangeView;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Invoker.Raise(ref this.ChangeView, this, "HomeView");
        }
    }
}