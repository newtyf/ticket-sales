using Spectre.Console;
using Newtonsoft.Json;

// title font
var font = FigletFont.Load(Path.Combine(Directory.GetCurrentDirectory(), "slant.flf"));
FigletText titleProyect = new FigletText(font, "Venta de Pasajes").LeftAligned().Color(Color.Blue);
AnsiConsole.Write(titleProyect);


// get data boletos
string pathData = Path.Combine(Directory.GetCurrentDirectory(), "data_tickets.json");
var jsonTickets = File.OpenText(pathData).ReadToEnd();
List<Boleto>? tickets = JsonConvert.DeserializeObject<List<Boleto>>(jsonTickets);

// filtros
var query =
    from ticket in tickets
    where (ticket.PlaceExit.Equals("Ucayali") && ticket.PlaceArrival.Equals("Lima"))
    select ticket;
var enumerableTickets = query.ToList();

// paint tickets
int offset = 2;
for (int i = 0; i < enumerableTickets.Count; i++)
{
    Boleto ticket = enumerableTickets[i];
    if (i < offset)
    {
        string dataOfTicket = $"[yellow]id:[/] {ticket.Id}\n" +
                              $"[deepskyblue3_1]Salida:[/] {ticket.DateOfExitArrival}\n" +
                              $"[deepskyblue3_1]Hora salida:[/] {ticket.HourExit}\n" +
                              $"[deepskyblue3_1]Hora llegada:[/] {ticket.HourArrival}\n" +
                              $"[deepskyblue3_1]Lugar salida:[/] {ticket.PlaceExit}\n" +
                              $"[deepskyblue3_1]Lugar llegada:[/] {ticket.PlaceArrival}\n" +
                              $"[green]precio:[/] {ticket.Price}\n" +
                              $"[deepskyblue3_1]empresa:[/] {ticket.Company}\n" +
                              $"[deepskyblue3_1]asiento:[/] #{ticket.SeatNumber}";
    
        var panel = new Panel(dataOfTicket);
        panel.Header = new PanelHeader("Boleto").SetAlignment(Justify.Center);
        panel.Padding = new Padding(1, 1, 1, 1);
        panel.Border = BoxBorder.Double;
        AnsiConsole.Write(panel);
    }

    if (i == offset - 1)
    {
        var rule = new Rule($"Se esta mostrando [yellow]{offset}/{enumerableTickets.Count}[/]");
        rule.LeftAligned();
        AnsiConsole.Write(rule);
        if (ShowMore())
        {
            offset += 2;
        }
        else
        {
            break;
        }
    }
    
}

//select ticket
int ticketSelect = TicketSelect();
Console.WriteLine(ticketSelect);
Console.ReadLine();




// utilities
bool ShowMore() 
{
    if (!AnsiConsole.Confirm("Mostras mas resultados?"))
    {
        AnsiConsole.MarkupLine("Ok...");
        return false;
    }

    return true;
}
int TicketSelect()
{
    return AnsiConsole.Prompt(
        new TextPrompt<int>("Ingrese el [yellow]ID[/] del boleto a comprar?")
            .PromptStyle("yellow")
            .ValidationErrorMessage("[red]El ID debe ser un numero![/]")
            .Validate(id =>
            {
                
                foreach (var ticket in enumerableTickets)
                {
                    if (ticket.Id == id)
                    {
                        return ValidationResult.Success();
                    }
                }
                return ValidationResult.Error("[red]El ID ingresado no es valido![/]");
            }));
}
// class for tickets
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