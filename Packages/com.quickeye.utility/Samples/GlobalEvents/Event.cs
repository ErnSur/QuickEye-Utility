using System;
using System.Runtime.CompilerServices;
using QuickEye.Utility;

namespace Samples.GlobalEvents
{
    class PlayerHpChanged : GlobalUnityEvent<PlayerHpChanged,float>
    {
    }
    
    class PlayerMpChanged : GlobalUnityEvent<PlayerMpChanged,float>
    {
    }

    class PlayerHealthChange : Event<PlayerHealthChange> { }
    class PlayerManaChange : Event<PlayerManaChange, float> { }
}