using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LibraryApplication.DTOs;
using LibraryUI.Services;
using LibraryUI.Views.Windows;

namespace LibraryUI.Views.UserControls;

/// <summary>
/// Lógica de interacción para UsersView.xaml
/// </summary>
public partial class UsersView : UserControl
{
    private readonly UserClient _client;
    private IEnumerable<UserDTO> _users;

    public UsersView()
    {
        DataContext = this;
        InitializeComponent();
        _client = new UserClient();
        GetUsersAsync();
    }

    private async void btnSearch_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) && !string.IsNullOrWhiteSpace(txtRole.Text))
            {
                if (txtRole.Text != "admin" && txtRole.Text != "user")
                {
                    MessageBox.Show(Application.Current.Resources["msgRole"].ToString());
                    return;
                }

                dgUsers.ItemsSource = await _client.GetAdmins(txtRole.Text);
            }
            else if (!string.IsNullOrWhiteSpace(txtEmail.Text) && string.IsNullOrWhiteSpace(txtRole.Text))
            {
                dgUsers.ItemsSource = await _client.GetUserByEmail(txtEmail.Text);
            }
            else if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !string.IsNullOrWhiteSpace(txtRole.Text))
                MessageBox.Show(Application.Current.Resources["msgOneFieldEmpty"].ToString());
            else
                GetUsersAsync();
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            _users = Enumerable.Empty<UserDTO>();
            dgUsers.ItemsSource = _users;
        }
    }

    private void dgUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if ((sender as DataGrid)?.SelectedItem is UserDTO userDTO)
        {
            var userDetailView = new UserDetailView(userDTO);

            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.MainContent.Content = userDetailView;
            }
        }
    }

    private async void GetUsersAsync()
    {
        try
        {
            dgUsers.ItemsSource = await _client.GetUsers();
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            dgUsers.ItemsSource = Enumerable.Empty<UserDTO>();
        }
    }

    private void btnCreate_Click(object sender, RoutedEventArgs e)
    {
        var userDetailView = new UserDetailView(null);

        if (Window.GetWindow(this) is MainWindow mainWindow)
        {
            mainWindow.MainContent.Content = userDetailView;
        }
    }
}
