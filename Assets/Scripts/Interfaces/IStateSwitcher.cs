using MobileRpg.States;

namespace MobileRpg.Interfaces
{
    public interface IStateSwitcher
    {
        void SwitchState<T>() where T : BaseState;
    }
}