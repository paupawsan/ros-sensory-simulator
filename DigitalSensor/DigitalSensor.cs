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
using System.Reflection;
using ROSCore.Data;
using ROSCore.System;

namespace DigitalSensor
{
    public class DigitalSensor : IDeviceStatus
    {
        const int DATA_UPDATE_FREQUENCY = 12000;
        private PortMemoryBlock allocatedPortMemory;
        
        /// <summary>
        /// Will be virtually initiated by system board
        /// </summary>
        /// <param name="allocatedMemory"></param>
        public void Start(PortMemoryBlock allocatedMemory)
        {
            Console.WriteLine("_pLog_ {0} [{1}@{2}] {3}", DateTime.UtcNow.Ticks, this.GetType(),
                              MethodBase.GetCurrentMethod().ToString(), string.Format("{0}", "Starting"));
            allocatedPortMemory = allocatedMemory;
        }

        public void Shutdown()
        {
            Console.WriteLine("_pLog_ {0} [{1}@{2}] {3}", DateTime.UtcNow.Ticks, this.GetType(),
                              MethodBase.GetCurrentMethod().ToString(), string.Format("{0}", "Shutdown"));
        }
        
        /// <summary>
        /// This will be virtually called by System Board.
        /// </summary>
        /// <param name="counter"></param>
        public void OnInternalClockUpdate(ulong counter)
        {
            // Console.WriteLine("_pLog_ {0} [{1}@{2}] {3}", DateTime.UtcNow.Ticks, this.GetType(),
            //                   MethodBase.GetCurrentMethod().ToString(), string.Format("Clock counter:{0}", counter));
            if (counter % DATA_UPDATE_FREQUENCY == 0)
            {
                Random rnd = new System.Random();
                allocatedPortMemory.DigitalIn = rnd.Next(0, Int32.MaxValue);
                allocatedPortMemory.DirtyTypes[(int) PortMemoryBlock.DirtyTypeEnum.DigitalIn] = true;
                Console.WriteLine(string.Format("DigitalSensor WriteData: portID:{0} data:{1} of DigitalIn", allocatedPortMemory.PortID, allocatedPortMemory.DigitalIn));
            }
        }
    }
}