using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyChildCore
{
    /// <summary>
    /// 파츠 자동 스캔/조합/실시간 감시 유틸 (유니크 칠드런식)
    /// </summary>
    public static class PartsScanner
    {
        /// <summary>
        /// 파츠 디렉토리 구조 전체 스캔 (서브폴더별 파츠명 → 파일명 리스트)
        /// </summary>
        /// <param name="root">루트 경로(상대/절대 모두 가능)</param>
        public static Dictionary<string, List<string>> ScanParts(string root)
        {
            var map = new Dictionary<string, List<string>>();
            try
            {
                if (!Directory.Exists(root))
                {
                    CustomLogger.Warn($"[PartsScanner] ScanParts: 폴더 없음: {root}");
                    return map;
                }

                foreach (var dir in Directory.GetDirectories(root, "*", SearchOption.AllDirectories))
                {
                    var partName = dir.Substring(root.Length).TrimStart(Path.DirectorySeparatorChar);
                    List<string> files = new List<string>();
                    try
                    {
                        files = Directory.GetFiles(dir, "*.png", SearchOption.TopDirectoryOnly)
                            .Select(f => Path.GetFileNameWithoutExtension(f)).ToList();
                    }
                    catch (Exception ex)
                    {
                        CustomLogger.Warn($"[PartsScanner] 폴더 접근 오류({partName}): {ex.Message}");
                    }

                    if (files.Count > 0)
                        map[partName] = files;
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[PartsScanner] ScanParts 예외: {ex.Message}");
            }
            return map;
        }

        /// <summary>
        /// 지정된 경로(배우자/나이/파츠타입) 내의 모든 PNG 파츠 파일명 리스트 반환
        /// </summary>
        /// <param name="spouse">배우자명</param>
        /// <param name="ageGroup">아기/유아 등 나이 그룹</param>
        /// <param name="partType">파츠 타입</param>
        /// <param name="root">루트 폴더(기본값 assets)</param>
        public static List<string> GetAvailableParts(string spouse, string ageGroup, string partType, string root = "assets")
        {
            try
            {
                string path = Path.Combine(root, spouse, ageGroup, partType);
                if (!Directory.Exists(path))
                {
                    CustomLogger.Warn($"[PartsScanner] 파츠 폴더 없음: {path}");
                    return new List<string>();
                }
                return Directory.GetFiles(path, "*.png")
                    .Select(f => Path.GetFileNameWithoutExtension(f))
                    .ToList();
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[PartsScanner] GetAvailableParts 예외: {ex.Message}");
                return new List<string>();
            }
        }

        /// <summary>
        /// (선택) 폴더/파일 실시간 변경 감지(핫리로드) 지원
        /// 변경 발생 시 onChanged 콜백 호출
        /// </summary>
        /// <param name="root">감시할 폴더</param>
        /// <param name="onChanged">변경 감지 시 호출될 콜백</param>
        /// <returns>FileSystemWatcher 인스턴스(수동으로 Dispose 필요)</returns>
        public static FileSystemWatcher WatchPartsFolder(string root, Action onChanged)
        {
            if (!Directory.Exists(root))
            {
                CustomLogger.Warn($"[PartsScanner] WatchPartsFolder: 폴더 없음: {root}");
                return null;
            }
            try
            {
                var watcher = new FileSystemWatcher(root, "*.png")
                {
                    IncludeSubdirectories = true,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName
                };
                watcher.Changed += (s, e) => { CustomLogger.Info($"[PartsScanner] 변경 감지: {e.FullPath}"); onChanged?.Invoke(); };
                watcher.Created += (s, e) => { CustomLogger.Info($"[PartsScanner] 추가 감지: {e.FullPath}"); onChanged?.Invoke(); };
                watcher.Deleted += (s, e) => { CustomLogger.Info($"[PartsScanner] 삭제 감지: {e.FullPath}"); onChanged?.Invoke(); };
                watcher.Renamed += (s, e) => { CustomLogger.Info($"[PartsScanner] 이름변경 감지: {e.FullPath}"); onChanged?.Invoke(); };
                watcher.EnableRaisingEvents = true;
                CustomLogger.Info($"[PartsScanner] 폴더 실시간 감시 시작: {root}");
                return watcher;
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[PartsScanner] WatchPartsFolder 예외: {ex.Message}");
                return null;
            }
        }
    }
}