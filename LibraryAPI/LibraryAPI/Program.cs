using FluentValidation;
using LibraryApplication.DTOs;
using LibraryApplication.Services;
using LibraryApplication.Validators;
using LibraryDomain.Interfaces;
using LibraryDomain.Repository;
using LibraryDomain.Settings;
using LibraryInfrastructure.Repository;
using LibraryInfrastructure.Utilities;

var builder = WebApplication.CreateBuilder(args);

var smtpServer = builder.Configuration["EmailSettings:SmtpServer"];
var smtpPort = builder.Configuration.GetValue<int>("EmailSettings:SmtpPort");
var username = builder.Configuration["EmailSettings:SmtpUsername"];
var password = builder.Configuration["EmailSettings:SmtpPassword"];

// Add services to the container.
builder.Services.AddSingleton<IEmailNotificationService>(x =>
    new EmailNotificationService(
        smtpServer!,
        smtpPort,
        username!,
        password!)
    );

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection(nameof(MongoDBSettings)));

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddTransient<IHashService, HashService>();
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddScoped<IValidator<BookCreationDTO>, BookDTOValidator>();
builder.Services.AddScoped<IValidator<UserBaseDTO>, UserDTOValidator>();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
