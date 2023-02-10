using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace PvZHCardEditor
{
    public class ComponentNode : ViewModelBase
    {
        private readonly string _key;
        private bool _expanded;
        private ComponentNode? _parent;
        private ComponentValue? _value;

        public bool Expanded
        {
            get => _expanded;
            set => SetProperty(ref _expanded, value);
        }

        public ComponentNode? Parent
        {
            get => _parent;
            set => SetProperty(ref _parent, value);
        }

        public ComponentValue? Value
        {
            get => _value;
            set => SetProperty(ref _value, value, null);
        }

        public JToken Token { get; private set; }
        public string Text => _value is not null && _value.Simple ? $"{_key} = {_value.SimpleText}" : _key;
        public ComponentCollection<ComponentNode> Children => _value is null || _value.Simple ? 
            new ComponentCollection<ComponentNode>(Array.Empty<ComponentNode>()) : _value.Children;

        public ComponentNode(string key, ComponentValue value)
        {
            _key = key;
            _value = value;
            Token = value.Token;
            foreach (var child in value.Children)
            {
                child.Parent = this;
            }
            value.PropertyChanged += Value_PropertyChanged;
        }

        public ComponentNode(string key, JToken token)
        {
            _key = key;
            Token = token;
        }

        public void Edit(ComponentValue value)
        {
            Value = value;
            Token.Replace(value.Token);
            if (Token.Parent is null)
                Token = value.Token;
            foreach (var child in value.Children)
            {
                child.Parent = this;
            }
        }

        public static Type? ParseComponentType(string s)
        {
            var match = Regex.Match(s, "^PvZCards\\.Engine\\.([a-zA-Z0-9_.]+),");
            return Type.GetType($"PvZHCardEditor.{match.Groups[1].Value}");
        }

        public static ComponentNode? ParseComponent(JToken token)
        {
            if (token.Type == JTokenType.Null)
                return null;

            var type = ParseComponentType((string)token["$type"]!);
            if (type is null)
                return null;

            return (ComponentNode?)Activator.CreateInstance(type, token["$data"]!);
        }

        private void Value_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateProperty(nameof(Text));
            UpdateProperty(nameof(Children));
        }
    }

    public abstract class ComponentValue : ViewModelBase
    {
        public virtual bool Simple => false;
        public virtual string SimpleText => "";
        public virtual ComponentCollection<ComponentNode> Children => new(Array.Empty<ComponentNode>());
        public JToken Token { get; }

        public ComponentValue(JToken token)
        {
            Token = token;
        }
    }

    public class ComponentInt : ComponentValue
    {
        private readonly int _value;

        public override bool Simple => true;
        public override string SimpleText => _value.ToString();

        public ComponentInt(JToken token) : base(token)
        {
            _value = (int)token;
        }
    }

    public class ComponentString : ComponentValue
    {
        private readonly string _value;

        public override bool Simple => true;
        public override string SimpleText => _value;

        public ComponentString(JToken token) : base(token)
        {
            _value = (string)token!;
        }
    }

    public class ComponentObject : ComponentValue
    {
        private readonly ComponentCollection<ComponentNode> _properties;

        public override ComponentCollection<ComponentNode> Children => _properties;

        public ComponentObject(JToken token, ComponentCollection<ComponentNode> properties) : base(token)
        {
            _properties = properties;
            foreach (var p in properties)
            {
                p.PropertyChanged += ChildPropertyChanged;
            }
        }

        private void ChildPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateProperty(nameof(Children));
        }
    }

    public class ComponentArray : ComponentValue
    {
        private readonly ComponentCollection<ComponentNode> _elements;

        public override ComponentCollection<ComponentNode> Children => _elements;

        public ComponentArray(JToken token, IEnumerable<ComponentValue> elements) : base(token)
        {
            _elements = new ComponentCollection<ComponentNode>(elements.Select((e, i) => new ComponentNode($"[{i}]", e)));
            foreach (var p in _elements)
            {
                p.PropertyChanged += ChildPropertyChanged;
            }
        }

        private void ChildPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateProperty(nameof(Children));
        }
    }
}
