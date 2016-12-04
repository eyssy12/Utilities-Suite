namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class ConfirmDialog : UserControl
    {
        private Grid grid;
        private TextBlock textBlock;
        private DockPanel dockPanel;

        public ConfirmDialog()
        {
            this.grid = new Grid();

            ColumnDefinition def = new ColumnDefinition();
            def.Width = new GridLength(0, GridUnitType.Star);

            ColumnDefinition def2 = new ColumnDefinition();
            def2.Width = new GridLength(1, GridUnitType.Star);

            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(0, GridUnitType.Star);

            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(1, GridUnitType.Star);

            grid.ColumnDefinitions.Add(def);
            grid.ColumnDefinitions.Add(def2);

            grid.RowDefinitions.Add(row);
            grid.RowDefinitions.Add(row2);

            this.textBlock = new TextBlock();
            this.textBlock.Text = "Are you sure?";
            this.textBlock.FontSize = 22;
            this.textBlock.Margin = new Thickness(16);
        }

        public string Text
        {
            get { return this.textBlock.Text; }
            set { this.textBlock.Text = value; }
        }

        public double TextFontSize
        {
            get { return this.textBlock.FontSize; }
            set { this.textBlock.FontSize = value; }
        }

    //    <DockPanel
    //        Grid.Row="1"
    //        Grid.Column= "0"
    //        Grid.ColumnSpan= "2"
    //        Margin= "16" >

    //    < Button
    //        Style= "{StaticResource MaterialDesignFlatButton}"
    //        IsCancel= "True"
    //        DockPanel.Dock= "Left"
    //        Command= "{x:Static materialDesign:DialogHost.CloseDialogCommand}"
    //        CommandParameter= "Cancel" >

    //        CANCEL
    //    </ Button >

    //    < Button Style= "{StaticResource MaterialDesignFlatButton}"
    //        Foreground= "OrangeRed"
    //        IsCancel= "False"
    //        HorizontalAlignment= "Right"
    //        DockPanel.Dock= "Right"
    //        Command= "{x:Static materialDesign:DialogHost.CloseDialogCommand}"
    //        CommandParameter= "Confirm" >

    //        CONFIRM
    //    </ Button >
    //</ DockPanel >
    }
}
