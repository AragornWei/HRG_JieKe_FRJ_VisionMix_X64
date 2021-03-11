using System;
using System.Reflection;

namespace Yoga.Tools
{
    public struct ToolReflection
    {
        public Assembly Assembly;
        public Type Type;
        public ToolReflection(Assembly assembly, Type type)
        {
            Assembly = assembly;
            Type = type;
        }
    }
}
