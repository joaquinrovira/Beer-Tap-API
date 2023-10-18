var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AutoRegister<Program>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
