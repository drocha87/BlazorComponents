namespace DrBlazor;

public class AttrBuilder
{
    private readonly HashSet<string> _classes = new();
    private readonly HashSet<string> _styles = new();
    private readonly Dictionary<string, object> _attributes = new();

    public AttrBuilder() { }

    public AttrBuilder AddClass(string cl)
    {
        _classes.Add(cl);
        return this;
    }

    public AttrBuilder RemClass(string cl)
    {
        _classes.Remove(cl);
        return this;
    }

    public AttrBuilder AddClass(string cl, bool when)
    {
        if (when)
        {
            _classes.Add(cl);
        }
        return this;
    }

    public AttrBuilder AddClasses(string? classes)
    {
        if (classes is not null)
        {
            foreach (var cl in classes.Split(" "))
            {
                if (!string.IsNullOrEmpty(cl))
                {
                    _classes.Add(cl.Trim());
                }
            }
        }
        return this;
    }

    public AttrBuilder AddStyles(string styles)
    {
        if (!string.IsNullOrEmpty(styles))
        {
            foreach (var style in styles.Split(";"))
            {
                if (!string.IsNullOrEmpty(style))
                {
                    _styles.Add(style.Trim());
                }
            }
        }
        return this;
    }

    public AttrBuilder AddStyle(string property, string value)
    {
        if (!string.IsNullOrEmpty(property))
        {
            _styles.Add($"{property}: {value}");
        }
        return this;
    }

    public AttrBuilder AddStyle(string property, string value, bool when)
    {
        if (when)
        {
            return AddStyle(property, value);
        }
        return this;
    }

    public AttrBuilder AddData(string key, string? value)
    {
        _attributes.Add(key, value ?? "");
        return this;
    }

    public AttrBuilder AddData(string key, string? value, bool when)
        => when ? AddData(key, value) : this;

    public AttrBuilder AddData(string key, bool value)
    {
        if (value)
        {
            _attributes.Add(key, "");
        }
        return this;
    }

    public AttrBuilder AddAttributes(Dictionary<string, object>? attrs)
    {
        if (attrs is not null)
        {
            foreach (var attr in attrs)
            {
                switch (attr.Key)
                {
                    case "class":
                        {
                            if (attr.Value is string value)
                            {
                                _classes.Add(value);
                            }
                        }
                        break;

                    case "style":
                        {
                            if (attr.Value is string value)
                            {
                                _styles.Add(value);
                            }
                            break;
                        }
                    default:
                        _attributes.Add(attr.Key, attr.Value);
                        break;
                }
            }
        }
        return this;
    }

    public string BuildClass() => string.Join(" ", _classes);
    public string BuildStyle() => string.Join(";", _styles);

    public Dictionary<string, object> Build()
    {
        if (_classes.Any())
        {
            _attributes.Add("class", BuildClass());
        }

        if (_styles.Any())
        {
            _attributes.Add("style", BuildStyle());
        }
        return _attributes;
    }
}