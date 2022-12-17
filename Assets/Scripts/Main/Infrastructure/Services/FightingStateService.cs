using System.Collections.Generic;
using Interfaces;

namespace Services
{
    public class FightingStateService : IFightingStateService, IHaveLastBreath
    {
        private IHaveFightState hero;
        private List<IFighter> allFighters;
        private List<IFighter> aliveFighters;

        public FightingStateService(IHaveFightState hero, int startUnitsSize)
        {
            this.hero = hero;
            allFighters = new List<IFighter>(startUnitsSize);
            aliveFighters = new List<IFighter>(startUnitsSize);
        }
        
        public void LastBreath()
        {
            foreach (var fighter in allFighters)
            {
                fighter.OnFightStartEvent -= TryActiveHeroFightState;
                fighter.OnFightEndEvent -= TryDeactiveHeroFightState;
            }
            
            allFighters.Clear();
            allFighters.Capacity = 2;
            
            aliveFighters.Clear();
            aliveFighters.Capacity = 2;
        }
        
        public void RegisterFighter(IFighter fighter)
        {
            if (!allFighters.Contains(fighter))
            {
                fighter.OnFightStartEvent += TryActiveHeroFightState;
                fighter.OnFightEndEvent += TryDeactiveHeroFightState;
                allFighters.Add(fighter);
            }
        }
        
        private void TryActiveHeroFightState(IFighter fighter)
        {
            if (TryRegisterFighter(fighter))
            {
                if (!IsHeroInFightState())
                {
                    hero.SetFighState(true);
                }
            }
        }

        private void TryDeactiveHeroFightState(IFighter fighter)
        {
            if (TryUnregisterFighter(fighter))
            {
                if (aliveFighters.Count < 1 && IsHeroInFightState())
                {
                    hero.SetFighState(false);
                }
            }
        }

        private bool TryRegisterFighter(IFighter fighter)
        {
            if (!aliveFighters.Contains(fighter))
            {
                aliveFighters.Add(fighter);
                return true;
            }

            return false;
        }

        private bool TryUnregisterFighter(IFighter fighter)
        {
            if (aliveFighters.Contains(fighter))
            {
                aliveFighters.Remove(fighter);
                return true;
            }

            return false;
        }
        
        private bool IsHeroInFightState()
        {
            return hero.IsFighting;
        }
    }
}