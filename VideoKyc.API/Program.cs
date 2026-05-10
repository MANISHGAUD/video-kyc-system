using Microsoft.EntityFrameworkCore;
using VideoKyc.API.Hubs;
using VideoKyc.Application.Interfaces;
using VideoKyc.Infrastructure.Data;
using VideoKyc.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddControllers();

builder.Services.AddSignalR();

builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IKycService, KycService>();
var app = builder.Build();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<CallHub>("/callHub");

app.Run();