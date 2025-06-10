using System;
using System.IO;
using StardewModdingAPI;

namespace MyChildCore
{
    public static class HotReloadWatcher
    {
        private static FileSystemWatcher _watcher;
        private static DateTime _lastReloadTime = DateTime.MinValue;

        public static void StartWatching(string path, IModHelper helper, ModConfig config)
        {
            // 경로/파일 체크 (예외 방지)
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                ModEntry.Log?.Log($"[HotReloadWatcher] 감시할 파일 없음: {path}", LogLevel.Warn);
                return;
            }

            try
            {
                if (_watcher != null)
                    _watcher.Dispose();

                _watcher = new FileSystemWatcher
                {
                    Path = Path.GetDirectoryName(path),
                    Filter = Path.GetFileName(path),
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
                };

                _watcher.Changed += (s, e) =>
                {
                    try
                    {
                        var now = DateTime.Now;
                        if ((now - _lastReloadTime).TotalMilliseconds < 500)
                            return;
                        _lastReloadTime = now;

                        DataManager.SyncFromDisk();
                        ChildManager.ForceAllChildrenAppearance(config);
                        ModEntry.Log?.Log("[HotReloadWatcher] 핫리로드 동작 완료!", LogLevel.Info);
                    }
                    catch (Exception ex)
                    {
                        ModEntry.Log?.Log("[HotReloadWatcher] 핫리로드 예외: " + ex.Message, LogLevel.Warn);
                    }
                };
                _watcher.EnableRaisingEvents = true;
                ModEntry.Log?.Log($"[HotReloadWatcher] 감시 시작: {path}", LogLevel.Info);
            }
            catch (Exception ex)
            {
                ModEntry.Log?.Log("[HotReloadWatcher] 감시 시작 예외: " + ex.Message, LogLevel.Warn);
            }
        }

        public static void StopWatching()
        {
            try
            {
                _watcher?.Dispose();
                _watcher = null;
                ModEntry.Log?.Log("[HotReloadWatcher] 감시 중단 완료", LogLevel.Info);
            }
            catch (Exception ex)
            {
                ModEntry.Log?.Log("[HotReloadWatcher] 감시 중단 예외: " + ex.Message, LogLevel.Warn);
            }
        }
    }
}