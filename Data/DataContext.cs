using MySqlConnector;

namespace mvc;

public class DataContext : IDataContext 
{
    private readonly MySqlConnection _sqlConnection;

    public DataContext(MySqlConnection mySqlConnection) 
    {
        _sqlConnection = mySqlConnection;
    }

    public async Task<List<Producto>> ObtenProductosAsync() 
    {
        // Abre la conexión hacia MySQL
        await _sqlConnection.OpenAsync();

        List<Producto> productos = [];

        using var command = new MySqlCommand(@"SELECT p.id, p.nombre, p.precio, f.nombre AS fabricante_nombre
                    FROM fabricante f INNER JOIN producto p ON f.id = p.id_fabricante", _sqlConnection);

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync()) 
        {
            Producto item = new()
            {
                ProductoId = Convert.ToInt32(reader["id"]),
                Nombre = reader["nombre"].ToString(),
                Precio = Convert.ToDecimal(reader["precio"]),
                Fabricante = reader["fabricante_nombre"].ToString()
            };

            productos.Add(item);
        }

        // Envía los productos
        return productos;
    }
}