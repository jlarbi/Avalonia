using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace Avalonia.DesignerSupport.Tests
{
    /// <summary>
    /// Definition of the <see cref="DesignerSupportTests"/> class.
    /// </summary>
    public class DesignerSupportTests
    {
        /// <summary>
        /// TO DO: Comment...
        /// </summary>
        /// <param name="outputDir"></param>
        /// <param name="xamlFile"></param>
        // TO DO: Comment...
        [Theory,
         InlineData(@"Avalonia.DesignerSupport.TestApp.exe", @"..\..\tests\Avalonia.DesignerSupport.TestApp\MainWindow.xaml"),
         InlineData(@"..\..\samples\ControlCatalog.Desktop\bin\$BUILD\ControlCatalog.dll", @"..\..\samples\ControlCatalog\MainWindow.xaml")]
        public void DesgignerApiShoudBeOperational(string outputDir, string xamlFile)
        {
            var xaml = File.ReadAllText(xamlFile);
#if DEBUG
            outputDir = outputDir.Replace("$BUILD", "Debug");
#else
            outputDir = outputDir.Replace("$BUILD", "Release");
#endif
            var domain = AppDomain.CreateDomain("TESTDOMAIN" + Guid.NewGuid());
            
            var checker = (Checker)domain.CreateInstanceFromAndUnwrap(typeof (Checker).Assembly.GetModules()[0].FullyQualifiedName,
                "Avalonia.DesignerSupport.Tests.Checker");
            checker.DoCheck(outputDir, xaml);

        }
    }
}
