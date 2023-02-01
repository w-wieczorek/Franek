using Avalonia.Controls;
using Franek.ViewModels;

namespace Franek.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MyReferences.MainWindow = this;
    }
}