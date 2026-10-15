namespace GeoGebraApp;

/// <summary>Hộp thoại nhỏ nhập một số thực dương (dùng cho công cụ Đường tròn biết bán kính).</summary>
public class NumberInputDialog : Form
{
    private readonly NumericUpDown _input;

    public NumberInputDialog(string title, string prompt, double defaultValue)
    {
        Text = title;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Font = new Font("Segoe UI", 9.5f);

        var layout = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            Padding = new Padding(12),
            Dock = DockStyle.Fill
        };

        var label = new Label { Text = prompt, AutoSize = true, Margin = new Padding(0, 0, 0, 6) };

        _input = new NumericUpDown
        {
            DecimalPlaces = 2,
            Increment = 0.5m,
            Minimum = 0.01m,
            Maximum = 10000m,
            Value = (decimal)Math.Clamp(defaultValue, 0.01, 10000),
            Width = 260,
            Margin = new Padding(0, 0, 0, 10)
        };

        var buttons = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.RightToLeft,
            AutoSize = true,
            Dock = DockStyle.Fill,
            Margin = Padding.Empty
        };
        var cancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, AutoSize = true };
        var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, AutoSize = true };
        buttons.Controls.Add(cancel);
        buttons.Controls.Add(ok);

        layout.Controls.Add(label);
        layout.Controls.Add(_input);
        layout.Controls.Add(buttons);
        Controls.Add(layout);

        AcceptButton = ok;
        CancelButton = cancel;
        Shown += (_, _) => _input.Select(0, _input.Text.Length);
    }

    public double Value => (double)_input.Value;

    /// <summary>Hiện hộp thoại; trả về null nếu người dùng hủy.</summary>
    public static double? Ask(IWin32Window owner, string title, string prompt, double defaultValue)
    {
        using var dlg = new NumberInputDialog(title, prompt, defaultValue);
        return dlg.ShowDialog(owner) == DialogResult.OK ? dlg.Value : null;
    }
}
