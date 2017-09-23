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
        private Int32 cardNum;
        private String name;
        private volatile Int32 need;


        //Constructor that instantiates Bank object and the cardNum
        public Dealer(String name, Bank bank, MultiCellBuffer buffer)
        {
            this.name = name;
            this.bank = bank;
            this.buffer = buffer;
            this.cardNum = bank.applyForCC();
        }

        //Determines the need and amount of cars needed at time of price cuts
        public void Run() {
            while(true) {
                need++;
                Thread.Sleep(600);
            }
        }

		//Called only when a Dealership receives notification of a price cut
		//event from Plant.cs
        //Creates an order and encodes it before sending the order to the MultiCellBuffer
		public void PriceChanged(String sender, Int32 price) {
            Int32 currentNeed = need;
            if (IsGoodPrice(price))
            {
                Console.WriteLine("{0}: Creating new order with price {1}", name, price);

                Order order = new Order();
                order.SenderID = name;
                order.Amount = currentNeed;
                order.CardNum = cardNum;
                order.RecieverID = sender;
                order.UnitPrice = price;
                order.TimeStamp = System.DateTime.Now;
                String orderString = Order.Encode(order);

                buffer.setOneCell(orderString);
            }
        }

        //Determines the amount of cars in the order and resets need ot zero
        private Int32 oldPrice = -1;
        private bool IsGoodPrice(Int32 price) {
            //price has to be 10% lower than the old price for an order to be made
            if ((oldPrice == -1 || price < (oldPrice*0.9)) && need > 3)
            {
                oldPrice = price;
                need = 0;
                return true;
            }

            return false;
        }

        //Confirm the orders that have been completed by OrderProcessing
        public void ConfirmOrder() {
            //TODO: get the params from OrderProcessing
            DateTime confirmTime = DateTime.Now;
            Console.WriteLine("Order confirmed at " + confirmTime);
        }
    }
}
