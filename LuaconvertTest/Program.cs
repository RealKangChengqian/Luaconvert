using NLua;
using System;
using System.Collections;
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
                var vectorSettingsTable = (LuaTable)lua["VectorSweepSettings"];
                foreach (var item in vectorSettingsTable.Keys)
                {
                    var overrideVectorSweepSetting = new OverrideVectorSweepSetting();
                    if (item.ToString() == "sweep_vector_overridePowerAsArray")
                        overrideVectorSweepSetting.FromLuaTable(vectorSettingsTable, item.ToString());
                }
            }

        }
    }
}