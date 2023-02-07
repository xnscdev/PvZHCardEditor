using System.Collections.Generic;

namespace PvZHCardEditor
{
    internal class TreeViewCompoundNode
    {
        public string Name { get; }
        public IEnumerable<object> Children { get; }

        public TreeViewCompoundNode(string name, IEnumerable<object> children)
        {
            Name = name;
            Children = children;
        }
    }
}
