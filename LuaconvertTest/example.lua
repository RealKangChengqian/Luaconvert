-- config revision
Revision = 0.6;

-- Easy to use constants
kilo = 1e3;
Mega = 1e6;
Giga = 1e9;

VectorSweepSettings={};
SourceCalibrationSettings={};
ReceiverCalibrationSettings={};

-- Please see the "Using the Sweep Settings Configuration File" topic in the NI-RFPM Help for more details about the sweep settings files.

-- The first step in each sweep must have all the required settings for that sweep type.
-- Subsequent steps will inherit any settings that aren't specified from the preceding step.


-- The first three sweeps show the minimum settings necessary for vector, source, and receiver sweeps.

-- Simple vector sweep
VectorSweepSettings["sweep_vector_simple"] =
{
   ["type"] = "list",
   -- frequency list
   ["list"] =
   {
      {
         -- Frequency to calibrate with at this point
         ["freq"] = 1*Giga,
         -- Output power in dBm at the blind mate (or DUT with deembedding enabled) during measurement or calibration for all ports
         ["portPower"] = 5,
      },
      {
         ["freq"] = 1.01*Giga,
      },
      {
         ["freq"] = 1.02*Giga,
      },
      {
         ["freq"] = 1.03*Giga,
         ["portPower"] = 28,
      },
      {
         ["freq"] = 1.04*Giga,
      },
      {
         ["freq"] = 1.05*Giga,
      },
      {
         ["freq"] = 2*Giga,
      }
   }
};

-- Simple source sweep
SourceCalibrationSettings["sweep_source_simple"] =
{
   ["type"] = "list",
   -- frequency list
   ["list"] =
   {
      {
         -- Frequency to calibrate with at this point
         ["freq"] = 1*Giga,
         -- Output power in dBm at the blind mate (or DUT with deembedding enabled) during calibration for all ports
         ["portPower"] = 0
      },
      {
         ["freq"] = 1.01*Giga,
      },
      {
         ["freq"] = 1.02*Giga,
      },
      {
         ["freq"] = 1.03*Giga,
      },
      {
         ["freq"] = 1.04*Giga,
      },
      {
         ["freq"] = 1.05*Giga,
      },
      {
         ["freq"] = 2*Giga,
      }
   }
};

-- Simple receiver sweep
ReceiverCalibrationSettings["sweep_receiver_simple"] =
{
   ["type"] = "list",
   -- frequency list
   ["list"] =
   {
      {
         -- Frequency to calibrate with at this point
         ["freq"] = 1*Giga,
         -- Since no reference level is specified, the default values at the blind mate reference plane will be calibrated
      },
      {
         ["freq"] = 1.01*Giga,
      },
      {
         ["freq"] = 1.02*Giga,
      },
      {
         ["freq"] = 1.03*Giga,
      },
      {
         ["freq"] = 1.04*Giga,
      },
      {
         ["freq"] = 1.05*Giga,
      },
      {
         ["freq"] = 2*Giga,
      }
   }
};


-- The next 3 sweeps show the optional settings available in the simple sweeps,
-- and show how to specify a power per port, rather than using the same power for all ports.

-- Simple vector sweep with optional settings and specify power per port
VectorSweepSettings["sweep_vector_simpleWithOptional"] =
{
   ["type"] = "list",
   -- frequency list
   ["list"] =
   {
      {
         -- Frequency to calibrate with at this point
         ["freq"] = 1*Giga,
         -- Output power used during measurement or calibration
         -- The number of ports used with this sweep will need to match the number of entries in this table
         ["portPower"] =
         {
            0,  -- dBm at blind mate (or DUT with deembedding enabled) for the first port
            -5,  -- dBm at blind mate (or DUT with deembedding enabled) for the second port
         },
         -- Input reference level in dBm at blind mate (or DUT with deembedding enabled), optional
         ["referenceLevel"] = 10,
         -- IF bandwidth used during the measurement, optional
         ["IFBW"] = 1000
      },
      {
         ["freq"] = 1.01*Giga,
		 ["portPower"] =
         {
            0,  -- dBm at blind mate (or DUT with deembedding enabled) for the first port
            -5,  -- dBm at blind mate (or DUT with deembedding enabled) for the second port
         },
      },
      {
         ["freq"] = 1.02*Giga,
      },
      {
         ["freq"] = 1.03*Giga,
      },
      {
         ["freq"] = 1.04*Giga,
      },
      {
         ["freq"] = 1.05*Giga,
      },
      {
         ["freq"] = 2*Giga,
      }
   }
};

-- Simple source sweep with power specified per port (the simple source sweep has no optional settings)
SourceCalibrationSettings["sweep_source_simplePowerAsArray"] =
{
   ["type"] = "list",
   -- frequency list
   ["list"] =
   {
      {
         -- Frequency to calibrate with at this point
         ["freq"] = 1*Giga,
         -- Output power used during calibration
         -- The number of ports used with this sweep will need to match the number of entries in this table
         ["portPower"] =
         {
            0,  -- dBm at blind mate (or DUT with deembedding enabled) for the first port
            -5,  -- dBm at blind mate (or DUT with deembedding enabled) for the second port
         }
      },
      {
         ["freq"] = 1.01*Giga,
      },
      {
         ["freq"] = 1.02*Giga,
      },
      {
         ["freq"] = 1.03*Giga,
      },
      {
         ["freq"] = 1.04*Giga,
      },
      {
         ["freq"] = 1.05*Giga,
      },
      {
         ["freq"] = 2*Giga,
      }
   }
};

-- Simple receiver sweep with optional settings
ReceiverCalibrationSettings["sweep_receiver_simpleWithOptional"] =
{
   ["type"] = "list",
   -- frequency list
   ["list"] =
   {
      {
         -- Frequency to calibrate with at this point.
         ["freq"] = 1*Giga,
         -- Input reference level in dBm at blind mate (or DUT with deembedding enabled), optional
         ["referenceLevel"] = 0
      },
      {
         ["freq"] = 1.01*Giga,
      },
      {
         ["freq"] = 1.02*Giga,
      },
      {
         ["freq"] = 1.03*Giga,
      },
      {
         ["freq"] = 1.04*Giga,
      },
      {
         ["freq"] = 1.05*Giga,
      },
      {
         ["freq"] = 2*Giga,
      }
   }
};


-- The final 3 are the override sweeps, which require all settings to be specified.
-- These are designed for advanced users to have greater control of the calibration and measurement
-- parameters for specific use cases.

-- Override vector sweep with powers as arrays
VectorSweepSettings["sweep_vector_overridePowerAsArray"] =
{
   ["type"] = "list",
   -- frequency list
   ["list"] =
   {
      {
         -- Frequency to calibrate with at this point
         ["freq"] = 1*Giga,
         -- Output power used during measurement or calibration
         -- The number of ports used with this sweep will need to match the number of entries in this table
         ["portPower"] =
         {
            0,  -- dBm at blind mate (or DUT with deembedding enabled) for the first port
            -5,  -- dBm at blind mate (or DUT with deembedding enabled) for the second port
         },
         -- Input reference level in dBm at blind mate (or DUT with deembedding enabled)
         ["referenceLevel"] = 10,
         -- IF bandwidth used during the measurement
         ["IFBW"] = 1000,
         -- Reference level in dBm set directly to RFSA during a measurement or calibration. Not impacted by deembedding
         ["RFSAReferenceLevel"] = 0,
         -- Path through the port control module to use for all sources in the measurement. The valid values are GAIN, DIRECT, LOOP
         ["5530_TXPath"] = "GAIN",
         -- Path through the port control module to use for all receivers in the measurement. The valid values are GAIN, DIRECT, LOOP
         ["5530_RXPath"] = "DIRECT",
         -- Table of settings that are only relevant to calibration
         ["calibrationSettings"] =
         {
            -- IF bandwidth to use during calibration
            ["IFBW"] = 1000,
            -- Output power at blind mate to use during the vector portion of calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["vectorCal_Power"] =
            {
               10,  -- dBm at blind mate for the first port
               -5,  -- dBm at blind mate for the second port
            },
            -- Output power at blind mate to use during the power meter measurement of the absolute vector calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["vectorCal_Power_powerMeter"] =
            {
               10,  -- dBm at blind mate for the first port
               -5,  -- dBm at blind mate for the second port
            },
            -- Output power at blind mate to use during the source calibration portion of calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["scalarCal_Power_sourceCal"] =
            {
               0,  -- dBm at blind mate for the first port
               -5,  -- dBm at blind mate for the second port
            }
         }
      },
      {
         ["freq"] = 1.01*Giga,
      },
      {
         ["freq"] = 1.02*Giga,
      },
      {
         ["freq"] = 1.03*Giga,
      },
      {
         ["freq"] = 1.04*Giga,
      },
      {
         ["freq"] = 1.05*Giga,
      },
      {
         ["freq"] = 2*Giga,
      }
   }
};

-- Override source sweep with powers as arrays
SourceCalibrationSettings["sweep_source_overridePowerAsArray"] =
{
   ["type"] = "list",
   -- frequency list
   ["list"] =
   {
      {
         -- Frequency to calibrate with at this point
         ["freq"] = 1*Giga,
         -- Output power used during calibration
         -- The number of ports used with this sweep will need to match the number of entries in this table
         ["portPower"] =
         {
            0,  -- dBm at blind mate (or DUT with deembedding enabled) for the first port
            -5,  -- dBm at blind mate (or DUT with deembedding enabled) for the second port
         },
         -- Input reference level in dBm at blind mate (or DUT with deembedding enabled) used during calibration
         ["referenceLevel"] = 10,
         -- Reference level in dBm set directly to RFSA during a calibration. Not impacted by deembedding
         ["RFSAReferenceLevel"] = 0,
         -- Path through the port control module to use for all sources in the calibration. The valid values are GAIN, DIRECT, LOOP
         ["5530_TXPath"] = "GAIN",
         -- Path through the port control module to use for all receivers in the calibration. The valid values are GAIN, DIRECT, LOOP
         ["5530_RXPath"] = "DIRECT",
         -- Table of settings that are only relevant to calibration
         ["calibrationSettings"] =
         {
            -- IF bandwidth to use during calibration.
            ["IFBW"] = 1000,
            -- Output power at blind mate to use during the vector portion of calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            -- Source scalar calibration derives from absolute vector calibration
            ["vectorCal_Power"] =
            {
               10,  -- dBm at blind mate for the first port
               -5,  -- dBm at blind mate for the second port
            },
            -- Output power at blind mate to use during the power meter measurement of the absolute vector calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["vectorCal_Power_powerMeter"] =
            {
               10,  -- dBm at blind mate for the first port
               -5,  -- dBm at blind mate for the second port
            },
            -- Output power at blind mate to use during the source calibration portion of calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["scalarCal_Power_sourceCal"] =
            {
               0,  -- dBm at blind mate for the first port
               -5,  -- dBm at blind mate for the second port
            }
         }
      },
      {
         ["freq"] = 1.01*Giga,
      },
      {
         ["freq"] = 1.02*Giga,
      },
      {
         ["freq"] = 1.03*Giga,
      },
      {
         ["freq"] = 1.04*Giga,
      },
      {
         ["freq"] = 1.05*Giga,
      },
      {
         ["freq"] = 2*Giga,
      }
   }
};

-- Override receiver sweep with powers as arrays
ReceiverCalibrationSettings["sweep_receiver_overridePowerAsArray"] =
{
   ["type"] = "list",
   -- frequency list
   ["list"] =
   {
      {
         -- Frequency to calibrate with at this point
         ["freq"] = 1*Giga,
         -- Input reference level in dBm at blind mate (or DUT with deembedding enabled) used during calibration
         ["referenceLevel"] = 0,
         -- Reference level in dBm set directly to RFSA during a calibration. Not impacted by deembedding
         ["RFSAReferenceLevel"] = 0,
         -- Required parameter but has no physical meaning for receiver scalar calibration. Recommended to set to the same value as scalarCal_power_receiverCal
         -- The number of ports used with this sweep will need to match the number of entries in this table
         ["portPower"] =
         {
            0,  -- dBm at blind mate (or DUT with deembedding enabled) for the first port
            -5,  -- dBm at blind mate (or DUT with deembedding enabled) for the second port
         },
         -- Path through the port control module to use for all sources in the calibration. The valid values are GAIN, DIRECT, LOOP
         ["5530_TXPath"] = "GAIN",
         -- Path through the port control module to use for all receivers in the calibration. The valid values are GAIN, DIRECT, LOOP
         ["5530_RXPath"] = "DIRECT",
         -- Path through the port module to use for all receivers in the calibration. The valid values are COUPLED, DIRECT, LNA
         ["5530_CouplingPath"] = "DIRECT",
         -- Table of settings that are only relevant to calibration
         ["calibrationSettings"] =
         {
            -- IF bandwidth to use during calibration.
            ["IFBW"] = 1000,
            -- Output power at blind mate to use during the vector portion of calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["vectorCal_Power"] =
            {
               10,  -- dbm at blind mate for the first port
               -5,  -- dbm at blind mate for the second port
            },
            -- Output power at blind mate to use during the power meter measurement of the absolute vector calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["vectorCal_Power_powerMeter"] =
            {
               10,  -- dbm at blind mate for the first port
               -5,  -- dbm at blind mate for the second port
            },
            -- Output power at blind mate to use during the receiver calibration portion of calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["scalarCal_Power_receiverCal"] =
            {
               0,  -- dbm at blind mate for the first port
               -5,  -- dbm at blind mate for the second port
            },
         }
      },
      {
         ["freq"] = 1.01*Giga,
      },
      {
         ["freq"] = 1.02*Giga,
      },
      {
         ["freq"] = 1.03*Giga,
      },
      {
         ["freq"] = 1.04*Giga,
      },
      {
         ["freq"] = 1.05*Giga,
      },
      {
         ["freq"] = 2*Giga,
      }
   }
};

-- The sweep settings file allows a different 5530_TXPath, 5530_RXPath, and 5530_CouplingPath to be specified per port
-- This is useful for specifying very different power levels for each port in a calibration, or calibrating ports of different types

-- Example of receiver calibration settings for an NI5531 RF port and an NI5534_RX high power RF receive port
-- The NI5534_RX port only supports the COUPLED path for the 5530_CouplingPath, so the settings for each port
-- must be specified differently.
-- This example assumes the NI5531 port is the first port of the calibration
ReceiverCalibrationSettings["sweep_receiver_NI5531_and_NI5534RX"] =
{
   ["type"] = "list",
   -- frequency list
   ["list"] =
   {
      {
         -- Frequency to calibrate with at this point
         ["freq"] = 1*Giga,
         -- Input reference level in dBm at blind mate (or DUT with deembedding enabled) used during calibration
         ["referenceLevel"] = 0,
         -- Reference level in dBm set directly to RFSA during a calibration. Not impacted by deembedding
         ["RFSAReferenceLevel"] = 0,
         -- Required parameter but has no physical meaning for receiver scalar calibration. Recommended to set to the same value as scalarCal_power_receiverCal
         -- The number of ports used with this sweep will need to match the number of entries in this table
         ["portPower"] =
         {
            0,  -- dBm at blind mate (or DUT with deembedding enabled) for the first port
            -5,  -- dBm at blind mate (or DUT with deembedding enabled) for the second port
         },
         -- Path through the port control module to use for each source in the calibration. The valid values are GAIN, DIRECT, LOOP
         -- If specifying the path per port, the number of values here needs to match the port power above
         ["5530_TXPath"] =
         {
            "GAIN",  -- setting for the NI5531 port
            "GAIN"   -- setting for the NI5534_RX port
         },
         -- Path through the port control module to use for each receiver in the calibration. The valid values are GAIN, DIRECT, LOOP
         -- If specifying the path per port, the number of values here needs to match the port power above
         ["5530_RXPath"] =
         {
            "DIRECT",   -- setting for the NI5531 port
            "GAIN"      -- setting for the NI5534_RX port
         },
         -- Path through the port module to use for each receiver in the calibration. The valid values are COUPLED, DIRECT, LNA
         -- If specifying the path per port, the number of values here needs to match the port power above
         ["5530_CouplingPath"] =
         {
            "DIRECT",   -- setting for the NI5531 port
            "COUPLED"   -- For NI5534_RX ports COUPLED is the only valid value
         },
         -- Table of settings that are only relevant to calibration
         ["calibrationSettings"] =
         {
            -- IF bandwidth to use during calibration.
            ["IFBW"] = 1000,
            -- Output power at blind mate to use during the vector portion of calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["vectorCal_Power"] =
            {
               10,  -- dbm at blind mate for the first port
               -5,  -- dbm at blind mate for the second port
            },
            -- Output power at blind mate to use during the power meter measurement of the absolute vector calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["vectorCal_Power_powerMeter"] =
            {
               10,  -- dbm at blind mate for the first port
               -5,  -- dbm at blind mate for the second port
            },
            -- Output power at blind mate to use during the receiver calibration portion of calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["scalarCal_Power_receiverCal"] =
            {
               0,  -- dbm at blind mate for the first port
               -5,  -- dbm at blind mate for the second port
            },
         }
      },
      {
         ["freq"] = 2*Giga,
      }
   }
};

-- Example of source calibration settings for two NI5531 RF ports at very different port powers
SourceCalibrationSettings["sweep_source_NI5531_and_NI5531"] =
{
   ["type"] = "list",
   -- frequency list
   ["list"] =
   {
      {
         -- Frequency to calibrate with at this point
         ["freq"] = 1*Giga,
         -- Output power used during calibration
         -- The number of ports used with this sweep will need to match the number of entries in this table
         ["portPower"] =
         {
            -15,  -- dBm at blind mate (or DUT with deembedding enabled) for the first port
            -50  -- dBm at blind mate (or DUT with deembedding enabled) for the second port
         },
         -- Input reference level in dBm at blind mate (or DUT with deembedding enabled) used during calibration
         ["referenceLevel"] = 0,
         -- Reference level in dBm set directly to RFSA during a calibration. Not impacted by deembedding
         ["RFSAReferenceLevel"] = 0,
         -- Path through the port control module to use for each source port in the calibration. The valid values are GAIN, DIRECT, LOOP
         ["5530_TXPath"] =
         {
            "GAIN",
            "DIRECT" -- Because the second power is very low, use the DIRECT path
         },
         -- Path through the port control module to use for all receiver ports in the calibration. The valid values are GAIN, DIRECT, LOOP
         -- For parameters that are the same for every port, only one value needs to be specified. This value will be used for all ports in
         -- the measurement.
         ["5530_RXPath"] = "GAIN",

         -- Table of settings that are only relevant to calibration
         ["calibrationSettings"] =
         {
            -- IF bandwidth to use during calibration.
            ["IFBW"] = 1000,
            -- Output power at blind mate to use during the vector portion of calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            -- Source scalar calibration derives from absolute vector calibration
            ["vectorCal_Power"] =
            {
               -5,  -- dBm at blind mate for the first port
               -5,  -- dBm at blind mate for the second port
            },
            -- Output power at blind mate to use during the power meter measurement of the absolute vector calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["vectorCal_Power_powerMeter"] =
            {
               -5,  -- dBm at blind mate for the first port
               -5,  -- dBm at blind mate for the second port
            },
            -- Output power at blind mate to use during the source calibration portion of calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["scalarCal_Power_sourceCal"] =
            {
               -15,  -- dBm at blind mate for the first port
               -50,  -- dBm at blind mate for the second port
            }
         }
      },
      {
         ["freq"] = 2*Giga,
      }
   }
};

-- Example of source calibration settings for an NI5534_TX high power RF port
-- and an NI5531 RF port
-- The NI5534_TX port is the first port in this example
SourceCalibrationSettings["sweep_source_NI5534TX_and_NI5531"] =
{
   ["type"] = "list",
   -- frequency list
   ["list"] =
   {
      {
         -- Frequency to calibrate with at this point
         ["freq"] = 1*Giga,
         -- Output power used during calibration
         -- The number of ports used with this sweep will need to match the number of entries in this table
         ["portPower"] =
         {
            20,  -- dBm at blind mate (or DUT with deembedding enabled) for the first port
            10  -- dBm at blind mate (or DUT with deembedding enabled) for the second port
         },
         -- Input reference level in dBm at blind mate (or DUT with deembedding enabled) used during calibration
         ["referenceLevel"] = 10,
         -- Reference level in dBm set directly to RFSA during a calibration. Not impacted by deembedding
         ["RFSAReferenceLevel"] = 0,
         -- Path through the port control module to use for each source port in the calibration. The valid values are GAIN, DIRECT, LOOP
         ["5530_TXPath"] =
         {
            "DIRECT", -- Because the NI5534_TX port has a higher gain than the NI5531 port, use the DIRECT path for 5530_TXPath
            "GAIN"
         },
         -- Path through the port control module to use for each receiver port in the calibration. The valid values are GAIN, DIRECT, LOOP
         ["5530_RXPath"] =
         {
            "DIRECT",
            "DIRECT"
         },
         -- Table of settings that are only relevant to calibration
         ["calibrationSettings"] =
         {
            -- IF bandwidth to use during calibration.
            ["IFBW"] = 1000,
            -- Output power at blind mate to use during the vector portion of calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            -- Source scalar calibration derives from absolute vector calibration
            ["vectorCal_Power"] =
            {
               10,  -- dBm at blind mate for the first port
               10,  -- dBm at blind mate for the second port
            },
            -- Output power at blind mate to use during the power meter measurement of the absolute vector calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["vectorCal_Power_powerMeter"] =
            {
               10,  -- dBm at blind mate for the first port
               10,  -- dBm at blind mate for the second port
            },
            -- Output power at blind mate to use during the source calibration portion of calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["scalarCal_Power_sourceCal"] =
            {
               20,  -- dBm at blind mate for the first port
               10,  -- dBm at blind mate for the second port
            }
         }
      },
      {
         ["freq"] = 2*Giga,
      }
   }
};

-- This sweep assumes the NI5534_TX port is the first port and the NI5534_RX port is the second port
VectorSweepSettings["sweep_vector_NI5534TX_and_NI5534RX"] =
{
   ["type"] = "list",
   -- frequency list
   ["list"] =
   {
      {
         -- Frequency to calibrate with at this point
         ["freq"] = 1*Giga,
         -- Output power used during calibration
         -- The number of ports used with this sweep will need to match the number of entries in this table
         ["portPower"] =
         {
            20,  -- dBm at blind mate (or DUT with deembedding enabled) for the first port
            0,  -- dBm at blind mate (or DUT with deembedding enabled) for the second port
         },
         -- Input reference level in dBm at blind mate (or DUT with deembedding enabled) used during calibration
         ["referenceLevel"] = 20,
         -- Reference level in dBm set directly to RFSA during a calibration. Not impacted by deembedding
         ["RFSAReferenceLevel"] = 0,
         -- Path through the port control module to use for each source port in the calibration. The valid values are GAIN, DIRECT, LOOP
         ["5530_TXPath"] =
         {
            "DIRECT", -- Because the NI5534_TX port has a high gain, use the DIRECT path for 5530_TXPath
            "GAIN"
         },
         -- Path through the port control module to use for each receiver port in the calibration. The valid values are GAIN, DIRECT, LOOP
         ["5530_RXPath"] =
         {
            "DIRECT",
            "DIRECT"
         },
         -- IF bandwidth to use during measurement.
         ["IFBW"] = 1000,
         -- Table of settings that are only relevant to calibration
         ["calibrationSettings"] =
         {
            -- IF bandwidth to use during calibration.
            ["IFBW"] = 1000,
            -- Output power at blind mate to use during the vector portion of calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            -- Source scalar calibration derives from absolute vector calibration
            ["vectorCal_Power"] =
            {
               20,  -- dBm at blind mate for the first port
               0,  -- dBm at blind mate for the second port
            },
            -- Output power at blind mate to use during the power meter measurement of the absolute vector calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["vectorCal_Power_powerMeter"] =
            {
               20,  -- dBm at blind mate for the first port
               0,  -- dBm at blind mate for the second port
            },
            -- Output power at blind mate to use during the source calibration portion of calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["scalarCal_Power_sourceCal"] =
            {
               20,  -- dBm at blind mate for the first port
               0,  -- dBm at blind mate for the second port
            }
         }
      },
      {
         ["freq"] = 2*Giga,
      }
   }
};
VectorSweepSettings["test"] =
{
   ["type"] = "list",
   -- frequency list
   ["list"] =
   {
      {
         -- Frequency to calibrate with at this point
         ["freq"] = 1*Giga,
         -- Output power used during calibration
         -- The number of ports used with this sweep will need to match the number of entries in this table
         ["portPower"] =20,
         -- Input reference level in dBm at blind mate (or DUT with deembedding enabled) used during calibration
         ["referenceLevel"] = 20,
         -- Reference level in dBm set directly to RFSA during a calibration. Not impacted by deembedding
         ["RFSAReferenceLevel"] = 0,
         -- Path through the port control module to use for each source port in the calibration. The valid values are GAIN, DIRECT, LOOP
         ["5530_TXPath"] ="GAIN",
         -- Path through the port control module to use for each receiver port in the calibration. The valid values are GAIN, DIRECT, LOOP
         ["5530_RXPath"] ="DIRECT",
         -- IF bandwidth to use during measurement.
         ["IFBW"] = 1000,
         -- Table of settings that are only relevant to calibration
         ["calibrationSettings"] =
         {
            -- IF bandwidth to use during calibration.
            ["IFBW"] = 1000,
            -- Output power at blind mate to use during the vector portion of calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            -- Source scalar calibration derives from absolute vector calibration
            ["vectorCal_Power"] =30,
            -- Output power at blind mate to use during the power meter measurement of the absolute vector calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["vectorCal_Power_powerMeter"] =40,
            -- Output power at blind mate to use during the source calibration portion of calibration. Not impacted by deembedding
            -- If specifying the power per port, the number of values here needs to match portPower above
            ["scalarCal_Power_sourceCal"] =50,
         },
	  },
      {
         ["freq"] = 2.2*Giga,
      },
	  {
         ["freq"] = 2.03*Giga,
      },
	  {
         ["freq"] = 2.004*Giga,
      }
    }
};

-- Source sweep, calibrated in the center of each range
SourceCalibrationSettings["source_full_frequency_range"] =
{
   ["type"] = "list",
   -- frequency list
   ["list"] = 
   {
   }
}

source = SourceCalibrationSettings["source_full_frequency_range"]["list"]

local currentfreq = 375*Mega
local i = 1
while currentfreq <= 6000*Mega do
   source[i] =
   {
      ["freq"] = currentfreq,
      ["portPower"] = -10
   }
   currentfreq = currentfreq + 25*Mega
   i = i + 1
end

-- Receiver sweep, calibrated at the top of each range
ReceiverCalibrationSettings["receiver_full_frequency_stepped_referenceLevel"] =
{
   ["type"] = "list",
   -- frequency list
   ["list"] = 
   {
   }
}

receiver = ReceiverCalibrationSettings["receiver_full_frequency_stepped_referenceLevel"]["list"]

currentfreq = 375*Mega
i = 1
while currentfreq <= 6000*Mega do
   receiver[i] = 
   {
      ["freq"] = currentfreq,
      ["referenceLevel"] = 30
   }
   receiver[i+1] = 
   {
      ["freq"] = currentfreq,
      ["referenceLevel"] = 20
   }
   receiver[i+2] = 
   {
      ["freq"] = currentfreq,
      ["referenceLevel"] = 0
   }
   receiver[i+3] = 
   {
      ["freq"] = currentfreq,
      ["referenceLevel"] = -15
   }
   currentfreq = currentfreq + 25*Mega
   i = i + 4
end