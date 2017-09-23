using System;
using System.Threading;
using System.Diagnostics;

namespace WeimoPlant
{
    //Declare a delegate for the price cut event
    public delegate void priceCutEvent(String sender, Int32 cutPrice);

    public class Plant
    {
        //---------------------------------------------------------------------
        
        //Q'MARIHA ADDED THE FOLLOWING CODE

        //When the dealer tries to buy from a plant, the plant sends the account number and funds to the bank
        private Bank bank;
        private MultiCellBuffer buffer;

        //Constructor that instantiates Bank object
        public Plant(Bank bank, MultiCellBuffer buffer)
        {
            this.bank = bank;
            this.buffer = buffer;
        }

        //-------------------------------------------------------------------------

        //Link event to delegate
        public event priceCutEvent priceCut;

        //This is just a temporary solution to changing the price of the cars over time
        static Random rng = new Random();

        //Set the default dealership car price
        //This is shared between all threads
        public LowestPrice carPrice = new LowestPrice(500000);

        //This is local data unique to each thread; only the host thread can change its
        //value. No other threads can touch it
        private Int32 nextPrice;
        private Int32 priceCutEventsLeft = 6;

        //Returns the current price of cars
        public Int32 getPrice() { return carPrice.getLowPrice(); }

        //Change the price of the car 
        public void changePrice(Int32 newPrice)
        {
            //If the new price is lower than the current lowest price
            if (newPrice < carPrice.getLowPrice()) {
                //Lock the car price object to prevent interrupts when changing the value
                
                lock (carPrice) {
                    if (priceCut != null) {                             //If an event exists
                        priceCut(Thread.CurrentThread.Name, newPrice);  //Announce a price cut event
                        carPrice.setLowPrice(newPrice);                 //Set the new low car price
                        priceCutEventsLeft -= 1;
                    }
                }
            }
        }

        //This is the function that the threads execute
        //Every half second, generates and sets a new car price
        public void Run()
        {
            //Subscribe to dealer purchase events

            //Instantiate each thread's local price to $500,000
            nextPrice = carPrice.getLowPrice();

            //Perform a loop to simulate real-time operation
            while (priceCutEventsLeft > 0) {
                Thread.Sleep(500);      //Every half second

                //Generate a new local price for each thread
                nextPrice = nextPrice + rng.Next(-100, 100);

                //By the way, Console.WriteLine works now. Don't know why it
                //didn't work earlier
                Console.WriteLine("Local Plant {0} Price is ${1}, {2} events left", Thread.CurrentThread.Name ,nextPrice, priceCutEventsLeft);

                //Check to see if the overall lowest value should be updated
                changePrice(nextPrice);
            }

            Console.WriteLine("Local Plant {0} has closed.", Thread.CurrentThread.Name);
        }

        //For dealers to subscribe themselves to the price cut event
        public void subscribe(Dealer dealer)
        {
            priceCut += new priceCutEvent(dealer.PriceChanged);
        }
    }

    //Thread locks cannot be performed on primitive data types, so we must
    //place the price value into an object container in order to create the
    //critical section
    public class LowestPrice
    {
        //Default starting price for the cars for all plants
        private Int32 lowPrice = 500000;

        //Constructor
        public LowestPrice(Int32 price) { this.lowPrice = price; }

        //Get overall lowest price
        public Int32 getLowPrice() { return this.lowPrice; }

        //Set overall lowest price
        public void setLowPrice(Int32 newPrice) { this.lowPrice = newPrice; }
    }
}