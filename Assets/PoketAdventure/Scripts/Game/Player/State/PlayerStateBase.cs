using CloudeDev.Game.StateMachine;

namespace CloudeDev.Game.Player.State
{
    public abstract class PlayerStateBase : IState
    {
        protected IPlayerStats _statsModel;
        protected IPlayerView _playerView;
        protected IStateSwitcher _stateSwitcher; 

        protected PlayerStateBase(IPlayerStats statsModel, IStateSwitcher stateSwitcher, IPlayerView playerView)
        {
            _statsModel = statsModel;
            _stateSwitcher = stateSwitcher;        
            _playerView = playerView;
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
