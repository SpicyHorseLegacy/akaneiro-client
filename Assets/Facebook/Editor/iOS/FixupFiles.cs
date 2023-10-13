using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace UnityEditor.FacebookEditor
{
    public class FixupFiles
    {
        protected static string Load(string fullPath)
        {
            string data;
            FileInfo projectFileInfo = new FileInfo( fullPath );
            StreamReader fs = projectFileInfo.OpenText();
            data = fs.ReadToEnd();
            fs.Close();

            return data;
        }

        protected static void Save(string fullPath, string data)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(fullPath, false);
            writer.Write(data);
            writer.Close();
        }

        public static void FixSimulator(string path)
        {
            string fullPath = Path.Combine(path, Path.Combine("Libraries", "RegisterMonoModules.cpp"));
            string data = Load (fullPath);


            data = Regex.Replace(data, @"\s+void\s+mono_dl_register_symbol\s+\(const\s+char\*\s+name,\s+void\s+\*addr\);", "");
            data = Regex.Replace(data, "typedef int gboolean;", "typedef int gboolean;\n\tvoid mono_dl_register_symbol (const char* name, void *addr);");

            data = Regex.Replace(data, @"#endif\s+//\s*!\s*\(\s*TARGET_IPHONE_SIMULATOR\s*\)\s*}\s*void RegisterAllStrippedInternalCalls\s*\(\s*\)", "}\n\nvoid RegisterAllStrippedInternalCalls()");
            data = Regex.Replace(data, @"mono_aot_register_module\(mono_aot_module_mscorlib_info\);", 
                                 "mono_aot_register_module(mono_aot_module_mscorlib_info);\n#endif // !(TARGET_IPHONE_SIMULATOR)");

            Save (fullPath, data);
        }

        public static void AddVersionDefine(string path)
        {
            string fullPath = Path.Combine(path, Path.Combine("Libraries", "RegisterMonoModules.h"));
            string data = Load(fullPath);

            data += "\n#define USE_NEW_APP_CLASS_NAME 1\n";

            Save(fullPath, data);
        }
    }
}
