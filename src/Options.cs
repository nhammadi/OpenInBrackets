using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace OpenInBrackets
{
    public class Options : DialogPage
    {
        [Category("General")]
        [DisplayName("Install path")]
        [Description(@"The absolute path to the Brackets.exe file.")]
        public string FolderPath { get; set; }

        [Category("General")]
        [DisplayName("Open solution as regular file")]
        [Description("When true, opens solutions as regular files and does not load folder path into Brackets.")]
        public bool OpenSolutionAsRegularFile { get; set; }

        [Category("General")]
        [DisplayName("Open project as regular file")]
        [Description("When true, opens projects as regular files and does not load folder path into Brackets.")]
        public bool OpenProjectAsRegularFile { get; set; }

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();

            if (string.IsNullOrEmpty(FolderPath))
            {
                FolderPath = FindBrackets();
            }
        }

        private static string FindBrackets()
        {
            var programFiles = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            var dirs = programFiles.Parent.GetDirectories(programFiles.Name.Replace(" (x86)", string.Empty) + "*");

            foreach (DirectoryInfo parent in dirs)
                foreach (DirectoryInfo dir in parent.GetDirectories("Brackets*").Reverse())
                {
                    string path = Path.Combine(dir.FullName, "Brackets.exe");

                    if (File.Exists(path))
                        return path;
                }

            return null;
        }
    }
}
