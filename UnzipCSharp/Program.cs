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

                var result = from currEntry in archive.Entries
                             where Path.GetDirectoryName(currEntry.FullName).Contains("example\\optionalSubfolder") // pasta a ser extraída dos zips
                             select currEntry;

                foreach (ZipArchiveEntry entry in result)
                {
                    var fullName = entry.FullName;

                    if (fullName.ToCharArray().Count(c => c == '/') == 0) // quando arquivo está na pasta raiz
                    {
                        entry.ExtractToFile(Path.Combine(extractFolder, entry.Name), true);
                    }
                    else
                    {
                        if (entry.Name.Count() > 0) // @todo corrigir adequadamente. Impede erro de entrada inexistente 
                        {
                            var newPath = fullName.Remove(fullName.LastIndexOf("/")); // nome da subpasta onde está o arquivo

                            Directory.CreateDirectory(Path.Combine(extractFolder, newPath)); // cria a(s) subpasta(s)

                            entry.ExtractToFile(Path.Combine(extractFolder + "\\" + newPath.Replace('/', '\\'), entry.Name), true);
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
