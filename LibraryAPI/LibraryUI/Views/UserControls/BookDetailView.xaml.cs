using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using LibraryApplication.DTOs;
using LibraryUI.Services;
using LibraryUI.Views.Windows;

namespace LibraryUI.Views.UserControls;

/// <summary>
/// Lógica de interacción para BookDetailView.xaml
/// </summary>
public partial class BookDetailView : UserControl
{
    private readonly BookClient _client;

    public BookDetailView(BookDTO? bookDTO)
    {
        InitializeComponent();
        _client = new BookClient();

        if (bookDTO != null)
        {
            txtISBN.Text = bookDTO.ISBN;
            txtTitle.Text = bookDTO.Title;
            txtAuthors.Text = bookDTO.Authors;
            txtGenres.Text = bookDTO.Genres;
            txtYear.Text = bookDTO.PublicationYear.ToString();
            txtPrice.Text = bookDTO.Price.ToString();
            txtItems.Text = bookDTO.NumberOfBooks.ToString();
            txtCreatedBy.Text = bookDTO?.CreatedBy?.Email ?? "";
        }
        else
        {
            txtISBN.Text = "";
            txtTitle.Text = "";
            txtAuthors.Text = "";
            txtGenres.Text = "";
            txtYear.Text = "";
            txtPrice.Text = "";
            txtItems.Text = "";
            txtCreatedBy.Text = "";
        }
    }

    private async void btnCreate_Click(object sender, RoutedEventArgs e)
    {
        var mainWindow = (MainWindow)Window.GetWindow(this);
        var currentUser = mainWindow.currentUser;

        try
        {
            if (!string.IsNullOrWhiteSpace(txtPrice.Text) && !string.IsNullOrWhiteSpace(txtItems.Text))
            {
                txtPrice.Text = txtPrice.Text.Replace(".", ",");

                if (!decimal.TryParse(txtPrice.Text, out decimal price))
                    MessageBox.Show(Application.Current.Resources["msgDec"].ToString());

                if (!int.TryParse(txtItems.Text, out int items))
                    MessageBox.Show(Application.Current.Resources["msgInt"].ToString());

                var bookCreationDTO = new BookCreationDTO
                {
                    ISBN = txtISBN.Text,
                    Title = txtTitle.Text,
                    Price = price,
                    NumberOfBooks = items,
                    CreatedBy = currentUser.Id
                };

                if (!string.IsNullOrWhiteSpace(txtYear.Text))
                {
                    if (int.TryParse(txtYear.Text, out int year))
                        bookCreationDTO.PublicationYear = year;
                }

                if (!string.IsNullOrWhiteSpace(txtAuthors.Text))
                {
                    bookCreationDTO.Authors = txtAuthors.Text.Split(',')
                          .Select(a => a.Trim())
                          .Where(a => !string.IsNullOrWhiteSpace(a))
                          .ToList();
                }

                if (!string.IsNullOrWhiteSpace(txtGenres.Text))
                {
                    bookCreationDTO.Genres = txtGenres.Text.Split(',')
                          .Select(a => a.Trim())
                          .Where(a => !string.IsNullOrWhiteSpace(a))
                          .ToList();
                }
                MessageBoxResult result = MessageBox.Show(
                    Application.Current.Resources["msgCreateBook?"].ToString(),
                    Application.Current.Resources["msgCreateBook"].ToString(),
                    MessageBoxButton.YesNo, MessageBoxImage.Question
                    );

                if (result == MessageBoxResult.Yes)
                {
                    await _client.CreateBookAsync(bookCreationDTO);
                    MessageBox.Show(Application.Current.Resources["msgBookCreated"].ToString());
                }
            }
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void btnUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(txtPrice.Text) && !string.IsNullOrWhiteSpace(txtItems.Text))
            {
                if (!decimal.TryParse(txtPrice.Text, out decimal price))
                    MessageBox.Show(Application.Current.Resources["msgDec"].ToString());

                if (!int.TryParse(txtItems.Text, out int items))
                    MessageBox.Show(Application.Current.Resources["msgInt"].ToString());

                var bookCreationDTO = new BookCreationDTO
                {
                    ISBN = txtISBN.Text,
                    Title = txtTitle.Text,
                    Price = price,
                    NumberOfBooks = items
                };

                if (!string.IsNullOrWhiteSpace(txtYear.Text))
                {
                    if (int.TryParse(txtYear.Text, out int year))
                        bookCreationDTO.PublicationYear = year;
                }

                if (!string.IsNullOrWhiteSpace(txtAuthors.Text))
                {
                    bookCreationDTO.Authors = txtAuthors.Text.Split(',')
                          .Select(a => a.Trim())
                          .Where(a => !string.IsNullOrWhiteSpace(a))
                          .ToList();
                }

                if (!string.IsNullOrWhiteSpace(txtGenres.Text))
                {
                    bookCreationDTO.Genres = txtGenres.Text.Split(',')
                          .Select(a => a.Trim())
                          .Where(a => !string.IsNullOrWhiteSpace(a))
                          .ToList();
                }

                MessageBoxResult result = MessageBox.Show(
                    Application.Current.Resources["msgUpdateBook?"].ToString(),
                    Application.Current.Resources["msgUpdateBook"].ToString(),
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await _client.UpdateBookAsync(bookCreationDTO.ISBN, bookCreationDTO);
                    MessageBox.Show(Application.Current.Resources["msgBookUpdated"].ToString());
                }
            }
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtISBN.Text))
            {
                MessageBox.Show(Application.Current.Resources["msgISBNReq"].ToString());
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                Application.Current.Resources["msgDeleteBook?"].ToString(),
                Application.Current.Resources["msgDeleteBook"].ToString(),
                   MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                await _client.DeleteBookAsync(txtISBN.Text);
                MessageBox.Show(Application.Current.Resources["msgBookDeleted"].ToString());
                txtAuthors.Text = string.Empty;
                txtGenres.Text = string.Empty;
                txtISBN.Text = string.Empty;
                txtItems.Text = string.Empty;
                txtPrice.Text = string.Empty;
                txtTitle.Text = string.Empty;
                txtYear.Text = string.Empty;
                txtCreatedBy.Text = string.Empty;
            }
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
