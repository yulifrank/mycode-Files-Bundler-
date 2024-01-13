
using System.CommandLine;
using System.Formats.Asn1;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;

using System.Linq;
static string[] GetExtensionOfLanguages(string language, string[] extensions, string[] allLanguages)
{
    if (language.Equals("all"))
        return extensions;

    string[] selectedExtensions = language
        .Split(' ')
        .Join(allLanguages.Zip(extensions, (lang, ext) => new { Language = lang, Extension = ext }),
              lang => lang,
              langExt => langExt.Language,
              (lang, langExt) => langExt.Extension)
        .ToArray();

    return selectedExtensions.Length > 0 ? selectedExtensions : extensions;
}

var bundleCommand = new Command("bundle", "Bundle code files to a single file");
var rspCommand = new Command("create-rsp", "Response file");

var outputOption = new Option<FileInfo>(new[] { "--output", "-o" }, "File pass or name");
var languageOption = new Option<string>(new[] { "--language", "-l" }, "Programming languages to include in the bundle") { IsRequired = true };
var noteOption = new Option<bool>(new[] { "--note", "-n" }, " Include source code origin as a comment");
var sortOption = new Option<string>(new[] { "--sort", "-s" }, " Sort order for code file by letters or type");
var removeEmptyLinesOption = new Option<bool>(new[] { "--remove-empty-lines", "-r" }, " Remove empty lines from code files");
var authorOption = new Option<string>(new[] { "--author", "-a" }, "Registering the name of the author of the file ");
noteOption.SetDefaultValue(false);
sortOption.SetDefaultValue("letter");
removeEmptyLinesOption.SetDefaultValue(false);
bundleCommand.AddOption(outputOption);
bundleCommand.AddOption(languageOption);
bundleCommand.AddOption(noteOption);
bundleCommand.AddOption(sortOption);
bundleCommand.AddOption(removeEmptyLinesOption);
bundleCommand.AddOption(authorOption);
string currentPath = Directory.GetCurrentDirectory();
List<string> allFolders = Directory.GetFiles(currentPath, "", SearchOption.AllDirectories).Where(file => !file.Contains("bin") && !file.Contains("Debug")).ToList();
string[] languagesArray = { "c#", "c", "c++", "java", "html", "css", "javascript", "pyton", "ruby", "swift", "php" };
string[] extensionsArray = { ".cs", ".c", ".cpp", ".java", ".html", ".css", ".js", ".py", ".rb", ".swift", ".php" };

bundleCommand.SetHandler((output, language, note, sort, removeEmptyLines, author) =>
{
    try
    {
        string[] languages = GetExtensionOfLanguages(language, extensionsArray, languagesArray);
        string[] files = allFolders.Where(file => languages.Contains(Path.GetExtension(file))).ToArray();
        if (files.Any())
        {
            //output = output.Replace(" ","-");
            using (StreamWriter writer = new StreamWriter(output.FullName))
            {
                if (!string.IsNullOrEmpty(author))
                {
                    author = author.Replace(" ", "-");
                    writer.WriteLine($"# Author: {author}");
                    writer.WriteLine();
                }

                if (note)
                {
                    writer.WriteLine($"# Source code origin: {Directory.GetCurrentDirectory()}");
                }

                if (sort.ToLower() == "type")
                {
                    Array.Sort(files, (a, b) => Path.GetExtension(a).CompareTo(Path.GetExtension(b)));
                }
                else
                {
                    Array.Sort(files);
                }

                foreach (var file in files)
                {
                    string content = File.ReadAllText(file);

                    if (removeEmptyLines)
                    {
                        content = string.Join(Environment.NewLine, content.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)));
                    }

                    writer.WriteLine($"--------------------------------------------------- {Path.GetFileName(file)}");
                    writer.WriteLine(content);
                    writer.WriteLine();
                }

                Console.WriteLine($"File {output.FullName} was created successfully");
            }
        }
        else
        {
            Console.WriteLine("Error: No files found for the specified languages.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}, outputOption, languageOption, noteOption, sortOption, removeEmptyLinesOption, authorOption);

rspCommand.SetHandler(() =>
{
    var responseFile = new FileInfo("responseFile.rsp");
    // Create a new response file
    using (StreamWriter rspWriter = new StreamWriter(responseFile.FullName))
    {
        // Output
        Console.Write("Enter the output file path: ");
        var pathInput = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(pathInput))
        {
            Console.Write("Enter the output file path: ");
            pathInput = Console.ReadLine();
        }
        //pathInput = pathInput.Replace(" ", "-");
        rspWriter.WriteLine($"--output {pathInput}");
        // Language
        Console.Write("Enter the programming languages to include :c,c++ ,java ,c#, html,css,java,script ,pyton or all: ");
        var languageInput = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(languageInput))
        {
            Console.Write("Please enter at least one programming language: ");
            languageInput = Console.ReadLine();
        }
        rspWriter.WriteLine($"--language {languageInput}");
        // Note
        Console.WriteLine("Include source code origin as a comment? (y/n)");
        var noteInput = Console.ReadLine();
        rspWriter.WriteLine(noteInput.Trim().ToLower() == "y" ? "--note" : "");
        // Sort
        Console.Write("Enter the sort order for code files ('letter' or 'type'): ");
        rspWriter.WriteLine($"--sort {Console.ReadLine()}");
        // Remove Empty Lines
        Console.WriteLine("Remove empty lines from code files? (y/n)");
        var removeEmptyLinesInput = Console.ReadLine();
        rspWriter.WriteLine(removeEmptyLinesInput.Trim().ToLower() == "y" ? "--remove-empty-lines" : "");
        // Author
        Console.Write("Enter the name of the author: ");
        rspWriter.WriteLine($"--author {Console.ReadLine().Replace(" ", "-")}");
        //rspWriter.WriteLine($"dotnet publish @{responseFile.FullName}");

    }
    Console.WriteLine($"Response file created successfully: {responseFile.FullName}");
});

var RootCommand = new RootCommand("Root command for file bundler for CLI");
RootCommand.AddCommand(bundleCommand);
RootCommand.AddCommand(rspCommand);
RootCommand.InvokeAsync(args);


