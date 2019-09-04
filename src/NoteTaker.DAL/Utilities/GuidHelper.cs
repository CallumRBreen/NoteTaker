using System;
using RT.Comb;

namespace NoteTaker.DAL.Utilities
{
    public static class GuidHelper
    {
        public static Guid GenerateSequential() => Provider.Sql.Create();
    }
}
