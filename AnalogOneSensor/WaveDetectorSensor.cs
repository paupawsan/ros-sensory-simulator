using System;
using System.Reflection;
using ROSCore.Data;
using ROSCore.System;

namespace AnalogOneSensor
{
    public class WaveDetectorSensor : IDeviceStatus
    {
        public void Start(PortMemoryBlock allocatedMemory)
        {
            Console.WriteLine("_pLog_ {0} [{1}@{2}] {3}", DateTime.UtcNow.Ticks, this.GetType(),
                              MethodBase.GetCurrentMethod().ToString(), string.Format("{0}", "Starting"));
        }

        public void Shutdown()
        {
            Console.WriteLine("_pLog_ {0} [{1}@{2}] {3}", DateTime.UtcNow.Ticks, this.GetType(),
                              MethodBase.GetCurrentMethod().ToString(), string.Format("{0}", "Shutdown"));
        }

        public void OnInternalClockUpdate(ulong counter)
        {
            Console.WriteLine("_pLog_ {0} [{1}@{2}] {3}", DateTime.UtcNow.Ticks, this.GetType(),
                              MethodBase.GetCurrentMethod().ToString(), string.Format("Clock counter:{0}", counter));
        }
    }
}