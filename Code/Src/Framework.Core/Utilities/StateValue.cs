using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Utilities
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StateValue : Attribute
    {
        public long Value { get; private set; }
        public string Text { get; private set; }
        public StateValue(long value, string text)
        {
            Value = value;
            Text = text;
        }
    }
}
