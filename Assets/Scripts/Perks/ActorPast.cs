using Scripts.Actors;
using System.Collections.Generic;
using UnityEngine;
using static Scripts.Constants;

namespace Scripts.Perks
{
    [CreateAssetMenu(fileName = "Dummy_PAST", menuName = "Alpha/Actor PAST")]
    public class ActorPast : Perk
    {

        [SerializeField]
        private List<Skill_Name> taggedSkills;

        //TODO: Add something here for Quirks later down the line

        public void OnValidate()
        {
            if (taggedSkills.Count == 0)
            {
                Debug.LogError("This PAST does not have any tagged Skills associated with it!");
            }
            else if (taggedSkills.Count > 4)
            {
                Debug.LogError("This PAST has too many tagged Skills associated with it!");
            }


        }

        public override bool MeetsRequirements(ActorSpecial actorSpecial)
        {
            return true; //Traits are always valid, so return true
        }

        public override void OnEffect(params GameObject[] gameObjects)
        {
            ActorSkills actor = gameObjects[0].GetComponent<ActorSkills>();
            foreach (Skill_Name taggedSkill in taggedSkills)
            {
                switch (taggedSkill)
                {
                    case Skill_Name.BARTER:
                        actor.SkillBarter += 10;
                        break;
                    case Skill_Name.DIPLOMACY:
                        actor.SkillDiplomacy += 10;
                        break;
                    case Skill_Name.EXPLOSIVES:
                        actor.SkillExplosives += 10;
                        break;
                    case Skill_Name.FIREARMS:
                        actor.SkillFirearms += 10;
                        break;
                    case Skill_Name.INTIMIDATION:
                        actor.SkillIntimidation += 10;
                        break;
                    case Skill_Name.LOCKPICK:
                        actor.SkillLockpick += 10;
                        break;
                    case Skill_Name.MAGICWEAPONS:
                        actor.SkillMagicEnergyWeapons += 10;
                        break;
                    case Skill_Name.MECHANICS:
                        actor.SkillMechanics += 10;
                        break;
                    case Skill_Name.MEDICINE:
                        actor.SkillMedicine += 10;
                        break;
                    case Skill_Name.MELEE:
                        actor.SkillMelee += 10;
                        break;
                    case Skill_Name.SCIENCE:
                        actor.SkillScience += 10;
                        break;
                    case Skill_Name.SLEIGHT:
                        actor.SkillSleight += 10;
                        break;
                    case Skill_Name.SNEAK:
                        actor.SkillSneak += 10;
                        break;
                    case Skill_Name.SURVIVAL:
                        actor.SkillSurvival += 10;
                        break;
                    case Skill_Name.THAUMATURGY:
                        actor.SkillThaumaturgy += 10;
                        break;
                    case Skill_Name.UNARMED:
                        actor.SkillUnarmed += 10;
                        break;
                }
            }
        }
    }
}