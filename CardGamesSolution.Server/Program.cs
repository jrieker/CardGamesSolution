using CardGamesSolution.Server.UserAccount;
using CardGamesSolution.Server.Database;
using CardGamesSolution.Server.Blackjack;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IDatabaseConnection, DatabaseConnection>();
builder.Services.AddScoped<IUserDataAccessor, UserDataAccessor>();

builder.Services.AddScoped<ILoginManager, LoginManager>();

builder.Services.AddSingleton<BlackJackEngine>();
builder.Services.AddScoped<IBlackJackManager, BlackJackManager>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
