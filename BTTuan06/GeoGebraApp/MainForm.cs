using GeoLib2D.Core;
using GeoLib2D.Objects;

namespace GeoGebraApp;

/// <summary>
/// Câu 10d: Cửa sổ chính — MenuStrip, ToolStrip chọn công cụ, cột Đại số (TreeView) bên trái,
/// vùng vẽ GeoCanvasPanel bên phải và StatusStrip. Giao diện dựng hoàn toàn bằng code.
/// </summary>
public class MainForm : Form
{
    private const string FreeRootText = "Đối tượng tự do (Free Objects)";
    private const string DependentRootText = "Đối tượng phụ thuộc (Dependent Objects)";
    private const string BlankImageKey = "blank";

    private readonly GeoCanvasPanel _canvas;
    private readonly BufferedTreeView _tree;
    private readonly TreeNode _freeRoot;
    private readonly TreeNode _dependentRoot;
    private readonly ImageList _swatches;
    private readonly SplitContainer _split;
    private readonly Dictionary<GeoObject, TreeNode> _nodes = new();
    private readonly Dictionary<ActiveTool, ToolStripButton> _toolButtons = new();

    private readonly ToolStripStatusLabel _lblCoords;
    private readonly ToolStripStatusLabel _lblTool;
    private readonly ToolStripStatusLabel _lblCount;

    private ToolStripMenuItem _miAxes, _miGrid, _miSnap;

    /// <summary>Chặn vòng lặp sự kiện khi đồng bộ Tree ↔ Canvas.</summary>
    private bool _syncing;

    public MainForm()
    {
        Text = "BTTuan06 - GeoGebra 2D (C# WinForms)";
        Size = new Size(1280, 800);
        MinimumSize = new Size(800, 500);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9f);

        // ----- Vùng vẽ -----
        _canvas = new GeoCanvasPanel { Dock = DockStyle.Fill, BorderStyle = BorderStyle.None };

        // ----- Cột Đại số -----
        _swatches = new ImageList { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(14, 14) };
        _swatches.Images.Add(BlankImageKey, new Bitmap(14, 14));

        _tree = new BufferedTreeView
        {
            Dock = DockStyle.Fill,
            CheckBoxes = true,
            HideSelection = false,
            ShowNodeToolTips = true,
            FullRowSelect = false,
            BorderStyle = BorderStyle.None,
            ItemHeight = 24,
            Font = new Font("Segoe UI", 10f),
            ImageList = _swatches,
            ImageKey = BlankImageKey,
            SelectedImageKey = BlankImageKey
        };
        _freeRoot = new TreeNode(FreeRootText) { ImageKey = BlankImageKey, SelectedImageKey = BlankImageKey, Checked = true };
        _dependentRoot = new TreeNode(DependentRootText) { ImageKey = BlankImageKey, SelectedImageKey = BlankImageKey, Checked = true };
        _freeRoot.NodeFont = new Font("Segoe UI", 10f, FontStyle.Bold);
        _dependentRoot.NodeFont = _freeRoot.NodeFont;
        _tree.Nodes.Add(_freeRoot);
        _tree.Nodes.Add(_dependentRoot);
        _tree.ContextMenuStrip = BuildTreeContextMenu();

        var algebraHeader = new Label
        {
            Text = "Đại số (Algebra)",
            Dock = DockStyle.Top,
            Height = 28,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0),
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            BackColor = Color.FromArgb(240, 240, 240)
        };

        _split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            FixedPanel = FixedPanel.Panel1,
            SplitterWidth = 5,
            BackColor = Color.FromArgb(215, 215, 215)
        };
        _split.Panel1.BackColor = Color.White;
        _split.Panel2.BackColor = Color.White;
        _split.Panel1.Controls.Add(_tree);
        _split.Panel1.Controls.Add(algebraHeader);
        _split.Panel2.Controls.Add(_canvas);

        // ----- Thanh trạng thái -----
        _lblCoords = new ToolStripStatusLabel("x = 0.00, y = 0.00")
        {
            AutoSize = false,
            Width = 190,
            TextAlign = ContentAlignment.MiddleLeft,
            BorderSides = ToolStripStatusLabelBorderSides.Right
        };
        _lblTool = new ToolStripStatusLabel { Spring = true, TextAlign = ContentAlignment.MiddleLeft };
        _lblCount = new ToolStripStatusLabel("Số đối tượng: 0")
        {
            BorderSides = ToolStripStatusLabelBorderSides.Left,
            TextAlign = ContentAlignment.MiddleRight
        };
        var status = new StatusStrip();
        status.Items.AddRange(new ToolStripItem[] { _lblCoords, _lblTool, _lblCount });

        var toolStrip = BuildToolStrip();
        var menu = BuildMenu();

        // Thứ tự Add quyết định thứ tự Dock: Fill phải được thêm đầu tiên
        Controls.Add(_split);
        Controls.Add(toolStrip);
        Controls.Add(menu);
        Controls.Add(status);
        MainMenuStrip = menu;

        // ----- Kết nối sự kiện -----
        _canvas.ObjectsChanged += (_, _) => RebuildTree();
        _canvas.ObjectsUpdated += (_, _) => RefreshTreeTexts();
        _canvas.SelectionChanged += (_, _) => SyncTreeSelection();
        _canvas.MouseWorldMoved += (_, e) =>
            _lblCoords.Text = $"x = {GeoMath.Fmt(e.World.X)}, y = {GeoMath.Fmt(e.World.Y)}";
        _canvas.ToolStateChanged += (_, _) => UpdateToolStatus();

        _tree.AfterCheck += Tree_AfterCheck;
        _tree.AfterSelect += Tree_AfterSelect;
        _tree.NodeMouseClick += Tree_NodeMouseClick;
        _tree.NodeMouseDoubleClick += Tree_NodeMouseDoubleClick;
        _tree.KeyDown += Tree_KeyDown;

        Load += (_, _) =>
        {
            _split.Panel1MinSize = 180;
            _split.SplitterDistance = 360;
            UpdateToolStatus();
            RebuildTree();
        };
    }

    // ===================== Menu =====================

    private MenuStrip BuildMenu()
    {
        var menu = new MenuStrip();

        var miNew = new ToolStripMenuItem("Mới / Xóa vùng vẽ (New / Clear Canvas)", null, (_, _) => NewCanvas())
        {
            ShortcutKeys = Keys.Control | Keys.N
        };
        var miDemo = new ToolStripMenuItem("Hình mẫu (Demo)", null, (_, _) => BuildDemo())
        {
            ShortcutKeys = Keys.Control | Keys.D
        };
        var miExit = new ToolStripMenuItem("Thoát (Exit)", null, (_, _) => Close())
        {
            ShortcutKeyDisplayString = "Alt+F4"
        };
        var file = new ToolStripMenuItem("&File");
        file.DropDownItems.AddRange(new ToolStripItem[] { miNew, miDemo, new ToolStripSeparator(), miExit });

        _miAxes = new ToolStripMenuItem("Hiện trục tọa độ (Show Axes)") { CheckOnClick = true, Checked = _canvas.Grid.ShowAxes };
        _miGrid = new ToolStripMenuItem("Hiện lưới (Show Grid)") { CheckOnClick = true, Checked = _canvas.Grid.ShowGrid };
        _miSnap = new ToolStripMenuItem("Bắt dính lưới (Snap to Grid)") { CheckOnClick = true, Checked = _canvas.Grid.SnapToGrid };
        _miAxes.CheckedChanged += (_, _) => { _canvas.Grid.ShowAxes = _miAxes.Checked; _canvas.Invalidate(); };
        _miGrid.CheckedChanged += (_, _) => { _canvas.Grid.ShowGrid = _miGrid.Checked; _canvas.Invalidate(); };
        _miSnap.CheckedChanged += (_, _) => { _canvas.Grid.SnapToGrid = _miSnap.Checked; _canvas.Invalidate(); };
        var miReset = new ToolStripMenuItem("Đặt lại khung nhìn (Reset View)", null, (_, _) => _canvas.ResetView())
        {
            ShortcutKeys = Keys.Control | Keys.R
        };
        var view = new ToolStripMenuItem("&View");
        view.DropDownItems.AddRange(new ToolStripItem[] { _miAxes, _miGrid, _miSnap, new ToolStripSeparator(), miReset });

        menu.Items.AddRange(new ToolStripItem[] { file, view });
        return menu;
    }

    // ===================== Thanh công cụ =====================

    private ToolStrip BuildToolStrip()
    {
        var ts = new ToolStrip
        {
            GripStyle = ToolStripGripStyle.Hidden,
            ImageScalingSize = new Size(32, 32),
            Padding = new Padding(6, 2, 6, 2),
            RenderMode = ToolStripRenderMode.System
        };

        ActiveTool[][] groups =
        {
            new[] { ActiveTool.Move },
            new[] { ActiveTool.NewPoint, ActiveTool.Midpoint, ActiveTool.Intersect },
            new[] { ActiveTool.Line, ActiveTool.Segment, ActiveTool.Ray },
            new[] { ActiveTool.Polygon, ActiveTool.Triangle, ActiveTool.Rectangle },
            new[] { ActiveTool.Circle, ActiveTool.CircleRadius }
        };

        foreach (var group in groups)
        {
            if (ts.Items.Count > 0) ts.Items.Add(new ToolStripSeparator());
            foreach (var tool in group)
            {
                var btn = new ToolStripButton
                {
                    Text = ShortToolText(tool),
                    Image = ToolIcons.Create(tool, 32),
                    DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                    TextImageRelation = TextImageRelation.ImageAboveText,
                    ImageScaling = ToolStripItemImageScaling.SizeToFit,
                    ToolTipText = $"{GeoCanvasPanel.GetToolName(tool)}\n{GeoCanvasPanel.GetToolDescription(tool)}",
                    AutoSize = true,
                    Margin = new Padding(2, 1, 2, 2),
                    Tag = tool
                };
                btn.Click += (_, _) => _canvas.ActiveTool = tool;
                _toolButtons[tool] = btn;
                ts.Items.Add(btn);
            }
        }
        return ts;
    }

    private static string ShortToolText(ActiveTool tool) => tool switch
    {
        ActiveTool.Move => "Di chuyển",
        ActiveTool.NewPoint => "Điểm",
        ActiveTool.Line => "Đường thẳng",
        ActiveTool.Segment => "Đoạn thẳng",
        ActiveTool.Ray => "Tia",
        ActiveTool.Circle => "Đ.tròn",
        ActiveTool.CircleRadius => "Đ.tròn (r)",
        ActiveTool.Polygon => "Đa giác",
        ActiveTool.Triangle => "Tam giác",
        ActiveTool.Rectangle => "Chữ nhật",
        ActiveTool.Midpoint => "Trung điểm",
        ActiveTool.Intersect => "Giao điểm",
        _ => tool.ToString()
    };

    private void UpdateToolStatus()
    {
        foreach (var (tool, btn) in _toolButtons)
            btn.Checked = tool == _canvas.ActiveTool;
        _lblTool.Text = $"{GeoCanvasPanel.GetToolName(_canvas.ActiveTool)}: {_canvas.CurrentHint}";
    }

    // ===================== File =====================

    private void NewCanvas()
    {
        _canvas.ClearObjects();
        GeoRegistry.Reset();
    }

    /// <summary>Hình mẫu: tam giác ABC + trung điểm + đường tròn + 2 đường thẳng cắt nhau và giao điểm.</summary>
    private void BuildDemo()
    {
        NewCanvas();

        var a = new GeoPoint(-7, -2);
        var b = new GeoPoint(-2, -2);
        var c = new GeoPoint(-4, 2.5);
        var triangle = new GeoTriangle(a, b, c);
        var mid = new MidpointPoint(a, b);
        var circle = new GeoCircle(mid, a);

        var p1 = new GeoPoint(1, -2);
        var p2 = new GeoPoint(7, 3);
        var p3 = new GeoPoint(1, 3);
        var p4 = new GeoPoint(7, -1);
        var line1 = new GeoLine(p1, p2);
        var line2 = new GeoLine(p3, p4);
        var intersection = new IntersectionPoint(line1, line2);

        _canvas.ResetView();
        _canvas.AddObjects(new GeoObject[]
        {
            a, b, c, triangle, mid, circle,
            p1, p2, p3, p4, line1, line2, intersection
        });
    }

    // ===================== Cột Đại số (TreeView) =====================

    /// <summary>Dựng lại cây khi thêm/xóa đối tượng.</summary>
    private void RebuildTree()
    {
        _syncing = true;
        _tree.BeginUpdate();
        try
        {
            _freeRoot.Nodes.Clear();
            _dependentRoot.Nodes.Clear();
            _nodes.Clear();

            foreach (var obj in _canvas.Objects)
            {
                var node = new TreeNode(obj.GetAlgebraDescription())
                {
                    Tag = obj,
                    Checked = obj.Visible,
                    ToolTipText = obj.GetAlgebraHeader()
                };
                ApplySwatch(node, obj);
                (obj.IsIndependent ? _freeRoot : _dependentRoot).Nodes.Add(node);
                _nodes[obj] = node;
            }

            SyncRootCheck(_freeRoot);
            SyncRootCheck(_dependentRoot);
            _freeRoot.Expand();
            _dependentRoot.Expand();

            var sel = _canvas.SelectedObject;
            _tree.SelectedNode = sel != null && _nodes.TryGetValue(sel, out var selNode) ? selNode : null;
        }
        finally
        {
            _tree.EndUpdate();
            _syncing = false;
        }
        _lblCount.Text = $"Số đối tượng: {_canvas.Objects.Count}";
    }

    /// <summary>Chỉ cập nhật text/ô màu/ô tick của node có sẵn (khi kéo chuột) để tránh nháy.</summary>
    private void RefreshTreeTexts()
    {
        _syncing = true;
        try
        {
            foreach (var (obj, node) in _nodes)
            {
                string text = obj.GetAlgebraDescription();
                if (node.Text != text) node.Text = text;
                if (node.Checked != obj.Visible) node.Checked = obj.Visible;
                ApplySwatch(node, obj);
            }
            SyncRootCheck(_freeRoot);
            SyncRootCheck(_dependentRoot);
        }
        finally
        {
            _syncing = false;
        }
    }

    private static void SyncRootCheck(TreeNode root)
    {
        bool all = root.Nodes.Cast<TreeNode>().All(n => n.Checked);
        if (root.Checked != all) root.Checked = all;
    }

    /// <summary>Ô màu nhỏ trước mỗi node, lấy theo màu Style của đối tượng.</summary>
    private void ApplySwatch(TreeNode node, GeoObject obj)
    {
        Color color = obj.Style.ToDrawingColor();
        string key = "c" + color.ToArgb().ToString("X8");
        if (!_swatches.Images.ContainsKey(key))
        {
            var bmp = new Bitmap(14, 14);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(Color.FromArgb(255, color));
                using var border = new Pen(Color.FromArgb(90, 90, 90), 1f);
                g.FillRectangle(brush, 1, 1, 11, 11);
                g.DrawRectangle(border, 1, 1, 11, 11);
            }
            _swatches.Images.Add(key, bmp);
        }
        if (node.ImageKey != key)
        {
            node.ImageKey = key;
            node.SelectedImageKey = key;
        }
    }

    private void SyncTreeSelection()
    {
        if (_syncing) return;
        _syncing = true;
        try
        {
            var sel = _canvas.SelectedObject;
            _tree.SelectedNode = sel != null && _nodes.TryGetValue(sel, out var node) ? node : null;
        }
        finally
        {
            _syncing = false;
        }
    }

    private void Tree_AfterCheck(object sender, TreeViewEventArgs e)
    {
        if (_syncing || e.Node == null) return;
        _syncing = true;
        try
        {
            if (e.Node.Tag is GeoObject obj)
            {
                obj.Visible = e.Node.Checked;
                if (e.Node.Parent != null) SyncRootCheck(e.Node.Parent);
            }
            else
            {
                // Tick nhánh gốc ⇒ ẩn/hiện mọi đối tượng trong nhánh
                foreach (TreeNode child in e.Node.Nodes)
                {
                    child.Checked = e.Node.Checked;
                    if (child.Tag is GeoObject o) o.Visible = e.Node.Checked;
                }
            }
        }
        finally
        {
            _syncing = false;
        }
        _canvas.Invalidate();
    }

    private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (_syncing) return;
        _syncing = true;
        try
        {
            _canvas.SelectObject(e.Node?.Tag as GeoObject);
        }
        finally
        {
            _syncing = false;
        }
    }

    private void Tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    {
        if (e.Button == MouseButtons.Right) _tree.SelectedNode = e.Node;
    }

    private void Tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
        var hit = _tree.HitTest(e.Location);
        if (hit.Location is TreeViewHitTestLocations.StateImage) return; // double-click vào ô tick
        if (e.Node?.Tag is GeoObject obj) ChangeColor(obj);
    }

    private void Tree_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Delete && _tree.SelectedNode?.Tag is GeoObject obj)
        {
            _canvas.DeleteObject(obj);
            e.Handled = true;
        }
    }

    private ContextMenuStrip BuildTreeContextMenu()
    {
        var cms = new ContextMenuStrip();
        var miColor = new ToolStripMenuItem("Đổi màu...", null, (_, _) =>
        {
            if (_tree.SelectedNode?.Tag is GeoObject obj) ChangeColor(obj);
        });
        var miToggle = new ToolStripMenuItem("Ẩn / Hiện", null, (_, _) =>
        {
            if (_tree.SelectedNode is { Tag: GeoObject } node) node.Checked = !node.Checked;
        });
        var miDelete = new ToolStripMenuItem("Xóa", null, (_, _) =>
        {
            if (_tree.SelectedNode?.Tag is GeoObject obj) _canvas.DeleteObject(obj);
        })
        {
            ShortcutKeyDisplayString = "Delete"
        };
        cms.Items.AddRange(new ToolStripItem[] { miColor, miToggle, new ToolStripSeparator(), miDelete });
        cms.Opening += (_, e) => e.Cancel = _tree.SelectedNode?.Tag is not GeoObject;
        return cms;
    }

    private void ChangeColor(GeoObject obj)
    {
        using var dlg = new ColorDialog
        {
            Color = Color.FromArgb(255, obj.Style.ToDrawingColor()),
            FullOpen = true,
            AnyColor = true
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        var style = obj.Style.Clone();
        style.SetColor(Color.FromArgb(obj.Style.Alpha, dlg.Color));
        obj.Style = style;
        if (_nodes.TryGetValue(obj, out var node)) ApplySwatch(node, obj);
        _canvas.NotifyObjectsUpdated();
    }

    /// <summary>TreeView bật double-buffer gốc (TVS_EX_DOUBLEBUFFER) để không nháy khi cập nhật text liên tục.</summary>
    private sealed class BufferedTreeView : TreeView
    {
        public BufferedTreeView()
        {
            DoubleBuffered = true;
        }
    }
}
