using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeimoPlant
{
    public class Bank
    {
        LinkedList<ccListItem> ccNumbers = new LinkedList<ccListItem>();

        //The Random class is used to generate random numbers
        Random random = new Random();

        /* This function generates a random 3-digit number from 900 to 999
         * It also generates a random amount of funds from 1 million to 9,999,999
         * It then adds this data to the linked lits
         * 
         * TODO: Ensure that there are no duplicate numbers
         */

        public int applyForCC()
        {
            ccListItem newItem = new ccListItem();
            newItem.cardNumber = random.Next(900, 1000);
            newItem.funds = random.Next(1000000, 10000000);
            ccNumbers.AddLast(newItem);
            return newItem.cardNumber;
        }

        //This function checks to see if the credit card accound exists
        //TODO: Add functionality to decrypt string
        public string validateCC(int cardNumber, double funds)
        {
            foreach (ccListItem ccLI in ccNumbers)
            {
                if (ccLI.cardNumber == cardNumber)
                {
                    if (ccLI.funds >= funds)
                    {
                        return "valid";
                    }
                }
            }
            return "invalid";
        }

        //This function enables account holders to deposit funds into their account
        public void depositFunds(int cardNumber, double funds)
        {
            foreach (ccListItem ccLI in ccNumbers)
            {
                if (ccLI.cardNumber == cardNumber)
                {
                    ccLI.funds += funds;
                }
            }
        }
    }
    
    //Created a specific node to be used with the LinkedList class
    class ccListItem
    {
        public int cardNumber;
        public double funds;
    }


}
