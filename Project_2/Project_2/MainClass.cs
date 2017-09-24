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
            const int NUM_OF_PLANTS = 3;
            const int BUFFER_SIZE = 3;

            Bank bank = new Bank();

            //initializes the MulitCellBuffer with correct size
            MultiCellBuffer buffer = new MultiCellBuffer(BUFFER_SIZE);

			List<Plant> plants = new List<Plant>();

            //Create (3) Plants
            for (int i = 0; i < NUM_OF_PLANTS; i++)
            {
                plants.Add(new Plant(bank, buffer));
            }

			List<Dealer> dealers = new List<Dealer>();

            //Create (5) Dealers and names them
            for (int i = 0; i < NUM_OF_DEALERS; i++)
			{
                Dealer dealer = new Dealer("Dealer #" + (i + 1), bank, buffer);
                dealers.Add(dealer);

                Thread t = new Thread(new ThreadStart(dealer.Run));
                t.IsBackground = true;
                t.Start();

                //Each dealer must 
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

				Thread t = new Thread(new ThreadStart(plants[i].RunProcessOrders));   //Multithread the plantFunc() section of Plant.cs
                t.IsBackground = true;
				t.Start();
			}
        }
    }
}
