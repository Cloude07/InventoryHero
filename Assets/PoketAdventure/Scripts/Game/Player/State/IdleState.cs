using CloudeDev.Game.StateMachine;

namespace CloudeDev.Game.Player.State
{
    public class IdleState : PlayerStateBase
    {
        public IdleState(IPlayerStats statsModel,
                     IStateSwitcher stateSwitcher, IPlayerView playerView)
       : base(statsModel,  stateSwitcher, playerView) { }

        public override void Enter()
        {
            _playerView.PlayAnimation("Idle");
        }

        public override void Exit() { }

        public override void Update()
        {

        }


    }
}
