using MyChildCore;
using System;
using System.Collections.Generic;
using StardewModdingAPI;

namespace MyChildCore
{
    /// <summary>
    /// DropdownConfig 전역 싱글턴 관리 클래스
    /// 모든 코드에서 DropdownConfigGlobal.Instance로 접근
    /// </summary>
    public static class DropdownConfigGlobal
    {
        /// <summary>
        /// 모든 자녀 관련 설정을 보관하는 전역 인스턴스
        /// </summary>
        public static DropdownConfig Instance { get; set; } = new DropdownConfig();
    }
}