//using EarlyBirdAPI;
//using EarlyBirdAPI.Model.Repositories;
using EarlyBird.Model.Repositories;
using EarlyBirdAPI.Model;
using Microsoft.EntityFrameworkCore;
using EarlyBirdAPI.Model.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure the DbContext with PostgreSQL
builder.Services.AddDbContext<EarlyBirdDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("EBDatabase")));

//This will make the UserRepository available for dependency injection in your controllers.
builder.Services.AddScoped<UserRepository, UserRepository>();

builder.Services.AddScoped<SavedJobRepository, SavedJobRepository>();
builder.Services.AddScoped<ResumeRepository, ResumeRepository>();
builder.Services.AddScoped<JobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<ApplicationManagementRepository>();
builder.Services.AddScoped<JobRepository>();





// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection(); //PROMPTED TO REMOVE THIS LINE

app.UseAuthorization();

app.MapControllers();

app.Run();
