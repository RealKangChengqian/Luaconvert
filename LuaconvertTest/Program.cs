using NLua;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LuaconvertTest
{
    /*
     * 这个类是Vector模式下的calibrationSettings的数据结构。
     * 对于可能会是向量类型的变量这里用列表来表示。
     */
    class CalibrationElements
    {
        public double IFBW { get; set; }
        public List<double> vectorCal_Power_powerMeter { get; set; } = new List<double>();
        public List<double> vectorCal_Power { get; set; } = new List<double>();
        public List<double> scalarCal_Power_sourceCal { get; set; } = new List<double>();

    }
    /*
     * 这个类是Vector模式下的simple里一个频率点的数据结构。
     * 对于可能会是向量类型的变量这里用列表来表示。
     */

    class VectorSweepSimplePoint
    {
        public double Frequency { get; set; }
        public List<double> PortPower { get; set; } = new List<double>();
        public List<double> ReferenceLevel { get; set; } = new List<double>();
        public double IFBW { get; set; }

    }
    /*
     * 这个类是Vector模式下的override里一个频率点的数据结构。
     * 对于可能会是向量类型的变量这里用列表来表示。
     */
    class VectorSweepOverridePoint : VectorSweepSimplePoint
    {
        public List<double> RFSARerenceLevel { get; set; } = new List<double>();
        public List<string> TXPath_5530 { get; set; } = new List<string>();
        public List<string> RXPath_5530 { get; set; } = new List<string>();
        public CalibrationElements calibrationSettings { get; set; } = new CalibrationElements();
    }
    /*
     * 这个类是Vector模式下的override里的数据结构，由若干个频率点组成。
     * 用频率点的数据类型的列表来表示。
     * 包含一个从文件中读取信息填充数据类型的 FromLuaTable 方法
     */
    class OverrideVectorSweepSetting : SimpleVectorSweepSetting
    {
        public new List<VectorSweepOverridePoint> VectorSweepSettingPoints { get; set; } = new List<VectorSweepOverridePoint>();
        public override void FromLuaTable(LuaTable luaTable, string name)
        {
            Name = name;
            var subVectorSettingsTable = (LuaTable)luaTable[Name + ".list"];
            var lastVectorSweepSettingPoint = new VectorSweepOverridePoint();
            int i = 0;
            foreach (LuaTable point in subVectorSettingsTable.Values)
            {
                var vectorSweepSettingPoint = new VectorSweepOverridePoint();
                if (i > 0)
                {
                    //把上次的数据存起来，如果本次循环数据缺省则沿用上次数据
                    vectorSweepSettingPoint.Frequency = lastVectorSweepSettingPoint.Frequency;
                    vectorSweepSettingPoint.PortPower = lastVectorSweepSettingPoint.PortPower;
                    vectorSweepSettingPoint.IFBW = lastVectorSweepSettingPoint.IFBW;
                    vectorSweepSettingPoint.ReferenceLevel = lastVectorSweepSettingPoint.ReferenceLevel;
                    vectorSweepSettingPoint.calibrationSettings.IFBW = lastVectorSweepSettingPoint.calibrationSettings.IFBW;
                    vectorSweepSettingPoint.calibrationSettings.scalarCal_Power_sourceCal = lastVectorSweepSettingPoint.calibrationSettings.scalarCal_Power_sourceCal;
                    vectorSweepSettingPoint.calibrationSettings.vectorCal_Power = lastVectorSweepSettingPoint.calibrationSettings.vectorCal_Power;
                    vectorSweepSettingPoint.calibrationSettings.vectorCal_Power_powerMeter = lastVectorSweepSettingPoint.calibrationSettings.vectorCal_Power_powerMeter;
                    vectorSweepSettingPoint.RFSARerenceLevel = lastVectorSweepSettingPoint.RFSARerenceLevel;
                    vectorSweepSettingPoint.TXPath_5530 = lastVectorSweepSettingPoint.TXPath_5530;
                    vectorSweepSettingPoint.RXPath_5530 = lastVectorSweepSettingPoint.RXPath_5530;
                }
                foreach (var item in point.Keys)
                {
                    if (item.ToString() == "portPower")
                    {
                        if (point[item].GetType()!=typeof(LuaTable))
                        {
                            List<double> tempppower = new List<double>();
                            tempppower.Add(double.Parse(point[item].ToString()));
                            vectorSweepSettingPoint.PortPower = tempppower;
                        }
                        else if (((LuaTable)point[item]).Values.Count > 1)
                        {
                            List<double> tempppower = new List<double>();
                            foreach (var yuan in ((LuaTable)point[item]).Values)
                                tempppower.Add(double.Parse(yuan.ToString()));
                            vectorSweepSettingPoint.PortPower = tempppower;
                        }

                    }
                    else if (item.ToString() == "freq")
                        vectorSweepSettingPoint.Frequency = double.Parse(point[item].ToString());
                    else if (item.ToString() == "referenceLevel")
                    {
                        if (point[item].GetType()!=typeof(LuaTable))
                        {
                            List<double> tempReferenceLevel = new List<double>();
                            tempReferenceLevel.Add(double.Parse(point[item].ToString()));
                            vectorSweepSettingPoint.ReferenceLevel = tempReferenceLevel;
                        }
                        else if (((LuaTable)point[item]).Values.Count > 1)
                            foreach (var yuan in ((LuaTable)point[item]).Values)
                                vectorSweepSettingPoint.ReferenceLevel.Add(double.Parse(yuan.ToString()));
                    }
                    else if (item.ToString() == "IFBW")
                        vectorSweepSettingPoint.IFBW = double.Parse(point[item].ToString());
                    else if (item.ToString() == "calibrationSettings")
                    {
                        LuaTable calibrationTable = (LuaTable)point["calibrationSettings"];
                        foreach (var subpoint in calibrationTable.Keys)
                        {
                            if (subpoint.ToString() == "IFBW")
                                vectorSweepSettingPoint.calibrationSettings.IFBW = double.Parse(calibrationTable[subpoint].ToString());
                            else if (subpoint.ToString() == "vectorCal_Power")
                            {
                                if (calibrationTable[subpoint].GetType() != typeof(LuaTable))
                                {
                                    List<double> tempVectorCalPower = new List<double>();
                                    tempVectorCalPower.Add(double.Parse(calibrationTable[subpoint].ToString()));
                                    vectorSweepSettingPoint.calibrationSettings.vectorCal_Power = tempVectorCalPower;
                                }
                                else if (((LuaTable)calibrationTable[subpoint]).Values.Count > 1)
                                {
                                    List<double> tempVectorCalPower = new List<double>();
                                    foreach (var yuan in ((LuaTable)calibrationTable[subpoint]).Values)
                                        tempVectorCalPower.Add(double.Parse(yuan.ToString()));
                                    vectorSweepSettingPoint.calibrationSettings.vectorCal_Power = tempVectorCalPower;
                                }
                            }
                            else if (subpoint.ToString() == "vectorCal_Power_powerMeter")
                            {
                                if (calibrationTable[subpoint].GetType()!=typeof(LuaTable))
                                {
                                    List<double> tempVectorCalPowerPowerMeter = new List<double>();
                                    tempVectorCalPowerPowerMeter.Add(double.Parse(calibrationTable[subpoint].ToString()));
                                    vectorSweepSettingPoint.calibrationSettings.vectorCal_Power_powerMeter = tempVectorCalPowerPowerMeter;
                                }
                                else if (((LuaTable)calibrationTable[subpoint]).Values.Count > 1)
                                {
                                    List<double> tempVectorCalPowerPowerMeter = new List<double>();
                                    foreach (var yuan in ((LuaTable)calibrationTable[subpoint]).Values)
                                        tempVectorCalPowerPowerMeter.Add(double.Parse(yuan.ToString()));
                                    vectorSweepSettingPoint.calibrationSettings.vectorCal_Power_powerMeter = tempVectorCalPowerPowerMeter;
                                }
                            }
                            else if(subpoint.ToString() == "scalarCal_Power_sourceCal")
                            {
                                if (calibrationTable[subpoint].GetType()!=typeof(LuaTable))
                                {
                                    List<double> tempScalarCalPowerSourceCal = new List<double>();
                                    tempScalarCalPowerSourceCal.Add(double.Parse(calibrationTable[subpoint].ToString()));
                                    vectorSweepSettingPoint.calibrationSettings.scalarCal_Power_sourceCal = tempScalarCalPowerSourceCal;
                                }
                                else if (((LuaTable)calibrationTable[subpoint]).Values.Count > 1)
                                {
                                    List<double> tempScalarCalPowerSourceCal = new List<double>();
                                    foreach (var yuan in ((LuaTable)calibrationTable[subpoint]).Values)
                                        tempScalarCalPowerSourceCal.Add(double.Parse(yuan.ToString()));
                                    vectorSweepSettingPoint.calibrationSettings.scalarCal_Power_sourceCal = tempScalarCalPowerSourceCal;
                                }

                            }
                        }
                    }
                    else if (item.ToString() == "RFSAReferenceLevel")
                    {
                        if ((point[item]).GetType() != typeof(LuaTable))
                        {
                            List<double> tempRFSAReferenceLevel = new List<double>();
                            tempRFSAReferenceLevel.Add(double.Parse(point[item].ToString()));
                            vectorSweepSettingPoint.RFSARerenceLevel = tempRFSAReferenceLevel;
                        }
                        else if (((LuaTable)point[item]).Values.Count > 1)
                        {
                            List<double> tempRFSAReferenceLevel = new List<double>();
                            foreach (var yuan in ((LuaTable)point[item]).Values)
                                tempRFSAReferenceLevel.Add(double.Parse(yuan.ToString()));
                            vectorSweepSettingPoint.RFSARerenceLevel = tempRFSAReferenceLevel;
                        }
                    }
                    else if (item.ToString() == "5530_TXPath")
                    {
                        if (point[item].GetType()!= typeof(LuaTable))
                        {
                            List<string> tempTXPaths = new List<string>();
                            tempTXPaths.Add(point[item].ToString());
                            vectorSweepSettingPoint.TXPath_5530 = tempTXPaths;
                        }
                        else if (((LuaTable)point[item]).Values.Count > 1)
                        {
                            List<string> tempTXPaths = new List<string>();
                            foreach (var yuan in ((LuaTable)point[item]).Values)
                                tempTXPaths.Add(yuan.ToString());
                            vectorSweepSettingPoint.TXPath_5530 = tempTXPaths;
                        }
                    }

                    else if (item.ToString() == "5530_RXPath")
                    {
                        if (point[item].GetType() != typeof(LuaTable))
                        {
                            List<string> tempRXPaths = new List<string>();
                            tempRXPaths.Add(point[item].ToString());
                            vectorSweepSettingPoint.RXPath_5530 = tempRXPaths;
                        }
                        else if (((LuaTable)point[item]).Values.Count > 1)
                        {
                            List<string> tempRXPaths = new List<string>();
                            foreach (var yuan in ((LuaTable)point[item]).Values)
                                tempRXPaths.Add(yuan.ToString());
                            vectorSweepSettingPoint.RXPath_5530 = tempRXPaths;
                        }
                    }
                }
                VectorSweepSettingPoints.Add(vectorSweepSettingPoint);
                lastVectorSweepSettingPoint = vectorSweepSettingPoint;
                i++;
            }
        }
    }
    /*
     * 这个类是Vector模式下的simple里的数据结构，由若干个频率点组成。
     * 用频率点的数据类型的列表来表示。
     * 包含一个从文件中读取信息填充数据类型的 FromLuaTable 方法
     * 已测试通过
     */

    class SimpleVectorSweepSetting
    {
        public string Name { get; set; }
        public List<VectorSweepSimplePoint> VectorSweepSettingPoints { get; set; } = new List<VectorSweepSimplePoint>();
        public virtual void FromLuaTable(LuaTable luaTable, string name)
        {
            Name = name;
            var subVectorSettingsTable = (LuaTable)luaTable[Name + ".list"];
            var lastVectorSweepSettingPoint = new VectorSweepSimplePoint();
            int i = 0;
            foreach (LuaTable point in subVectorSettingsTable.Values)
            {
                var vectorSweepSettingPoint = new VectorSweepSimplePoint();
                if (i > 0)
                {
                    vectorSweepSettingPoint.Frequency = lastVectorSweepSettingPoint.Frequency;
                    vectorSweepSettingPoint.PortPower = lastVectorSweepSettingPoint.PortPower;
                    vectorSweepSettingPoint.IFBW = lastVectorSweepSettingPoint.IFBW;
                    vectorSweepSettingPoint.ReferenceLevel = lastVectorSweepSettingPoint.ReferenceLevel;
                }
                foreach (var item in point.Keys)
                {
                    if (item.ToString() == "portPower")
                    {
                        if (point[item].GetType() != typeof(LuaTable))
                        {
                            List<double> tempppower = new List<double>();
                            tempppower.Add(double.Parse(point[item].ToString()));
                            vectorSweepSettingPoint.PortPower = tempppower;
                        }
                        else if (((LuaTable)point[item]).Values.Count > 1) 
                        {
                            List<double> tempPortPower = new List<double>();
                            foreach (var yuan in ((LuaTable)point[item]).Values)
                                tempPortPower.Add(double.Parse(yuan.ToString()));
                            vectorSweepSettingPoint.PortPower = tempPortPower;
                        }
                    }
                    if (item.ToString() == "freq")
                        vectorSweepSettingPoint.Frequency = double.Parse(point[item].ToString());
                    if (item.ToString() == "ReferenceLevel")
                    {
                        if (point[item].GetType() != typeof(LuaTable))
                        {
                            List<double> tempReferenceLevel = new List<double>();
                            tempReferenceLevel.Add(double.Parse(point[item].ToString()));
                            vectorSweepSettingPoint.ReferenceLevel = tempReferenceLevel;
                        }
                        else if (((LuaTable)point[item]).Values.Count > 1)
                        {
                            List<double> tempReferenceLevel = new List<double>();
                            foreach (var yuan in ((LuaTable)point[item]).Values)
                                tempReferenceLevel.Add(double.Parse(yuan.ToString()));
                            vectorSweepSettingPoint.ReferenceLevel = tempReferenceLevel;
                        }
                    }
                    if (item.ToString() == "IFBW")
                        vectorSweepSettingPoint.Frequency = double.Parse(point[item].ToString());
                }
                VectorSweepSettingPoints.Add(vectorSweepSettingPoint);
                lastVectorSweepSettingPoint = vectorSweepSettingPoint;
                i++;
            }

        }
    }
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
                var simpleVectorSweepSettings = new List<SimpleVectorSweepSetting>();

                foreach (var item in vectorSettingsTable.Keys)
                {
                    var overrideVectorSweep = new OverrideVectorSweepSetting();
                    if (item.ToString() == "test")
                        overrideVectorSweep.FromLuaTable(vectorSettingsTable, item.ToString());
                }
            }
        }
    }
}