using LinqTraining.Data;
using LinqTraining.Models;

List<Product> products = SampleData.GetProducts();
List<Book> books = SampleData.GetBooks();
List<Order> orders = SampleData.GetOrders();
List<Employee> employees = SampleData.GetEmployees();

var employeesDictionary = employees.ToDictionary(e => e.Id);
Console.WriteLine($"Employee with ID 3: {employeesDictionary[3].Name}");



//  TAREA: REPORTES CON LINQ



//  1. Pedidos del último mes agrupados por estado 
Console.WriteLine("\n 1. Pedidos del último mes agrupados por estado");

DateTime desde = DateTime.Today.AddDays(-30);

var pedidosPorEstado = orders
    .Where(o => o.Date >= desde)
    .GroupBy(o => o.Status)
    .Select(g => new
    {
        Estado    = g.Key,
        Cantidad  = g.Count(),
        TotalMonto = g.Sum(o => o.Amount)
    })
    .OrderBy(g => g.Estado);
    
foreach (var grupo in pedidosPorEstado)
    Console.WriteLine($"  {grupo.Estado,-12} → {grupo.Cantidad} pedido(s)  |  Total: ${grupo.TotalMonto:N2}");



//  2. Top 5 libros más vendidos por género
Console.WriteLine("\n 2. Top 5 libros más vendidos por género");

var top5LibrosPorGenero = books
    .GroupBy(b => b.Genre)
    .Select(g => new
    {
        Genero   = g.Key,
        Libro    = g.OrderByDescending(b => b.UnitsSold).First()
    })
    .OrderByDescending(x => x.Libro.UnitsSold)
    .Take(5);

foreach (var item in top5LibrosPorGenero)
    Console.WriteLine($"  [{item.Genero,-12}]  {item.Libro.Title,-42}  {item.Libro.UnitsSold,4} unidades vendidas");



//  3. Búsqueda de libro por nombre parcial
Console.WriteLine("\n 3. Búsqueda de libro por nombre parcial");

string terminoBusqueda = "pro"; // cambia este valor para probar otras búsquedas
var librosencontrados = books
    .Where(b => b.Title.Contains(terminoBusqueda, StringComparison.OrdinalIgnoreCase))
    .OrderBy(b => b.Title);

Console.WriteLine($"  Término: \"{terminoBusqueda}\"");
if (!librosencontrados.Any())
{
    Console.WriteLine("  No se encontraron libros.");
}
else
{
    foreach (var libro in librosencontrados)
        Console.WriteLine($"  Id {libro.Id,2} | {libro.Title,-42} | {libro.Genre} ({libro.Year})");
}



//  4. Top 3 clientes que más compraron
Console.WriteLine("\n 4. Top 3 clientes que más compraron");

var top3Clientes = orders
    .GroupBy(o => o.CustomerName)
    .Select(g => new
    {
        Cliente       = g.Key,
        TotalComprado = g.Sum(o => o.Amount),
        NumeroPedidos = g.Count()
    })
    .OrderByDescending(x => x.TotalComprado)
    .Take(3);

int posicion = 1;
foreach (var c in top3Clientes)
    Console.WriteLine($"  #{posicion++}  {c.Cliente,-25}  ${c.TotalComprado:N2}  ({c.NumeroPedidos} pedido(s))");



//  5. Clientes con ningún pedido completado
Console.WriteLine("\n 5. Clientes sin ningún pedido completado");

var clientesConPedidoCompletado = orders
    .Where(o => o.Status == "Completed")
    .Select(o => o.CustomerName)
    .ToHashSet(StringComparer.OrdinalIgnoreCase);

var clientesSinCompletado = orders
    .Select(o => o.CustomerName)
    .Distinct()
    .Where(nombre => !clientesConPedidoCompletado.Contains(nombre))
    .OrderBy(nombre => nombre);

foreach (var cliente in clientesSinCompletado)
    Console.WriteLine($"  {cliente}");