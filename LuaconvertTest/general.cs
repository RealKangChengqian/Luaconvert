using NLua;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LuaconvertTest
{

    class General
    {
        //从Lua File中抓取浮点型数组变量
        public static List<double> MutiDoubleFromFile(LuaTable luaTable, string tableskr)
        {
            List<double> tempList=new List<double>();
            if (luaTable[tableskr].GetType() != typeof(LuaTable))
                tempList.Add(double.Parse(luaTable[tableskr].ToString()));
            else if (((LuaTable)luaTable[tableskr]).Values.Count > 1)
            {
                foreach (var item in ((LuaTable)luaTable[tableskr]).Values)
                    tempList.Add(double.Parse(item.ToString()));
            }
            return tempList;
        }
        //从Lua File中抓取字符串型数组变量
        public static List<string> MutiStringFromFile(LuaTable luaTable, string tableskr)
        {
            List<string> tempList = new List<string>();
            if (luaTable[tableskr].GetType() != typeof(LuaTable))
                tempList.Add(luaTable[tableskr].ToString());
            else if(((LuaTable)luaTable[tableskr]).Values.Count > 1)
            {
                foreach (var item in ((LuaTable)luaTable[tableskr]).Values)
                    tempList.Add(item.ToString());
            }
            return tempList;
        }
    }
}