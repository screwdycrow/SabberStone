using System;
using SabberStoneCore.Config;
using SabberStoneCore.Enums;
using SabberStoneCoreAi.POGame;
using SabberStoneCoreAi.Agent.ExampleAgents;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.Meta;
using System.Collections.Generic;
using SabberStoneCore.Model;

namespace SabberStoneCoreAi
{
	//for some reason i couldnt make a method inside the internal class, so i made another class :P 
	static class Helper
	{
		/// <summary>
		/// plays all games that are required for the competition. My deck against all other three. It plays 100 matches and 100 matched (mirrored) for each opponent deck.
		/// </summary>
		/// <param name="gameConfig"></param>
		/// <param name="player1"></param>
		/// <param name="player2"></param>
		/// <param name="decks"></param>
		internal static void SimulateAllGames(GameConfig gameConfig, AbstractAgent player1, AbstractAgent player2, Dictionary<string, List<Card>> decks)
		{
			foreach(KeyValuePair<string, List<Card>> deck in decks)
			{
				Console.WriteLine("-> RenokazakusMage (me)  vs  " + deck.Key);

				gameConfig.Player2Deck = deck.Value;

				playGame(gameConfig, player1, player2);

				Console.WriteLine("-> Mirror:"+deck.Key+" vs RenokazakusMage (me)" );
			
				playGame(gameConfig, player2, player1);
				Console.WriteLine("");

			}

			Console.WriteLine(" -----------------------------");
		}
		/// <summary>
		/// plays the game, a function to reduce repeated code.
		/// </summary>
		/// <param name="gameConfig"></param>
		/// <param name="player1"></param>
		/// <param name="player2"></param>
		internal static void playGame(GameConfig gameConfig, AbstractAgent player1, AbstractAgent player2)
		{
			var gameHandler = new POGameHandler(gameConfig, player1, player2, debug: false);
			gameHandler.PlayGames(100);
			GameStats gameStats = gameHandler.getGameStats();
			gameStats.printResults();
		}
	}

	internal class Program
	{
		
		

		private static void Main(string[] args)
		{
			
			Console.WriteLine("Setup gameConfig");

			//todo: rename to Main
			GameConfig gameConfig = new GameConfig
			{
				StartPlayer = 1,

				Logging = true
			};
		
			gameConfig.Player1Name = "Sky";
			gameConfig.Player2Name = "Net";
			gameConfig.Player1Deck = Decks.RenoKazakusMage;											//My Deck

			Dictionary <string, List<Card>> decksAvailable = new Dictionary<string, List<Card>>();	//set opponents decks.

			decksAvailable.Add("AggroPirateWarrior", Decks.AggroPirateWarrior);                     //AggroPirateWarrior
			decksAvailable.Add("MidrangeJadeShaman", Decks.MidrangeJadeShaman);                     //MidrangeJadeShaman
			decksAvailable.Add("RenoKazakusMage", Decks.RenoKazakusMage);                           //RenoKazakusMage

			Console.WriteLine("Setup POGameHandler");
			AbstractAgent player1 = new src.Agent.MyAgent();
			AbstractAgent player2;
			
			Console.WriteLine("Start Games ");

			Console.WriteLine("=== MyAgent vs Random Agent=== ");
		    player2 = new RandomAgent();														//play all games against the Random Agent
			Helper.SimulateAllGames(gameConfig, player1, player2, decksAvailable);

	
			Console.WriteLine("=== MyAgent vs RandomLateEnd Agent ===");

			/* i achieve around  >80 % on one occasions against AggroPirate
			 * with this agent and >90% in others, but it was not announced,
			 * until 25 june that i had to compete against this as well,
			 * so i didn' try to make something better and sophisticated, cause
			 * no time.
			*/

			//play all games against the RandomLateEnd Agent,
			player2 = new RandomAgentLateEnd();
			Helper.SimulateAllGames(gameConfig, player1, player2, decksAvailable);

			Console.WriteLine("=== My Agent vs FaceHunter Agent ===");                           //play all games against the RandomLateEnd Agent
			player2 = new FaceHunter();
			Helper.SimulateAllGames(gameConfig, player1, player2, decksAvailable);

			Console.WriteLine("Test Ended");
			Console.ReadLine();


		}

	}

}
