using Business.Abstract;
using Business.Concrete;
using Core.Infrastructure;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Filters;
using MailSendler.Abstract;
using MailSendler.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IUserService, UserManager>();
builder.Services.AddSingleton<IUserDal, UserDal>();
builder.Services.AddSingleton<ILogDal, LogDal>();
builder.Services.AddSingleton<ILogService, LogManager>();
builder.Services.AddSingleton<IUserTicketDal, UserTicketDal>();
builder.Services.AddSingleton<JwtTokenConfig, JwtTokenConfig>(); //Soru ??
builder.Services.AddSingleton<IJwtAuthManager, JwtAuthManager>();
builder.Services.AddCors(options =>
{
  options.AddPolicy("MyPolicy", builder =>
  {
    builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
  });
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddSingleton<ICompanySettingDal, CompanySettingDal>();
builder.Services.AddSingleton<ICompanySettingService, CompanySettingManager>();
builder.Services.AddSingleton<IMailSendlerService, MailSendlerService>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("MyPolicy");
app.MapControllers();

app.Run();
