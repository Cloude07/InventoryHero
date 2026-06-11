using CloudeDev.Game.Player;

namespace CloudeDev.Game.StateMachine
{
    public interface IState 
    {
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();

    }

}
