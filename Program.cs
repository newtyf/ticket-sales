using Spectre.Console;
using Newtonsoft.Json;

// title font slant
var font = FigletFont.Load(Path.Combine(Directory.GetCurrentDirectory(), "slant.flf"));
FigletText titleProyect = new FigletText(font, "Venta de Pasajes").LeftAligned().Color(Color.Blue);
AnsiConsole.Write(titleProyect);

Console.ReadLine();
