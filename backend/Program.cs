using Kanban.Repositories;
using Kanban.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITableroRepository, TableroRepository>();
builder.Services.AddScoped<ITareaRepository, TareaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

string connectionString = builder.Configuration.GetConnectionString("SqliteConnection")!.ToString();

builder.Services.AddSingleton<string>(connectionString);

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
    {
      options.IdleTimeout = TimeSpan.FromMinutes(30);
      options.Cookie.HttpOnly = true;
      options.Cookie.IsEssential = true;
    });

builder.Services.AddControllers();

string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
  options.AddPolicy(name: myAllowSpecificOrigins,
      policy =>
      {
        policy.WithOrigins("http://localhost:5173")
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials();
      });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();
