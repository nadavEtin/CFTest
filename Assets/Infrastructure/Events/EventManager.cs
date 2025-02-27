using System;
using System.Collections.Generic;

namespace Assets.Infrastructure.Events
{
    public enum TypeOfEvent
    {
        //game flow events
        GameStart,
        GameOver,
        ReplayLevel,
        ReturnToMainMenu,
        DifficultySelection,

        //gameplay events
        SpawnNormalBalls,
        SpawnSpecialBall,
        BallClick,
        MissedMove,
        TimeUpdate,
        ScoreUpdate,
        ScoreTargetReached
    }

    public class EventManager
    {
        public static readonly EventManager Instance = new EventManager();
        private readonly Dictionary<TypeOfEvent, List<Action<BaseEventParams>>> _subscription = new();

        //register method as a listener to specific event
        public void Subscribe(TypeOfEvent eventType, Action<BaseEventParams> handler)
        {
            if (_subscription.ContainsKey(eventType) == false)
                _subscription.Add(eventType, new List<Action<BaseEventParams>>());

            var handlerList = _subscription[eventType];
            if (handlerList.Contains(handler) == false)
                handlerList.Add(handler);
        }

        //remove method as listener
        public void Unsubscribe(TypeOfEvent eventType, Action<BaseEventParams> handler)
        {
            if (_subscription.ContainsKey(eventType))
                _subscription[eventType]?.Remove(handler);
        }

        //message all listeners
        public void Publish(TypeOfEvent eventType, BaseEventParams eventParams)
        {
            if (_subscription.ContainsKey(eventType) == false)
                return;

            var handlerList = _subscription[eventType];
            foreach (var handler in handlerList)
                handler?.Invoke(eventParams);
        }
    }
}
