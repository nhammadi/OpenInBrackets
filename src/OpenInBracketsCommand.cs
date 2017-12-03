using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using EnvDTE80;
using System.IO;
using EnvDTE;

namespace OpenInBrackets
{
    internal sealed class OpenInBracketsCommand
    {
        public const int CommandId = 0x0100;

        public static readonly Guid CommandSet = new Guid("50b67178-5ba4-47fe-8351-365d38c16de8");

        private readonly Package package;

        private OpenInBracketsCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.OpenPath, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        public static OpenInBracketsCommand Instance
        {
            get;
            private set;
        }

        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        public static void Initialize(Package package)
        {
            Instance = new OpenInBracketsCommand(package);
        }

        private void OpenPath(object sender, EventArgs e)
        {
            var dte = (DTE2)ServiceProvider.GetService(typeof(DTE));

            try
            {
                string path = Utility.GetSelectedPath(dte, OpenInBracketsCommandPackage.Options.OpenSolutionAsRegularFile, OpenInBracketsCommandPackage.Options.OpenProjectAsRegularFile);
                string exe = OpenInBracketsCommandPackage.Options.FolderPath;

                if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(exe))
                {
                    OpenBrackets(exe, path);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Couldn't resolve the folder");
                }
            }
            catch (Exception)
            {
            }
        }

        private static void OpenBrackets(string exe, string path)
        {
            bool isDirectory = Directory.Exists(path);
            var start = new System.Diagnostics.ProcessStartInfo()
            {
                WorkingDirectory = path,
                FileName = $"\"{exe}\"",
                Arguments = isDirectory ? "." : $"{path}",
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
            };

            using (System.Diagnostics.Process.Start(start))
            {
                string evt = isDirectory ? "directory" : "file";
            }
        }
    }
}
