using DmmLogDriver;
using System;
using System.Diagnostics;

namespace DmmLog {
    [DebuggerDisplay("{DisplayName}")]
    internal class Driver {

        public Driver(Type type) {
            this.Type = type;
            this.Capabilities = DmmDriverCapabilities.GetDriverCapabilities(this.Type);
        }


        private readonly Type Type;

        public String Name { get { return this.Type.Name; } }
        public DmmDriverCapabilities Capabilities { get; private set; }


        public DmmDriver GetInstance(String settings) {
            var constructior = this.Type.GetConstructor(new Type[] { typeof(String) });
            return (DmmDriver)constructior.Invoke(new Object[] { settings }); ;
        }

    }
}
