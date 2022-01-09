using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Framework.Core.Utilities
{
    public class StateFactory<T> where T : class
    {
        private List<T> _states = new List<T>();
        public StateFactory(List<T> states)
        {
            states.ForEach(state => _states.Add(state));
        }
        public T Create(long state)
        {
            return _states.First(a => a.GetType()
                .GetCustomAttributes(typeof(StateValue), true)
                .OfType<StateValue>().First().Value == state);
        }

        public long Create(T state)
        {
            return state.GetType().GetCustomAttributes<StateValue>().First().Value;
        }
    }
}
