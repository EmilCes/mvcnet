using mvc;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DataContext");
Console.WriteLine("Retrieved Connection String: " + connectionString);


// Agrega el soporte para MySQL
builder.Services.AddMySqlDataSource(builder.Configuration.GetConnectionString("DataContext")!);

// Agrega la funcionalidad de MVC
builder.Services.AddControllersWithViews();

// Soporte para consultar datos
builder.Services.AddScoped<IDataContext, DataContext>();

// Construye la aplciación web
var app = builder.Build();

if (!app.Environment.IsDevelopment()) 
{
    // En caso de error en producción, oculta los errores y manda a una página personalizada
    app.UseExceptionHandler("/Home/Error");

    // Establece que la aplciación debe ejecutarse en HTTPS
    app.UseHsts();
}

// Utiliza rutas para los endpoints de los controladores
app.UseRouting();

// Establece el patrón de rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();


