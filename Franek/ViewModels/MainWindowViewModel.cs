using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Franek.Models;
using Franek.Views;

namespace Franek.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private string? _wyszukiwanieKompozytor;
    [ObservableProperty] private string? _wyszukiwanieTytul;
    [ObservableProperty] private List<string> _dostepneOkresy = new();
    [ObservableProperty] private List<string> _dostepneFormy = new();
    [ObservableProperty] private List<string> _dostepneCharaktery = new();
    [ObservableProperty] private List<string> _dostepnePoziomy = new();

    [ObservableProperty] private int _wybranyIdx;
    
    public ObservableCollection<Utwor> ZnalezioneUtwory { get; }

    public MainWindowViewModel()
    {
        ZnalezioneUtwory = new(new Utwor[] {});
        WybranyIdx = 0;
        DostepneOkresy.Add("Dowolny okres");
        DostepneOkresy.Add("romantyzm");
        DostepneOkresy.Add("XX wiek");
    }
    
    [RelayCommand]
    public void OnEnter()
    {
        if (ZnalezioneUtwory.Count > 0 && WybranyIdx >= 0 && WybranyIdx < ZnalezioneUtwory.Count)
        {
            string plikDoSkasowania = ZnalezioneUtwory[WybranyIdx].Pdf ?? throw new ArgumentNullException("ZnalezioneUtwory[WybranyIdx].Pdf");
            Process foxit = new Process();

            foxit.StartInfo.FileName = $"\"{ConfigurationManager.AppSettings["pdfreader"]}\"";
            foxit.StartInfo.Arguments = $"{ConfigurationManager.AppSettings["katalog"]}\\{plikDoSkasowania}";

            foxit.Start();
        }
    }

    [RelayCommand]
    public void OnWyszukaj()
    {
        DataAccess db = new DataAccess();
        List<Utwor> listaUtworow = db.ZnajdzUtwory(new Utwor { Id = 0 });
        ZnalezioneUtwory.Clear();
        foreach (var d in listaUtworow)
        {
            ZnalezioneUtwory.Add(d);
        }
    }

    [RelayCommand]
    public async Task OnDelete()
    {
        if (ZnalezioneUtwory.Count > 0 && WybranyIdx >= 0 && WybranyIdx < ZnalezioneUtwory.Count)
        {
            int idUtworu = ZnalezioneUtwory[WybranyIdx].Id;
            var odpowiedz = await MessageBox.Show(MyReferences.MainWindow, 
                $"\n          Czy na pewno usunąć utwór nr {idUtworu}?          \n",
                "Proszę potwierdzić", MessageBox.MessageBoxButtons.YesNo);
        }
    }                                   
}