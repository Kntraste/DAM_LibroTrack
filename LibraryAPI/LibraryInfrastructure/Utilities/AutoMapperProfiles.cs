using AutoMapper;
using LibraryApplication.DTOs;
using LibraryDomain.Models;

namespace LibraryInfrastructure.Utilities;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, UserDTO>()
            .ForMember(dto => dto.Role, config => config.MapFrom(user => MapUserRole(user)));

        CreateMap<UserBaseDTO, User>();

        CreateMap<UserCreationDTO, User>();

        CreateMap<UserUpdateDTO, User>();

        CreateMap<Book, BookDTO>()
            .ForMember(dto => dto.Authors, config => config.MapFrom(book => MapAuthorsList(book)))
            .ForMember(dto => dto.Genres, config => config.MapFrom(book => MapGenresList(book)))
            .ForMember(dto => dto.CreatedBy, config => config.Ignore());

        CreateMap<BookCreationDTO, Book>();
    }

    private string MapUserRole(User user) => user.Is_Admin ? "admin" : "user";

    private string MapAuthorsList(Book book) => book.Authors?.Any() == true ? string.Join(", ", book.Authors) : "N/A";

    private string MapGenresList(Book book) => book.Genres?.Any() == true ? string.Join(", ", book.Genres) : "N/A";
}
