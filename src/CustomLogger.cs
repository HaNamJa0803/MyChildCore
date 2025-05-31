using System;

namespace MyChildCore.Utilities
{
    public static class CustomLogger
    {
        public static void Info(string msg)  => Console.WriteLine("[INFO] " + msg);
        public static void Warn(string msg)  => Console.WriteLine("[WARN] " + msg);
        public static void Error(string msg) => Console.WriteLine("[ERROR] " + msg);
    }
}