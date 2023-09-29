using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace PZ18;

public partial class GroupsWindow : Window
{
    private string _connString = "server=10.10.1.24;database=pro1_4;port=3306;User Id=user_01;password=user01pro";
    private List<Groups> _groups;
    private MySqlConnection _connection;
    public GroupsWindow()
    {
        InitializeComponent();
        string fullTable = "select * from pro1_4.`groups`;";
        ShowTable(fullTable);
    }
    public void ShowTable(string sql)
    {
        _groups = new List<Groups>();
        _connection = new MySqlConnection(_connString);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentGroup = new Groups()
            {
                group_id = reader.GetInt32("group_id"),
                group_name = reader.GetString("group_name")
            };
            _groups.Add(currentGroup);
        }
        _connection.Close();
        GroupGrid.ItemsSource = _groups;
    }

    private void MainWindowBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Hide();
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
    }
}