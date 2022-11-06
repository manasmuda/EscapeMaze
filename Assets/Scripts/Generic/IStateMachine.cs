using System;

namespace EscapeMaze.StateMachine {


    public interface IStateMachine<TState, TEnterData> where TState : Enum {

        public TState CurrentState { get; }

        public void EnterState(TState t, TEnterData data);

        public void SetState(TState t);

        public void Exit();
    }
}
