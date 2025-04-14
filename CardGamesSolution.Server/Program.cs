using CardGamesSolution.Server.UserAccount;
using CardGamesSolution.Server.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<LoginManager>();
builder.Services.AddScoped<IUserDataAccessor, UserDataAccessor>();
builder.Services.AddScoped<IDatabaseConnection, DatabaseConnection>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();