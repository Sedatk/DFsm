using System;
using System.Collections.Generic;

namespace DFsm.Infrastructure
{
    public class ExtensionManager : IExtensionManager
    {
        public ExtensionManager()
        {
            _extensions = new Dictionary<Type, object>();
        }
        public void AddExtension(object extension)
        {
            _extensions[extension.GetType()] = extension;
        }
        public TExtension GetExtension<TExtension>()
            where TExtension : class
        {
            object ret;
            _extensions.TryGetValue(typeof(TExtension), out ret);
            return (TExtension)ret;
        }
        private readonly Dictionary<Type, object> _extensions;
    }
}
