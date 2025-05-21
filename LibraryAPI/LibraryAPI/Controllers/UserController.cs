using AutoMapper;
using FluentValidation;
using LibraryApplication.DTOs;
using LibraryApplication.Services;
using LibraryDomain.Interfaces;
using LibraryDomain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<UserBaseDTO> _validatorDTO;
    private readonly IHashService _hashService;
    private readonly IEmailNotificationService _notificationService;

    public UserController(IUserRepository userRepository, IMapper mapper,
        IValidator<UserBaseDTO> validatorDTO, IHashService hashService, IEmailNotificationService notificationService)
    {
        _repository = userRepository;
        _mapper = mapper;
        _validatorDTO = validatorDTO;
        _hashService = hashService;
        _notificationService = notificationService;
    }

    [HttpGet("users")]
    public async Task<IEnumerable<UserDTO>> GetAllUsers()
    {
        var users = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDTO>>(users);
    }

    [HttpGet("role/{role}")]
    public async Task<IEnumerable<UserDTO>> GetAllAdmins(string role)
    {
        var isAdmin = false;

        if (role != "admin" && role != "user")
            return Enumerable.Empty<UserDTO>();

        if (role == "admin")
            isAdmin = true;

        var admins = await _repository.GetAdminsAsync(isAdmin);

        return _mapper.Map<IEnumerable<UserDTO>>(admins);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUserById(string id)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var userDTO = _mapper.Map<UserDTO>(user);

        return Ok(userDTO);
    }

    [HttpGet("email/{email}")]
    public async Task<IEnumerable<UserDTO>> GetUserByEmail(string email)
    {
        var user = await _repository.GetByEmailAsync(email);

        if (user == null)
        {
            return Enumerable.Empty<UserDTO>();
        }

        List<User> users = new List<User> { user };

        return _mapper.Map<List<UserDTO>>(users);
    }

    [HttpGet("login/{email},{password}")]
    public async Task<ActionResult<UserDTO>> LogIn(string email, string password)
    {
        var validPassword = IsValidPassword(password);

        if (!validPassword)
        {
            return BadRequest("Password must be 12 characters length");
        }

        var user = await _repository.GetByEmailAsync(email);

        if (user == null)
        {
            return BadRequest("Incorrect login");
        }

        var isValidPassword = _hashService.VerifyHashedPassword(password, user.Password);

        if (!isValidPassword)
        {
            return BadRequest("Incorrect login");
        }

        var userDTO = _mapper.Map<UserDTO>(user);

        return Ok(userDTO);
    }

    [HttpPost]
    public async Task<ActionResult> PostUser(UserCreationDTO userCreationDTO)
    {
        var result = _validatorDTO.Validate(userCreationDTO);

        if (!result.IsValid)
        {
            return BadRequest(result.ToDictionary());
        }

        userCreationDTO.Password = _hashService.HashPassword(userCreationDTO.Password);
        var userExists = await _repository.GetByEmailAsync(userCreationDTO.Email);

        if (userExists != null)
        {
            return BadRequest("This email is used");
        }

        var user = _mapper.Map<User>(userCreationDTO);
        await _repository.CreateAsync(user);

        //_notificationService.SendEmail(
        //    destination: user.Email,
        //    subject: "User Created",
        //    body: "Your user has been created succesfully");

        var userDTO = _mapper.Map<UserDTO>(user);

        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, userDTO);
    }

    [HttpPut("{email}")]
    public async Task<ActionResult> PutUser(string email, UserUpdateDTO userUpdateDTO)
    {
        var result = _validatorDTO.Validate(userUpdateDTO);

        if (!result.IsValid)
        {
            return BadRequest(result.ToDictionary());
        }

        var userExists = await _repository.GetByEmailAsync(userUpdateDTO.Email);

        if (userExists == null)
        {
            return NotFound();
        }

        var user = _mapper.Map<User>(userUpdateDTO);
        user.Id = userExists.Id;
        user.Password = userExists.Password;
        user.Language = userExists.Language;
        user.Theme = userExists.Theme;

        //_notificationService.SendEmail(
        //    destination: user.Email,
        //    subject: "User Updated",
        //    body: "Your user has been updated succesfully");

        await _repository.UpdateAsync(user.Id, user);

        return NoContent();
    }

    [HttpPut("configuration/{id}")]
    public async Task<ActionResult> PutUserConfiguration(string id, UserUpdateDTO userUpdateDTO)
    {
        var result = _validatorDTO.Validate(userUpdateDTO);

        if (!result.IsValid)
        {
            return BadRequest(result.ToDictionary());
        }

        var userExists = await _repository.GetByIdAsync(id);

        if (userExists == null)
        {
            return NotFound();
        }

        var user = _mapper.Map<User>(userUpdateDTO);
        user.Id = id;
        user.Is_Admin = userExists.Is_Admin;
        user.Password = userExists.Password;

        if (string.IsNullOrEmpty(user.Language))
            user.Language = userExists.Language;

        if (string.IsNullOrEmpty(user.Theme))
            user.Theme = userExists.Theme;

        await _repository.UpdateAsync(id, user);

        return NoContent();
    }

    [HttpDelete("{email}")]
    public async Task<ActionResult> DeleteUser(string email)
    {
        var userExists = await _repository.GetByEmailAsync(email);

        if (userExists == null)
        {
            return NotFound();
        }

        var result = await _repository.DeleteAsync(userExists.Id);

        if (result == 0)
        {
            return NotFound();
        }

        return NoContent();
    }

    private bool IsValidPassword(string password)
    {
        if (password.Length != 12)
        {
            return false;
        }

        return true;
    }
}
