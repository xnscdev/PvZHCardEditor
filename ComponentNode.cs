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
        private string _key;
        private ComponentValue? _value;
        private string? _componentName;
        private bool _allowAdd;

        public bool Expanded
        {
            get => _expanded;
            set => SetProperty(ref _expanded, value);
        }

        public string Key
        {
            get => _key;
            set => SetProperty(ref _key, value, null);
        }

        public ComponentValue? Value
        {
            get => _value;
            set => SetProperty(ref _value, value, null);
        }

        public string? ComponentName
        {
            get => _componentName;
            set => SetProperty(ref _componentName, value, null);
        }

        public bool AllowAdd
        {
            get => _allowAdd;
            set => SetProperty(ref _allowAdd, value, null);
        }

        public JToken Token { get; private set; }
        public JToken? RootToken { get; private set; }
        public ComponentNode? Parent { get; set; }
        public virtual string Text => _value is not null && _value.Simple ? $"{_key} = {_value.SimpleText}" : _componentName is null ? _key : $"{_key} = {_componentName}";
        public ComponentCollection<ComponentNode> Children => _value is null || _value.Simple ? 
            new ComponentCollection<ComponentNode>(Array.Empty<ComponentNode>()) : _value.Children;

        public virtual ComponentValue? EditedValue(CardComponent component) => component.IsolatedObject;

        public ComponentNode(string key, ComponentValue value, bool allowAdd = true, JToken? rootToken = null)
        {
            _key = key;
            _value = value;
            Token = value.Token;
            RootToken = rootToken;
            AllowAdd = allowAdd;
            value.PropertyChanged += Value_PropertyChanged;
            foreach (var child in value.Children)
                child.Parent = this;
        }

        public ComponentNode(string key, JToken token, bool allowAdd = true)
        {
            _key = key;
            Token = token;
            AllowAdd = allowAdd;
        }

        public ComponentValue? Edit(ComponentValue? value)
        {
            if (Value is not null)
            {
                foreach (var child in Value.Children)
                    child.Parent = null;
            }
            var oldValue = Value;

            Value = value;
            if (value is null)
            {
                Token.Remove();
            }
            else
            {
                Token.Replace(value.Token);
                if (Token.Parent is null)
                    Token = value.Token;
                foreach (var child in value.Children)
                    child.Parent = this;
            }
            
            ComponentName = null;
            AllowAdd = true;
            return oldValue;
        }

        public virtual ComponentValue? Edit(CardComponent component)
        {
            if (component.FullToken is null)
                return null;

            if (Value is not null)
            {
                foreach (var child in Value.Children)
                    child.Parent = null;
            }
            var oldValue = Value;

            Value = EditedValue(component);
            ComponentName = component.GetType().Name;
            AllowAdd = component.AllowAdd;
            if (Value is not null)
            {
                foreach (var child in Value.Children)
                    child.Parent = this;
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

            return Value;
        }

        public int RemoveComponent(ComponentNode component)
        {
            if (component.Parent != this)
                throw new InvalidOperationException("Component to remove is not a child of this component");
            var index = Value!.Remove(component);
            var token = component.RootToken ?? component.Token;
            if (token.Parent is JProperty)
                token.Parent.Remove();
            else
                token.Remove();
            component.Parent = null;
            return index;
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
            return component.Value is null ? new AutoComponentNode(name, component.Token, component.AllowAdd) : new AutoComponentNode(name, component.Value, component.AllowAdd, component.FullToken);
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
        public AutoComponentNode(string key, JToken token, bool allowAdd) : base(key, token, allowAdd)
        {
            ComponentName = key;
        }

        public AutoComponentNode(string key, ComponentValue value, bool allowAdd, JToken? rootToken = null) : base(key, value, allowAdd, rootToken)
        {
            ComponentName = key;
        }

        public override string Text => Value is not null && Value.Simple ? $"{Key} = {Value.SimpleText}" : Key;

        public override ComponentValue? EditedValue(CardComponent component) => component.Value;

        public override ComponentValue? Edit(CardComponent component)
        {
            var oldValue = base.Edit(component);
            if (RootToken is not null)
                Key = component.GetType().Name;
            return oldValue;
        }
    }

    public abstract class ComponentValue : ViewModelBase
    {
        public virtual bool Simple => false;
        public virtual string SimpleText => "";
        public virtual ValueTargetType AddValueType => ValueTargetType.None;
        public virtual ComponentCollection<ComponentNode> Children => new(Array.Empty<ComponentNode>());
        public JToken Token { get; }

        public ComponentValue(JToken token)
        {
            Token = token;
        }

        internal virtual void Add(int index, ComponentNode node) => throw new NotImplementedException();
        internal virtual ComponentNode Add(AddValueViewModel model, JToken token, ComponentValue value, string? componentName, bool allowAdd) => throw new NotImplementedException();
        internal virtual int Remove(ComponentNode node) => throw new NotImplementedException();
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
        public override string SimpleText => $"\"{_value}\"";

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

        public override ValueTargetType AddValueType => ValueTargetType.Key;
        public override ComponentCollection<ComponentNode> Children => _properties;

        public ComponentObject(JToken token) : this(token, new ComponentCollection<ComponentNode>()) { }

        public ComponentObject(JToken token, ComponentCollection<ComponentNode> properties) : base(token)
        {
            _properties = properties;
            foreach (var p in properties)
                p.PropertyChanged += ChildPropertyChanged;
        }

        internal override void Add(int index, ComponentNode node)
        {
            node.PropertyChanged += ChildPropertyChanged;
            _properties.Insert(index, node);
            var obj = (JObject)Token;
            if (node.RootToken is not null)
                obj.Add(node.Key, node.RootToken);
            else if (node.Token.Parent is JProperty)
                obj.Add(node.Token.Parent);
            else
                obj.Add(node.Key, node.Token);
        }

        internal override ComponentNode Add(AddValueViewModel model, JToken token, ComponentValue value, string? componentName, bool allowAdd)
        {
            var node = new ComponentNode(model.Key, value, allowAdd, componentName is null ? null : token)
            {
                ComponentName = componentName
            };
            node.PropertyChanged += ChildPropertyChanged;
            _properties.Add(node);
            ((JObject)Token).Add(model.Key, token);
            return node;
        }

        internal override int Remove(ComponentNode node)
        {
            var index = _properties.IndexOf(node);
            if (index == -1)
                throw new InvalidOperationException("Could not find child component to remove");
            _properties.RemoveAt(index);
            node.PropertyChanged -= ChildPropertyChanged;
            return index;
        }

        private void ChildPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateProperty(nameof(Children));
        }
    }

    public class ComponentArray : ComponentValue
    {
        private readonly ComponentCollection<ComponentNode> _elements;

        public override ValueTargetType AddValueType => ValueTargetType.Index;
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

        internal override void Add(int index, ComponentNode node)
        {
            node.PropertyChanged += ChildPropertyChanged;
            for (var i = index; i < _elements.Count; i++)
                _elements[i].Key = $"[{i + 1}]";
            _elements.Insert(index, node);
            ((JArray)Token).Insert(index, node.RootToken ?? node.Token);
        }

        internal override ComponentNode Add(AddValueViewModel model, JToken token, ComponentValue value, string? componentName, bool allowAdd)
        {
            return Add(model.Index, token, value, componentName, allowAdd);
        }

        internal override int Remove(ComponentNode node)
        {
            var index = _elements.IndexOf(node);
            if (index == -1)
                throw new InvalidOperationException("Could not find child component to remove");
            _elements.RemoveAt(index);
            node.PropertyChanged -= ChildPropertyChanged;
            for (var i = index; i < _elements.Count; i++)
                _elements[i].Key = $"[{i}]";
            return index;
        }

        internal void Add(int? index, ComponentValue value) => Add(index, value.Token, value, null, true);

        private ComponentNode Add(int? index, JToken token, ComponentValue value, string? componentName, bool allowAdd)
        {
            index ??= _elements.Count;
            if (index < 0)
                index = 0;
            else if (index > _elements.Count)
                index = _elements.Count;

            var node = new ComponentNode($"[{index}]", value, allowAdd, componentName is null ? null : token)
            {
                ComponentName = componentName
            };
            node.PropertyChanged += ChildPropertyChanged;
            for (var i = index.Value; i < _elements.Count; i++)
                _elements[i].Key = $"[{i + 1}]";
            _elements.Insert(index.Value, node);
            ((JArray)Token).Insert(index.Value, token);
            return node;
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
        public virtual bool AllowAdd => false;
        public virtual ComponentValue IsolatedObject => Value ?? new ComponentObject(Token);
        protected virtual JToken DefaultToken => new JObject();

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
    }

    public class TraitCardComponent : CardComponent
    {
        public TraitCardComponent() { }
        public TraitCardComponent(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["Counters"] = new JObject
            {
                ["IsPersistent"] = true,
                ["Counters"] = new JArray
                {
                    new JObject
                    {
                        ["SourceId"] = -1,
                        ["Duration"] = 0,
                        ["Value"] = 0
                    }
                }
            }
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var counters = token["Counters"]!;
            var counters2 = (JArray)counters["Counters"]!;
            return new ComponentObject(counters, new ComponentCollection<ComponentNode>(new[]
            {
                new ComponentNode("IsPersistent", new ComponentBool(counters["IsPersistent"]!)),
                new ComponentNode("Counters", new ComponentArray(counters2, counters2.Select(c => new ComponentObject(c, new ComponentCollection<ComponentNode>(new[]
                {
                    new ComponentNode("SourceId", new ComponentInt(c["SourceId"]!)),
                    new ComponentNode("Duration", new ComponentInt(c["Duration"]!)),
                    new ComponentNode("Value", new ComponentInt(c["Value"]!))
                })))))
            }));
        }
    }
}
