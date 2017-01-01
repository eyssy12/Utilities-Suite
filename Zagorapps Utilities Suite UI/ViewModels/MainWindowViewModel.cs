namespace Zagorapps.Utilities.Suite.UI.ViewModels
{
    using System;
    using System.Windows.Input;
    using Controls;

    public class MainWindowViewModel : ViewModelBase
    {
        private int suiteIndex = 0,
            previousIndex = 0;

        private string mainColorzoneText;

        public int SuiteIndex
        {
            get { return this.suiteIndex; }
            set { this.suiteIndex = value; }
        }

        public string MainColorzoneText
        {
            get { return this.mainColorzoneText; }
            set { this.SetFieldIfChanged(ref this.mainColorzoneText, value, nameof(this.MainColorzoneText)); }
        }

        public void FormatAndSetMainColorzoneText(string suiteName)
        {
            this.MainColorzoneText = Environment.UserName + "'s Utilities Suite - " + suiteName;
        }

        public bool IsSuiteChangeWithKeyboardShortcutApplied(KeyEventArgs e, int maxiumItems)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key != Key.LeftCtrl)
            {
                if (e.Key == Key.Home)
                {
                    this.previousIndex = this.suiteIndex = 0;

                    return true;
                }
                else if (e.Key == Key.Up || e.Key == Key.Down)
                {
                    int min = 0;
                    int max = maxiumItems;

                    this.previousIndex = this.suiteIndex;

                    if (e.Key == Key.Up && this.suiteIndex < max)
                    {
                        this.suiteIndex++;
                    }
                    else if (e.Key == Key.Down && this.suiteIndex > min)
                    {
                        this.suiteIndex--;
                    }

                    // we want to trigger navigation only when the e.Key value is Up or Down
                    if (this.suiteIndex != this.previousIndex)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}