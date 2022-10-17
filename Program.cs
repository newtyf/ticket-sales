using Spectre.Console;
using Newtonsoft.Json;
using System.IO;

var font = FigletFont.Load(Path.Combine(Directory.GetParent("../../../").ToString(), "slant.flf"));
FigletText titleProyect = new FigletText(font, "Venta de Pasajes").LeftAligned().Color(Color.Blue);
AnsiConsole.Write(titleProyect);


// get data boletos
string pathData =
    @"C:\Users\AXEL\Desktop\UNIVERSITY\UPN\C#\Fundamentos de Algoritmos\T4\SimulacionVentaPasajes\data_tickets.json";
StreamReader jsonStream = File.OpenText(pathData);
var json = jsonStream.ReadToEnd();
List<Boleto>? tickets = JsonConvert.DeserializeObject<List<Boleto>>(json);

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