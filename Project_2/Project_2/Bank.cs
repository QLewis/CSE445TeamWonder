using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Proj_2_Attempt_2.EncryptDecrypt;
using WeimoPlant;

namespace WeimoPlant
{
    public class Bank
    {
        LinkedList<ccListItem> ccNumbers = new LinkedList<ccListItem>();

        //The Random class is used to generate random numbers
        Random random = new Random();

        /* This function will go through the list of existing credit card numbers
         * to ensure that there are no duplicates
         */
        bool noRepeat(int input)
        {
            foreach(ccListItem ccLI in ccNumbers)
            {
                if (ccLI.cardNumber == input)
                {
                    return false;
                }
            }
            return true;
        }

        /* This function generates a random 3-digit number from 900 to 999
        * It also generates a random amount of funds from 1 million to 9,999,999
        * It then adds this data to the linked list
        */
        public int applyForCC()
        {
            ccListItem newItem = new ccListItem();
            //newItem.cardNumber = random.Next(900, 1000);
            int input = random.Next(900, 1000);

            //make sure the number is not already being used
            if (noRepeat(input) == true)
            {
                newItem.cardNumber = input;
            }

            newItem.funds = random.Next(1000000, 10000000);
            ccNumbers.AddLast(newItem);

            return newItem.cardNumber;
        }

        //This function checks to see if the credit card accound exists
        public string validateCC(String encryptedCardNumber, double funds)
        {
            Int32 cardNumber = DecryptCardNumber(encryptedCardNumber);

            bool foundCreditCard = false;
            foreach (ccListItem ccLI in ccNumbers)
            {
                if (ccLI.cardNumber == cardNumber)
                {
                    foundCreditCard = true;
                    if (ccLI.funds >= funds)
                    {
                        ccLI.funds -= funds;
                        //Console.WriteLine("Credit card account: {0}, New funds: ${1}", cardNumber, funds);
                        return "valid";
                    }
                    Console.WriteLine("ERROR -- insufficient funds");
                }
            }
            if(!foundCreditCard) {
                Console.WriteLine("ERROR -- credit card account does not exist");    
            }

            return "invalid";
        }

        //This function enables account holders to deposit funds into their account
        public void depositFunds(String encryptedCardNumber, double funds)
        {
            Int32 cardNumber = DecryptCardNumber(encryptedCardNumber);

            foreach (ccListItem ccLI in ccNumbers)
            {
                if (ccLI.cardNumber == cardNumber)
                {
                    ccLI.funds += funds;
                    Console.WriteLine("${0} has been deposited into account {1}, new funds: ${2}", funds, cardNumber, ccLI.funds);
                }
            }
        }

        //Decrypt the CardNumber
		public Int32 DecryptCardNumber(String encryptedNum)
		{
			ServiceClient decryptService = null;
			for (int i = 0; i < 10 && decryptService == null; i++)
			{
				try
				{
					decryptService = new ServiceClient();
				}
				catch
				{
					Console.WriteLine("Error connecting to decryption service");
				}
			}

			String cardNum = decryptService.Decrypt(encryptedNum);
			Int32 cardNumber = Int32.Parse(cardNum);
			return cardNumber;
		}
    }

    //Created a specific node to be used with the LinkedList class
    class ccListItem
    {
        public int cardNumber;
        public double funds;
    }
}
