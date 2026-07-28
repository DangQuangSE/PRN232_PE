using Q2;

var builder = WebApplication.CreateBuilder(args);

//Initialize UrlUtilities with configuration
//DO NOT change this code
Utilities.Initialize(builder.Configuration);
//End

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddRazorPages(); 
var app = builder.Build();
app.MapRazorPages();

app.Run();
