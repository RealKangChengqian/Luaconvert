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
                var receiverSettingsTable = (LuaTable)lua["ReceiverCalibrationSettings"];
                foreach (var item in receiverSettingsTable.Keys)
                {
                    var overrideReceiverSweep = new OverrideReceiverSweepSetting();
                    if (item.ToString() == "sweep_receiver_NI5531_and_NI5534RX")
                        overrideReceiverSweep.FromLuaTable(receiverSettingsTable, item.ToString());
                }
            }

        }
    }
}