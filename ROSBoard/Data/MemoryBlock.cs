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
using System.Collections.Generic;
using ROSCore.Data;

namespace ROSBoard.Data
{
    public class MemoryBlock
    {
    #region PORT MEMORY BLOCK

        /// <summary>
        /// 
        /// </summary>
        private List<PortMemoryBlock> allocatedPortMemoryBlocks;

        public class PortMemoryUpdateEvent
        {
            public static OnInDataDigitalUpdatedEvent  OnInDataDigitalUpdated;
            public static OnInDataAnalogUpdatedEvent   OnInDataAnalogUpdated;
            public static OnOutDataDigitalUpdatedEvent OnOutDataDigitalUpdated;
            public static OnOutDataAnalogUpdatedEvent  OnOutDataAnalogUpdated;

            public delegate void OnInDataDigitalUpdatedEvent(int portID, int data);
            public delegate void OnInDataAnalogUpdatedEvent(int portID, float data);
            public delegate void OnOutDataDigitalUpdatedEvent(int portID, int data);
            public delegate void OnOutDataAnalogUpdatedEvent(int portID, float data);
        }

        public MemoryBlock()
        {
            allocatedPortMemoryBlocks = new List<PortMemoryBlock>();
        }

        public void RegisterPortMemory(int portID)
        {
            PortMemoryBlock pmb = GetPortMemory(portID);
            if (pmb == null)
            {
                pmb = new PortMemoryBlock();
                pmb.PortID = portID;
                allocatedPortMemoryBlocks.Add(pmb);
            }
        }

        public PortMemoryBlock GetPortMemory(int portID)
        {
            return allocatedPortMemoryBlocks.Find(m => m.PortID.Equals(portID));
        }

    #endregion
    }
}