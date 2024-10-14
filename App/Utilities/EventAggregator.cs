
namespace TAC_COM.Utilities
{
    public class EventAggregator
    {
        private readonly Dictionary<Type, List<Delegate>> subscribers = [];
    
        public void Publish<TEvent>(TEvent eventMessage)
        {
            if (subscribers.ContainsKey(typeof(TEvent)))
            {
                foreach (var action in subscribers[typeof(TEvent)].OfType<Action<TEvent>>())
                {
                    action(eventMessage);
                }
            }
        }

        public void Subscribe<TEvent>(Action<TEvent> action)
        {
            if (!subscribers.ContainsKey(typeof(TEvent)))
            {
                subscribers[typeof(TEvent)] = [];
            }
            subscribers[typeof(TEvent)].Add(action);
        }
    }

    public class ChangeNotifyIconMessage(string iconPath, string tooltip)
    {
        public string IconPath { get; set; } = iconPath;
        public string Tooltip { get; set; } = tooltip;
    }

    public class SetActiveProfileIconMessage(System.Windows.Media.ImageSource icon)
    {
        public System.Windows.Media.ImageSource Icon { get; set; } = icon;
    }

}
