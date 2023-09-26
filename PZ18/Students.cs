using MySql.Data.MySqlClient;

namespace PZ18;

public class Students 
{
    private string _connString = "server=localhost;database=pz18;port=3306;User Id=root;password=IGraf123*;Max Pool Size=100";
    public int id { get; set; }
    public string surname { get; set; }
    public string name { get; set; }
    public string group_name { get; set; }

    public void Update(string newSurname, string newName, string newGroupName)
    {
        surname = newSurname;
        name = newName;
        group_name = newGroupName;
        using (MySqlConnection connection = new MySqlConnection(_connString))
        {
            connection.Open();
            string sql = "UPDATE pz18.students SET surname = @Surname, name = @Name, group_id = @GroupName WHERE id = @Id";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Surname", newSurname);
            command.Parameters.AddWithValue("@Name", newName);
            command.Parameters.AddWithValue("@GroupName", newGroupName);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
    }

    
}