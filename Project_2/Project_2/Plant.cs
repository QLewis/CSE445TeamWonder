using System;
using System.Threading;
using System.Diagnostics;
using Proj_2_Attempt_2.EncryptDecrypt;

namespace WeimoPlant
{
    //Declare a delegate for the price cut event
    public delegate void priceCutEvent(String sender, Int32 cutPrice);
    public delegate void orderProcessedEvent(Order order, string validation);

    public class Plant
    {
        //When the dealer tries to buy from a plant, the plant sends the account number and funds to the bank
        private Bank bank;
        private MultiCellBuffer buffer;
        
        //Link event to delegate
        public event priceCutEvent priceCut;
        public event orderProcessedEvent orderComplete;

        //Set the default dealership car price
        //This is shared between all threads
        public static LowestPrice carPrice = new LowestPrice(500000);

        //This is local data unique to each thread; only the host thread can change these
        //value. No other threads can touch them
        private Int32 nextPrice;
        private Int32 priceCutEventsLeft = 20;
        private double timeSaleModifier = 0.0;
        private Int32 carsInStock = 100;

		//Constructor that instantiates Bank and Multi-Cell Buffer objects
		public Plant(Bank bank, MultiCellBuffer buffer)
		{
			this.bank = bank;
			this.buffer = buffer;
		}

        //Returns the current price of cars
        //public Int32 getPrice() { return carPrice.getLowPrice(); }

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

        public void sendOrderCompleteEvent(Order _order, string _validation)
        {
            if (orderComplete != null) {
                orderComplete(_order, _validation);
            }

            if (_validation.Equals("valid")) {
                timeSaleModifier = 0.0;
                carsInStock -= _order.Amount;
                lock (carPrice) {
                    carPrice.setLowPrice((int)(carPrice.getLowPrice() * 1.05));   
                }
            }
        }

        public int newPrice(Double timeMod, int stock)
        {
            double price = (carPrice.getLowPrice()) / ((stock / 100) + timeMod);
            return (int)price;
        }

        public void RunProcessOrders() {
            while(true) {
				String orderString = buffer.getOneCell();
				if (orderString != null)
				{
					OrderProcessor processor = new OrderProcessor(this, this.bank, orderString);
					Thread orderProcessing = new Thread(new ThreadStart(processor.ProcessOrderString));
					orderProcessing.Start();
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
                Thread.Sleep(500);          //Every half second
                timeSaleModifier += 0.01;   
                carsInStock += 2;           //Manufacture 2 cars

                //Generate a new local price for each thread
                nextPrice = newPrice(timeSaleModifier, carsInStock);

                //By the way, Console.WriteLine works now. Don't know why it
                //didn't work earlier
                Console.WriteLine("{0} Price is ${1}, {2} events left", Thread.CurrentThread.Name ,nextPrice, priceCutEventsLeft);

                //Check to see if the overall lowest value should be updated
                changePrice(nextPrice);
            }

            Console.WriteLine("{0} has closed.", Thread.CurrentThread.Name);
        }

        //For dealers to subscribe themselves to the price cut event
        public void subscribe(Dealer dealer)
        {
            priceCut += new priceCutEvent(dealer.PriceChanged);
            orderComplete += new orderProcessedEvent(dealer.ConfirmOrder);
        }

    }

    //Thread locks cannot be performed on primitive data types, so we must
    //place the price value into an object container in order to create the
    //critical section
    //LowestPrice: The lowest price among all of the plant threads
    public class LowestPrice
    {
        //Default starting price for the cars for all plants
        private Int32 lowPrice;

        //Constructor
        public LowestPrice(Int32 price) { this.lowPrice = price; }

        public Int32 getLowPrice() { return this.lowPrice; }
        public void setLowPrice(Int32 newPrice) { this.lowPrice = newPrice; }
    }

    class OrderProcessor
    {
        private Plant plant;
        private Bank bank;
        private string orderString;

        public OrderProcessor(Plant _plant, Bank _bank, string _orderString)
        {
            this.plant = _plant;
            this.bank = _bank;
            this.orderString = _orderString;
        }

        public void ProcessOrderString()
        {
            Order order = Order.Decode(this.orderString);

            string encryptedCCString = encryptCC(order.CardNum);

            string validation = bank.validateCC(encryptedCCString, (order.Amount * order.UnitPrice * 1.08) + (order.Amount * 700));
            plant.sendOrderCompleteEvent(order, validation);
        }

		public string encryptCC(Int32 cardNum)
		{
            ServiceClient encryptService = null;
            for (int i = 0; i < 10 && encryptService == null; i++) {
				try
				{
					encryptService = new ServiceClient();
				}
				catch
				{
                    Console.WriteLine("Error connecting to encryption service");
				}    
            }

			//Console.WriteLine("String to be encrypted: {0}", cardNum);

			string encryptedCC = encryptService.Encrypt(cardNum.ToString());
			//Console.WriteLine("Encrypted string: {0}", encryptedCC);

			return encryptedCC;
		}
    }
}