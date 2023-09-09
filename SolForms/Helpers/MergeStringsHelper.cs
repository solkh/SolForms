using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolForms.Helpers
{
    public static class MergeStringsHelper
    {
        public static string MergeStrings(this string prefix, params Guid[] guids)
        {
            foreach (var guid in guids)
                prefix += $"_{guid}";
            return prefix;
        }
        public static string[] AddPrefix(this string prefix, params Guid[] guids) =>
            guids.Select(x => $"{prefix}_{x}").ToArray();
    }
}
