using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyChildCore
{
    /// <summary>
    /// 파츠 자동 스캔/조합 유틸 (유니크 칠드런식)
    /// </summary>
    public static class PartsScanner
    {
        public static Dictionary<string, List<string>> ScanParts(string root)
        {
            var map = new Dictionary<string, List<string>>();
            if (!Directory.Exists(root))
                return map;

            foreach (var dir in Directory.GetDirectories(root, "*", SearchOption.AllDirectories))
            {
                var partName = dir.Substring(root.Length).TrimStart(Path.DirectorySeparatorChar);
                var files = Directory.GetFiles(dir, "*.png", SearchOption.TopDirectoryOnly)
                    .Select(f => Path.GetFileNameWithoutExtension(f)).ToList();
                if (files.Count > 0)
                    map[partName] = files;
            }
            return map;
        }

        // 예시: 특정 배우자/나이/파츠 기준으로 자동 파츠 옵션 제공
        public static List<string> GetAvailableParts(string spouse, string ageGroup, string partType)
        {
            string path = Path.Combine("assets", spouse, ageGroup, partType);
            if (!Directory.Exists(path)) return new List<string>();
            return Directory.GetFiles(path, "*.png")
                .Select(f => Path.GetFileNameWithoutExtension(f))
                .ToList();
        }
    }
}