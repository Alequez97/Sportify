using System.Collections.Generic;
using System.Text;

namespace SeedScript.Models
{
    public class SeedReport
    {
        public int TotalMessageCount => SkipedSeedsMessages.Count;

        public HashSet<string> SkipedSeedsMessages { get; set; } = new HashSet<string>();

        public override string ToString()
        {
            if (TotalMessageCount > 0)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("\nSeed report: ");

                if (SkipedSeedsMessages.Count > 0)
                {
                    stringBuilder.AppendLine("  Skipped seeds: ");
                    foreach (var skeepSeedMessage in SkipedSeedsMessages)
                    {
                        stringBuilder.AppendLine($"   * {skeepSeedMessage}");
                    }
                }

                return stringBuilder.ToString();
            }

            return string.Empty;
        }

    }
}
