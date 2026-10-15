using System.Globalization;

namespace Calculator
{
    /// <summary>
    ///  Calculator logic, kept separate from the form so it can be unit tested.
    ///  Behaves like the classic Windows calculator (standard mode).
    /// </summary>
    public class CalculatorEngine
    {
        public const string DivideByZeroMessage = "Cannot divide by zero.";
        public const string InvalidInputMessage = "Invalid input for function.";
        private const int MaxDigits = 16;

        private double _accumulator;
        private string? _pendingOperator;
        private double _lastOperand;
        private string? _lastOperator;
        private bool _startNewEntry = true;
        private bool _operatorJustPressed;

        public string Display { get; private set; } = "0";
        public double Memory { get; private set; }
        public bool HasMemory => Memory != 0;
        public bool IsError { get; private set; }

        public double CurrentValue => double.Parse(Display, CultureInfo.InvariantCulture);

        public void InputDigit(int digit)
        {
            if (digit < 0 || digit > 9) throw new ArgumentOutOfRangeException(nameof(digit));
            if (IsError) Clear();

            if (_startNewEntry)
            {
                Display = digit.ToString(CultureInfo.InvariantCulture);
                _startNewEntry = false;
            }
            else if (CountDigits(Display) < MaxDigits)
            {
                Display = Display == "0" ? digit.ToString(CultureInfo.InvariantCulture) : Display + digit;
            }
            _operatorJustPressed = false;
        }

        public void InputDecimalPoint()
        {
            if (IsError) Clear();

            if (_startNewEntry)
            {
                Display = "0.";
                _startNewEntry = false;
            }
            else if (!Display.Contains('.'))
            {
                Display += ".";
            }
            _operatorJustPressed = false;
        }

        public void Backspace()
        {
            if (IsError || _startNewEntry) return;

            Display = Display.Length > 1 ? Display[..^1] : "0";
            if (Display == "-" || Display == "") Display = "0";
        }

        /// <summary>CE: clears only the number currently being entered.</summary>
        public void ClearEntry()
        {
            if (IsError)
            {
                Clear();
                return;
            }
            Display = "0";
            _startNewEntry = true;
        }

        /// <summary>C: clears the whole calculation (memory is kept).</summary>
        public void Clear()
        {
            Display = "0";
            _accumulator = 0;
            _pendingOperator = null;
            _lastOperator = null;
            _lastOperand = 0;
            _startNewEntry = true;
            _operatorJustPressed = false;
            IsError = false;
        }

        public void Negate()
        {
            if (IsError) return;

            if (Display == "0" || Display == "0.") return;
            Display = Display.StartsWith('-') ? Display[1..] : "-" + Display;
        }

        /// <summary>Handles +, -, *, /.</summary>
        public void SetOperator(string op)
        {
            if (op is not ("+" or "-" or "*" or "/")) throw new ArgumentException("Unknown operator", nameof(op));
            if (IsError) return;

            // Pressing a second operator right after the first just replaces it.
            if (_operatorJustPressed && _pendingOperator != null)
            {
                _pendingOperator = op;
                return;
            }

            if (_pendingOperator != null && !_startNewEntry)
            {
                if (!TryCompute(_accumulator, _pendingOperator, CurrentValue, out double result)) return;
                _accumulator = result;
                ShowResult(result);
            }
            else
            {
                _accumulator = CurrentValue;
            }

            _pendingOperator = op;
            _lastOperator = null;
            _startNewEntry = true;
            _operatorJustPressed = true;
        }

        public void Calculate()
        {
            if (IsError) return;

            if (_pendingOperator != null)
            {
                _lastOperator = _pendingOperator;
                _lastOperand = CurrentValue;
                if (!TryCompute(_accumulator, _pendingOperator, _lastOperand, out double result)) return;
                _pendingOperator = null;
                ShowResult(result);
            }
            else if (_lastOperator != null)
            {
                // Pressing "=" again repeats the last operation.
                if (!TryCompute(CurrentValue, _lastOperator, _lastOperand, out double result)) return;
                ShowResult(result);
            }

            _startNewEntry = true;
            _operatorJustPressed = false;
        }

        public void SquareRoot()
        {
            if (IsError) return;

            double value = CurrentValue;
            if (value < 0)
            {
                SetError(InvalidInputMessage);
                return;
            }
            ShowResult(Math.Sqrt(value));
            _startNewEntry = true;
            _operatorJustPressed = false;
        }

        /// <summary>
        ///  %: turns the current entry into a percentage of the first operand,
        ///  e.g. 200 + 10 % gives 200 + 20.
        /// </summary>
        public void Percent()
        {
            if (IsError) return;

            double value = _pendingOperator != null ? _accumulator * CurrentValue / 100 : 0;
            ShowResult(value);
            _startNewEntry = true;
            _operatorJustPressed = false;
        }

        public void Reciprocal()
        {
            if (IsError) return;

            double value = CurrentValue;
            if (value == 0)
            {
                SetError(DivideByZeroMessage);
                return;
            }
            ShowResult(1 / value);
            _startNewEntry = true;
            _operatorJustPressed = false;
        }

        public void MemoryClear() => Memory = 0;

        public void MemoryRecall()
        {
            if (IsError) Clear();
            ShowResult(Memory);
            _startNewEntry = true;
            _operatorJustPressed = false;
        }

        public void MemoryStore()
        {
            if (IsError) return;
            Memory = CurrentValue;
            _startNewEntry = true;
        }

        public void MemoryAdd()
        {
            if (IsError) return;
            Memory += CurrentValue;
            _startNewEntry = true;
        }

        private bool TryCompute(double left, string op, double right, out double result)
        {
            if (op == "/" && right == 0)
            {
                SetError(DivideByZeroMessage);
                result = 0;
                return false;
            }

            result = op switch
            {
                "+" => left + right,
                "-" => left - right,
                "*" => left * right,
                "/" => left / right,
                _ => throw new InvalidOperationException()
            };
            return true;
        }

        private void ShowResult(double value)
        {
            Display = Format(value);
        }

        private void SetError(string message)
        {
            Display = message;
            IsError = true;
            _pendingOperator = null;
            _lastOperator = null;
            _startNewEntry = true;
        }

        public static string Format(double value)
        {
            // G15 hides binary rounding noise such as 0.1 + 0.2 = 0.30000000000000004.
            string text = value.ToString("G15", CultureInfo.InvariantCulture);
            return text == "-0" ? "0" : text;
        }

        private static int CountDigits(string text) => text.Count(char.IsDigit);
    }
}
