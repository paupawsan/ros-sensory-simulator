using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ROSBoard.Data;
using ROSCore.System;

namespace ROSBoard
{
    public class SystemBoard
    {
        private const int                 CLOCK_TIMEOUT           = 1000;
        private const int                 SCAN_NEW_DEVICE_TIMEOUT = 10000;
        private       MemoryBlock         memoryBlock;
        private       List<IDeviceStatus> registeredDevices;
        private       List<Type>          registeredDevicesType;

        private bool isStarting     = false;
        private bool isShuttingDown = false;

        private bool  isScanNewDevices = false;
        public  ulong CurrentClockCounter { get; private set; } = 0;
        public  ulong internalCounter           = 0;
        public  ulong internalDeviceScanCounter = 0;

        private void InitMe()
        {
            memoryBlock = new MemoryBlock();
            registeredDevices = new List<IDeviceStatus>();
            registeredDevicesType = new List<Type>();
            CurrentClockCounter = 0;
            internalDeviceScanCounter = 0;
            internalCounter = 0;
        }

        public IEnumerator Start()
        {
            //Quit if it is starting
            if (isStarting) yield break;

            isStarting = true;
            isShuttingDown = false;
            InitMe();

            while (!isShuttingDown)
            {
                if (isScanNewDevices)
                {
                    isScanNewDevices = false;
                    GetDevices();
                }

                UpdateInternalClock();
                yield return null;
            }

            ShutdownDevices();
        }

        private void ShutdownDevices()
        {
            for (int i = 0; i < registeredDevices.Count; i++)
            {
                registeredDevices[i].Shutdown();
            }
        }

        public void Shutdown()
        {
            isShuttingDown = true;
        }

        public void ScanNewDevices()
        {
            isScanNewDevices = true;
        }

        private void UpdateInternalClock()
        {
            if (internalDeviceScanCounter % SCAN_NEW_DEVICE_TIMEOUT == 0)
            {
                isScanNewDevices = true;
                internalDeviceScanCounter = 0;
            }

            if (internalCounter % CLOCK_TIMEOUT == 0)
            {
                internalCounter = 0;
                for (int i = 0; i < registeredDevices.Count; i++)
                {
                    registeredDevices[i].OnInternalClockUpdate(CurrentClockCounter);
                }

                CurrentClockCounter++;
            }

            internalCounter++;
            internalDeviceScanCounter++;
        }

        private void GetDevices()
        {
            string workDir = Path.GetDirectoryName(System.Environment.CommandLine);
            string targetLibDir = workDir + Path.DirectorySeparatorChar + "sensors";
            Console.WriteLine(workDir);
            Console.WriteLine(targetLibDir);
            if (!Directory.Exists(targetLibDir))
            {
                return;
            }

            string[] files = Directory.GetFiles(targetLibDir, "*.dll");
            foreach (var file in files)
            {
                var DLL =
                    Assembly.LoadFile(file);
                foreach (Type type in DLL.GetExportedTypes())
                {
                    if (!registeredDevicesType.Contains(type))
                    {
                        IDeviceStatus c = Activator.CreateInstance(type) as IDeviceStatus;
                        if (c != null)
                        {
                            memoryBlock.RegisterPortMemory(1);
                            c.Start(memoryBlock.GetPortMemory(1));
                            registeredDevices.Add(c);
                            registeredDevicesType.Add(type);
                            Console.WriteLine("Lib " + file);
                            break;
                        }
                    }
                }
            }
        }
    }
}