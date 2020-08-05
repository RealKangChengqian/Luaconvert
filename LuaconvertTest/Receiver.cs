using System;
using System.Collections.Generic;
using NLua;

namespace LuaconvertTest
{
    /*
     * 这个类是Source模式下的calibrationSettings的数据结构.
     * 对于可能会是向量类型的变量这里用列表来表示。
     */
    public class ReceiverCalibrationElements
    {
        public double IFBW { get; set; }
        public List<double> vectorCal_Power_powerMeter { get; set; } = new List<double>();
        public List<double> vectorCal_Power { get; set; } = new List<double>();
        public List<double> scalarCal_Power_receiverCal { get; set; } = new List<double>();
    }
    /*
     * 这个类是Source模式下的simple里一个频率点的数据结构。
     * 对于可能会是向量类型的变量这里用列表来表示。
     */

    class ReceiverSweepSimplePoint
    {
        public double Frequency { get; set; }
        public List<double> ReferenceLevel { get; set; } = new List<double>();
    }
    /*
     * 这个类是Source模式下的simple里一个频率点的数据结构。
     * 对于可能会是向量类型的变量这里用列表来表示。
     */
    class ReceiverSweepOverridePoint : ReceiverSweepSimplePoint
    {
        public List<double> RFSARerenceLevel { get; set; } = new List<double>();
        public List<double> PortPower { get; set; } = new List<double>();
        public List<string> TXPath_5530 { get; set; } = new List<string>();
        public List<string> RXPath_5530 { get; set; } = new List<string>();
        public List<string> ComplingPath_5530 { get; set; } = new List<string>();
        public ReceiverCalibrationElements calibrationSettings { get; set; } = new ReceiverCalibrationElements();
    }
    /*
    * 这个类是Vector模式下的simple里的数据结构，由若干个频率点组成。
    * 用频率点的数据类型的列表来表示。
    * 包含一个从文件中读取信息填充数据类型的 FromLuaTable 方法
    * 已测试通过
    */
    class SimpleReceiverSweepSetting
    {
        public string Name { get; set; }
        public List<ReceiverSweepSimplePoint> ReceiverSweepSettingPoints { get; set; } = new List<ReceiverSweepSimplePoint>();
        public virtual void FromLuaTable(LuaTable luaTable, string name)
        {
            Name = name;
            var subReceiverSettingsTable = (LuaTable)luaTable[Name + ".list"];
            var lastReceiverSweepSettingPoint = new ReceiverSweepSimplePoint();
            int i = 0;
            foreach (LuaTable point in subReceiverSettingsTable.Values)
            {
                var receiverSweepSettingPoint = new ReceiverSweepSimplePoint();
                if (i > 0)
                {
                    receiverSweepSettingPoint.Frequency = lastReceiverSweepSettingPoint.Frequency;
                    receiverSweepSettingPoint.ReferenceLevel = lastReceiverSweepSettingPoint.ReferenceLevel;
                }
                foreach (var item in point.Keys)
                {
                    if (item.ToString() == "freq")
                        receiverSweepSettingPoint.Frequency = double.Parse(point[item].ToString());
                    if (item.ToString() == "referenceLevel")
                        receiverSweepSettingPoint.ReferenceLevel = General.MutiDoubleFromFile(point, item.ToString());
                }
                ReceiverSweepSettingPoints.Add(receiverSweepSettingPoint);
                lastReceiverSweepSettingPoint = receiverSweepSettingPoint;
                i++;
            }
        }
    }
    class OverrideReceiverSweepSetting : SimpleReceiverSweepSetting
    {
        public new string Name { get; set; }
        public new List<ReceiverSweepOverridePoint> ReceiverSweepSettingPoints { get; set; } = new List<ReceiverSweepOverridePoint>();
        public override void FromLuaTable(LuaTable luaTable, string name)
        {
            Name = name;
            var subReceiverSettingsTable = (LuaTable)luaTable[Name + ".list"];
            var lastReceiverSweepSettingPoint = new ReceiverSweepOverridePoint();
            int i = 0;
            foreach (LuaTable point in subReceiverSettingsTable.Values)
            {
                var receiverSweepSettingPoint = new ReceiverSweepOverridePoint();
                if (i > 0)
                {
                    receiverSweepSettingPoint.Frequency = lastReceiverSweepSettingPoint.Frequency;
                    receiverSweepSettingPoint.ReferenceLevel = lastReceiverSweepSettingPoint.ReferenceLevel;
                    receiverSweepSettingPoint.RFSARerenceLevel = lastReceiverSweepSettingPoint.RFSARerenceLevel;
                    /////////////////////////////////////////////////
                    ////////////////////////////////////////////////
                }
                foreach (var item in point.Keys)
                {
                    if (item.ToString() == "freq")
                        receiverSweepSettingPoint.Frequency = double.Parse(point[item].ToString());
                    if (item.ToString() == "referenceLevel")
                        receiverSweepSettingPoint.ReferenceLevel = General.MutiDoubleFromFile(point, item.ToString());
                }
                ReceiverSweepSettingPoints.Add(receiverSweepSettingPoint);
                lastReceiverSweepSettingPoint = receiverSweepSettingPoint;
                i++;
            }

        }
    }
}
