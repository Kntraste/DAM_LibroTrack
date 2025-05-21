using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using LibraryApplication.DTOs;
using LibraryUI.Services;
using LibraryUI.Views.Windows;

namespace LibraryUI.Views.UserControls;

/// <summary>
/// Lógica de interacción para UserDetailView.xaml
/// </summary>
public partial class UserDetailView : UserControl
{
    private readonly UserClient _client;

    public UserDetailView(UserDTO? userDTO)
    {
        InitializeComponent();
        _client = new UserClient();

        if (userDTO != null)
        {
            txtEmail.Text = userDTO.Email;
            txtPassword.Password = "";
            txtRole.Text = userDTO.Role;
        }
        else
        {
            txtEmail.Text = "";
            txtPassword.Password = "";
            txtRole.Text = "";
        }
    }

    private async void btnCreate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !string.IsNullOrWhiteSpace(txtPassword.Password)
                && !string.IsNullOrWhiteSpace(txtRole.Text))
            {
                var userCreationDTO = new UserCreationDTO
                {
                    Email = txtEmail.Text,
                    Password = txtPassword.Password,
                    Is_Admin = txtRole.Text == "admin" ? true : false
                };

                MessageBoxResult result = MessageBox.Show(
                    Application.Current.Resources["msgCreateUser?"].ToString(),
                    Application.Current.Resources["msgCreateUser"].ToString(),
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await _client.CreateUserAsync(userCreationDTO);
                    MessageBox.Show(Application.Current.Resources["msgUserCreated"].ToString());
                }
            }
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void btnUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !string.IsNullOrWhiteSpace(txtRole.Text))
            {
                if (txtRole.Text != "admin" && txtRole.Text != "user")
                {
                    MessageBox.Show(Application.Current.Resources["msgRole"].ToString());
                    return;
                }

                var userUpdateDTO = new UserUpdateDTO
                {
                    Email = txtEmail.Text,
                    Is_Admin = txtRole.Text == "admin" ? true : false
                };

                MessageBoxResult result = MessageBox.Show(
                    Application.Current.Resources["msgUpdateUser?"].ToString(),
                    Application.Current.Resources["msgUpdateUser"].ToString(),
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await _client.UpdateUserAsync(userUpdateDTO.Email, userUpdateDTO);
                    MessageBox.Show(Application.Current.Resources["msgUserUpdated"].ToString());

                    if (Window.GetWindow(this) is MainWindow mainWindow)
                    {
                        if (mainWindow.currentUser!.Email == txtEmail.Text && !userUpdateDTO.Is_Admin)
                        {
                            mainWindow.currentUser = null;
                            mainWindow.MenuBar.miLogOut.Visibility = Visibility.Collapsed;
                            mainWindow.MenuBar.miBooks.Visibility = Visibility.Collapsed;
                            mainWindow.MenuBar.miConfiguration.Visibility = Visibility.Collapsed;
                            mainWindow.MenuBar.miUsers.Visibility = Visibility.Collapsed;
                            mainWindow.NavigateToLogInFromLogOut();
                            mainWindow.MainContent.UpdateLayout();
                        }
                    }
                }
            }
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show(Application.Current.Resources["msgEmailReq"].ToString());
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                Application.Current.Resources["msgDeleteUser?"].ToString(),
                Application.Current.Resources["msgDeleteUser"].ToString(),
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                await _client.DeleteUserAsync(txtEmail.Text);
                MessageBox.Show(Application.Current.Resources["msgUserDeleted"].ToString());
                var email = txtEmail.Text;
                txtEmail.Text = string.Empty;
                txtPassword.Password = string.Empty;
                txtRole.Text = string.Empty;

                if (Window.GetWindow(this) is MainWindow mainWindow)
                {
                    if (mainWindow.currentUser!.Email == email)
                    {
                        mainWindow.currentUser = null;
                        mainWindow.MenuBar.miLogOut.Visibility = Visibility.Collapsed;
                        mainWindow.MenuBar.miBooks.Visibility = Visibility.Collapsed;
                        mainWindow.MenuBar.miConfiguration.Visibility = Visibility.Collapsed;
                        mainWindow.MenuBar.miUsers.Visibility = Visibility.Collapsed;
                        mainWindow.NavigateToLogInFromLogOut();
                        mainWindow.MainContent.UpdateLayout();
                    }
                }
            }
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
