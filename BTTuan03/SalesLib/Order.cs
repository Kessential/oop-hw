using Customer;

namespace Order
{
    public class Grocery_items
    {
        public void Ord_grocery()
        {
            Cust_details objCust1 = new Cust_details();
            objCust1.getName();
            Console.WriteLine("Hello {0}", objCust1.strName);
            Console.WriteLine("You have ordered grocery items");
        }
    }
    public class Bakery_items
    {
        public void Ord_bakery()
        {
            Cust_details objCust2 = new Cust_details();
            objCust2.getName();
            Console.WriteLine("Hello {0}", objCust2.strName);
            Console.WriteLine("You have ordered bakery items");
        }
    }
}