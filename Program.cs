using Spectre.Console;
using Newtonsoft.Json;
using System.IO;

FigletText titleProyect = new FigletText("Simulación Venta de Pasajes").LeftAligned().Color(Color.Blue);
AnsiConsole.Write(titleProyect);


// get data boletos
string pathData = @"C:\Users\AXEL\Desktop\UNIVERSITY\UPN\C#\Fundamentos de Algoritmos\T4\SimulacionVentaPasajes\data_tickets.json";
List<Boleto>? tickets;
StreamReader jsonStream = File.OpenText(pathData);
var json = jsonStream.ReadToEnd();
tickets = JsonConvert.DeserializeObject<List<Boleto>>(json);

Console.WriteLine(tickets?[1].DateOfExitArrival);

Console.ReadLine();

public class Boleto
{
    public int Id;
    public string? DateOfExitArrival;
    public string? HourExit;
    public string? HourArrival;
    public string? PlaceExit;
    public string? PlaceArrival;
    public string? Price;
    public string? Company;
    public int SeatNumber;
}