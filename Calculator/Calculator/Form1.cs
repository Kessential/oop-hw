namespace Calculator
{
    public partial class Form1 : Form
    {
        private readonly CalculatorEngine _engine = new();

        public Form1()
        {
            InitializeComponent();
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            string text = _engine.Display;
            // Like the classic calculator, whole numbers are shown with a trailing point: "0."
            if (!_engine.IsError && !text.Contains('.') && !text.Contains('E'))
                text += ".";
            txtDisplay.Text = text;
            lblMemory.Text = _engine.HasMemory ? "M" : "";
        }

        private void Run(Action action)
        {
            action();
            UpdateDisplay();
            ActiveControl = null; // keep Enter from re-clicking the last focused button
        }

        private void Digit_Click(object? sender, EventArgs e) =>
            Run(() => _engine.InputDigit(int.Parse(((Button)sender!).Text)));

        private void Operator_Click(object? sender, EventArgs e) =>
            Run(() => _engine.SetOperator(((Button)sender!).Text));

        private void Decimal_Click(object? sender, EventArgs e) => Run(_engine.InputDecimalPoint);
        private void Equals_Click(object? sender, EventArgs e) => Run(_engine.Calculate);
        private void Backspace_Click(object? sender, EventArgs e) => Run(_engine.Backspace);
        private void ClearEntry_Click(object? sender, EventArgs e) => Run(_engine.ClearEntry);
        private void Clear_Click(object? sender, EventArgs e) => Run(_engine.Clear);
        private void Negate_Click(object? sender, EventArgs e) => Run(_engine.Negate);
        private void Sqrt_Click(object? sender, EventArgs e) => Run(_engine.SquareRoot);
        private void Percent_Click(object? sender, EventArgs e) => Run(_engine.Percent);
        private void Reciprocal_Click(object? sender, EventArgs e) => Run(_engine.Reciprocal);
        private void MemoryClear_Click(object? sender, EventArgs e) => Run(_engine.MemoryClear);
        private void MemoryRecall_Click(object? sender, EventArgs e) => Run(_engine.MemoryRecall);
        private void MemoryStore_Click(object? sender, EventArgs e) => Run(_engine.MemoryStore);
        private void MemoryAdd_Click(object? sender, EventArgs e) => Run(_engine.MemoryAdd);

        private void Form1_KeyPress(object? sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (char.IsDigit(c)) Run(() => _engine.InputDigit(c - '0'));
            else if (c is '+' or '-' or '*' or '/') Run(() => _engine.SetOperator(c.ToString()));
            else if (c is '.' or ',') Run(_engine.InputDecimalPoint);
            else if (c is '=' or '\r') Run(_engine.Calculate);
            else if (c == '%') Run(_engine.Percent);
            else if (c == '\b') Run(_engine.Backspace);
            else if (c == (char)Keys.Escape) Run(_engine.Clear);
            else return;
            e.Handled = true;
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                Run(_engine.ClearEntry);
                e.Handled = true;
            }
        }
    }
}
