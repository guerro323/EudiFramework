using System;

namespace EudiFramework
{
    public class InjectTuplesAttribute : Attribute
    {
        public bool InjectOnChilds;

        public InjectTuplesAttribute(bool injectOnChilds = false)
        {
            InjectOnChilds = injectOnChilds;
        }

        public bool Equals(InjectTuplesAttribute other) => other.InjectOnChilds == InjectOnChilds;

        public override bool Equals(object obj)
        {
            var attr = obj as InjectTuplesAttribute;
            return attr != null && Equals(attr);
        }
    }
}