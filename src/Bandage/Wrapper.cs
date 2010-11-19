using System;
using System.Collections;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Bandage
{
    public static class Wrapper
    {
        public static IWrapper Create(object value, DynamicPropertyProvider properties)
        {
            if (object.ReferenceEquals(null, value)) return null;

            if (value is IWrapper)
            {
                return value as IWrapper;
            }
            else
            {
                return (IWrapper)Activator.CreateInstance(
                    typeof(Wrapper<>).MakeGenericType(value.GetType()),
                    value,
                    properties
                );
            }
        }
    }

    /// <summary>
    /// A marker interface that indicates a type is some kind of wrapper.
    /// </summary>
    public interface IWrapper { }

    public class Wrapper<T> : DynamicObject, IWrapper
    {
        public Wrapper(T value, DynamicPropertyProvider properties)
        {
            Value = value;
            this.properties = properties;
        }

        DynamicPropertyProvider properties;

        public T Value { get; set; }

        /// <remarks>Allow us to pass a wrapper to a method expecting the wrapped type.</remarks>
        public static implicit operator T(Wrapper<T> w)
        {
            return w.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var key = Tuple.Create(Value.GetType(), binder.Name);
            DynamicProperty property;
            if (properties.TryGetProperty(Value.GetType(), binder.Name, out property))
            {
                result = property.GetValue(Value);
            }
            else
            {
                // TODO: Probably need to cache this callsite somehow
                // I'm guessing it's expensive to create each time (?)
                var site = CallSite<Func<CallSite, object, object>>.Create(binder);
                result = Wrap(site.Target(site, Value));
            }
            return true;
        }
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var site = CallSite<Func<CallSite, object, int, object>>.Create(binder);
            result = Wrap(site.Target(site, Value, (int)indexes[0]));

            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var types = (from a in args select a.GetType()).ToArray();

            var method = Value.GetType().GetMethod(
                binder.Name, 
                BindingFlags.Public | BindingFlags.Instance, 
                null,
                types,
                null
            );

            if (method == null)
                return base.TryInvokeMember(binder, args, out result);
            else
            {
                result = Wrap(method.Invoke(Value, args));
                return true;
            }
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.Type == typeof(IEnumerable))
            {
                var items = Value as IEnumerable;
                result = items.Cast<object>().Select(i => Wrap(i));
            }
            else
            {
                var site = CallSite<Func<CallSite, object, object>>.Create(binder);
                result = site.Target(site, Value);
            }
            return true;
        }

        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            var funcType = typeof(Func<,,,>).MakeGenericType(typeof(CallSite), Value.GetType(), arg.GetType(), binder.ReturnType);
            dynamic site = typeof(CallSite<>).MakeGenericType(funcType)
                .GetMethod("Create", BindingFlags.Static | BindingFlags.Public)
                .Invoke(null, new[] { binder });

            result = site.Target(site, (dynamic)Value, (dynamic)arg);
            return true;
        }

        object Wrap(object obj)
        {
            return Wrapper.Create(obj, properties);
        }
    }
}
