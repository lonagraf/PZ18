using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;


namespace PZ18;

public partial class MainWindow : Window
{
    protected string _connString = "server=10.10.1.24;database=pro1_4;port=3306;User Id=user_01;password=user01pro;Max Pool Size=100";
    private List<Students> _students;
    private MySqlConnection _connection;
    private List<Groups> _groups;
    public string fullTable = "select id, surname, name, group_name from pro1_4.students\njoin pro1_4.`groups` g on g.group_id = students.group_id;";

    public MainWindow()
    {
        InitializeComponent();
        ShowTable(fullTable);
        //FilterComboBox.ItemsSource = _students;
        FillGroupsList();
    }

    public void ShowTable(string sql)
    {
        _students = new List<Students>();
        _connection = new MySqlConnection(_connString);
        _connection.Open();
        //string sql = "select id, surname, name, group_name from pz18.students\njoin pz18.`groups` g on g.group_id = students.group_id;";
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
            ShowTable(fullTable);
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
                ShowTable(fullTable);
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
            string sql = "DELETE FROM pro1_4.students WHERE id = @StudentId";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@StudentId", id);
            command.ExecuteNonQuery();
        }
    }

    public void FillGroupsList()
    {
        _groups = new List<Groups>();
        _connection = new MySqlConnection(_connString);
        _connection.Open();
        MySqlCommand command = new MySqlCommand("select * from pro1_4.groups;", _connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentGroup = new Groups()
            {
                group_id = reader.GetInt32("group_id"),
                group_name = reader.GetString("group_name"),
            };
            _groups.Add(currentGroup);
        }
        _connection.Close();
        var groupsComboBox = this.Find<ComboBox>("FilterComboBox");
        groupsComboBox.ItemsSource = _groups;
    }

    private void FilterComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var filterComboBox = (ComboBox)sender;
        var currentGroup = filterComboBox.SelectedItem as Groups;
        var filteredStudents = _students.Where(x => x.group_name == currentGroup.group_name).ToList();
        StudentGrid.ItemsSource = filteredStudents;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        /*string orderBy =
            "select id, surname, name, group_name from pro1_4.students join pro1_4.`groups` g on g.group_id = students.group_id order by group_name;";*/
        //ShowTable(orderBy);
        List<Students> OrderBy = _students.OrderBy(x => x.group_name).ToList();
        StudentGrid.ItemsSource = OrderBy;
    }

    private void TxtSearch_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        /*string searchSql = "select id, surname, name, group_name from pro1_4.students join pro1_4.`groups` g on g.group_id = students.group_id where surname like '%" +
                            txtSearch.Text + "%';";*/
        //ShowTable(searchSql);
        List<Students> Search = _students.Where(x => x.surname.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
        StudentGrid.ItemsSource = Search;
    }
    
}