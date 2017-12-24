using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EudiFramework
{
    internal class ReflectionUtility
    {
        public static bool IsOverride(MethodInfo m)
        {
            if (m == null)
                return false;
            if (m.GetBaseDefinition()?.DeclaringType == null)
                return false;
            if (m.DeclaringType == null)
                return false;

            return m.GetBaseDefinition().DeclaringType != m.DeclaringType;
        }

        public static bool IsOverride(object obj, string str)
        {
            if (obj == null)
            {
                return false;
            }

            return IsOverride(obj.GetType().GetMethod(str));
        }
    }
}
