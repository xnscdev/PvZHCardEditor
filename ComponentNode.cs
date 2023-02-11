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
        private bool _expanded;
        private ComponentNode? _parent;
        private string _key;
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

        public string Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        public ComponentValue? Value
        {
            get => _value;
            set => SetProperty(ref _value, value, null);
        }

        public JToken Token { get; private set; }
        public JToken? RootToken { get; private set; }
        public string Text => _value is not null && _value.Simple ? $"{_key} = {_value.SimpleText}" : _key;
        public ComponentCollection<ComponentNode> Children => _value is null || _value.Simple ? 
            new ComponentCollection<ComponentNode>(Array.Empty<ComponentNode>()) : _value.Children;

        public ComponentNode(string key, ComponentValue value, JToken? rootToken = null)
        {
            _key = key;
            _value = value;
            Token = value.Token;
            RootToken = rootToken;
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

        public virtual void Edit(CardComponent component)
        {
            if (component.FullToken is null)
                return;

            Value = component.Value;
            if (Value is not null)
            {
                foreach (var child in Value.Children)
                {
                    child.Parent = this;
                }
            }

            if (RootToken is null)
            {
                Token.Replace(component.FullToken);
                if (Token.Parent is null)
                    Token = component.FullToken;
            }
            else
            {
                RootToken.Replace(component.FullToken);
                if (RootToken.Parent is null)
                    RootToken = component.FullToken;
                Token = component.Token;
            }
        }

        public static Type? ParseComponentType(string s)
        {
            var match = Regex.Match(s, "^PvZCards\\.Engine\\.([a-zA-Z0-9_.]+),");
            return Type.GetType($"PvZHCardEditor.{match.Groups[1].Value}");
        }

        public static CardComponent? ParseComponent(JToken token)
        {
            if (token.Type == JTokenType.Null)
                return null;

            var type = ParseComponentType((string)token["$type"]!);
            if (type is null)
                return null;

            return (CardComponent?)Activator.CreateInstance(type, token["$data"]!, token);
        }

        public static AutoComponentNode? ParseComponentNode(JToken token)
        {
            var component = ParseComponent(token);
            if (component is null)
                return null;
            var name = component.GetType().Name;
            return component.Value is null ? new AutoComponentNode(name, component.Token) : new AutoComponentNode(name, component.Value, component.FullToken);
        }

        public static CardComponent? CreateComponent(string name)
        {
            var type = Type.GetType($"{typeof(ComponentNode).Namespace}.{name}");
            return type is null ? null : (CardComponent?)Activator.CreateInstance(type);
        }

        private void Value_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateProperty(nameof(Text));
            UpdateProperty(nameof(Children));
        }
    }

    public class AutoComponentNode : ComponentNode
    {
        public AutoComponentNode(string key, JToken token) : base(key, token) { }
        public AutoComponentNode(string key, ComponentValue value, JToken? rootToken = null) : base(key, value, rootToken) { }

        public override void Edit(CardComponent component)
        {
            base.Edit(component);
            if (RootToken is not null)
                Key = component.GetType().Name;
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

    public class ComponentBool : ComponentValue
    {
        private readonly bool _value;

        public override bool Simple => true;
        public override string SimpleText => _value.ToString();

        public ComponentBool(JToken token) : base(token)
        {
            _value = (bool)token;
        }
    }

    public class ComponentObject : ComponentValue
    {
        private readonly ComponentCollection<ComponentNode> _properties;

        public override ComponentCollection<ComponentNode> Children => _properties;

        public ComponentObject(JToken token) : this(token, new ComponentCollection<ComponentNode>()) { }

        public ComponentObject(JToken token, ComponentCollection<ComponentNode> properties) : base(token)
        {
            _properties = properties;
            foreach (var p in properties)
            {
                p.PropertyChanged += ChildPropertyChanged;
            }
        }

        public void AddNode(ComponentNode node)
        {
            node.PropertyChanged += ChildPropertyChanged;
            _properties.Add(node);
        }

        public void RemoveNode(ComponentNode node)
        {
            if (_properties.Remove(node))
                node.PropertyChanged -= ChildPropertyChanged;
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

        public ComponentArray(JToken token) : this(token, Enumerable.Empty<ComponentValue>()) { }

        public ComponentArray(JToken token, IEnumerable<ComponentValue> elements) : base(token)
        {
            _elements = new ComponentCollection<ComponentNode>(elements.Select((e, i) => new ComponentNode($"[{i}]", e)));
            foreach (var p in _elements)
            {
                p.PropertyChanged += ChildPropertyChanged;
            }
        }

        public void AddValue(ComponentValue value)
        {
            var node = new ComponentNode($"[{_elements.Count}]", value);
            node.PropertyChanged += ChildPropertyChanged;
            _elements.Add(node);
        }

        public void RemoveNode(ComponentNode node)
        {
            if (_elements.Remove(node))
                node.PropertyChanged -= ChildPropertyChanged;
        }

        private void ChildPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateProperty(nameof(Children));
        }
    }

    public class ComponentNull : ComponentValue
    {
        public override bool Simple => true;
        public override string SimpleText => "null";

        public ComponentNull(JToken token) : base(token) { }
    }

    public abstract class CardComponent
    {
        public ComponentValue? Value { get; }
        public JToken Token { get; }
        public JToken FullToken { get; }

        public CardComponent()
        {
            Token = DefaultToken;
            Value = DefaultValue(Token);
            var lastNamespace = GetType().Namespace?.Split('.').Last();
            var name = GetType().Name;
            FullToken = new JObject
            {
                ["$type"] = $"PvZCards.Engine.{lastNamespace}.{name}, EngineLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                ["$data"] = Token
            };
        }

        public CardComponent(JToken token, JToken fullToken)
        {
            Token = token;
            FullToken = fullToken;
            Value = DefaultValue(token);
        }

        protected virtual ComponentValue? DefaultValue(JToken token) => null;
        protected virtual JToken DefaultToken => new JObject();
    }
}
