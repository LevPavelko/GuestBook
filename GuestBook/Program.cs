using Microsoft.EntityFrameworkCore;
using GuestBook.Models;
using GuestBook.Repository;

var builder = WebApplication.CreateBuilder(args);

// Все сессии работают поверх объекта IDistributedCache, и 
// ASP.NET Core предоставляет встроенную реализацию IDistributedCache
builder.Services.AddDistributedMemoryCache();// добавляем IDistributedMemoryCache
builder.Services.AddSession(); 

// Получаем строку подключения из файла конфигурации
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<GuestBookContext>(options => options.UseSqlServer(connection));

// Добавляем сервисы MVC
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepository, GuestBookRepository>();

var app = builder.Build();
app.UseSession();   // Добавляем middleware-компонент для работы с сессиями
app.UseStaticFiles(); // обрабатывает запросы к файлам в папке wwwroot

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();