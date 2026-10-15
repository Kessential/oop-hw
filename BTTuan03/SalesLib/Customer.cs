namespace Customer
{
    public class Cust_details
    {
        public string strName;
        public void getName()
        {
            Console.WriteLine("Enter your name :");
            strName = Console.ReadLine();
        }
    }
}