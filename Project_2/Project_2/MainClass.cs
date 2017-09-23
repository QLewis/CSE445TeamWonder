using System;
using System.Collections.Generic;
using System.Threading;
using WeimoPlant;

/* This is just a test class. All it does is hold the main so that multithreading can be
 * tested as though it were the actual implementation.
 */

namespace WeimoPlant
{
    class MainClass
    {
        static void Main(string[] args)
        {
            const int NUM_OF_DEALERS = 5;

            Bank bank = new Bank();
            MultiCellBuffer buffer = new MultiCellBuffer(3);

			List<Plant> plants = new List<Plant>();
			plants.Add(new Plant(bank, buffer));
			plants.Add(new Plant(bank, buffer));
            plants.Add(new Plant(bank, buffer));

			List<Dealer> dealers = new List<Dealer>();

			Thread[] dealerThreads = new Thread[NUM_OF_DEALERS];
            for (int i = 0; i < dealerThreads.Length; i++)
			{
                Dealer dealer = new Dealer(bank, buffer);
                dealers.Add(dealer);

				dealerThreads[i] = new Thread(new ThreadStart(dealer.Run));
                dealerThreads[i].Name = "Dealer #" + (i + 1);
				dealerThreads[i].Start();

				foreach (Plant p in plants)
				{
					p.subscribe(dealer);
				}
			}

			//Create (3) car manufacturers
			Thread[] plantThreads = new Thread[plants.Count];
			for (int i = 0; i < plants.Count; i++)
			{
				plantThreads[i] = new Thread(new ThreadStart(plants[i].Run));   //Multithread the plantFunc() section of Plant.cs
				plantThreads[i].Name = "Plant #" + (i + 1);                    //Name each thread
				plantThreads[i].Start();                                        //Start each thread
			}
        }
    }
}
