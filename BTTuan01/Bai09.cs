using System;

namespace BTTuan01
{
    // Exercise 5 (variant): Array of Object - Employee
    public class Employee
    {
        private int empID;

        public Employee(int empID)
        {
            this.empID = empID;
        }

        public override string ToString()
        {
            return empID.ToString();
        }
    }

    public class Bai09Runner
    {
        public void Run()
        {
            int[] intArray;
            Employee[] empArray;
            intArray = new int[5];
            empArray = new Employee[3];

            // populate the array
            for (int i = 0; i < empArray.Length; i++)
            {
                empArray[i] = new Employee(i + 5);
            }

            Console.WriteLine("The int array...");
            for (int i = 0; i < intArray.Length; i++)
            {
                Console.WriteLine(intArray[i].ToString());
            }

            Console.WriteLine("\nThe employee array...");
            for (int i = 0; i < empArray.Length; i++)
            {
                Console.WriteLine(empArray[i].ToString());
            }
        }
    }

    public static class Bai09
    {
        public static void Chay()
        {
            Console.WriteLine("=== BAI 09: ARRAY OF OBJECT - EMPLOYEE ===");
            Bai09Runner arr = new Bai09Runner();
            arr.Run();
        }
    }
}
