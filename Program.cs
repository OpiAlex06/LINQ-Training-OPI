using LinqTraining.Data;
using LinqTraining.Models;

List<Product> products = SampleData.GetProducts();
List<Book> books = SampleData.GetBooks();
List<Order> orders = SampleData.GetOrders();
List<Employee> employees = SampleData.GetEmployees();

var employeesDictionary = employees.ToDictionary(e => e.Id);
Console.WriteLine($"Employee with ID 3: {employeesDictionary[3].Name}");

// ══════════════════════════════════════════════════════
//  TAREA: REPORTES CON LINQ
// ══════════════════════════════════════════════════════

// 1. Pedidos del último mes agrupados por estado
Console.WriteLine("\n1. Pedidos del último mes agrupados por estado");

var pedidosPorEstado = orders
    .Where(o => o.Date >= DateTime.Today.AddDays(-30))
    .GroupBy(o => o.Status)
    .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
    .OrderBy(g => g.Estado);

foreach (var g in pedidosPorEstado)
    Console.WriteLine($"  {g.Estado}: {g.Cantidad} pedido(s)");

// 2. Top 5 libros más vendidos por género
Console.WriteLine("\n2. Top 5 libros más vendidos por género");

var top5 = books
    .GroupBy(b => b.Genre)
    .Select(g => g.OrderByDescending(b => b.UnitsSold).First())
    .OrderByDescending(b => b.UnitsSold)
    .Take(5);

foreach (var b in top5)
    Console.WriteLine($"  [{b.Genre}] {b.Title} - {b.UnitsSold} unidades");

// 3. Búsqueda de libro por nombre parcial
Console.WriteLine("\n3. Búsqueda de libro por nombre parcial");

string busqueda = "cle";
var resultados = books
    .Where(b => b.Title.ToLower().Contains(busqueda.ToLower()))
    .OrderBy(b => b.Title);

Console.WriteLine($"  Término: \"{busqueda}\"");
foreach (var b in resultados)
    Console.WriteLine($"  {b.Title} ({b.Genre})");

// 4. Top 3 clientes que más compraron
Console.WriteLine("\n4. Top 3 clientes que más compraron");

var top3 = orders
    .GroupBy(o => o.CustomerName)
    .Select(g => new { Cliente = g.Key, Total = g.Sum(o => o.Amount) })
    .OrderByDescending(x => x.Total)
    .Take(3);

foreach (var c in top3)
    Console.WriteLine($"  {c.Cliente}: ${c.Total:N2}");

// 5. Clientes con ningún pedido completado
Console.WriteLine("\n5. Clientes sin ningún pedido completado");

var sinCompletado = orders
    .GroupBy(o => o.CustomerName)
    .Select(g => new { Cliente = g.Key, Completados = g.Where(o => o.Status == "Completed").Count() })
    .Where(x => x.Completados == 0)
    .OrderBy(x => x.Cliente);

foreach (var c in sinCompletado)
    Console.WriteLine($"  {c.Cliente}");