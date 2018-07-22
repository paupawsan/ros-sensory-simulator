using System;
using System.Collections;
using System.IO;

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
    }
}