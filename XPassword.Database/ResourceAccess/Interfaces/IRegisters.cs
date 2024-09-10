using XPassword.Database.Model;

namespace XPassword.Database.ResourceAccess.Interfaces;

internal interface IRegisters : IDisposable
{
    int InsertRegisters(List<Register> registers, long userId);
}