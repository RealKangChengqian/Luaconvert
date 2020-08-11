using NLua;
using System.Collections.Generic;
using System.Linq;

namespace LuaconvertTest
{
 /*
 * 这个类是Source模式下的calibrationSettings的数据结构，它因为与Vector模式下的数据结
 * 构一致，所以直接继承。
 * 对于可能会是向量类型的变量这里用列表来表示。
 */
    public class SourceCalibrationElements
    {
        public double IFBW { get; set; }
        public List<double> vectorCal_Power_powerMeter { get; set; } = new List<double>();
        public List<double> vectorCal_Power { get; set; } = new List<double>();
        public List<double> scalarCal_Power_sourceCal { get; set; } = new List<double>();
        public SourceCalibrationElements Clone()
        {
            
            return this.MemberwiseClone() as SourceCalibrationElements;
        }
    }

 /*
  * 这个类是Source模式下的simple里一个频率点的数据结构。
  * 对于可能会是向量类型的变量这里用列表来表示。
  */

    class SourceSweepSimplePoint
    {
        public double Frequency { get; set; }
        public List<double> PortPower { get; set; } = new List<double>();
    }
/*
 * 这个类是Source模式下的override里一个频率点的数据结构。
 * 对于可能会是向量类型的变量这里用列表来表示。
 */
    class SourceSweepOverridePoint : SourceSweepSimplePoint
    {
        public List<double> ReferenceLevel { get; set; } = new List<double>();
        public List<double> RFSARerenceLevel { get; set; } = new List<double>();
        public List<string> TXPath_5530 { get; set; } = new List<string>();
        public List<string> RXPath_5530 { get; set; } = new List<string>();
        public SourceCalibrationElements calibrationSettings { get; set; } = new SourceCalibrationElements(); 
    }
 /*
 * 这个类是Vector模式下的simple里的数据结构，由若干个频率点组成。
 * 用频率点的数据类型的列表来表示。
 * 包含一个从文件中读取信息填充数据类型的 FromLuaTable 方法
 * 已测试通过
 */
    class SimpleSourceSweepSetting
    {
        public string Name { get; set; }
        public List<SourceSweepSimplePoint> SourceSweepSettingPoints { get; set; } = new List<SourceSweepSimplePoint>();
        public virtual void FromLuaTable(LuaTable luaTable, string name)
        {
            Name = name;
            var subSourceSettingsTable = (LuaTable)luaTable[Name + ".list"];
            var lastSourceSweepSettingPoint = new SourceSweepSimplePoint();
            int i = 0;
            foreach (LuaTable point in subSourceSettingsTable.Values)
            {
                var sourceSweepSettingPoint = new SourceSweepSimplePoint();
                {
                    sourceSweepSettingPoint.Frequency = Utilities.HasField(point, "freq") ? double.Parse(point["freq"].ToString()) : i > 0 ? lastSourceSweepSettingPoint.Frequency : default;
                    sourceSweepSettingPoint.PortPower = Utilities.HasField(point, "portPower") ? Utilities.MutiDoubleFromFile(point, "portPower") : i > 0 ? lastSourceSweepSettingPoint.PortPower : default;
                };
                SourceSweepSettingPoints.Add(sourceSweepSettingPoint);
                lastSourceSweepSettingPoint = sourceSweepSettingPoint;
                i++;
            }
        }
    }
    class OverrideSourceSweepSetting : SimpleSourceSweepSetting
    {
        public new List<SourceSweepOverridePoint> SourceSweepSettingPoints { get; set; } = new List<SourceSweepOverridePoint>();
        public override void FromLuaTable(LuaTable luaTable, string name)
        {
            Name = name;
            var subSourceSettingsTable = (LuaTable)luaTable[Name + ".list"];
            var lastSourceSweepSettingPoint = new SourceSweepOverridePoint();
            int i = 0;
            foreach (LuaTable point in subSourceSettingsTable.Values)
            {
                var sourceSweepSettingPoint = new SourceSweepOverridePoint();
                if (i > 0)
                {
                    //把上次的数据存起来，如果本次循环数据缺省则沿用上次数据
                    sourceSweepSettingPoint.Frequency = lastSourceSweepSettingPoint.Frequency;
                    sourceSweepSettingPoint.PortPower = lastSourceSweepSettingPoint.PortPower;
                    sourceSweepSettingPoint.ReferenceLevel = lastSourceSweepSettingPoint.ReferenceLevel;
                    sourceSweepSettingPoint.calibrationSettings = lastSourceSweepSettingPoint.calibrationSettings;
                    sourceSweepSettingPoint.RFSARerenceLevel = lastSourceSweepSettingPoint.RFSARerenceLevel;
                    sourceSweepSettingPoint.TXPath_5530 = lastSourceSweepSettingPoint.TXPath_5530;
                    sourceSweepSettingPoint.RXPath_5530 = lastSourceSweepSettingPoint.RXPath_5530;
                }
                sourceSweepSettingPoint.Frequency = Utilities.HasField(point, "freq") ? double.Parse(point["freq"].ToString()) : i > 0 ? lastSourceSweepSettingPoint.Frequency : default;
                sourceSweepSettingPoint.PortPower = Utilities.HasField(point, "portPower") ? Utilities.MutiDoubleFromFile(point, "portPower") : i > 0 ? lastSourceSweepSettingPoint.PortPower : default;
                sourceSweepSettingPoint.ReferenceLevel = Utilities.HasField(point, "referenceLevel") ? Utilities.MutiDoubleFromFile(point, "referenceLevel") : i > 0 ? lastSourceSweepSettingPoint.ReferenceLevel : default;
                sourceSweepSettingPoint.RFSARerenceLevel = Utilities.HasField(point, "RFSAReferenceLevel") ? Utilities.MutiDoubleFromFile(point, "RFSAReferenceLevel") : i > 0 ? lastSourceSweepSettingPoint.RFSARerenceLevel : default;
                sourceSweepSettingPoint.TXPath_5530 = Utilities.HasField(point, "5530_TXPath") ? Utilities.MutiStringFromFile(point, "5530_TXPath") : i > 0 ? lastSourceSweepSettingPoint.TXPath_5530 : default;
                sourceSweepSettingPoint.RXPath_5530 = Utilities.HasField(point, "5530_RXPath") ? Utilities.MutiStringFromFile(point, "5530_RXPath") : i > 0 ? lastSourceSweepSettingPoint.RXPath_5530 : default;
                sourceSweepSettingPoint.calibrationSettings = Utilities.HasField(point, "calibrationSettings") ? GetCalibrationSettings((LuaTable)point["calibrationSettings"]) : i > 0 ? lastSourceSweepSettingPoint.calibrationSettings.Clone() : default;

                SourceSweepSettingPoints.Add(sourceSweepSettingPoint);
                lastSourceSweepSettingPoint = sourceSweepSettingPoint;
                i++;
            }
        }
        private SourceCalibrationElements GetCalibrationSettings(LuaTable luaTable)
        {
            return new SourceCalibrationElements()
            {
                IFBW = Utilities.HasField(luaTable, "IFBW") ? double.Parse(luaTable["IFBW"].ToString()) : default,
                vectorCal_Power = Utilities.HasField(luaTable, "vectorCal_Power") ? Utilities.MutiDoubleFromFile(luaTable, "vectorCal_Power") : default,
                vectorCal_Power_powerMeter = Utilities.HasField(luaTable, "vectorCal_Power_powerMeter") ? Utilities.MutiDoubleFromFile(luaTable, "vectorCal_Power_powerMeter") : default,
                scalarCal_Power_sourceCal = Utilities.HasField(luaTable, "scalarCal_Power_sourceCal") ? Utilities.MutiDoubleFromFile(luaTable, "scalarCal_Power_sourceCal") : default,
            };
        }
    }
}
