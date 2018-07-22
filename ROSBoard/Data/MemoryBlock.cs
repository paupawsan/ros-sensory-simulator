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