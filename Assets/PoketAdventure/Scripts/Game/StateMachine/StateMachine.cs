using CloudeDev.Game.StateMachine;
using System.Collections.Generic;
using System.Linq;

namespace CloudeDev.GameStateMachine
{
    public class StateMachine : IStateSwitcher
    {
        private List<IState> _states;
        private IState _currentState;
        
        public StateMachine()
        {
            _states = new List<IState>()
            {

            };
            _currentState = _states[0];
            _currentState.Enter();
        }

        public void SwitchState<T>() where T : IState
        {
            var state = _states.FirstOrDefault(state => state is T);
            _currentState.Exit();
            _currentState = state;
            _currentState?.Enter();
        }
    }
}
