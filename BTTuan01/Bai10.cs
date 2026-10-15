using System;

namespace BTTuan01
{
    // Part III - Do it yourself Exercise 1: SIN Validator (Canadian Social Insurance Number)
    //
    // Algorithm (from the handout, worked example SIN 193456787, check digit 7):
    //   - Split the number into digits; the last digit is the check digit.
    //   - Walk the remaining digits from right to left. The digit immediately left of the
    //     check digit, and every second digit after that going left, gets DOUBLED and its
    //     result's own digits are summed (e.g. digit 8 -> 16 -> 1+6=7); these digit-sums are
    //     added together into sumA.
    //   - The other digits (the ones in between) are added as-is into sumB.
    //   - total = sumA + sumB. Find the next multiple of 10 that is >= total (if total is
    //     already a multiple of 10, that counts as itself, difference 0).
    //   - The number is a valid SIN iff (nextMultipleOf10 - total) == checkDigit.
    // Verified by hand: SIN 193456787 -> sumA=27, sumB=16, total=43, next multiple=50,
    // diff=7 == check digit 7 -> valid. SIN 193456788 -> same total=43 (only the check
    // digit changed), diff=7 != check digit 8 -> not valid. Matches the handout's example.
    public static class Bai10
    {
        public static bool KiemTraSIN(string sinText)
        {
            int n = sinText.Length;
            int[] digit = new int[n];
            for (int i = 0; i < n; i++)
                digit[i] = sinText[i] - '0';

            int checkDigit = digit[n - 1];
            int sumA = 0;
            int sumB = 0;

            for (int i = n - 2; i >= 0; i--)
            {
                int posFromCheck = (n - 1) - i; // 1 = immediately left of check digit
                if (posFromCheck % 2 == 1)
                {
                    int doubled = digit[i] * 2;
                    sumA += (doubled / 10) + (doubled % 10);
                }
                else
                {
                    sumB += digit[i];
                }
            }

            int total = sumA + sumB;
            int nextMultipleOf10 = (total % 10 == 0) ? total : ((total / 10) + 1) * 10;
            int diff = nextMultipleOf10 - total;

            return diff == checkDigit;
        }

        public static void Chay()
        {
            Console.WriteLine("=== BAI 10: SIN VALIDATOR (DO IT YOURSELF) ===");
            Console.WriteLine("SIN Validator");
            Console.WriteLine("=============");

            while (true)
            {
                Console.Write("SIN (0 to quit): ");
                string? line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) break;
                line = line.Trim();

                if (line == "0")
                {
                    Console.WriteLine("Have a Nice Day!");
                    break;
                }

                bool hopLe = line.Length >= 2;
                foreach (char c in line)
                {
                    if (!char.IsDigit(c)) { hopLe = false; break; }
                }

                if (!hopLe)
                {
                    Console.WriteLine("This is not a valid SIN.");
                    continue;
                }

                if (KiemTraSIN(line))
                    Console.WriteLine("This is a valid SIN.");
                else
                    Console.WriteLine("This is not a valid SIN.");
            }
        }
    }
}
