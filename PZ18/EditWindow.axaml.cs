using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace PZ18;

public partial class EditWindow : MainWindow
{
    private Students _students;
    public EditWindow(Students students)
    {
        InitializeComponent();
        _students = students;
        surnameTextBox.Text = _students.surname;
        nameTextBox.Text = _students.name;
        groupTextBox.Text = _students.group_name;
    }


    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        string newSurname = surnameTextBox.Text;
        string newName = nameTextBox.Text;
        string newGroupName = groupTextBox.Text;
        _students.Update(newSurname, newName, newGroupName);
        this.Close();
        ShowTable(fullTable);
    }
    
}