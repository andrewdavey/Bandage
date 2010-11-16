using System;
using System.Linq;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;

namespace Bandage
{
    public class Wrapper : DynamicObject
    {
        public Wrapper(object objectToWrap, IDictionary<Tuple<Type, string>, DynamicProperty> properties)
        {
            wrapped = objectToWrap;
            this.properties = properties;
        }

        object wrapped;
        IDictionary<Tuple<Type, string>, DynamicProperty> properties;

        public object Value
        {
            get { return wrapped; }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var key = Tuple.Create(wrapped.GetType(), binder.Name);
            if (properties.ContainsKey(key))
            {
                result = Wrap(properties[key].Getter(wrapped));
            }
            else
            {
                // TODO: Probably need to cache this callsite somehow
                // I'm guessing it's expensive to create each time (?)
                var site = CallSite<Func<CallSite, object, object>>.Create(binder);
                result = Wrap(site.Target(site, wrapped));
            }
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var site = CallSite<Func<CallSite, object, int, object>>.Create(binder);
            result = Wrap(site.Target(site, wrapped, (int)indexes[0]));

            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var types = from a in args
                        select a.GetType();

            var method = wrapped.GetType().GetMethod
                (binder.Name, BindingFlags.Public | BindingFlags.Instance, null, types.ToArray(), null);

            if (method == null)
                return base.TryInvokeMember(binder, args, out result);
            else
            {
                result = Wrap(method.Invoke(wrapped, args));
                return true;
            }  
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.Type == typeof(IEnumerable))
            {
                var items = wrapped as IEnumerable;
                result = items.Cast<object>().Select(i => Wrap(i));
            }
            else
            {
                var site = CallSite<Func<CallSite, object, object>>.Create(binder);
                result = site.Target(site, wrapped);
            }
            return true;
        }

        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            var funcType = typeof(Func<,,,>).MakeGenericType(typeof(CallSite), wrapped.GetType(), arg.GetType(), binder.ReturnType);
            dynamic site = typeof(CallSite<>).MakeGenericType(funcType)
                .GetMethod("Create", BindingFlags.Static | BindingFlags.Public)
                .Invoke(null, new[] { binder });

            result = site.Target(site, (dynamic)wrapped, (dynamic)arg);
            return true;
        }

        public override string ToString()
        {
            return wrapped.ToString();
        }

        object Wrap(object result)
        {
            if (!(result is Wrapper))
            {
                result = new Wrapper(result, properties);
            }
            return result;
        }
    }
}
