using Application.Services.AddListingImage;
using Application.Users.LogIn;
using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Core.Abstractions.UnitOfWork;
using Core.Entities;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Xml.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//-eklenenler-----------------------------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ConnContext>(options =>
    options.UseSqlite(connectionString));

//MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(LogInQuery).Assembly);
    
});

//Repository kaydı 
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IListingRepository, ListingRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();

builder.Services.AddScoped<IRepository<Transaction>, TransactionRepository>();
builder.Services.AddScoped<IRepository<Question>, QuestionRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Listing>, ListingRepository>();
builder.Services.AddScoped<IRepository<Image>, ImageRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//service kaydı
builder.Services.AddScoped<IAddListingImageService, AddListingImageService>();


//controller kaydı

builder.Services.AddControllers();

//Swagger ekleme
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen();



//-jwt token kaydı, swagger ın içine eklenecek.
//setup =>
//{
//    var jwtSecurityScheme = new OpenApiSecurityScheme
//    {

//        BearerFormat = "JWT",
//        Name = "JWT Authentication",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.Http,
//        Scheme = JwtBearerDefaults.AuthenticationScheme,
//        Description = "Bearer'a gerek yok.",

//        Reference = new OpenApiReference
//        {
//            Id = JwtBearerDefaults.AuthenticationScheme,
//            Type = ReferenceType.SecurityScheme
//        }
//    };
//    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

//    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        { jwtSecurityScheme, Array.Empty<string>() }
//    });
//}

//--------------------------------------------------------------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
}
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();




app.MapControllers();


app.Run();
