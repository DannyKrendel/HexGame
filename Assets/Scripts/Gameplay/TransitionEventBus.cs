using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using HexGame.Utils;
using PolyternityStuff.Utils;

namespace HexGame.Gameplay
{
    public class TransitionEventBus
    {
        private readonly Dictionary<EventType, ActionAsync> _events;

        public TransitionEventBus()
        {
            _events = new Dictionary<EventType, ActionAsync>();
        }
        
        public void Subscribe(EventType eventType, ActionAsync listener)
        {
            if (_events.ContainsKey(eventType))
                _events[eventType] += listener;
            else
                _events.Add(eventType, listener);
        }
        
        public void Unsubscribe(EventType eventType, ActionAsync listener)
        {
            if (_events.ContainsKey(eventType))
                _events[eventType] -= listener;
            else
                _events.Remove(eventType);
        }

        public async UniTask Raise(EventType eventType, CancellationToken cancellationToken = default)
        {
            if (_events.ContainsKey(eventType))
                await _events[eventType](cancellationToken);
        }
        
        public enum EventType { BeforeAction, AfterAction }
    }
}