using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Framework.Core.Utilities
{
    public static class StateFactory<T>
    {
        static StateFactory()
        {
            States.Clear();
        }
        private static readonly List<T> States = new();

        public static T Create(long state)
        {
            return States.First(a => a.GetType()
                .GetCustomAttributes(typeof(StateValue), true)
                .OfType<StateValue>().First().Value == state);
        }

        public static long Create(T state)
        {
            return state.GetType().GetCustomAttributes<StateValue>().First().Value;
        }

        public static void AddStates(params T[] states)
        {
            States.AddRange(states);
        }
        public static void ClearStates()
        {
            States.Clear();
        }
    }
}
