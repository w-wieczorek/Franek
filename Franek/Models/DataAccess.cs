using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using Dapper;

namespace Franek.Models;

public class DataAccess
{
    public List<Utwor> ZnajdzUtwory(Utwor pytanie)
    {
        SQLiteConnection connection = new SQLiteConnection();
        connection.ConnectionString = ConfigurationManager.ConnectionStrings["Myconnstr"].ConnectionString;
        connection.Open();
        var output = connection.Query<Utwor>("SELECT * FROM tabela").ToList();
        connection.Close();
        return output;
    }
}