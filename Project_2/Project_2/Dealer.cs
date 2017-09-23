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

        //Constructor that instantiates Bank object
        public Dealer(Bank bank)
        {
            this.bank = bank();
        }
      
        public void PriceChanged(Int32 price) {
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

        //Called only when a Dealership receives notification of a price cut
        //event from Plant.cs
        public void carsOnSale(Int32 salePrice)
        {
            Console.WriteLine("Manufacturing Plant {0}'s cars have a new low price: ${1} each", Thread.CurrentThread.Name, salePrice);
        }
    }
}
