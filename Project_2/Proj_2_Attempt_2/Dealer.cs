using System;
using System.Threading;
using WeimoPlant;

/* This is just a test class. All it does is provide a simple dealer class to consume the
 * events broadcast by Plant.cs in order to test.
 */

namespace WeimoPlant
{
    public class Dealer
    {
        //This is the function that the threads execute
        public void dealerFunc()
        {
            //Create a new plant object
            Plant car = new Plant();

            //Every one second, querry the manufactuerer manually for the price
            for (Int32 i = 0; i < 10; i++) {
                Thread.Sleep(1000);
                Int32 carPrice = car.getPrice();
                Console.WriteLine("Dealer {0} wants to buy a car for ${1}", Thread.CurrentThread.Name, carPrice);
            }
        }

        //Called only when a Dealership receives notification of a price cut
        //event from Plant.cs
        public void carsOnSale(Int32 salePrice)
        {
            Console.WriteLine("Manufacturing Plant {0}'s cars have a new low price: ${1} each", Thread.CurrentThread.Name, salePrice);
        }
    }
}
