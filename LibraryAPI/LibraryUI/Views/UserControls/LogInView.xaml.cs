using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using LibraryApplication.DTOs;
using LibraryUI.Services;

namespace LibraryUI.Views.UserControls;

/// <summary>
/// Lógica de interacción para LogInView.xaml
/// </summary>
public partial class LogInView : UserControl
{
    public event EventHandler<UserDTO> OnLogInSuccesful;

    public LogInView()
    {
        InitializeComponent();
    }

    private async void btnLogIn_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
        {
            MessageBox.Show(Application.Current.Resources["msgBothFieldsFilled"].ToString(),
                "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            return;
        }

        try
        {
            var email = txtEmail.Text;
            var password = txtPassword.Password;
            var client = new UserClient();
            var userDTO = await client.GetLogIn(email!, password!);

            OnLogInSuccesful?.Invoke(this, userDTO);
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(Application.Current.Resources["msgWrongAuth"].ToString(),
                "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
