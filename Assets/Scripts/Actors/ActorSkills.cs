using System.Collections;
using UnityEngine;

namespace Scripts.Actors
{
    [RequireComponent(typeof(ActorStats))]
    public class ActorSkills : MonoBehaviour
    {
        //Properties for cleaner access of ActorSpecial Specials
        private int Strength { get { return actorSpecial.Strength; } }
        private int Perception { get { return actorSpecial.Perception; } }
        private int Endurance { get { return actorSpecial.Endurance; } }
        private int Charisma { get { return actorSpecial.Charisma; } }
        private int Intelligence { get { return actorSpecial.Intelligence; } }
        private int Agility { get { return actorSpecial.Agility; } }
        private int Luck { get { return actorSpecial.Luck; } }

        public int SkillPoints { get; private set; }

        //Skills
        [field: Header("Skills")]
        public int SkillBarter { get; set; }
        public int SkillDiplomacy { get; set; }
        public int SkillExplosives { get; set; }
        public int SkillFirearms { get; set; }
        public int SkillIntimidation { get; set; }
        public int SkillLockpick { get; set; }
        public int SkillMagicEnergyWeapons { get; set; }
        public int SkillMechanics { get; set; }
        public int SkillMedicine { get; set; }
        public int SkillMelee { get; set; }
        public int SkillScience { get; set; }
        public int SkillSleight { get; set; }
        public int SkillSneak { get; set; }
        public int SkillSurvival { get; set; }
        public int SkillThaumaturgy { get; set; }
        public int SkillUnarmed { get; set; }

        //Used for getting SPECIAL values needed for stat calculations
        private ActorSpecial actorSpecial;

        // Use this for initialization
        void Start()
        {
            actorSpecial = GetComponent<ActorSpecial>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetStats()
        {
            SkillBarter = (2 * Charisma) + (Luck / 2);
            SkillDiplomacy = (2 * Charisma) + (Luck / 2);
            SkillExplosives = (2 * Perception) + (Luck / 2);
            SkillFirearms = (2 * Agility) + (Luck / 2);
            SkillIntimidation = (2 * Strength) + (Luck / 2);
            SkillLockpick = (2 * Perception) + (Luck / 2);
            SkillMagicEnergyWeapons = (2 * Perception) + (Luck / 2);
            SkillMechanics = (2 * Intelligence) + (Luck / 2);
            SkillMelee = (2 * Strength) + (Luck / 2);
            SkillScience = (2 * Intelligence) + (Luck / 2);
            SkillSleight = (2 * Agility) + (Luck / 2);
            SkillSneak = (2 * Agility) + (Luck / 2);
            SkillSurvival = (2 * Endurance) + (Luck / 2);
            SkillThaumaturgy = (2 * actorSpecial.BaseActorSpecial.MagicSpecialValue) + (Luck / 2);
            SkillUnarmed = (2 * Endurance) + (Luck / 2);
        }
    }
}