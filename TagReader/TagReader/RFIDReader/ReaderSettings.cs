
using System;
using Org.LLRP.LTK.LLRPV1.Impinj;
using TagReader.Properties;

namespace TagReader.RFIDReader
{
    public class ReaderSettings
    {
        public string Ip { get; set; }
        public double TransmitPower { get; set; }
        public ushort ModeIndex { get; set; }
        public ushort TagPopulation { get; set; }
        public ushort TagTransitTime { get; set; }
        public bool[] AntennaId { get; set; }
        public ushort Tari { get; set; }
        public ushort ReaderSensitivity { get; set; }
        public ushort ChannelIndex { get; set; }
        public ushort HopTableIndex { get; set; }
        public ushort PeriodicTriggerValue { get; set; }
        public ushort Duration { get; set; }
        public ENUM_ImpinjInventorySearchType SearchType { get; set; }

        public ReaderSettings()
        {
           Ip = "169.254.41.39"; // default: 192.168.1.2
            
            /*
            ** Set Channel Index, 16 in total, which represents different frequency (MHz). Namely,
            ** [1: 920.375]
            ** [2: 920.875]
            ** ...... 
            ** [16: 924.375]
            */
            ChannelIndex = 1;
            // Power(dbm) [10 : 0.25 : 32.5], 90 different values in total
            TransmitPower = 25;
            /*
            ** ModeIndex  NAME                  SENSITIVITY     INTERFERENCE TOLERANCE
            **   0        Max Throughput        good            poor
            **   1        Hybrid                good            good
            **   2        Dense Reader (M=4)    better          excellent
            **   3        Dense Reader (M=8)    best            excellent
            **   4*       MaxMiller             better          good
            **   1000     Auto set Dense Reader
            **   1001     Auto set Single Reader
            **   * MaxMiller is not available in all regions
            */
            ModeIndex = 1000;
            HopTableIndex = 1;          // ?
            PeriodicTriggerValue = 0;   // ?
            TagPopulation = 2;
            Tari = 10;
            TagTransitTime = 0;         // ?
            ReaderSensitivity = 1;
            // each value in the array represents Antenna 1, Antenna 2, Antenna 3, Antenna 4, respectively.
            AntennaId = new[] { true, false, false, false };
            Duration = 1; // transmission duration, (s)
            SearchType = ENUM_ImpinjInventorySearchType.Dual_Target;
        } // end Reader constructor

        public static double Channal2Frequency(ushort channal)
        {
            return 920.375 + channal * 0.25;
        }

        public static ushort Frequency2Channal(double frequency)
        {
            return Convert.ToUInt16((frequency - 920.375) / 0.25);
        }
    } // end Reader
} // end namespace
