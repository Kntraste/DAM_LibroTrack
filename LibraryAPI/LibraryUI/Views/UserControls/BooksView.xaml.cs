using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LibraryApplication.DTOs;
using LibraryUI.Services;
using LibraryUI.Views.Windows;

namespace LibraryUI.Views.UserControls;

/// <summary>
/// Lógica de interacción para BooksView.xaml
/// </summary>
public partial class BooksView : UserControl
{
    private readonly BookClient _client;
    private IEnumerable<BookDTO> _books;

    public BooksView()
    {
        DataContext = this;
        InitializeComponent();
        _client = new BookClient();
        GetBooksAsync();
    }

    private async void btnSearch_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(txtTitle.Text) && string.IsNullOrWhiteSpace(txtAuthor.Text) && string.IsNullOrWhiteSpace(txtGenre.Text)
                 && string.IsNullOrWhiteSpace(txtISBN.Text) && string.IsNullOrWhiteSpace(txtMaxPrice.Text) && string.IsNullOrWhiteSpace(txtMinPrice.Text)
                  && string.IsNullOrWhiteSpace(txtYearAfter.Text) && string.IsNullOrWhiteSpace(txtYearBefore.Text))
            {
                dgBooks.ItemsSource = await _client.GetBookByTitle(txtTitle.Text);
            }
            else if (string.IsNullOrWhiteSpace(txtTitle.Text) && !string.IsNullOrWhiteSpace(txtAuthor.Text) && string.IsNullOrWhiteSpace(txtGenre.Text)
                 && string.IsNullOrWhiteSpace(txtISBN.Text) && string.IsNullOrWhiteSpace(txtMaxPrice.Text) && string.IsNullOrWhiteSpace(txtMinPrice.Text)
                  && string.IsNullOrWhiteSpace(txtYearAfter.Text) && string.IsNullOrWhiteSpace(txtYearBefore.Text))
            {
                dgBooks.ItemsSource = await _client.GetBooksByAuthor(txtAuthor.Text);
            }
            else if (string.IsNullOrWhiteSpace(txtTitle.Text) && string.IsNullOrWhiteSpace(txtAuthor.Text) && !string.IsNullOrWhiteSpace(txtGenre.Text)
                 && string.IsNullOrWhiteSpace(txtISBN.Text) && string.IsNullOrWhiteSpace(txtMaxPrice.Text) && string.IsNullOrWhiteSpace(txtMinPrice.Text)
                  && string.IsNullOrWhiteSpace(txtYearAfter.Text) && string.IsNullOrWhiteSpace(txtYearBefore.Text))
            {
                dgBooks.ItemsSource = await _client.GetBookByGenre(txtGenre.Text);
            }
            else if (string.IsNullOrWhiteSpace(txtTitle.Text) && string.IsNullOrWhiteSpace(txtAuthor.Text) && string.IsNullOrWhiteSpace(txtGenre.Text)
                 && !string.IsNullOrWhiteSpace(txtISBN.Text) && string.IsNullOrWhiteSpace(txtMaxPrice.Text) && string.IsNullOrWhiteSpace(txtMinPrice.Text)
                  && string.IsNullOrWhiteSpace(txtYearAfter.Text) && string.IsNullOrWhiteSpace(txtYearBefore.Text))
            {
                dgBooks.ItemsSource = await _client.GetBookByISBN(txtISBN.Text);
            }
            else if (string.IsNullOrWhiteSpace(txtTitle.Text) && string.IsNullOrWhiteSpace(txtAuthor.Text) && string.IsNullOrWhiteSpace(txtGenre.Text)
                 && string.IsNullOrWhiteSpace(txtISBN.Text) && !string.IsNullOrWhiteSpace(txtMaxPrice.Text) && !string.IsNullOrWhiteSpace(txtMinPrice.Text)
                  && string.IsNullOrWhiteSpace(txtYearAfter.Text) && string.IsNullOrWhiteSpace(txtYearBefore.Text))
            {
                if (decimal.TryParse(txtMinPrice.Text, out decimal min) && decimal.TryParse(txtMaxPrice.Text, out decimal max))
                    dgBooks.ItemsSource = await _client.GetBooksByPriceRange(min, max);
                else
                    MessageBox.Show(Application.Current.Resources["msgDec"].ToString());
            }
            else if (string.IsNullOrWhiteSpace(txtTitle.Text) && string.IsNullOrWhiteSpace(txtAuthor.Text) && string.IsNullOrWhiteSpace(txtGenre.Text)
                 && string.IsNullOrWhiteSpace(txtISBN.Text) && string.IsNullOrWhiteSpace(txtMaxPrice.Text) && string.IsNullOrWhiteSpace(txtMinPrice.Text)
                  && !string.IsNullOrWhiteSpace(txtYearAfter.Text) && !string.IsNullOrWhiteSpace(txtYearBefore.Text))
            {
                if (int.TryParse(txtYearAfter.Text, out int after) && int.TryParse(txtYearBefore.Text, out int before))
                    dgBooks.ItemsSource = await _client.GetBooksByYearRange(after, before);
                else
                    MessageBox.Show(Application.Current.Resources["msgInt"].ToString());
            }
            else if (string.IsNullOrWhiteSpace(txtTitle.Text) && string.IsNullOrWhiteSpace(txtAuthor.Text) && string.IsNullOrWhiteSpace(txtGenre.Text)
                 && string.IsNullOrWhiteSpace(txtISBN.Text) && string.IsNullOrWhiteSpace(txtMaxPrice.Text) && string.IsNullOrWhiteSpace(txtMinPrice.Text)
                  && string.IsNullOrWhiteSpace(txtYearAfter.Text) && string.IsNullOrWhiteSpace(txtYearBefore.Text))
            {
                GetBooksAsync();
            }
            else
            {
                MessageBox.Show(Application.Current.Resources["msgOneFieldFilled"].ToString());
            }
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            _books = Enumerable.Empty<BookDTO>();
            dgBooks.ItemsSource = _books;
        }
    }

    private async void dgBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if ((sender as DataGrid)?.SelectedItem is BookDTO bookDTO)
        {
            var bookWithUserDTO = await _client.GetBookById(bookDTO.Id);
            var bookDetailView = new BookDetailView(bookWithUserDTO);

            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.MainContent.Content = bookDetailView;
            }
        }
    }

    private async void GetBooksAsync()
    {
        try
        {
            dgBooks.ItemsSource = await _client.GetBooks();
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            dgBooks.ItemsSource = Enumerable.Empty<BookDTO>();
        }
    }

    private void btnCreate_Click(object sender, RoutedEventArgs e)
    {
        var bookDetailView = new BookDetailView(null);

        if (Window.GetWindow(this) is MainWindow mainWindow)
        {
            mainWindow.MainContent.Content = bookDetailView;
        }
    }
}
