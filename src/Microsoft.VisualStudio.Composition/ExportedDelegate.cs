﻿// Copyright (c) Microsoft. All rights reserved.

namespace Microsoft.VisualStudio.Composition
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ExportedDelegate
    {
        private readonly object target;
        private readonly MethodInfo method;

        public ExportedDelegate(object target, MethodInfo method)
        {
            Requires.NotNull(method, nameof(method));

            this.target = target;
            this.method = method;
        }

        public Delegate CreateDelegate(Type delegateType)
        {
            Requires.NotNull(delegateType, nameof(delegateType));

            if (delegateType == typeof(Delegate) || delegateType == typeof(MulticastDelegate))
            {
                delegateType = ReflectionHelpers.GetContractTypeForDelegate(this.method);
            }

            try
            {
                return this.method.CreateDelegate(delegateType, this.target);
            }
            catch (ArgumentException)
            {
                // Bind failure occurs return null;
                return null;
            }
        }
    }
}
