using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Perk : ScriptableObject
{
    [field: SerializeField]
    public string PerkName { get; private set; }
    [field: SerializeField, TextArea(minLines: 5, maxLines: 5)]
    public string PerkDescription { get; private set; }
    [field: SerializeField]
    public Sprite PerkSprite { get; private set; }

    public abstract bool MeetsRequirements(ActorStats actorStats);

    public abstract void OnEffect(params GameObject[] gameObjects);
}
