using System;

namespace App.Scripts.Mics
{
    /// <summary>
    /// Fields / Properties annotated with this attribute will be destroyed during the
    /// logout flow.  If created with reinstantiate: true, the object will be reinstantiated
    /// post logout (value types will be implicitly reinstantiated).
    /// NOTE: Reinstatiation is handled via reflection of the type, so if the object was
    /// instantiated with custom parameters, this may not work as expected.  In this
    /// case, you may want to reconsider your usage of a static variable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class OnLogoutDestroyAttribute : Attribute
    {
        public OnLogoutDestroyAttribute()
        {

        }

        public OnLogoutDestroyAttribute(bool reinstantiate)
        {
            Reinstantiate = reinstantiate;
        }

        public bool Reinstantiate { get; private set; }
    }

    /// <summary>
    /// Fields / Properties annotated with this attribute are considered valid and as such
    /// will not fail builds during static validation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class OnLogoutPersistAttribute : Attribute
    {

    }
}
