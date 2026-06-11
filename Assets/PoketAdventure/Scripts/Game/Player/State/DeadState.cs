using CloudeDev.Game.StateMachine;

namespace CloudeDev.Game.Player.State
{
    public class DeadState : PlayerStateBase
    {
        public DeadState(IPlayerStats statsModel, IStateSwitcher stateSwitcher, IPlayerView playerView) :
            base(statsModel, stateSwitcher, playerView)
        {
        }

        public override void Enter()
        {
            _playerView.PlayAnimation("Die");
        }

        public override void Exit()
        {
           
        }

        public override void Update()
        {
           
        }
    }
}
