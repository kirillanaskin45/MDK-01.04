using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

var filter = args.Length > 0 ? args[0] : "";
var outputFile = args.Length > 1 ? args[1] : "";

var processes = Process.GetProcesses()
    .Where(p => string.IsNullOrEmpty(filter) ||
           p.ProcessName.Contains(filter, StringComparison.OrdinalIgnoreCase))
    .OrderBy(p => p.ProcessName);

using (var writer = outputFile != "" ? new StreamWriter(outputFile) : null)
{
    Console.WriteLine($"{"Имя",-30} {"PID",-8} {"Память MB",-12}");
    Console.WriteLine(new string('-', 50));
    writer?.WriteLine($"{"Имя",-30} {"PID",-8} {"Память MB",-12}");
    writer?.WriteLine(new string('-', 50));

    foreach (var p in processes)
    {
        try
        {
            var mem = p.WorkingSet64 / 1024.0 / 1024.0;
            var line = $"{p.ProcessName,-30} {p.Id,-8} {mem,12:F2}";
            Console.WriteLine(line);
            writer?.WriteLine(line);
        }
        catch { }
    }
}

if (outputFile != "")
    Console.WriteLine($"\nСохранено в {outputFile}");
