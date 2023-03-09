using System;
using QuickEye.Samples.CharacterCreation;

namespace QuickEye.Samples.UIEvents
{
    public static class CharacterSelected
    {
        public static event Action<CharacterTemplate> Event;
        public static void Trigger(CharacterTemplate arg) => Event?.Invoke(arg);
        public static void Register(Action<CharacterTemplate> callback) => Event +=callback;
        public static void Unregister(Action<CharacterTemplate> callback) => Event -= callback;
    }
}