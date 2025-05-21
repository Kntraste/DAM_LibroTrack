using System.Windows;
using System.Windows.Controls;
using LibraryApplication.DTOs;
using LibraryUI.Services;
using LibraryUI.Views.Windows;

namespace LibraryUI.Views.UserControls;

/// <summary>
/// Lógica de interacción para MenuBar.xaml
/// </summary>
public partial class MenuBar : UserControl
{
    private readonly UserClient _client;
    private MainWindow _mainWindow;
    private UserDTO _currentUser;
    private UserUpdateDTO _userUpdateDTO;

    public event EventHandler OnBooksClicked;

    public event EventHandler OnUsersClicked;

    public event EventHandler OnAboutUsClicked;

    public event EventHandler OnLogOutClicked;

    public MenuBar()
    {
        InitializeComponent();
        _client = new UserClient();
    }

    private void miBooks_Click(object sender, RoutedEventArgs e)
    {
        OnBooksClicked?.Invoke(this, EventArgs.Empty);
    }

    private void miUsers_Click(object sender, RoutedEventArgs e)
    {
        OnUsersClicked?.Invoke(this, EventArgs.Empty);
    }

    private void miAboutUs_Click(object sender, RoutedEventArgs e)
    {
        OnAboutUsClicked?.Invoke(this, EventArgs.Empty);
    }

    private void miLogOut_Click(object sender, RoutedEventArgs e)
    {
        miAboutUs.Visibility = Visibility.Collapsed;
        miLogOut.Visibility = Visibility.Collapsed;
        miBooks.Visibility = Visibility.Collapsed;
        miUsers.Visibility = Visibility.Collapsed;
        miConfiguration.Visibility = Visibility.Collapsed;

        OnLogOutClicked?.Invoke(this, EventArgs.Empty);
    }

    private void miExit_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private async void miEnglish_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow = (MainWindow)Window.GetWindow(this);
        _currentUser = _mainWindow.currentUser!;
        _currentUser.Language = "english";
        _userUpdateDTO = new UserUpdateDTO
        {
            Email = _mainWindow.currentUser!.Email,
            Language = _currentUser.Language
        };

        ((App)Application.Current).ChangeLanguage("english");

        await _client.UpdateUserConfigAsync(_currentUser.Id, _userUpdateDTO);
    }

    private async void miSpanish_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow = (MainWindow)Window.GetWindow(this);
        _currentUser = _mainWindow.currentUser!;
        _currentUser.Language = "spanish";
        _userUpdateDTO = new UserUpdateDTO
        {
            Email = _mainWindow.currentUser!.Email,
            Language = _currentUser.Language
        };

        ((App)Application.Current).ChangeLanguage("spanish");

        await _client.UpdateUserConfigAsync(_currentUser.Id, _userUpdateDTO);
    }

    private async void miDark_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow = (MainWindow)Window.GetWindow(this);
        _currentUser = _mainWindow.currentUser!;
        _currentUser.Theme = "dark";
        _userUpdateDTO = new UserUpdateDTO
        {
            Email = _mainWindow.currentUser!.Email,
            Theme = _currentUser.Theme
        };

        ((App)Application.Current).ChangeTheme("dark");

        await _client.UpdateUserConfigAsync(_currentUser.Id, _userUpdateDTO);
    }

    private async void miLight_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow = (MainWindow)Window.GetWindow(this);
        _currentUser = _mainWindow.currentUser!;
        _currentUser.Theme = "light";
        _userUpdateDTO = new UserUpdateDTO
        {
            Email = _mainWindow.currentUser!.Email,
            Theme = _currentUser.Theme
        };

        ((App)Application.Current).ChangeTheme("light");

        await _client.UpdateUserConfigAsync(_currentUser.Id, _userUpdateDTO);
    }
}
