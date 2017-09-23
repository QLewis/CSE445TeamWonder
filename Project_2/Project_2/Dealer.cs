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
        private Bank bank;
        private MultiCellBuffer buffer;

        //Constructor that instantiates Bank object
        public Dealer(Bank bank, MultiCellBuffer buffer)
        {
            this.bank = bank;
            this.buffer = buffer;
        }

        public void Run() {
            // thread maybe
        }

		//Called only when a Dealership receives notification of a price cut
		//event from Plant.cs
		public void PriceChanged(String sender, Int32 price) {
            Console.WriteLine("Manufacturing Plant {0}'s cars have a new low price: ${1} each", Thread.CurrentThread.Name, price);

            Order order = new Order();
            order.SenderID = "dealer";
            order.Amount = 1;
            order.CardNum = bank.applyForCC();
            order.RecieverID = "plant";
            order.UnitPrice = price;
            order.TimeStamp = System.DateTime.Now;
            String orderString = Order.Encode(order);

            //TODO: add to buffer

        }
    }
}
