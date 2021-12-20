using System;
using System.Runtime.InteropServices;

namespace MF.Core.Runtime
{
    public static class RuntimeUtil
    {
        public static string System()
        {
            return IsWindows() ? "Windows" : IsLinux() ? "Linux" : IsOsx() ? "OSX" : "";
        }

        public static bool IsLinux()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        public static bool IsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public static bool IsOsx()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        }

        public static bool IsMono { get; } = Type.GetType("Mono.Runtime") != null;
    }
}