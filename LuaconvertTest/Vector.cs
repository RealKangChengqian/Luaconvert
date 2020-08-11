using NLua;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LuaconvertTest
{
    public class VectorCalibrationElements
    {
        public double IFBW { get; set; }
        public List<double> vectorCal_Power_powerMeter { get; set; } = new List<double>();
        public List<double> vectorCal_Power { get; set; } = new List<double>();
        public List<double> scalarCal_Power_sourceCal { get; set; } = new List<double>();
        public virtual VectorCalibrationElements Clone()
        {
            return this.MemberwiseClone() as VectorCalibrationElements;
        }
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
        public VectorCalibrationElements calibrationSettings { get; set; } = new VectorCalibrationElements();
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
                    vectorSweepSettingPoint.calibrationSettings = lastVectorSweepSettingPoint.calibrationSettings;
                    vectorSweepSettingPoint.RFSARerenceLevel = lastVectorSweepSettingPoint.RFSARerenceLevel;
                    vectorSweepSettingPoint.TXPath_5530 = lastVectorSweepSettingPoint.TXPath_5530;
                    vectorSweepSettingPoint.RXPath_5530 = lastVectorSweepSettingPoint.RXPath_5530;
                }
                vectorSweepSettingPoint.Frequency = Utilities.HasField(point, "freq") ? double.Parse(point["freq"].ToString()) : i > 0 ? lastVectorSweepSettingPoint.Frequency : default;
                vectorSweepSettingPoint.IFBW = Utilities.HasField(point, "IFBW") ? double.Parse(point["IFBW"].ToString()) : i > 0 ? lastVectorSweepSettingPoint.IFBW : default;
                vectorSweepSettingPoint.PortPower = Utilities.HasField(point, "portPower") ? Utilities.MutiDoubleFromFile(point, "portPower") : i > 0 ? lastVectorSweepSettingPoint.PortPower : default;
                vectorSweepSettingPoint.ReferenceLevel = Utilities.HasField(point, "referenceLevel") ? Utilities.MutiDoubleFromFile(point, "referenceLevel") : i > 0 ? lastVectorSweepSettingPoint.ReferenceLevel : default;
                vectorSweepSettingPoint.RFSARerenceLevel = Utilities.HasField(point, "RFSAReferenceLevel") ? Utilities.MutiDoubleFromFile(point, "RFSAReferenceLevel") : i > 0 ? lastVectorSweepSettingPoint.RFSARerenceLevel : default;
                vectorSweepSettingPoint.TXPath_5530 = Utilities.HasField(point, "5530_TXPath") ? Utilities.MutiStringFromFile(point, "5530_TXPath") : i > 0 ? lastVectorSweepSettingPoint.TXPath_5530 : default;
                vectorSweepSettingPoint.RXPath_5530 = Utilities.HasField(point, "5530_RXPath") ? Utilities.MutiStringFromFile(point, "5530_RXPath") : i > 0 ? lastVectorSweepSettingPoint.RXPath_5530 : default;
                vectorSweepSettingPoint.calibrationSettings = Utilities.HasField(point, "calibrationSettings") ? GetCalibrationSettings((LuaTable)point["calibrationSettings"]) : i > 0 ? lastVectorSweepSettingPoint.calibrationSettings.Clone() : default;

                VectorSweepSettingPoints.Add(vectorSweepSettingPoint);
                lastVectorSweepSettingPoint = vectorSweepSettingPoint;
                i++;
            }
        }
        private VectorCalibrationElements GetCalibrationSettings(LuaTable luaTable)
        {
            return new VectorCalibrationElements()
            {
                IFBW = Utilities.HasField(luaTable, "IFBW") ? double.Parse(luaTable["IFBW"].ToString()) : default,
                vectorCal_Power = Utilities.HasField(luaTable, "vectorCal_Power") ? Utilities.MutiDoubleFromFile(luaTable, "vectorCal_Power") : default,
                vectorCal_Power_powerMeter = Utilities.HasField(luaTable, "vectorCal_Power_powerMeter") ? Utilities.MutiDoubleFromFile(luaTable, "vectorCal_Power_powerMeter") : default,
                scalarCal_Power_sourceCal = Utilities.HasField(luaTable, "scalarCal_Power_sourceCal") ? Utilities.MutiDoubleFromFile(luaTable, "scalarCal_Power_sourceCal") : default,
            };
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
                        vectorSweepSettingPoint.PortPower = Utilities.MutiDoubleFromFile(point, item.ToString());
                    if (item.ToString() == "freq")
                        vectorSweepSettingPoint.Frequency = double.Parse(point[item].ToString());
                    if (item.ToString() == "ReferenceLevel")
                        vectorSweepSettingPoint.ReferenceLevel = Utilities.MutiDoubleFromFile(point, item.ToString());
                    if (item.ToString() == "IFBW")
                        vectorSweepSettingPoint.IFBW = double.Parse(point[item].ToString());
                }
                VectorSweepSettingPoints.Add(vectorSweepSettingPoint);
                lastVectorSweepSettingPoint = vectorSweepSettingPoint;
                i++;
            }

        }
    }
}