using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace OpenInBrackets
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]       
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(OpenInBracketsCommandPackage.PackageGuidString)]
    [ProvideOptionPage(typeof(Options), "Open in Brackets", "General", 0, 0, true)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class OpenInBracketsCommandPackage : Package
    {
        public const string PackageGuidString = "bda186a7-b774-4df7-8158-4e1ada4959c0";
        public static Options Options;

        public OpenInBracketsCommandPackage()
        {
        }

        #region Package Members

        protected override void Initialize()
        {
            Options = (Options)GetDialogPage(typeof(Options));
            OpenInBracketsCommand.Initialize(this);
            base.Initialize();
        }

        #endregion
    }
}
