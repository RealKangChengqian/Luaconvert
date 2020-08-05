using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LuaconvertTest
{
 /*
 * 这个类是Source模式下的calibrationSettings的数据结构，它因为与Vector模式下的数据结
 * 构一致，所以直接继承。
 * 对于可能会是向量类型的变量这里用列表来表示。
 */
    public class SourceCalibrationElements:VectorCalibrationElements{}

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
                if (i > 0)
                {
                    sourceSweepSettingPoint.Frequency = lastSourceSweepSettingPoint.Frequency;
                    sourceSweepSettingPoint.PortPower = lastSourceSweepSettingPoint.PortPower;
                }
                foreach (var item in point.Keys)
                {
                    if (item.ToString() == "portPower")
                        sourceSweepSettingPoint.PortPower = General.MutiDoubleFromFile(point, item.ToString());
                    if (item.ToString() == "freq")
                        sourceSweepSettingPoint.Frequency = double.Parse(point[item].ToString());
                }
                SourceSweepSettingPoints.Add(sourceSweepSettingPoint);
                lastSourceSweepSettingPoint = sourceSweepSettingPoint;
                i++;
            }
        }
    }
    class OverrideSourceSweepSetting : SimpleSourceSweepSetting
    {
        public new string Name { get; set; }
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
                    sourceSweepSettingPoint.calibrationSettings.IFBW = lastSourceSweepSettingPoint.calibrationSettings.IFBW;
                    sourceSweepSettingPoint.calibrationSettings.scalarCal_Power_sourceCal = lastSourceSweepSettingPoint.calibrationSettings.scalarCal_Power_sourceCal;
                    sourceSweepSettingPoint.calibrationSettings.vectorCal_Power = lastSourceSweepSettingPoint.calibrationSettings.vectorCal_Power;
                    sourceSweepSettingPoint.calibrationSettings.vectorCal_Power_powerMeter = lastSourceSweepSettingPoint.calibrationSettings.vectorCal_Power_powerMeter;
                    sourceSweepSettingPoint.RFSARerenceLevel = lastSourceSweepSettingPoint.RFSARerenceLevel;
                    sourceSweepSettingPoint.TXPath_5530 = lastSourceSweepSettingPoint.TXPath_5530;
                    sourceSweepSettingPoint.RXPath_5530 = lastSourceSweepSettingPoint.RXPath_5530;
                }
                foreach (var item in point.Keys)
                {
                    if (item.ToString() == "portPower")
                        sourceSweepSettingPoint.PortPower = General.MutiDoubleFromFile(point, item.ToString());
                    else if (item.ToString() == "freq")
                        sourceSweepSettingPoint.Frequency = double.Parse(point[item].ToString());
                    else if (item.ToString() == "referenceLevel")
                        sourceSweepSettingPoint.ReferenceLevel = General.MutiDoubleFromFile(point, item.ToString());
                    else if (item.ToString() == "calibrationSettings")
                    {
                        LuaTable calibrationTable = (LuaTable)point["calibrationSettings"];
                        foreach (var subpoint in calibrationTable.Keys)
                        {
                            if (subpoint.ToString() == "IFBW")
                                sourceSweepSettingPoint.calibrationSettings.IFBW = double.Parse(calibrationTable[subpoint].ToString());
                            else if (subpoint.ToString() == "vectorCal_Power")
                                sourceSweepSettingPoint.calibrationSettings.vectorCal_Power = General.MutiDoubleFromFile(calibrationTable, subpoint.ToString());
                            else if (subpoint.ToString() == "vectorCal_Power_powerMeter")
                                sourceSweepSettingPoint.calibrationSettings.vectorCal_Power_powerMeter = General.MutiDoubleFromFile(calibrationTable, subpoint.ToString());
                            else if (subpoint.ToString() == "scalarCal_Power_sourceCal")
                                sourceSweepSettingPoint.calibrationSettings.scalarCal_Power_sourceCal = General.MutiDoubleFromFile(calibrationTable, subpoint.ToString());
                        }
                    }
                    else if (item.ToString() == "RFSAReferenceLevel")
                        sourceSweepSettingPoint.RFSARerenceLevel = General.MutiDoubleFromFile(point, item.ToString());
                    else if (item.ToString() == "5530_TXPath")
                        sourceSweepSettingPoint.TXPath_5530 = General.MutiStringFromFile(point, item.ToString());
                    else if (item.ToString() == "5530_RXPath")
                        sourceSweepSettingPoint.RXPath_5530 = General.MutiStringFromFile(point, item.ToString());
                }
                SourceSweepSettingPoints.Add(sourceSweepSettingPoint);
                lastSourceSweepSettingPoint = sourceSweepSettingPoint;
                i++;
            }
        }
    }
}
