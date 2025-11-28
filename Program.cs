using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("==== OCR PDF Integrator (.NET) ====");

        if (args.Length < 2)
        {
            Console.WriteLine("Uso: OCRPdfIntegrator <input.pdf> <output.pdf> [idioma]");
            return;
        }

        string input = args[0];
        string output = args[1];
        string language = args.Length >= 3 ? args[2] : "por";

        RunOCRmyPDF(input, output, language);
    }

    static void RunOCRmyPDF(string input, string output, string language)
    {
        ProcessStartInfo psi = new()
        {
            FileName = "ocrmypdf",
            Arguments = $"--language {language} --force-ocr \"{input}\" \"{output}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = false
        };

        try
        {
            using var process = Process.Start(psi)!;

            process.OutputDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    Console.WriteLine("[INFO] " + e.Data);
            };

            process.ErrorDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    Console.WriteLine("[ERRO] " + e.Data);
            };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            Console.WriteLine($"\nProcesso finalizado com código: {process.ExitCode}");

            if (process.ExitCode == 0)
                Console.WriteLine("PDF OCR criado com sucesso!");
            else
                Console.WriteLine("Ocorreu um erro durante o OCR.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Falha ao executar OCRmyPDF: " + ex.Message);
        }
    }
}