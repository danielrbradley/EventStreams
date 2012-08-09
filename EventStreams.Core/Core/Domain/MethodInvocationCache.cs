﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EventStreams.Core.Domain {
    using HandleMethod = Action<object, EventArgs>;
 
    internal sealed class MethodInvocationCache<TAggregateRoot> {
        /// <summary>
        /// Heuristic that predicts most aggregate roots will have up to 4 event handlers.
        /// For those that have more, not a problem as the dictionary will just expand itself.
        /// </summary>
        private const int InitialCapacity = 4;

        private readonly Dictionary<Type, HandleMethod> _cache =
            new Dictionary<Type, HandleMethod>(InitialCapacity);

        public MethodInvocationCache() {
            var handledTypes = GetMethods().Select(mi => mi.GetParameters().First().ParameterType);
            foreach (var handledType in handledTypes) {
                var mi = GetMethodFor(handledType);
                var handleMethod = mi != null
                    ? CreateOpenInstanceDelegate<HandleMethod>(mi)
                    : null;

                _cache.Add(handledType, handleMethod);
            }
        }

        public bool TryGetMethod(EventArgs args, out HandleMethod method) {
            return _cache.TryGetValue(args.GetType(), out method);
        }

        private MethodInfo GetMethodFor(Type handledType) {
            return
                GetMethods()
                    .SingleOrDefault(mi => handledType == mi.GetParameters().First().ParameterType);
        }

        private IEnumerable<MethodInfo> GetMethods() {
            return
                typeof(TAggregateRoot)
                    .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(mi =>
                           mi.Name.Equals("Handle", StringComparison.OrdinalIgnoreCase) ||
                           mi.Name.Equals("When", StringComparison.OrdinalIgnoreCase))
                    .Where(mi => mi.GetParameters().Length == 1)
                    .Where(mi => typeof(EventArgs).IsAssignableFrom(mi.GetParameters().First().ParameterType));
        }

        /// <summary>
        /// Creates a delegate of a specified type that represents a method which can be executed on an instance passed as parameter.
        /// </summary>
        /// <typeparam name="TDelegate">
        /// The type for the delegate.
        /// This delegate needs at least one (first) type parameter denoting the type of the instance  which will be passed.
        /// </typeparam>
        /// <param name="method">The <see cref="MethodInfo"/> describing the method of the instance type.</param>
        private static TDelegate CreateOpenInstanceDelegate<TDelegate>(MethodInfo method) where TDelegate : class {
            var delegateMethodInfo = typeof(TDelegate).GetMethod("Invoke");
            var delegateParameters = delegateMethodInfo.GetParameters();

            // Convert instance type when necessary.
            var delegateInstanceType = delegateParameters.Select(p => p.ParameterType).First();
            var methodInstanceType = method.DeclaringType;
            var instance = Expression.Parameter(delegateInstanceType);
            var convertedInstance = Expression.Convert(instance, methodInstanceType);

            // Create delegate original and converted arguments.
            var delegateTypes = delegateParameters.Select(d => d.ParameterType).Skip(1);
            var methodTypes = method.GetParameters().Select(m => m.ParameterType).ToArray();

            // Prepare delegate parameter expressions.
            var sourceDelegateParameterExpressions = delegateTypes.Select(Expression.Parameter).ToArray();
            var targetDelegateParameterExpressions = (IEnumerable<Expression>)new Expression[] {
                // Wrap the EventArgs parameter in a cast to the more derived concrete type.
		        Expression.Convert(sourceDelegateParameterExpressions[0], methodTypes[0])
		    };

            // Create method call.
            var methodCall = Expression.Call(
                convertedInstance,
                method,
                targetDelegateParameterExpressions);

            // Build a lambda and compile it into a delegate.
            return Expression
                .Lambda<TDelegate>(
                    methodCall,
                    new[] { instance }.Concat(sourceDelegateParameterExpressions))
                .Compile();
        }
    }
}
