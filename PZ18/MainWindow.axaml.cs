using System;
using System.Collections.Generic;
using System.Data;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;


namespace PZ18;

public partial class MainWindow : Window
{
    protected string _connString = "server=localhost;database=pz18;port=3306;User Id=root;password=IGraf123*;Max Pool Size=100";
    private List<Students> _students;
    private MySqlConnection _connection;
    public MainWindow()
    {
        InitializeComponent();
        ShowTable();
    }

    public void ShowTable()
    {
        _students = new List<Students>();
        _connection = new MySqlConnection(_connString);
        _connection.Open();
        string sql = "select id, surname, name, group_name from pz18.students\njoin pz18.`groups` g on g.group_id = students.group_id;";
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentStudent = new Students()
            {
                id = reader.GetInt32("id"),
                surname = reader.GetString("surname"),
                name = reader.GetString("name"),
                group_name = reader.GetString("group_name")
            };
            _students.Add(currentStudent);
        }
        _connection.Close();
        StudentGrid.ItemsSource = _students;
    }

    private void GroupsWindowBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Hide();
        GroupsWindow groupsWindow = new GroupsWindow();
        groupsWindow.Show();
    }

    private void EditBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Students selectedStudent = StudentGrid.SelectedItem as Students;
        if (selectedStudent != null)
        {
            EditWindow editWindow = new EditWindow(selectedStudent);
            editWindow.Show();
            //ShowTable();
        }
        else
        {
           Console.WriteLine("Выберите студента!");
        }
    }

    private void DeleteBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Students selectedStudent = StudentGrid.SelectedItem as Students;
    
        if (selectedStudent != null)
        {
                Delete(selectedStudent.id);
                ShowTable();
        }
        else
        {
            Console.WriteLine("Выберите студента!");
        }
    }
    public void Delete(int id)
    {
        using (MySqlConnection connection = new MySqlConnection(_connString))
        {
            connection.Open();
            string sql = "DELETE FROM pz18.students WHERE id = @StudentId";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@StudentId", id);
            command.ExecuteNonQuery();
        }
    }
}