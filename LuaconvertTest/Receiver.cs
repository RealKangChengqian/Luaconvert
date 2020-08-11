using System;
using System.Collections.Generic;
using System.Linq;
using NLua;

namespace LuaconvertTest
{
    /*
     * 这个类是Source模式下的calibrationSettings的数据结构.
     * 对于可能会是向量类型的变量这里用列表来表示。
     */
    [Serializable]
    public class ReceiverCalibrationElements
    {
        public double IFBW { get; set; }
        public List<double> vectorCal_Power_powerMeter { get; set; } = new List<double>();
        public List<double> vectorCal_Power { get; set; } = new List<double>();
        public List<double> scalarCal_Power_receiverCal { get; set; } = new List<double>();

        public ReceiverCalibrationElements Clone()
        {
            return this.MemberwiseClone() as ReceiverCalibrationElements;
        }
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
                var receiverSweepSettingPoint = new ReceiverSweepSimplePoint
                {
               
                    Frequency = Utilities.HasField(point, "freq") ? double.Parse(point["freq"].ToString()) :
                                                            i > 0 ? lastReceiverSweepSettingPoint.Frequency :
                                                            default,
                    ReferenceLevel = Utilities.HasField(point, "referenceLevel") ? Utilities.MutiDoubleFromFile(point, "referenceLevel") :
                                                                           i > 0 ? lastReceiverSweepSettingPoint.ReferenceLevel :
                                                                           default
                };

                ReceiverSweepSettingPoints.Add(receiverSweepSettingPoint);
                lastReceiverSweepSettingPoint = receiverSweepSettingPoint;
                i++;
            }
        }

    }
    class OverrideReceiverSweepSetting : SimpleReceiverSweepSetting
    {
        public new List<ReceiverSweepOverridePoint> ReceiverSweepSettingPoints { get; set; }
        public override void FromLuaTable(LuaTable luaTable, string name)
        {
            base.FromLuaTable(luaTable, name);
            ReceiverSweepSettingPoints = (from p in base.ReceiverSweepSettingPoints
                                         select new ReceiverSweepOverridePoint
                                         {
                                             Frequency = p.Frequency,
                                             ReferenceLevel = p.ReferenceLevel,
                                         })
                                         .ToList();

            var subReceiverSettingsTable = (LuaTable)luaTable[Name + ".list"];
            var lastReceiverSweepSettingPoint = new ReceiverSweepOverridePoint();
            int i = 0;
            foreach (LuaTable point in subReceiverSettingsTable.Values)
            {
                var receiverSweepSettingPoint = ReceiverSweepSettingPoints[i];

                receiverSweepSettingPoint.RFSARerenceLevel = Utilities.HasField(point, "RFSAReferenceLevel") ? Utilities.MutiDoubleFromFile(point, "RFSAReferenceLevel") : i > 0 ? lastReceiverSweepSettingPoint.RFSARerenceLevel : default;
                receiverSweepSettingPoint.PortPower = Utilities.HasField(point, "portPower") ? Utilities.MutiDoubleFromFile(point, "portPower") : i > 0 ? lastReceiverSweepSettingPoint.PortPower : default;
                receiverSweepSettingPoint.ComplingPath_5530 = Utilities.HasField(point, "5530_CouplingPath") ? Utilities.MutiStringFromFile(point, "5530_CouplingPath") : i > 0 ? lastReceiverSweepSettingPoint.ComplingPath_5530 : default;
                receiverSweepSettingPoint.RXPath_5530 = Utilities.HasField(point, "5530_RXPath") ? Utilities.MutiStringFromFile(point, "5530_RXPath") : i > 0 ? lastReceiverSweepSettingPoint.RXPath_5530 : default;
                receiverSweepSettingPoint.TXPath_5530 = Utilities.HasField(point, "5530_TXPath") ? Utilities.MutiStringFromFile(point, "5530_TXPath") : i > 0 ? lastReceiverSweepSettingPoint.TXPath_5530 : default;
                receiverSweepSettingPoint.calibrationSettings = Utilities.HasField(point, "calibrationSettings") ? GetCalibrationSettings((LuaTable)point["calibrationSettings"]) :  i > 0 ? lastReceiverSweepSettingPoint.calibrationSettings.Clone() : default;

                ReceiverSweepSettingPoints[i] = receiverSweepSettingPoint;
                lastReceiverSweepSettingPoint = receiverSweepSettingPoint;
                i++;
            }


        }
        private ReceiverCalibrationElements GetCalibrationSettings(LuaTable luaTable)
        {
            return new ReceiverCalibrationElements()
            {
                IFBW = Utilities.HasField(luaTable, "IFBW") ? double.Parse(luaTable["IFBW"].ToString()) : default,
                vectorCal_Power = Utilities.HasField(luaTable, "vectorCal_Power") ? Utilities.MutiDoubleFromFile(luaTable, "vectorCal_Power") : default,
                vectorCal_Power_powerMeter = Utilities.HasField(luaTable, "vectorCal_Power_powerMeter") ? Utilities.MutiDoubleFromFile(luaTable, "vectorCal_Power_powerMeter") : default,
                scalarCal_Power_receiverCal = Utilities.HasField(luaTable, "scalarCal_Power_receiverCal") ? Utilities.MutiDoubleFromFile(luaTable, "scalarCal_Power_receiverCal") : default,
            };
        }
    }
}
