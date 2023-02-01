using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Franek.Models;

public class DataAccess
{
    public List<Utwor> ZnajdzUtwory(Utwor pytanie)
    {
        SQLiteConnection connection = new SQLiteConnection();
        connection.ConnectionString = ConfigurationManager.ConnectionStrings["Myconnstr"].ConnectionString;
        connection.Open();
        var sb = new StringBuilder();
        sb.Append("SELECT * FROM tabela WHERE id >= 0");
        if (pytanie.Kompozytor is not null && pytanie.Kompozytor != "")
        {
            sb.Append($" AND kompozytor LIKE '%{pytanie.Kompozytor}%' COLLATE utf8_general_ci");
        }
        if (pytanie.Tytul is not null && pytanie.Tytul != "")
        {
            sb.Append($" AND tytul LIKE '%{pytanie.Tytul}%' COLLATE utf8_general_ci");
        }
        if (pytanie.Okres is not null)
        {
            sb.Append($" AND okres = '{pytanie.Okres}'");
        }
        if (pytanie.Forma is not null)
        {
            sb.Append($" AND forma = '{pytanie.Forma}'");
        }
        

        var output = connection.Query<Utwor>(sb.ToString()).ToList();
        connection.Close();
        return output;
    }

    public void ModyfikujUwagi(int nr, string nowaUwaga)
    {
        SQLiteConnection connection = new SQLiteConnection();
        connection.ConnectionString = ConfigurationManager.ConnectionStrings["Myconnstr"].ConnectionString;
        connection.Open();
        connection.Execute($"UPDATE tabela SET inne = '{nowaUwaga}' WHERE id = {nr}");
        connection.Close();
    }

    public async Task<int> WykonajZmiany(List<Utwor> nowaLista, List<Utwor> staraLista)
    {
        int liczbaZmian = 0;
        SQLiteConnection connection = new SQLiteConnection();
        connection.ConnectionString = ConfigurationManager.ConnectionStrings["Myconnstr"].ConnectionString;
        await connection.OpenAsync();
        foreach (Utwor stary in staraLista)
        {
            Utwor? nowy = nowaLista.Find(u => u.Id == stary.Id);
            if (nowy is null)
            {
                await connection.ExecuteAsync($"DELETE FROM tabela WHERE id = {stary.Id}");
                ++liczbaZmian;
            }
            else if (nowy.Inne != stary.Inne)
            {
                await connection.ExecuteAsync($"UPDATE tabela SET inne = '{nowy.Inne}' WHERE id = {stary.Id}");
                ++liczbaZmian;
            }
        }
        await connection.CloseAsync();
        return liczbaZmian;
    }
}