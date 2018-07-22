/*******************************************************************************
Author:
      Paulus Ery Wasito Adhi <paupawsan@gmail.com>

Copyright (c) 2018

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ROSBoard.Data;
using ROSCore.Data;
using ROSCore.System;

namespace ROSBoard
{
    public class SystemBoard
    {
        private const int                 CLOCK_TIMEOUT           = 1000;
        private const int                 SCAN_NEW_DEVICE_TIMEOUT = 100000000;
        public        MemoryBlock         ActiveMemoryBlock { get; private set; }
        private       List<IDeviceStatus> registeredDevices;
        private       List<Type>          registeredDevicesType;

        private bool isStarting     = false;
        private bool isShuttingDown = false;
        private bool isRunning = false;

        private bool  isScanNewDevices = false;
        public  ulong CurrentClockCounter { get; private set; } = 0;
        public  ulong internalCounter           = 0;
        public  ulong internalDeviceScanCounter = 0;


        public OnNewDeviceRegisteredEvent OnNewDeviceRegistered;

        public delegate void OnNewDeviceRegisteredEvent(int portID);

        private void InitMe()
        {
            ActiveMemoryBlock = new MemoryBlock();
            registeredDevices = new List<IDeviceStatus>();
            registeredDevicesType = new List<Type>();
            CurrentClockCounter = 0;
            internalDeviceScanCounter = 0;
            internalCounter = 0;
            isScanNewDevices = false;
            isStarting = false;
            isShuttingDown = false;
            isRunning = false;
        }

        private void CleanUp()
        {
            ActiveMemoryBlock = null;
            registeredDevices.Clear();
            registeredDevices = null;
            registeredDevicesType.Clear();
            registeredDevicesType = null;
            internalCounter = 0;
            internalDeviceScanCounter = 0;
            isScanNewDevices = false;
            isStarting = false;
            isShuttingDown = false;
            isRunning = false;
        }

        public IEnumerator Start()
        {
            //Quit if it is starting
            if (isStarting) yield break;

            isStarting = true;
            isShuttingDown = false;
            InitMe();
            isRunning = true;

            while (!isShuttingDown)
            {
                if (isScanNewDevices)
                {
                    isScanNewDevices = false;
                    GetDevices();
                }

                UpdateInternalClock();
                
                UpdateMemoryBlock();
                yield return null;
            }

            ShutdownDevices();
            CleanUp();
        }

        private void ShutdownDevices()
        {
            for (int i = 0; i < registeredDevices.Count; i++)
            {
                registeredDevices[i].Shutdown();
                registeredDevices[i] = null;
                registeredDevicesType[i] = null;
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
            // Console.WriteLine(workDir);
            Console.WriteLine("Scanning new devices in " + targetLibDir);
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
                            int availablePort = registeredDevices.Count + 1;
                            ActiveMemoryBlock.RegisterPortMemory(availablePort);
                            c.Start(ActiveMemoryBlock.GetPortMemory(availablePort));
                            registeredDevices.Add(c);
                            registeredDevicesType.Add(type);
                            Console.WriteLine("Lib " + file);
                            OnNewDeviceRegistered.Invoke(availablePort);
                            break;
                        }
                    }
                }
            }
        }

        private void UpdateMemoryBlock()
        {
            //PortMemory
            for (int i = 0; i < registeredDevices.Count; i++)
            {
                PortMemoryBlock pmb = ActiveMemoryBlock.GetPortMemory(i + 1);
                if (pmb.DirtyTypes[(int) PortMemoryBlock.DirtyTypeEnum.AnalogIn])
                {
                    MemoryBlock.PortMemoryUpdateEvent.OnInDataAnalogUpdated.Invoke(i + 1, pmb.AnalogIn);
                    pmb.DirtyTypes[(int) PortMemoryBlock.DirtyTypeEnum.AnalogIn] = false;
                }
                
                if (pmb.DirtyTypes[(int) PortMemoryBlock.DirtyTypeEnum.AnalogOut])
                {
                    MemoryBlock.PortMemoryUpdateEvent.OnOutDataAnalogUpdated.Invoke(i + 1, pmb.AnalogOut);
                    pmb.DirtyTypes[(int) PortMemoryBlock.DirtyTypeEnum.AnalogOut] = false;
                }

                if (pmb.DirtyTypes[(int) PortMemoryBlock.DirtyTypeEnum.DigitalIn])
                {
                    MemoryBlock.PortMemoryUpdateEvent.OnInDataDigitalUpdated.Invoke(i + 1, pmb.DigitalIn);
                    pmb.DirtyTypes[(int) PortMemoryBlock.DirtyTypeEnum.DigitalIn] = false;
                }
                
                if (pmb.DirtyTypes[(int) PortMemoryBlock.DirtyTypeEnum.DigitalOut])
                {
                    MemoryBlock.PortMemoryUpdateEvent.OnOutDataDigitalUpdated.Invoke(i + 1, pmb.DigitalOut);
                    pmb.DirtyTypes[(int) PortMemoryBlock.DirtyTypeEnum.DigitalOut] = false;
                }
                
            }
            
        }
    }
}