using Backend;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string corsKey = "corsPolicy";

builder.Services.AddSignalR();
builder.Services.AddSingleton<ClientRepository>();
builder.Services.AddSingleton<ChatHub>();
builder.Services.AddCors(options => options.AddPolicy(
    corsKey,
    x => x.
        SetIsOriginAllowed(_ => true)
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsKey);
//app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapHub<ChatHub>("/chat");
app.MapControllers();

app.Run();