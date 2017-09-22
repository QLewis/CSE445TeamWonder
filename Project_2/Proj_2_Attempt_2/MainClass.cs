using System;
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
            //Create a new plant object
            Plant carPlant = new Plant();

            //Create (3) car manufacturers
            Thread[] plants = new Thread[3];
            for (int i = 0; i < 3; i++) {
                plants[i] = new Thread(new ThreadStart(carPlant.plantFunc));   //Multithread the plantFunc() section of Plant.cs
                plants[i].Name = (i + 1).ToString();                           //Name each thread
                plants[i].Start();                                             //Start each thread
            }

            //Create a test dealer
            Dealer dealership = new Dealer();
            //Subscribe dealer to the price cut event
            Plant.priceCut += new priceCutEvent(dealership.carsOnSale);

            //Create (3) car dealerships
            Thread[] dealerships = new Thread[3];
            for (int i = 0; i < 3; i++) {
                dealerships[i] = new Thread(new ThreadStart(dealership.dealerFunc));
                dealerships[i].Name = (i + 1).ToString();
                dealerships[i].Start();
            }
        }
    }
}
