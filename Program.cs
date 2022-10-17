using Spectre.Console;
using Newtonsoft.Json;

// title font
var font = FigletFont.Load(Path.Combine(Directory.GetParent("../../../").ToString(), "slant.flf"));
FigletText titleProyect = new FigletText(font, "Venta de Pasajes").LeftAligned().Color(Color.Blue);
AnsiConsole.Write(titleProyect);

Console.ReadLine();
