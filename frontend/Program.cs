using frontend;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient("weather-client", hc => {
    hc.BaseAddress = new Uri("http://backend:5000/");
}).AddPolicyHandler(ResiliencyPolicies.GetRetryPolicy());

builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.AddSeq(builder.Configuration.GetSection("Seq"));
    });

// builder.Services.AddHttpClient<WeatherClient>(client =>
//     {
//          client.BaseAddress = builder.Configuration.GetServiceUri("backend");
//     });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
