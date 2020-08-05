using NLua;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LuaconvertTest
{
    /*
     * 测试函数
     */
    class Program
    {
        static void Main(string[] args)
        {
            using (Lua lua = new Lua())
            {
                lua.DoFile("example.lua");
                var sourceSettingsTable = (LuaTable)lua["SourceCalibrationSettings"];
                foreach (var item in sourceSettingsTable.Keys)
                {
                    var overrideSourceSweep = new OverrideSourceSweepSetting();
                    if (item.ToString() == "source_full_frequency_range")
                        overrideSourceSweep.FromLuaTable(sourceSettingsTable, item.ToString());
                }
            }
        }
    }
}