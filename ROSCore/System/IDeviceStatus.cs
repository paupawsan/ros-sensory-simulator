using ROSCore.Data;

namespace ROSCore.System
{
    public interface IDeviceStatus
    {
        void Start(PortMemoryBlock allocatedMemory);
        void Shutdown();
        void OnInternalClockUpdate(ulong counter);
    }
}