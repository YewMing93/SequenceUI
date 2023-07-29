using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySeq.Common
{
    public static class EventHandler
    {
        private static readonly IEventAggregator _eventAgg = new EventAggregator();

        public static void SubEvent<TEvent>(Action eventAction) where TEvent : PubSubEvent, new()
        {
            _eventAgg.GetEvent<TEvent>().Subscribe(eventAction);
        }


        public static void UnSubEvent<TEvent>(Action eventAction) where TEvent : PubSubEvent, new()
        {
            _eventAgg.GetEvent<TEvent>().Unsubscribe(eventAction);
        }


        public static void SubEvent<TEvent, TEventParam>(Action<TEventParam> eventAction) where TEvent : PubSubEvent<TEventParam>, new()
        {
            _eventAgg.GetEvent<TEvent>().Subscribe(eventAction);
        }

        public static void UnSubEvent<TEvent, TEventParam>(Action<TEventParam> eventAction) where TEvent : PubSubEvent<TEventParam>, new()
        {
            _eventAgg.GetEvent<TEvent>().Unsubscribe(eventAction);
        }
    }
}
