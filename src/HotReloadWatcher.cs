using MyChildCore;
using System;
using System.Linq;
using System.IO;
using StardewModdingAPI;

namespace MyChildCore
{
    public static class HotReloadWatcher
    {
        private static FileSystemWatcher _watcher;
        private static DateTime _lastReloadTime = DateTime.MinValue;

        /// <summary>
        /// 핫리로드 시 외부 모듈에서 동작을 구독(Observer)할 수 있게 이벤트 제공
        /// </summary>
        public static event Action OnHotReload;

        /// <summary>
        /// 세이브 폴더 내 실제 세이브 파일(BIN, 무확장, 모바일) 자동 감지
        /// </summary>
        private static string FindSaveFile()
        {
            var folder = StardewModdingAPI.Constants.SaveFolderName;
            if (string.IsNullOrEmpty(folder))
                folder = "Unknown";

            var saveDir = Path.Combine("Saves", folder);
            var baseName = folder;

            // 1. 무확장(정상)
            var mainPath = Path.Combine(saveDir, baseName);
            if (File.Exists(mainPath))
                return mainPath;

            // 2. BIN (특정 플랫폼/버전)
            var binPath = mainPath + ".BIN";
            if (File.Exists(binPath))
                return binPath;

            // 3. 모바일/안드로이드 예외(.bin 소문자)
            var androidBin = Path.Combine(saveDir, baseName + ".bin");
            if (File.Exists(androidBin))
                return androidBin;

            // 4. 폴더 내 가장 최근 파일 (혹시라도 세이브 파일명이 예외일 때)
            var dir = new DirectoryInfo(saveDir);
            if (dir.Exists)
            {
                var file = dir.GetFiles()
                    .OrderByDescending(f => f.LastWriteTime)
                    .FirstOrDefault();
                if (file != null)
                    return file.FullName;
            }

            return null;
        }

        /// <summary>
        /// 세이브파일 실시간 감시 시작 (반드시 SaveLoaded에서 호출)
        /// </summary>
        public static void StartWatching(IModHelper helper, ModConfig config)
        {
            string path = FindSaveFile();
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                ModEntry.Log?.Log($"[HotReloadWatcher] 감시할 세이브 파일 없음: {path}", LogLevel.Warn);
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

                        // 데이터, 외형 동기화 (핫리로드)
                        DataManager.SyncFromDisk();
                        ChildManager.ForceAllChildrenAppearance(config);

                        // 실시간 옵저버 알림
                        OnHotReload?.Invoke();

                        ModEntry.Log?.Log("[HotReloadWatcher] 세이브 감시 핫리로드 완료!", LogLevel.Info);
                    }
                    catch (Exception ex)
                    {
                        ModEntry.Log?.Log("[HotReloadWatcher] 핫리로드 예외: " + ex.Message, LogLevel.Warn);
                    }
                };
                _watcher.EnableRaisingEvents = true;
                ModEntry.Log?.Log($"[HotReloadWatcher] 감시 시작(세이브 파일): {path}", LogLevel.Info);
            }
            catch (Exception ex)
            {
                ModEntry.Log?.Log("[HotReloadWatcher] 감시 시작 예외: " + ex.Message, LogLevel.Warn);
            }
        }

        /// <summary>
        /// 감시 중단 (반드시 ReturnedToTitle 등에서 호출)
        /// </summary>
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