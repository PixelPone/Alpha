using Scripts.Perks;
using System.Collections;
using UnityEngine;
using static Scripts.Constants;

namespace Scripts.Actors
{
    public class ActorInfo : MonoBehaviour
    {
        [field: Header("General Info"), SerializeField]
        public string ActorName { get; private set; }
        public int Level { get; set; } = 1;
        private int Experience { get; set; } = 0;
        private int Karma { get; set; } = 0;

        [SerializeField]
        private ActorPast actorPast;
        [field: SerializeField]
        public Actor_Gender Gender { get; private set; } = Actor_Gender.MALE;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}