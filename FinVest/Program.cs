var builder = WebApplication.CreateBuilder(args);

// Add Clerk services
builder.Services.AddClerk(options =>
{
    options.ApiKey = "your_clerk_api_key";
});

builder.Services.AddRazorPages();
builder.Services.AddSingleton<PlaidService>();

var app = builder.Build();

// Add Clerk middleware
app.UseClerk();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
