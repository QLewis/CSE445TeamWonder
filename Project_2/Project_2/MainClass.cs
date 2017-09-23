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
            Plant carPlant1 = new Plant();
            Plant carPlant2 = new Plant();
            Plant carPlant3 = new Plant();

            Thread plant1 = new Thread(new ThreadStart(carPlant1.plantFunc));
            Thread plant2 = new Thread(new ThreadStart(carPlant2.plantFunc));
            Thread plant3 = new Thread(new ThreadStart(carPlant3.plantFunc));

            plant1.Name = "Plant 1";
            plant2.Name = "Plant 2";
            plant3.Name = "Plant 3";

            plant1.Start();
            plant2.Start();
            plant3.Start();

            //Create a test dealer
            Dealer dealership = new Dealer();
            //Subscribe dealer to the price cut event
            carPlant1.priceCut += new priceCutEvent(dealership.carsOnSale);
            carPlant2.priceCut += new priceCutEvent(dealership.carsOnSale);
            carPlant3.priceCut += new priceCutEvent(dealership.carsOnSale);

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
