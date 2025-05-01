using AzureBlob.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//builder.Services.AddScoped<IBlobStorageService, ImageBlobStorageService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Video}/{action=Index}/{id?}");

app.Run();
