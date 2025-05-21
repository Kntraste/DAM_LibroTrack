using FluentValidation;
using LibraryApplication.DTOs;

namespace LibraryApplication.Validators;

public class UserDTOValidator : AbstractValidator<UserBaseDTO>
{
    private const int PasswordLength = 12;

    public UserDTOValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("{PropertyName} field is required")
            .EmailAddress().WithMessage("Invalid email format");
        RuleFor(u => u.Is_Admin)
            .NotNull().WithMessage("{PropertyName} field is required");

        When(x => x is UserCreationDTO, () =>
        {
            RuleFor(u => ((UserCreationDTO)u).Password)
            .NotEmpty().WithMessage("{PropertyName} field is required")
            .Length(PasswordLength).WithMessage("{PropertyName} must be {MinLength} length");
        });

        When(x => x is UserUpdateDTO, () =>
        {
            RuleFor(u => ((UserUpdateDTO)u).Theme)
                .IsEnumName(typeof(ThemeOptions), caseSensitive: false).WithMessage("{PropertyName} field must be one of: " +
                    string.Join(", ", Enum.GetNames(typeof(ThemeOptions))));
            RuleFor(u => ((UserUpdateDTO)u).Language)
                .IsEnumName(typeof(LanguageOptions), caseSensitive: false).WithMessage("{PropertyName} field must be one of: " +
                    string.Join(", ", Enum.GetNames(typeof(LanguageOptions))));
        });
    }

    public enum ThemeOptions
    {
        dark,
        light
    }

    public enum LanguageOptions
    {
        english,
        spanish
    }
}
