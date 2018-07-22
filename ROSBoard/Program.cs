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
using System.IO;
using ROSBoard.Data;
using ROSCore.Data;

namespace ROSBoard
{
    class Program
    {
        private const int SHUTDOWN_TIME = 1000000;
        private static SystemBoard m_systemBoard;
        static void Main(string[] args)
        {
            m_systemBoard = new SystemBoard();
            
            //Get program loop to simulate system board is turned on
            IEnumerator programLoop = m_systemBoard.Start();

            //Application level new devices listeners
            m_systemBoard.OnNewDeviceRegistered += OnNewDeviceRegistered;
            MemoryBlock.PortMemoryUpdateEvent.OnInDataAnalogUpdated += OnInDataAnalogUpdated;
            MemoryBlock.PortMemoryUpdateEvent.OnInDataDigitalUpdated += OnInDataDigitalUpdated;
            while (programLoop.MoveNext())
            {
                //Simulate system shutdown after SHUTDOWN_TIME clock reached
                if (m_systemBoard.CurrentClockCounter > SHUTDOWN_TIME)
                {
                    m_systemBoard.Shutdown();
                }
            }
            Console.WriteLine("Bye!");
        }

        static void OnNewDeviceRegistered(int portID)
        {
            Console.WriteLine("PortID" + portID + " registered!");
        }

        static void OnInDataAnalogUpdated(int portID, float data)
        {
            Console.WriteLine(string.Format("Received analogData: portID:{0} data:{1} AnalogIn", portID, data));
        }
        
        static void OnInDataDigitalUpdated(int portID, int data)
        {
            Console.WriteLine(string.Format("Received digitalData: portID:{0} data:{1} DigitalIn", portID, data));
        }
    }
}