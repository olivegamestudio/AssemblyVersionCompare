using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AssemblyVersionCompare
{
    class Program
    {
        static bool GetAssemblyVersion(string filename, out Version version)
        {
            try
            {
                var domain = AppDomain.CreateDomain(nameof(AssemblyLoader), AppDomain.CurrentDomain.Evidence, new AppDomainSetup { ApplicationBase = Path.GetDirectoryName(typeof(AssemblyLoader).Assembly.Location) });
                try
                {
                    var loader = (AssemblyLoader)domain.CreateInstanceAndUnwrap(typeof(AssemblyLoader).Assembly.FullName, typeof(AssemblyLoader).FullName);
                    version = loader.Load(filename);
                }
                finally
                {
                    AppDomain.Unload(domain);
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            version = new Version();
            return false;
        }

        static string GetRelativeName(string filename, string path)
        {
        //    return Path.GetRelativePath(path, filename); is missing from NET462
            return filename.Replace(path, string.Empty).TrimStart('\\');
        }

        static IEnumerable<AssemblyInfo> GetAssemblies(string path)
        {
            foreach (string assembly in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
            {
                if (!GetAssemblyVersion(assembly, out Version version))
                {
                    continue;
                }

                AssemblyInfo info = new AssemblyInfo
                {
                    Name = assembly,
                    Version = version,
                    RelativeName = GetRelativeName(assembly, path)
                };

                yield return info;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Assembly Version Comparer");

            string sourcePath = args[0];
            string comparePath = args[1];

            IEnumerable<AssemblyInfo> sourceFiles = GetAssemblies(sourcePath).ToList();
            IEnumerable<AssemblyInfo> compareFiles = GetAssemblies(comparePath).ToList();

            foreach (AssemblyInfo item in sourceFiles)
            {
                if (!compareFiles.Any(it=>it.RelativeName == item.RelativeName))
                {
                    Console.WriteLine("Assembly in source folder but missing from compare: " + item.RelativeName);
                    continue;
                }

                AssemblyInfo compare = compareFiles.First(it => it.RelativeName == item.RelativeName);
                if (item.Version != compare.Version)
                {
                    Console.WriteLine(item.RelativeName + " version " + item.Version + " is different to compare version of " + compare.Version);
                }
            }

            foreach (AssemblyInfo item in compareFiles)
            {
                if (!sourceFiles.Any(it=>it.RelativeName == item.RelativeName))
                {
                    Console.WriteLine("Assembly in compare folder but missing from source: " + item.RelativeName);
                }
            }
        }
    }
}
