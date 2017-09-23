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
            /*
             List<Plant> plants = new List<Plant>();
             plants.Add(new Plant());
             plants.Add(new Plant());
             plants.Add(new Plant());


             //Create (3) car manufacturers
             Thread[] plantThreads = new Thread[plants.Count];
             for (int i = 0; i < plants.Count; i++) {
                 plantThreads[i] = new Thread(new ThreadStart(plants[i].plantFunc));   //Multithread the plantFunc() section of Plant.cs
                 plantThreads[i].Name = (i + 1).ToString();                           //Name each thread
                 plantThreads[i].Start();                                             //Start each thread
             }

             List<Dealer> dealers = new List<Dealer>();
             dealers.Add(new Dealer());
             dealers.Add(new Dealer());
             dealers.Add(new Dealer());
             dealers.Add(new Dealer());
             dealers.Add(new Dealer());

             Thread[] dealerThreads = new Thread[dealers.Count];
             for (int i = 0; i < dealers.Count; i++) {
                 dealerThreads[i] = new Thread(new ThreadStart(dealers[i].CreateOrders));
                 dealerThreads[i].Name = (i + 1).ToString();
                 dealerThreads[i].Start();
             }              

             //Subscribe dealer to the price cut event
             Plant.priceCut += new priceCutEvent(dealership.carsOnSale);

             //Create (3) car dealerships
             Thread[] dealerships = new Thread[3];
             for (int i = 0; i < 3; i++) {
                 dealerships[i] = new Thread(new ThreadStart(dealership.dealerFunc));
                 dealerships[i].Name = (i + 1).ToString();
                 dealerships[i].Start();
             }

         */
            Order o = new Order();
            o.SenderID = "dealer1";
            o.Amount = 10;
            o.CardNum = 1234;
            o.UnitPrice = 10000;
            o.RecieverID = "plant1";

            Console.WriteLine(o);
            String s = Order.Encode(o);
            Console.WriteLine(s);
            Order o2 = Order.Decode(s);
            Console.WriteLine(o2.SenderID);
            Console.WriteLine(o2.Amount);
            Console.WriteLine(o2.CardNum);
            Console.WriteLine(o2.UnitPrice);
            Console.WriteLine(o2.RecieverID);
        }
    }
}
