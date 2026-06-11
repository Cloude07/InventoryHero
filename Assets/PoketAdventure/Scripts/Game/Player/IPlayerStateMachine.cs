using CloudeDev.Game.StateMachine;

namespace CloudeDev.Game.Player
{
    public interface IPlayerStateMachine
    {
        bool CurrentStateIs<T>() where T : IState;
        IState GetCurrentState();
        void Update();
    }
}