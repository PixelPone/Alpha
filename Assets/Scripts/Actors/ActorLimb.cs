using UnityEngine;
using System;
using static Scripts.Constants;

namespace Scripts.Actors
{
    [Serializable]
    public class ActorLimb
    {
        [field: SerializeField]
        public string LimbName { get; private set; }

        [field: SerializeField]
        public Limb_Type LimbType { get; private set; }

        [field: SerializeField, Range(1, 10)]
        public int MaxLimbHealth { get; private set; }

        public int CurrentLimbHealth { get; set; }

        [field: SerializeField]
        public bool CanCripple { get; set; }

        [field: SerializeField]
        public bool AllowsFlight { get; set; }

        [field: SerializeField]
        public Cripple_Status CrippleStatus { get; set; }

        public ActorLimb()
        { 
            CurrentLimbHealth = MaxLimbHealth;
        }
    }
}