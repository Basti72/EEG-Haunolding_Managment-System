using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace EGG_Haunolding_Management_System.Class
{
    public class BMSBackroundService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine(GetBMSValue());
                await Task.Delay(2000);
            }
        }
        private int GetBMSValue()
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\Resources\\DynamicCode.txt";
            string code = File.ReadAllText(path);

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            string assemblyName = Path.GetRandomFileName();
            MetadataReference[] references = new MetadataReference[]
            {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    // Fehlerbehandlung hier
                    foreach (Diagnostic diagnostic in result.Diagnostics)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());

                    Type type = assembly.GetType("DynamicCode");
                    object obj = Activator.CreateInstance(type);

                    MethodInfo methodInfo = type.GetMethod("GetValue");
                    if (methodInfo != null)
                    {
                        int resultValue = (int)methodInfo.Invoke(obj, null);
                        //Console.WriteLine($"Ergebnis: {resultValue}");
                        return resultValue;
                    }
                }
            }
            return 0;
        }
    }
}