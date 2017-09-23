using System;
using System.Threading;
using System.Diagnostics;

namespace WeimoPlant
{
    //Declare a delegate for the price cut event
    public delegate void priceCutEvent(Int32 cutPrice);

    class Plant
    {
        //---------------------------------------------------------------------
        
        //Q'MARIHA ADDED THE FOLLOWING CODE

        //When the dealer tries to buy from a plant, the plant sends the account number and funds to the bank
        private Bank bank;

        //Constructor that instantiates Bank object
        public Plant(Bank bank)
        {
            this.bank = bank;
        }

        //-------------------------------------------------------------------------

        //Link event to delegate
        public static event priceCutEvent priceCut;

        //This is just a temporary solution to changing the price of the cars over time
        static Random rng = new Random();

        //Set the default dealership car price
        //This is shared between all threads
        public static LowestPrice carPrice = new LowestPrice(500000);

        //This is local data unique to each thread; only the host thread can change its
        //value. No other threads can touch it
        private ThreadLocal<Int32> nextPrice = new ThreadLocal<Int32>();

        //Returns the current price of cars
        public Int32 getPrice() { return carPrice.getLowPrice(); }

        //Change the price of the car 
        public static void changePrice(Int32 newPrice)
        {
            //If the new price is lower than the current lowest price
            if (newPrice < carPrice.getLowPrice()) {
                //Lock the car price object to prevent interrupts when changing the value
                lock (carPrice) {
                    if (priceCut != null) {                 //If an event exists
                        priceCut(newPrice);                 //Announce a price cut event
                        carPrice.setLowPrice(newPrice);     //Set the new low car price
                    }
                }
            }
        }

        //This is the function that the threads execute
        //Every half second, generates and sets a new car price
        public void plantFunc()
        {
            //Instantiate each thread's local price to $500,000
            nextPrice.Value = carPrice.getLowPrice();

            //Perform a loop to simulate real-time operation
            for (Int32 i = 0; i < 25; i++) {
                Thread.Sleep(500);      //Every half second

                //Generate a new local price for each thread
                nextPrice.Value = nextPrice.Value + rng.Next(-100, 100);

                //By the way, Console.WriteLine works now. Don't know why it
                //didn't work earlier
                //Console.WriteLine("Local Plant {0} Price is ${1}", Thread.CurrentThread.Name ,nextPrice);

                //Check to see if the overall lowest value should be updated
                Plant.changePrice(nextPrice.Value);
            }
        }
    }

    //Thread locks cannot be performed on primitive data types, so we must
    //place the price value into an object container in order to create the
    //critical section
    class LowestPrice
    {
        //Default starting price for the cars for all plants
        private Int32 lowPrice = 500000;

        //Constructor
        public LowestPrice(int price)
        {
            this.lowPrice = price;
        }

        //Get overall lowest price
        public int getLowPrice()
        {
            return this.lowPrice;
        }

        //Set overall lowest price
        public void setLowPrice(Int32 newPrice)
        {
            this.lowPrice = newPrice;
        }
    }
}