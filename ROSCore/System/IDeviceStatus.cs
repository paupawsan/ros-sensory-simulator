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
using ROSCore.Data;

namespace ROSCore.System
{
    public interface IDeviceStatus
    {
        /// <summary>
        /// Will be virtually initiated by system board during startup process
        /// </summary>
        /// <param name="allocatedMemory"></param>
        void Start(PortMemoryBlock allocatedMemory);
        
        /// <summary>
        /// Will be virtually call by the system board during shutdown process
        /// </summary>
        void Shutdown();
        
        /// <summary>
        /// This will be virtually called by System Board.
        /// </summary>
        /// <param name="counter"></param>
        void OnInternalClockUpdate(ulong counter);
    }
}