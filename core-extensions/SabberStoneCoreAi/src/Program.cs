using System;
using SabberStoneCore.Config;
using SabberStoneCore.Enums;
using SabberStoneCoreAi.POGame;
using SabberStoneCoreAi.Agent.ExampleAgents;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.Meta;

namespace SabberStoneCoreAi
{
	internal class Program
	{

		private static void Main(string[] args)
		{
			
			Console.WriteLine("Setup gameConfig");

			//todo: rename to Main
			GameConfig gameConfig = new GameConfig
			{
				StartPlayer = 1,
				Player1HeroClass = CardClass.WARRIOR,
				Player2HeroClass = CardClass.WARRIOR,
				Logging = true
			};
			gameConfig.Player1Name = "Sky";
			gameConfig.Player2Name = "Net";
			gameConfig.Player1Deck = Decks.RenoKazakusMage;			//the actual deck i decided to play since i am Barchelor : RenoKazakusMage
			Console.WriteLine("Setup POGameHandler");
			AbstractAgent player2 = new src.Agent.MyAgent();
			AbstractAgent player1 = new RandomAgent();
			

			Console.WriteLine("PlayGame");
			//gameHandler.PlayGame();

			// play against every deck 
			for (int j = 0; j<3; j++)
			{
				switch (j)
				{
					case 0:
						Console.WriteLine("Playing against AggroPirateWarrior");
						gameConfig.Player2Deck = Decks.AggroPirateWarrior;
						break;
					case 1:
						Console.WriteLine("Playing against RenoKazakusMage");
						gameConfig.Player2Deck = Decks.RenoKazakusMage;
						break;
					case 2:
						Console.WriteLine("Playing against MidrangeJadeShaman");
						gameConfig.Player2Deck = Decks.MidrangeJadeShaman;
						break;
				}

				//100 games per opponent deck  
					var gameHandler = new POGameHandler(gameConfig, player1, player2, debug: false);
					gameHandler.PlayGames(100);
					GameStats gameStats = gameHandler.getGameStats();
					gameStats.printResults();
					Console.WriteLine("Test successful");
				
			}
			Console.WriteLine("Test Ended");
			Console.ReadLine();


		}
	}
}
