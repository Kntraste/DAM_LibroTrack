using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace LibraryUI.Views.UserControls;

/// <summary>
/// Lógica de interacción para AboutUsView.xaml
/// </summary>
public partial class AboutUsView : UserControl
{
    public AboutUsView()
    {
        InitializeComponent();
    }

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        e.Handled = true;
    }
}
