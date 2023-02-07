using System.Collections.Generic;
using System.Linq;

namespace PvZHCardEditor
{
    public class TreeViewNode
    {
        public string Text { get; set; }
        public bool Expanded { get; set; }

        public TreeViewNode(string text)
        {
            Text = text;
        }
    }

    public class TreeViewCompoundNode : TreeViewNode
    {
        public IEnumerable<TreeViewNode> Children { get; set; }

        public TreeViewCompoundNode(string text, IEnumerable<TreeViewNode> children) : base(text)
        {
            Children = children;
        }

        public TreeViewNode this[string text]
        {
            get => Children.First(node => node.Text == text);
        }
    }
}
