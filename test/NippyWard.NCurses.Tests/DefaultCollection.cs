using NippyWard.NCurses.Tests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NippyWard.NCurses.Tests
{
    [CollectionDefinition("Default", DisableParallelization = true)]
    public class DefaultCollection : ICollectionFixture<StdScrState>
    {
    }
}
