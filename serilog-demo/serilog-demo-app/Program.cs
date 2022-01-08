using custom_serilog_lib.SetupExtensions;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging(x => x.ClearProviders());
var configuration = builder.Configuration;
// Add services to the container.


builder.Services
.AddCustomSerilog(configuration)
.AddApiVersioning()
.AddControllers();
var MyAllowSpecificOrigins = "_allowSpecificOrigins";
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: MyAllowSpecificOrigins,
					  builder =>
					  {
						  builder.WithOrigins("http://localhost:4200");
					  });
});


var app = builder.Build();

app.Logger.LogInformation("Demo Web Application Started.");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action?}/{id?}");

app.MapFallbackToFile("index.html");
app.UseCors(MyAllowSpecificOrigins);
app.Run();
