using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCoreAi.POGame;

namespace SabberStoneCoreAi.src.Agent.Helpers
{
     class ActionResults
    {
		public int MySpentMana;
		public int MonstersPlaced;
		public int DamageDealt;
		public int HealthDiff;
		public int MonstersKilled;
		public ActionResults(POGame.POGame startState, POGame.POGame lastState)
		{
			this.HealthDiff = startState.CurrentPlayer.Hero.Health -   lastState.CurrentPlayer.Hero.Health;
			this.MySpentMana = startState.CurrentPlayer.RemainingMana - lastState.CurrentPlayer.RemainingMana;
			this.DamageDealt = startState.CurrentOpponent.Hero.Health - lastState.CurrentOpponent.Hero.Health;
			this.MonstersKilled = startState.CurrentOpponent.BoardZone.Count - lastState.CurrentOpponent.BoardZone.Count;
			this.MonstersPlaced = lastState.CurrentPlayer.BoardZone.Count - startState.CurrentPlayer.BoardZone.Count;

		}

	}

	
}
