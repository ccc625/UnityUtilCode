using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class CachedValue2<T>
{
    private Dictionary<string, object> _fileds = new Dictionary<string, object>();
    
    public CachedValue2()
    {
        var type = typeof(T);

        foreach (var field in type.GetFields())
        {
            var fieldType = field.FieldType;
            _fileds.Add(field.Name, null);
        }
    }

    public void Set(T target)
    {
        var type = target.GetType();

        foreach (var field in type.GetFields())
        {
            if (!_fileds.ContainsKey(field.Name))
                continue;

            _fileds[field.Name] = field.GetValue(target);
        }
    }

    public F Get<F>(string fieldName)
    {
        if (!_fileds.ContainsKey(fieldName) || _fileds[fieldName] == null)
        {
            return default(F);
        }
        
        return (F)_fileds[fieldName];
    }
}

public class CachedValue
{
    public void Clear()
    {
        foreach (var field in this.GetType().GetFields())
        {
            field.SetValue(this, null);
        }
    }

    public void Clone(CachedValue target)
    {
        var inObjType = target.GetType();

        foreach (var field in this.GetType().GetFields())
        {
            var targetField = inObjType.GetField(field.Name);

            if (targetField != null)
            {
                field.SetValue(this, targetField.GetValue(target));
            }
            else
            {
                field.SetValue(this, null);
            }
        }
    }
}

public class InGameCachedValue : CachedValue
{
    public int id;
}
