using System;
using UnityEngine;
using WhereIsMyWife.Managers;
using WhereIsMyWife.Managers.Properties;
using WhereIsMyWife.Player.StateMachine;

namespace WhereIsMyWife.Player.State
{
    public class PlayerState 
    {
        protected PlayerState(PlayerStateMachine.PlayerState stateKey)
        {
            StateKey = stateKey;
            NextState = stateKey;
        }
        
        protected IPlayerStateInput _playerStateInput => PlayerManager.Instance.PlayerStateInput;
        protected IPlayerStateIndicator _playerStateIndicator => PlayerManager.Instance.PlayerStateIndicator;
        
        protected IPlayerProperties _properties => PlayerManager.Instance.Properties;
        
        protected virtual void SubscribeToObservables()
        {
            throw new NotImplementedException();
        }

        protected virtual void UnsubscribeToObservables()
        {
            throw new NotImplementedException();
        }

        public virtual void EnterState()
        {
            NextState = StateKey;
            Debug.Log($"Entering {StateKey}");
            SubscribeToObservables();
        }

        public virtual void ExitState()
        {
            Debug.Log($"Exiting {StateKey}");
            UnsubscribeToObservables();
        }

        public virtual void UpdateState() { }

        public virtual void FixedUpdateState() { }
        public PlayerStateMachine.PlayerState NextState { get; protected set; }
        public PlayerStateMachine.PlayerState StateKey { get; private set; }
        public virtual void OnTriggerEnter2D(Collider2D other) { }
        public virtual void OnTriggerStay2D(Collider2D other) { }
        public virtual void OnTriggerExit2D(Collider2D other) { }
    }

}