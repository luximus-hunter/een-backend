using Een.Api.Middleware;
using Een.Socket;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Policy",
        policy =>
        {
            policy
                .WithOrigins(
                    "https://een.luximus.eu", // Linked URL
                    "https://uno-frontend", // Deployed container
                    "http://localhost:3000" // Development environment
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

WebApplication app = builder.Build();

app.UseAuthMiddleware();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("Policy");
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHub<GamesHub>("/games");

app.Run("http://::5000");