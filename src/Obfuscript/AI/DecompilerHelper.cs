using System;
using System.IO;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.Decompiler.TypeSystem;

namespace obfuscript.AI
{
    public class DecompilerHelper
    {
        public static void DecompileAssembly(string assemblyPath, string outputDir)
        {
            // Ensure the output directory exists.
            Directory.CreateDirectory(outputDir);

            // Create decompiler settings.
            var settings = new DecompilerSettings();

            // Load the assembly.
            var module = new PEFile(assemblyPath);

            // Initialize the resolver with the target framework ID.
            var resolver = new UniversalAssemblyResolver(assemblyPath, false, module.DetectTargetFrameworkId());

            // Add the directory of the target assembly to the search directories.
            resolver.AddSearchDirectory(Path.GetDirectoryName(assemblyPath));

            // Optionally, add additional directories where referenced assemblies might be located.
            // resolver.AddSearchDirectory(@"path\to\additional\assemblies");

            // Create the decompiler with the resolver and settings.
            var decompiler = new CSharpDecompiler(assemblyPath, resolver, settings);

            // Iterate over all type definitions.
            foreach (var type in decompiler.TypeSystem.MainModule.TypeDefinitions)
            {
                // Skip the special <Module> type.
                if (type.Name == "<Module>")
                    continue;

                // Create a FullTypeName instance using the type's full name.
                var fullTypeName = new FullTypeName(type.FullName);

                // Decompile the type.
                string code = decompiler.DecompileTypeAsString(fullTypeName);

                // Create a file name (consider including namespaces for better organization).
                string fileName = $"{type.Namespace}.{type.Name}.cs";
                string filePath = Path.Combine(outputDir, fileName);

                // Ensure the directory for the namespace exists.
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                // Save the decompiled code.
                File.WriteAllText(filePath, code);
            }
        }
    }
}
