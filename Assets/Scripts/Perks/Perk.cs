using Scripts.Actors;
using UnityEngine;

namespace Scripts.Perks
{
    public abstract class Perk : ScriptableObject
    {
        [field: SerializeField]
        public string PerkName { get; private set; }
        [field: SerializeField, TextArea(minLines: 5, maxLines: 5)]
        public string PerkDescription { get; private set; }
        [field: SerializeField]
        public Sprite PerkSprite { get; private set; }

        public abstract bool MeetsRequirements(ActorSpecial actorSpecial);

        public abstract void OnEffect(params GameObject[] gameObjects);
    }
}