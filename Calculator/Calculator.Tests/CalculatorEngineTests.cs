using Calculator;

namespace Calculator.Tests
{
    /// <summary>
    ///  10 test case kiểm tra chức năng của ứng dụng Calculator.
    ///  Mỗi test mô phỏng chuỗi nút người dùng bấm, rồi so sánh nội dung màn hình.
    /// </summary>
    [TestClass]
    public sealed class CalculatorEngineTests
    {
        private CalculatorEngine _calc = null!;

        [TestInitialize]
        public void Setup() => _calc = new CalculatorEngine();

        /// <summary>
        ///  Bấm lần lượt các nút, cách nhau bởi dấu cách. Số nhiều chữ số được bấm từng chữ số.
        ///  Ví dụ: "12 + 7 =" bấm 1, 2, +, 7, =.
        /// </summary>
        private void Press(string keys)
        {
            foreach (string key in keys.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                switch (key)
                {
                    case "+" or "-" or "*" or "/": _calc.SetOperator(key); break;
                    case "=": _calc.Calculate(); break;
                    case "sqrt": _calc.SquareRoot(); break;
                    case "%": _calc.Percent(); break;
                    case "1/x": _calc.Reciprocal(); break;
                    case "+/-": _calc.Negate(); break;
                    case "Backspace": _calc.Backspace(); break;
                    case "CE": _calc.ClearEntry(); break;
                    case "C": _calc.Clear(); break;
                    case "MC": _calc.MemoryClear(); break;
                    case "MR": _calc.MemoryRecall(); break;
                    case "MS": _calc.MemoryStore(); break;
                    case "M+": _calc.MemoryAdd(); break;
                    default:
                        foreach (char c in key)
                        {
                            if (c == '.') _calc.InputDecimalPoint();
                            else _calc.InputDigit(c - '0');
                        }
                        break;
                }
            }
        }

        // TC01 - Phép cộng hai số nguyên: 12 + 7 = 19
        [TestMethod]
        public void TC01_Addition()
        {
            Press("12 + 7 =");
            Assert.AreEqual("19", _calc.Display);
        }

        // TC02 - Phép trừ cho kết quả âm, sau đó đổi dấu bằng +/-: 5 - 9 = -4 -> 4
        [TestMethod]
        public void TC02_SubtractionNegativeResultAndNegate()
        {
            Press("5 - 9 =");
            Assert.AreEqual("-4", _calc.Display);

            Press("+/-");
            Assert.AreEqual("4", _calc.Display);
        }

        // TC03 - Phép nhân với số thập phân: 2.5 * 4 = 10
        [TestMethod]
        public void TC03_MultiplicationWithDecimal()
        {
            Press("2.5 * 4 =");
            Assert.AreEqual("10", _calc.Display);
        }

        // TC04 - Phép chia, và chia cho 0 phải báo lỗi: 7 / 2 = 3.5 ; 9 / 0 = lỗi
        [TestMethod]
        public void TC04_DivisionAndDivideByZero()
        {
            Press("7 / 2 =");
            Assert.AreEqual("3.5", _calc.Display);

            Press("C 9 / 0 =");
            Assert.IsTrue(_calc.IsError);
            Assert.AreEqual(CalculatorEngine.DivideByZeroMessage, _calc.Display);
        }

        // TC05 - Chuỗi phép tính liên tiếp tính từ trái sang phải, bấm "=" lặp lại phép cuối:
        // 2 + 3 * 4 = 20 ; bấm "=" lần nữa -> 80
        [TestMethod]
        public void TC05_ChainedOperationsAndRepeatEquals()
        {
            Press("2 + 3 *");
            Assert.AreEqual("5", _calc.Display); // kết quả trung gian hiện ra khi bấm phép tiếp theo

            Press("4 =");
            Assert.AreEqual("20", _calc.Display);

            Press("=");
            Assert.AreEqual("80", _calc.Display);
        }

        // TC06 - Căn bậc hai: sqrt(144) = 12 ; sqrt(-9) báo lỗi
        [TestMethod]
        public void TC06_SquareRoot()
        {
            Press("144 sqrt");
            Assert.AreEqual("12", _calc.Display);

            Press("C 9 +/- sqrt");
            Assert.IsTrue(_calc.IsError);
            Assert.AreEqual(CalculatorEngine.InvalidInputMessage, _calc.Display);
        }

        // TC07 - Phần trăm: 200 + 10 % = 220 (10% của 200 là 20)
        [TestMethod]
        public void TC07_Percent()
        {
            Press("200 + 10 %");
            Assert.AreEqual("20", _calc.Display);

            Press("=");
            Assert.AreEqual("220", _calc.Display);
        }

        // TC08 - Nghịch đảo: 1/x của 8 = 0.125 ; 1/x của 0 báo lỗi
        [TestMethod]
        public void TC08_Reciprocal()
        {
            Press("8 1/x");
            Assert.AreEqual("0.125", _calc.Display);

            Press("C 0 1/x");
            Assert.IsTrue(_calc.IsError);
        }

        // TC09 - Backspace, CE và C:
        // 123 Backspace -> 12 ; 5 + 9 CE 3 = 8 ; C -> 0
        [TestMethod]
        public void TC09_BackspaceClearEntryAndClear()
        {
            Press("123 Backspace");
            Assert.AreEqual("12", _calc.Display);

            Press("C 5 + 9 CE 3 =");
            Assert.AreEqual("8", _calc.Display); // CE chỉ xoá số 9, vẫn giữ phép 5 +

            Press("C");
            Assert.AreEqual("0", _calc.Display);
            Assert.IsFalse(_calc.IsError);
        }

        // TC10 - Bộ nhớ MS, M+, MR, MC:
        // 5 MS, 3 M+ -> bộ nhớ = 8 ; C rồi MR -> 8 ; MC -> bộ nhớ trống
        [TestMethod]
        public void TC10_MemoryFunctions()
        {
            Press("5 MS");
            Assert.IsTrue(_calc.HasMemory);

            Press("3 M+");
            Assert.AreEqual(8, _calc.Memory);

            Press("C MR");
            Assert.AreEqual("8", _calc.Display);

            Press("MC");
            Assert.IsFalse(_calc.HasMemory);
            Assert.AreEqual(0, _calc.Memory);
        }
    }
}
