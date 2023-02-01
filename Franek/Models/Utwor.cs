using System.Text;

namespace Franek.Models;

public class Utwor
{
    public int Id { get; set; }
    public string? Kompozytor { get; set; }
    public string? Tytul { get; set; }
    public string? Okres { get; set; }
    public string? Forma { get; set; }
    public string? Charakter { get; set; }
    public string? Poziom { get; set; }
    public string? Inne { get; set; }
    public string? Pdf { get; set; }

    public Utwor()
    {
        Id = -1;
        Kompozytor = null;
        Tytul = null;
        Okres = null;
        Forma = null;
        Charakter = null;
        Poziom = null;
        Inne = null;
        Pdf = null;
    }
    
    public Utwor(Utwor other)
    {
        Id = other.Id;
        Kompozytor = other.Kompozytor;
        Tytul = other.Tytul;
        Okres = other.Okres;
        Forma = other.Forma;
        Charakter = other.Charakter;
        Poziom = other.Poziom;
        Inne = other.Inne;
        Pdf = other.Pdf;
    }
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"{Id}. {Kompozytor}: {Tytul}\n");
        string drugiWiersz = Inne is null || Inne == "" ? "Brak uwag" : Inne;
        sb.Append(drugiWiersz);
        return sb.ToString();
    }
}
