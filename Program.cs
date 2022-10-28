﻿using Spectre.Console;
using Newtonsoft.Json;

// Title font slant
var fontTitle = FigletFont.Load(Path.Combine(Directory.GetCurrentDirectory(), "slant.flf"));
FigletText titleProyect = new FigletText(fontTitle, "Venta de Pasajes").Centered().Color(Color.Blue);
AnsiConsole.Write(titleProyect);
AnsiConsole.WriteLine();

// TODO REQ_1: program description
var descriptionTable = new Table().Centered();
descriptionTable.AddColumn("Description");
descriptionTable.AddRow(
    $"[bold deepskyblue1]El presente proyecto tiene como finalidad diseñar y construir \nun simulador de ventas de pasajes para buses en consola.[/]\n[bold yellow]Puede revisar el codigo del progrmama aqui ->[/] [link]https://github.com/newtyf/proyecto-final[/]");
AnsiConsole.Write(descriptionTable);

// TODO REQ_2: login user
var fontLogin = FigletFont.Load(Path.Combine(Directory.GetCurrentDirectory(), "small.flf"));
FigletText titleLogin = new FigletText(fontLogin, "Ingresar").Centered().Color(Color.Blue);
AnsiConsole.Write(titleLogin);
AnsiConsole.WriteLine();
string pathUsers = Path.Combine(Directory.GetCurrentDirectory(), "users.json");
var jsonUsers = File.OpenText(pathUsers).ReadToEnd();
List<User>? users = JsonConvert.DeserializeObject<List<User>>(jsonUsers);
while (true)
{
    // input fields
    var inputUsername = AnsiConsole.Ask<string>("[yellow]Ingrese su usuario:[/]");
    var inputPassword = AnsiConsole.Prompt(
        new TextPrompt<string>("[yellow]Ingrese su contraseña password[/]?")
            .PromptStyle("red")
            .Secret());
    // validate users
    var validateQuery =
        from user in users
        where user.Username == inputUsername && user.Password == inputPassword
        select user;
    if (validateQuery.ToList().Count != 0)
    {
        Console.Clear();
        break;
    }

    var rule = new Rule("[red]Usuario y/o contraseña no validos[/]");
    rule.RuleStyle("red dim");
    AnsiConsole.Write(rule);
}

// welcome
var fontWelcome = FigletFont.Load(Path.Combine(Directory.GetCurrentDirectory(), "small.flf"));
FigletText titleWelcome = new FigletText(fontWelcome, "Bienvenido").Centered().Color(Color.Blue);
AnsiConsole.Write(titleWelcome);
AnsiConsole.WriteLine();

// TODO REQ_3: select filters



// get data tickets
string pathData = Path.Combine(Directory.GetCurrentDirectory(), "data_tickets.json");
var jsonTickets = File.OpenText(pathData).ReadToEnd();
List<Ticket>? tickets = JsonConvert.DeserializeObject<List<Ticket>>(jsonTickets);

// applied filters 
var filterQuery =
    from ticket in tickets
    where ticket.PlaceExit.Equals("Ucayali") && ticket.PlaceArrival.Equals("Lima")
    select ticket;
var enumerableTickets = filterQuery.ToList();

// paint tickets
int offset = 2;
if (enumerableTickets.Count != 0)
{
    for (int i = 0; i < enumerableTickets.Count; i++)
    {
        var ticket = enumerableTickets[i];
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

// structs
public struct Ticket
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

public struct User
{
    public string Username;
    public string Password;
}