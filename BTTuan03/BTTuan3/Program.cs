using Order;
class OrderTest
{
    public static void Main()
    {
        string choice;
        Console.WriteLine("What would you like to order? 1-Grocery Items, 2-Bakery Items"); 
       
        choice = Console.ReadLine();
        if (choice == "1")
        {
            Grocery_items objGrocery = new Grocery_items();
            objGrocery.Ord_grocery();
        }
        else
        {
            if (choice == "2")
            {
                Bakery_items objBakery = new Bakery_items();
                // C# - Lab 5 – Namespace and Exception Handling 
                // © 2009 FPT Aptech 4
                objBakery.Ord_bakery();
            }
            else
            {
                Console.WriteLine("Enter either 1 or 2");
            }
        }
        Console.ReadLine();
    }
}
