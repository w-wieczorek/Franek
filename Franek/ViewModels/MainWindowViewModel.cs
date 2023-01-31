using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Franek.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private string? _wyszukiwanieKompozytor;
    [ObservableProperty] private string? _wyszukiwanieTytul;
    [ObservableProperty] private List<string> _dostepneOkresy = new();
    [ObservableProperty] private List<string> _dostepneFormy = new();
    [ObservableProperty] private List<string> _dostepneCharaktery = new();
    [ObservableProperty] private List<string> _dostepnePoziomy = new();

    public MainWindowViewModel()
    {
        DostepneOkresy.Add("romantyzm");
        DostepneOkresy.Add("XX wiek");
    }
}