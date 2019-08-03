using System.Collections.Generic;
using System.ComponentModel.Composition;
using NuGetPackageExplorer.Types;
using NuGetPe;

namespace PackageExplorer.Extensions.ILSpy
{
    [Export(typeof(IPackageRule))]
    internal class ILSpyPackageRule : IPackageRule
    {
        public IEnumerable<PackageIssue> Validate(IPackage package, string packageFileName)
        {
            // no package validation required so far
            // sticking to the contract and returning an empty IEnumerable<PackageIssue>
            return new List<PackageIssue>();
        }
    }
}
