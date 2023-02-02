using Avalonia.Controls;
using Franek.Models;
using Franek.ViewModels;

namespace Franek.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MyReferences.MainWindow = this;
        MyReferences.MyDataGrid = this.MyDataGrid;

        MyDataGrid.DoubleTapped += (sender, args) =>
        {
            Utwor? utwor = (Utwor?)MyDataGrid.SelectedItem;
            if (utwor is not null)
            {
                var flyout = new Flyout();
                var tb = new TextBlock();
                tb.Text = utwor.ToString();
                flyout.Content = tb;
                flyout.ShowMode = FlyoutShowMode.TransientWithDismissOnPointerMoveAway;
                flyout.ShowAt(MyDataGrid, true);
            }
        };
    }
}