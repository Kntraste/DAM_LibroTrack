using System.Text.RegularExpressions;
using FluentValidation;
using LibraryApplication.DTOs;

namespace LibraryApplication.Validators;

public class BookDTOValidator : AbstractValidator<BookCreationDTO>
{
    private const int MinNumberOfBooks = 0;
    private const int MaxNumberOfBooks = 10000;
    private const decimal MinPrice = 0;
    private const decimal MaxPrice = 9000000;
    private const int MinLength = 1;
    private const int MaxLength = 50;
    private const int MinYear = 868;

    public BookDTOValidator()
    {
        RuleFor(b => b.ISBN)
            .NotEmpty().WithMessage("{PropertyName} field is required")
            .Must(IsValidISBN).WithMessage("Invalid {PropertyName} format");

        RuleFor(b => b.Title)
            .NotEmpty().WithMessage("{PropertyName} field is required")
            .Length(MinLength, MaxLength).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} length");

        RuleFor(b => b.PublicationYear)
            .InclusiveBetween(MinYear, DateTime.UtcNow.Year).WithMessage("{PropertyName} field must be between {From} and {To}");

        RuleForEach(b => b.Authors)
            .Length(MinLength, MaxLength).WithMessage("{PropertyName}' names must be between {MinLength} and {MaxLength} length");

        RuleForEach(b => b.Genres)
            .Length(MinLength, MaxLength).WithMessage("{PropertyName}' names must be between {MinLength} and {MaxLength} length");

        RuleFor(b => b.Price)
            .NotNull().WithMessage("{PropertyName} field is required")
            .InclusiveBetween(MinPrice, MaxPrice).WithMessage("{PropertyName} must be between {From} and {To}");

        RuleFor(b => b.NumberOfBooks)
            .NotNull().WithMessage("{PropertyName} field is required")
            .InclusiveBetween(MinNumberOfBooks, MaxNumberOfBooks).WithMessage("{PropertyName} must be between {From} and {To}");
    }

    private bool IsValidISBN(string isbn) =>
        Regex.IsMatch(isbn, @"^[0-9]{9}[0-9X]$") || Regex.IsMatch(isbn, @"^97[89]\d{10}$");
}
