using System;
using RT.Comb;

namespace NoteTaker.DAL.Utilities
{
    internal static class GuidHelper
    {
        public static Guid GenerateSequential() => Provider.Sql.Create();
    }
}
