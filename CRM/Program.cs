using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CRM.Data;
using System.Text;
using CRM.Interfaces;
using CRM.Helpers;
using CRM.Repository;
using CRM.Services;
using CRM.Helper;
using CRM.Models;

var builder = WebApplication.CreateBuilder(args);

string? conn = builder.Configuration.GetConnectionString(name: "DataBase");

builder.Services.AddDbContext<Context>(options =>
options.UseSqlServer((conn) ?? throw new InvalidOperationException("Connection string 'CRM' not found.")));
//Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ISendEmail, SendEmail>();
builder.Services.AddSingleton<IUserSession, Session>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<SendFileImageRepository, SendFileImageRepository>();
builder.Services.AddScoped<SendFileService, SendFileService>();
builder.Services.AddScoped<ICustomerPurchasesRepository, CustomerPurchasesRepository>();
builder.Services.AddScoped<AnalyticsServices, AnalyticsServices>();
builder.Services.AddScoped<CustomerStatusVerify, CustomerStatusVerify>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddCors();
var key = Encoding.ASCII.GetBytes(SettingsToken.Secret);

builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
	x.RequireHttpsMetadata = true;
	x.SaveToken = true;
	x.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer = false,
		ValidateAudience = false,
		ValidateLifetime = true

	};
});


builder.Services.AddSession(obj =>
{
	obj.Cookie.HttpOnly = true;
	obj.Cookie.IsEssential = true;
	obj.IdleTimeout = TimeSpan.FromHours(8);
	obj.IOTimeout = TimeSpan.FromSeconds(30);

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Login/Login");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(x => x
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
