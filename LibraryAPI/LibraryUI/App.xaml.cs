using LibraryUI.Views.Windows;
using System.Windows;

namespace LibraryUI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public void ChangeLanguage(string language)
    {
        var dict = new ResourceDictionary();

        switch (language)
        {
            case "english":
                dict.Source = new Uri("/Resources/Dictionaries/Languages/English.xaml", UriKind.Relative);
                break;

            case "spanish":
                dict.Source = new Uri("/Resources/Dictionaries/Languages/Spanish.xaml", UriKind.Relative);
                break;
        }

        Application.Current.Resources.MergedDictionaries.Clear();
        Application.Current.Resources.MergedDictionaries.Add(dict);

        var mainWindow = Current.MainWindow as MainWindow;
        var themeDict = new ResourceDictionary();

        if (mainWindow!.currentUser!.Theme is null || mainWindow!.currentUser.Theme?.ToLower() == "dark")
        {
            themeDict.Source = new Uri("/Resources/Dictionaries/Themes/Dark.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(themeDict);
        }
        else if (mainWindow.currentUser.Theme?.ToLower() == "light")
        {
            themeDict.Source = new Uri("/Resources/Dictionaries/Themes/Light.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(themeDict);
        }
    }

    public void ChangeTheme(string theme)
    {
        var dict = new ResourceDictionary();

        switch (theme)
        {
            case "dark":
                dict.Source = new Uri("/Resources/Dictionaries/Themes/Dark.xaml", UriKind.Relative);
                break;

            case "light":
                dict.Source = new Uri("/Resources/Dictionaries/Themes/Light.xaml", UriKind.Relative);
                break;
        }

        Application.Current.Resources.MergedDictionaries.Clear();
        Application.Current.Resources.MergedDictionaries.Add(dict);

        var mainWindow = Current.MainWindow as MainWindow;
        var langDict = new ResourceDictionary();

        if (mainWindow!.currentUser!.Language is null || mainWindow!.currentUser.Language?.ToLower() == "english")
        {
            langDict.Source = new Uri("/Resources/Dictionaries/Languages/English.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(langDict);
        }
        else if (mainWindow.currentUser.Language?.ToLower() == "spanish")
        {
            langDict.Source = new Uri("/Resources/Dictionaries/Languages/Spanish.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(langDict);
        }
    }
}
