using LinqTraining.Data;
using LinqTraining.Models;

List <Product> products = SampleData.GetProducts(); 
List <Book> books = SampleData.GetBooks();
List<Order> orders = SampleData.GetOrders();
List<Employee> employees = SampleData.GetEmployees();

var employeesDictionary = employees.ToDictionary(e => e.Id);

Console.WriteLine($"Employee with ID 3: {employeesDictionary[3].Name}");

 
