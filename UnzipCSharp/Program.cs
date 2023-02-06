using System.IO.Compression;

namespace UnzipCSharp
{
    public class Program
    {
        public static void Main()
        {
            string extractFolder = @"c:\example\extract";

            foreach (string zip in Directory.GetFiles(@"c:\example\zips"))
            {
                using ZipArchive archive = ZipFile.OpenRead(zip);

                // pasta ou subpasta a ser extraída dos zips
                // informe vazio "" se deseja extrair todos os arquivos
                var result = from currEntry in archive.Entries
                             where Path.GetDirectoryName(currEntry.FullName).Contains("exampleFolder\\subfolder")
                             select currEntry;

                foreach (ZipArchiveEntry entry in result)
                {
                    var fullName = entry.FullName;

                    // quando arquivo está na pasta raiz
                    if (fullName.ToCharArray().Count(c => c == '/') == 0)
                    {
                        entry.ExtractToFile(Path.Combine(extractFolder, entry.Name), true);
                    }
                    else
                    {
                        if (entry.Length > 0) 
                        {
                            // cria a(s) subpasta(s)
                            Directory.CreateDirectory(Path.Combine(extractFolder, fullName.Remove(fullName.LastIndexOf("/"))));

                            entry.ExtractToFile(Path.Combine(extractFolder, fullName), true);
                        }
                    }
                }
            }
        }
    }
}

///
/// Anotações
///

/// Compactar arquivos
/// string sourceFolder = @"c:\example\ziped";
/// ZipFile.CreateFromDirectory(sourceFolder, targetZipFile);

/// Extrair arquivos sobrescrevendo
/// ZipFile.ExtractToDirectory(targetZipFile, extractFolder);
/// ZipFile.ExtractToDirectory(targetZipFile2, extractFolder, Encoding.UTF8, true);
