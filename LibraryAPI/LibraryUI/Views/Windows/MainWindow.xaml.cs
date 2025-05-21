using LibraryApplication.DTOs;
using LibraryUI.Views.UserControls;
using System.Windows;

namespace LibraryUI.Views.Windows;

/// <summary>
/// Lógica de interacción para MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public UserDTO? currentUser;

    public MainWindow()
    {
        DataContext = this;
        InitializeComponent();
        NavigateToLogIn();
        MenuBar.OnAboutUsClicked += (sender, eventArgs) => NavigateToAboutUs();
        MenuBar.OnBooksClicked += (sender, eventArgs) => NavigateToBooks();
        MenuBar.OnLogOutClicked += (sender, eventArgs) => NavigateToLogInFromLogOut();
        MenuBar.OnUsersClicked += (sender, eventArgs) => NavigateToUsers();
    }

    private void NavigateToLogIn()
    {
        var logInView = new LogInView();
        logInView.OnLogInSuccesful += (sender, userDTO) =>
        {
            currentUser = userDTO;

            MenuBar.miAboutUs.Visibility = Visibility.Visible;
            MenuBar.miLogOut.Visibility = Visibility.Visible;
            MenuBar.miBooks.Visibility = Visibility.Visible;
            MenuBar.miConfiguration.Visibility = Visibility.Visible;
            if (currentUser.Role == "admin")
                MenuBar.miUsers.Visibility = Visibility.Visible;
            NavigateToBooks();
        };

        MainContent.Content = logInView;
    }

    public void NavigateToLogInFromLogOut()
    {
        currentUser = null;
        var logInView = new LogInView();
        logInView.OnLogInSuccesful += (sender, userDTO) =>
        {
            currentUser = userDTO;

            ((App)Application.Current).ChangeLanguage(userDTO.Language);

            MenuBar.miAboutUs.Visibility = Visibility.Visible;
            MenuBar.miLogOut.Visibility = Visibility.Visible;
            MenuBar.miBooks.Visibility = Visibility.Visible;
            MenuBar.miConfiguration.Visibility = Visibility.Visible;
            if (currentUser.Role == "admin")
                MenuBar.miUsers.Visibility = Visibility.Visible;
            NavigateToBooks();
        };

        MainContent.Content = logInView;
    }

    private void NavigateToBooks() => MainContent.Content = new BooksView();

    private void NavigateToAboutUs() => MainContent.Content = new AboutUsView();

    private void NavigateToUsers() => MainContent.Content = new UsersView();
}
