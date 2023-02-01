using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
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
    [ObservableProperty] private int _idxWybranyOkres = 0;
    [ObservableProperty] private int _idxWybranaForma = 0;
    [ObservableProperty] private int _idxWybranyCharakter = 0;
    [ObservableProperty] private int _idxWybranyPoziom = 0;

    [ObservableProperty] private object _wybranyItem;
    
    public ObservableCollection<Utwor> ZnalezioneUtwory { get; }
    private List<Utwor> znalezionePrzedZmianami;

    public MainWindowViewModel()
    {
        ZnalezioneUtwory = new(new Utwor[] {});
        znalezionePrzedZmianami = new();
        DostepneOkresy.Add("Dowolny okres");
        DostepneOkresy.AddRange(ConfigurationManager.AppSettings["okres"]!.Split(','));
        DostepneFormy.Add("Dowolna forma");
        DostepneFormy.AddRange(ConfigurationManager.AppSettings["forma"]!.Split(','));
        DostepneCharaktery.Add("Dowolny charakter");
        DostepneCharaktery.AddRange(ConfigurationManager.AppSettings["charakter"]!.Split(','));
        DostepnePoziomy.Add("Dowolny poziom");
        DostepnePoziomy.AddRange(ConfigurationManager.AppSettings["poziom"]!.Split(','));
    }
    
    [RelayCommand]
    public void OnWyszukaj()
    {
        Utwor utworPytanie = new Utwor
        {
            Kompozytor = WyszukiwanieKompozytor,
            Tytul = WyszukiwanieTytul,
            Okres = IdxWybranyOkres == 0 ? null : DostepneOkresy[IdxWybranyOkres],
            Forma = IdxWybranaForma == 0 ? null : DostepneFormy[IdxWybranaForma],
            Charakter = IdxWybranyCharakter == 0 ? null : DostepneCharaktery[IdxWybranyCharakter],
            Poziom = IdxWybranyPoziom == 0 ? null : DostepnePoziomy[IdxWybranyPoziom],
            Inne = null,
            Pdf = null
        };
        DataAccess db = new DataAccess();
        List<Utwor> listaUtworow = db.ZnajdzUtwory(utworPytanie);
        ZnalezioneUtwory.Clear();
        znalezionePrzedZmianami.Clear();
        foreach (var d in listaUtworow)
        {
            ZnalezioneUtwory.Add(d);
            znalezionePrzedZmianami.Add(new Utwor(d));
        }
    }

    [RelayCommand]
    public async Task OnZmiany()
    {
        DataAccess db = new DataAccess();
        int liczbaZmian = await db.WykonajZmiany(ZnalezioneUtwory.ToList(), znalezionePrzedZmianami);
        await MessageBox.Show(MyReferences.MainWindow,
            $"\n          Liczba wykonanych zmian wynosi {liczbaZmian}.          \n",
            "Podsumowanie", MessageBox.MessageBoxButtons.Ok);
    }

    /*
    [RelayCommand]
    public void OnEnter()
    {
        if (ZnalezioneUtwory.Count > 0)
        {
            Utwor? utwor = (Utwor?)WybranyItem;
            if (utwor is not null)
            {
                DataAccess db = new DataAccess();
                db.ModyfikujUwagi(utwor.Id, utwor.Inne ?? "");
            }
        }
    }
    */

    [RelayCommand]
    public async Task OnDelete()
    {
        if (ZnalezioneUtwory.Count > 0)
        {
            Utwor? utwor = (Utwor?)WybranyItem;
            if (utwor is not null)
            {
                var odpowiedz = await MessageBox.Show(MyReferences.MainWindow,
                    $"\n          Czy na pewno usunąć utwór nr {utwor.Id}?          \n",
                    "Proszę potwierdzić", MessageBox.MessageBoxButtons.YesNo);
                if (odpowiedz == MessageBox.MessageBoxResult.Yes)
                {
                    ZnalezioneUtwory.Remove(utwor);
                }
            }
        }
    }

    [RelayCommand]
    public void OnF10()
    {
        if (ZnalezioneUtwory.Count > 0)
        {
            Utwor? utwor = (Utwor?)WybranyItem;
            if (utwor is not null)
            {
                Process foxit = new Process();

                foxit.StartInfo.FileName = $"\"{ConfigurationManager.AppSettings["pdfreader"]}\"";
                foxit.StartInfo.Arguments = $"{ConfigurationManager.AppSettings["katalog"]}\\{utwor.Pdf}";

                foxit.Start();
            }
        }
    }                                   

}