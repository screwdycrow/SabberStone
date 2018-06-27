using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.POGame;
using SabberStoneCoreAi.src.Agent.Helpers;

namespace SabberStoneCoreAi.src.Agent
{
	enum Weight { Damage, MonstersKilled, MonstersPlaced}
	class MyAgent : AbstractAgent
	{
		private SabberStoneCoreAi.POGame.POGame CurrentPoGame;
		private Dictionary<Weight, int> Weights;

		public override void FinalizeAgent()
		{
		}

		public override void FinalizeGame()
		{
		}

		public override PlayerTask GetMove(SabberStoneCoreAi.POGame.POGame poGame)
		{
			CurrentPoGame = poGame;
			ChangeStrategy();
			List<PlayerTask> actions = poGame.CurrentPlayer.Options();
			Dictionary<PlayerTask, POGame.POGame> resultedictionary = poGame.Simulate(actions);
			List<int> rewards = GetActionsRewards(actions, resultedictionary);
			return actions[pickAction(rewards)];
		}

		/// <summary>
		/// change the reward to weights to  exploit the chance to deal damage to the enemy's
		/// </summary>
		private void FullDamage()
		{
			this.Weights[Weight.Damage] = 6;			// maximize the damage
			this.Weights[Weight.MonstersPlaced] = 2;	// keep the ones in case there is no move for damage.
			this.Weights[Weight.MonstersKilled] = 1;
		}

		/// <summary>
		/// change the reward to try and gain control over the zone board
		/// </summary>
		private void Control()
		{
			this.Weights[Weight.Damage] = 1;
			this.Weights[Weight.MonstersPlaced] = 4;	//we want to balance the ammount of monsters we lost to the monsters we kill
			this.Weights[Weight.MonstersKilled] = 4;	// 1 - 4 - 4 strategy 
		}

		/// <summary>
		/// change the weights to try to keep a relative balance .
		/// </summary>
		private void Balanced()
		{
			this.Weights[Weight.Damage] = 2;
			this.Weights[Weight.MonstersPlaced] = 3;    
			this.Weights[Weight.MonstersKilled] = 4;
		}
		/// <summary>
		/// change the strategy based on some checkpoints
		/// </summary>
		private void ChangeStrategy()
		{
			//if (CurrentPoGame.CurrentOpponent.Hero.Health < 10 && CurrentPoGame.CurrentPlayer.Hero.Health > 15 ) FullDamage();
			if (CurrentPoGame.CurrentPlayer.Hero.Health < 10) Control();
			//if (CurrentPoGame.CurrentOpponent.Hero.Health < 2 ) FullDamage();

		}
		/// <summary>
		/// calculates the reward for an action.
		/// </summary>
		/// <param name="resultedState"></param>
		/// <returns> reward of the action as int</returns>
		private int ActionReward(POGame.POGame resultedState)
		{
			ActionResults results = new ActionResults(this.CurrentPoGame, resultedState);
			if (resultedState.CurrentOpponent.Hero.Health <= 0)
			{
				return  100;
			}
			else {

				return 
					results.DamageDealt * Weights[Weight.Damage] +
					results.MonstersKilled * Weights[Weight.MonstersKilled] +
					results.MonstersPlaced * Weights[Weight.MonstersPlaced];
			}
			
		}
		/// <summary>
		/// returns the rewards for every available action
		/// </summary>
		/// <param name="actions"></param>
		/// <returns> a list with rewards </returns>
		private List<int> GetActionsRewards(List<PlayerTask> actions, Dictionary<PlayerTask, POGame.POGame> taskResults)
		{
			List<int> rewards = new List<int>();
			foreach (PlayerTask action in actions)
			{
				int reward = 0;
				if(action.PlayerTaskType == PlayerTaskType.END_TURN)
				{
					reward = -1;
				}
				else
				{
					try
					{
						reward = ActionReward(taskResults[action]);

					}
					catch (NullReferenceException)
					{
						reward = 0; 
					}

				}
				rewards.Add(reward);

			}
			return rewards;
		}

		/// <summary>
		/// returns an action based on the calculated rewards 
		/// </summary>
		/// <param name="rewards"></param>
		/// <returns> action number </returns>
		private int pickAction(List<int> rewards)
		{
			return rewards.IndexOf(rewards.Max());
		}

		public override void InitializeAgent()
		{
			this.Weights = new Dictionary<Weight, int>();
			FullDamage();
		}

		public override void InitializeGame()
		{
		}
	}
}
