using System;
using System.Threading;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace WeimoPlant
{
    public class Order
    {
        public String SenderID { get; set; }
        public Int32 CardNum { get; set; }
        public String RecieverID { get; set; }
        public Int32 Amount { get; set; }
        public Int32 UnitPrice { get; set; }
        public DateTime TimeStamp { get; set; }

        public static String Encode(Order order) {
            XmlSerializer orderSerializer = new XmlSerializer(typeof(Order));
            StringBuilder stringBuilder = new StringBuilder();

            TextWriter writer = null;
            try {
                writer = new StringWriter(stringBuilder);
                orderSerializer.Serialize(writer, order);
            }finally {
                if(writer != null) {
                    writer.Close();
                }
            }
            return stringBuilder.ToString();
        }

        public static Order Decode(String str)
        {
            XmlSerializer orderSerializer = new XmlSerializer(typeof(Order));

            using (TextReader reader = new StringReader(str)) {
                return (Order)orderSerializer.Deserialize(reader);
            }
        }
    }
}