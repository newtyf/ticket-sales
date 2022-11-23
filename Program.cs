using Spectre.Console;
using Newtonsoft.Json;

// Title font slant
var fontTitle = FigletFont.Load(Path.Combine(Directory.GetCurrentDirectory(), "slant.flf"));
FigletText titleProyect = new FigletText(fontTitle, "Venta de Pasajes").Centered().Color(Color.Blue);
AnsiConsole.Write(titleProyect);
AnsiConsole.WriteLine();

// get data tickets
string pathData = Path.Combine(Directory.GetCurrentDirectory(), "data_tickets.json");
var jsonTickets = File.OpenText(pathData).ReadToEnd();
List<Ticket>? tickets = JsonConvert.DeserializeObject<List<Ticket>>(jsonTickets);

// get data users
string pathUsers = Path.Combine(Directory.GetCurrentDirectory(), "users.json");
var jsonUsers = File.OpenText(pathUsers).ReadToEnd();
List<User>? users = JsonConvert.DeserializeObject<List<User>>(jsonUsers);

// TODO REQ_1: program description
var descriptionTable = new Table().Centered();
descriptionTable.AddColumn("Description");
descriptionTable.AddRow(
    $"[bold deepskyblue1]El presente proyecto tiene como finalidad diseñar y construir \nun simulador de ventas de pasajes para buses en consola.[/]\n[bold yellow]Puede revisar el codigo del progrmama aqui ->[/] [link]https://github.com/newtyf/proyecto-final[/]");
AnsiConsole.Write(descriptionTable);

// TODO REQ_2: login user
// Usuario : Axel
// Contraseña: 123456
var fontLogin = FigletFont.Load(Path.Combine(Directory.GetCurrentDirectory(), "small.flf"));
FigletText titleLogin = new FigletText(fontLogin, "Ingresar").Centered().Color(Color.Blue);
AnsiConsole.Write(titleLogin);
AnsiConsole.WriteLine();
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

// TODO REQ_3: get data user for ticket
AnsiConsole.MarkupLine("[italic underline yellow]Datos para la compra de su boleto[/]\n");
string nameForTicket = AnsiConsole.Ask<string>("Ingresa tu Nombre", "ejemplo: juanito");
string lastNameForTicket = AnsiConsole.Ask<string>("Ingresa tus Apellidos", "ejemplo: gonzales");
int ageForTicket = AnsiConsole.Prompt(new TextPrompt<int>("Ingresa tu edad: ")
    .PromptStyle("green")
    .ValidationErrorMessage("[red]El valor ingresado no es una edad valida[/]")
    .Validate(age =>
    {
        return age switch
        {
            < 18 => ValidationResult.Error("[red]Debes ser mayor de edad para poder comprar un boleto![/]"),
            >= 18 => ValidationResult.Success(),
        };
    }));
int dniForTicket = AnsiConsole.Prompt(new TextPrompt<int>("Ingresa tu DNI: ")
    .PromptStyle("green")
    .ValidationErrorMessage("[red]El valor ingresado no es valido[/]")
    .Validate(dni => dni.ToString().Length == 8
        ? ValidationResult.Success()
        : ValidationResult.Error("[red]El DNI ingresado no es valido[/]")));
SeparatorRuler();

// TODO REQ_4,5: select filters
AnsiConsole.MarkupLine("[italic underline yellow]Filtros[/]\n");
var enumerableTickets = TicketFiltered();
SeparatorRuler();

// TODO REQ_6: paint tickets
AnsiConsole.MarkupLine("[italic underline yellow]Boletos Disponibles[/]\n");
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

// TODO REQ_7,8: Ticket Selection
int ticketSelect = 0;
while (true)
{
    ticketSelect = TicketSelect();
    if (Verification()) break;
}
SeparatorRuler();

// TODO REQ_9: Pay method
var payMethod = PayMethod();
AnsiConsole.MarkupLine($"Método de pago seleccionado: [green]{payMethod}[/]");

// TODO REQ_10:
Console.Clear();
var filterTicket =
    from ticket in tickets
    where ticket.Id.Equals(ticketSelect)
    select ticket;
var ticketInfo = filterTicket.ToList()[0];
string finalInfo = $"[yellow]id:[/] {ticketInfo.Id}\n" +
                   $"[deepskyblue3_1]Nombre:[/] {nameForTicket}\n" +
                   $"[deepskyblue3_1]Apellido:[/] {lastNameForTicket}\n" +
                   $"[deepskyblue3_1]Edad:[/] {ageForTicket}\n" +
                   $"[deepskyblue3_1]DNI:[/] {dniForTicket}\n" +
                   $"[deepskyblue3_1]Salida:[/] {ticketInfo.DateOfExitArrival}\n" +
                   $"[deepskyblue3_1]Hora salida:[/] {ticketInfo.HourExit}\n" +
                   $"[deepskyblue3_1]Hora llegada:[/] {ticketInfo.HourArrival}\n" +
                   $"[deepskyblue3_1]Lugar salida:[/] {ticketInfo.PlaceExit}\n" +
                   $"[deepskyblue3_1]Lugar llegada:[/] {ticketInfo.PlaceArrival}\n" +
                   $"[deepskyblue3_1]Empresa:[/] {ticketInfo.Company}\n" +
                   $"[deepskyblue3_1]Asiento:[/] #{ticketInfo.SeatNumber}\n" +
                   $"[green]Método de Pago:[/] {payMethod}\n" +
                   $"[green]Precio:[/] {ticketInfo.Price}";

var ticketPanel = new Panel(finalInfo);
ticketPanel.Header = new PanelHeader("Boleto").SetAlignment(Justify.Center);
ticketPanel.Padding = new Padding(1, 1, 1, 1);
ticketPanel.Border = BoxBorder.Double;
AnsiConsole.Write(ticketPanel);
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

bool Verification()
{
    if (!AnsiConsole.Confirm("¿Está seguro de la selección de su boleto?"))
    {
        AnsiConsole.MarkupLine("[red]vuelva a ingresar el ID de su boleto[/]");
        return false;
    }

    return true;
}

void SeparatorRuler(string color = "white")
{
    var separator = new Rule();
    separator.RuleStyle($"{color} dim");
    AnsiConsole.WriteLine();
    AnsiConsole.Write(separator);
    AnsiConsole.WriteLine();
}

string ChoicesPlaces(string title)
{
    string[] places =
    {
        "Amazonas", "Ancash", "Apurimac", "Arequipa", "Ayacucho", "Cajamarca", "Callao", "Cusco", "Huancavelica",
        "Huánuco", "Ica", "Junín", "La Libertad", "Lambayeque", "Lima", "Loreto", "Madre de Dios", "Moquegua",
        "Pasco"
    };

    return AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title(title)
            .PageSize(10)
            .MoreChoicesText("[grey](suba o baje para ver mas lugares)[/]")
            .AddChoices(places)
    );
}

string ChoicesCompany(string title)
{
    string[] companies =
    {
        "Cualquiera", "Oltursa", "Tepsa", "Cruz del Sur", "Civa", "MovilBus", "Wari", "Soyuz", "Shalom"
    };

    return AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title(title)
            .PageSize(10)
            .MoreChoicesText("[grey](suba o baje para ver mas lugares)[/]")
            .AddChoices(companies)
    );
}

string PayMethod()
{
    string[] payTypes =
    {
        "Tarjeta de Crédito/Débito", "Yape", "Plin", "Paypal", "Pago Efectivo"
    };
    return AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Escoga su método de pago:")
            .PageSize(10)
            .AddChoices(payTypes)
    );
}

List<Ticket> TicketFiltered()
{
    while (true)
    {
        var placeArrivalFilter = ChoicesPlaces("[blue]Seleccione el destino al que desea viajar:[/]");
        AnsiConsole.MarkupLine($"Seleccione el destino al que desea viajar: [green]{placeArrivalFilter}[/]");
        var placeExitFilter = ChoicesPlaces("[blue]Seleccione el lugar de salida para tomar su bus:[/]");
        AnsiConsole.MarkupLine($"Seleccione el lugar de salida para tomar su bus: [green]{placeExitFilter}[/]");
        var companiesFilter = ChoicesCompany("[blue]Seleccione la empresa con la que quiere viajar:[/]");
        AnsiConsole.MarkupLine($"Seleccione la empresa con la que quiere viajar: [green]{companiesFilter}[/]");
        // applied filters 
        var filterQuery =
            from ticket in tickets
            where ticket.PlaceExit.Equals(placeExitFilter) && ticket.PlaceArrival.Equals(placeArrivalFilter)
            select ticket;
        var enumerable = filterQuery.ToList();
        if (enumerable.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No se econtraron tickets disponibles[/]");
            AnsiConsole.MarkupLine("[red]Vuelve a seleccionar los filtros[/]");
            AnsiConsole.WriteLine();
        }
        else
        {
            return enumerable;
        }
    }
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