using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CipherPad
{
    internal class MainForm : Form
    {
        private readonly StatusStrip statusStrip;
        private readonly ToolStripStatusLabel totalStripStatusLabel;
        private readonly ToolStripStatusLabel selectStripStatusLabel;
        private readonly ToolStripStatusLabel keyStripStatusLabel;
        private readonly ToolStripStatusLabel openStripStatusLabel;
        private readonly ToolStrip toolStrip;
        private readonly ToolStripButton openStripButton;
        private readonly ToolStripButton saveStripButton;
        private readonly ToolStripTextBox searchStripTextBox;
        private readonly ToolStripButton searchStripButton;
        private readonly ToolStripTextBox keyStripTextBox;
        private readonly ToolStripButton keyStripButton;
        private readonly ToolStripButton helpStripButton;
        private readonly ToolStripButton clearStripButton;
        private readonly ToolStripButton closeStripButton;
        private readonly ListBox listBox;
        private readonly TextBox textBox;
        private readonly TextBox addTextBox;
        private readonly Button addButton;
        private readonly Button removeButton;

        private readonly EncryptString es;
        private List<string> pool;
        private string filePath = "";
        private int foundIndex = 0;
        private bool isKey = false;
        public MainForm()
        {
            es = new EncryptString();
            pool = new List<string>();
            statusStrip = new StatusStrip();
            totalStripStatusLabel = new ToolStripStatusLabel();
            selectStripStatusLabel = new ToolStripStatusLabel();
            keyStripStatusLabel = new ToolStripStatusLabel();
            openStripStatusLabel = new ToolStripStatusLabel();
            toolStrip = new ToolStrip();
            openStripButton = new ToolStripButton();
            saveStripButton = new ToolStripButton();
            searchStripTextBox = new ToolStripTextBox();
            searchStripButton = new ToolStripButton();
            keyStripTextBox = new ToolStripTextBox();
            keyStripButton = new ToolStripButton();
            helpStripButton = new ToolStripButton();
            clearStripButton = new ToolStripButton();
            closeStripButton = new ToolStripButton();
            listBox = new ListBox();
            textBox = new TextBox();
            addTextBox = new TextBox();
            addButton = new Button();
            removeButton = new Button();
            SuspendLayout();

            statusStrip.Items.AddRange(new ToolStripItem[] {
            totalStripStatusLabel,
            selectStripStatusLabel,
            keyStripStatusLabel,
            openStripStatusLabel});

            totalStripStatusLabel.BorderSides = ToolStripStatusLabelBorderSides.Left
            | ToolStripStatusLabelBorderSides.Top
            | ToolStripStatusLabelBorderSides.Right
            | ToolStripStatusLabelBorderSides.Bottom;
            totalStripStatusLabel.Text = "总数：0";

            selectStripStatusLabel.BorderSides = ToolStripStatusLabelBorderSides.Left
            | ToolStripStatusLabelBorderSides.Top
            | ToolStripStatusLabelBorderSides.Right
            | ToolStripStatusLabelBorderSides.Bottom;
            selectStripStatusLabel.Text = "选中：0";

            keyStripStatusLabel.BorderSides = ToolStripStatusLabelBorderSides.Left
            | ToolStripStatusLabelBorderSides.Top
            | ToolStripStatusLabelBorderSides.Right
            | ToolStripStatusLabelBorderSides.Bottom;
            keyStripStatusLabel.Text = "默认密钥";

            openStripStatusLabel.BorderSides = ToolStripStatusLabelBorderSides.Left
            | ToolStripStatusLabelBorderSides.Top
            | ToolStripStatusLabelBorderSides.Right
            | ToolStripStatusLabelBorderSides.Bottom;
            openStripStatusLabel.Text = "未打开文件";

            openStripButton.Image = Properties.Resources.open;
            openStripButton.Text = "打开";
            openStripButton.TextImageRelation = TextImageRelation.ImageAboveText;
            openStripButton.Click += OpenStripButton_Click;

            saveStripButton.Image = Properties.Resources.save;
            saveStripButton.Text = "保存";
            saveStripButton.TextImageRelation = TextImageRelation.ImageAboveText;
            saveStripButton.Click += SaveStripButton_Click;

            searchStripTextBox.BorderStyle = BorderStyle.FixedSingle;
            searchStripTextBox.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);

            searchStripButton.Image = Properties.Resources.find;
            searchStripButton.Text = "查找";
            searchStripButton.TextImageRelation = TextImageRelation.ImageAboveText;
            searchStripButton.Click += SearchStripButton_Click;

            keyStripTextBox.BorderStyle = BorderStyle.FixedSingle;
            keyStripTextBox.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);

            keyStripButton.Image = Properties.Resources.key;
            keyStripButton.Text = "密钥";
            keyStripButton.TextImageRelation = TextImageRelation.ImageAboveText;
            keyStripButton.Click += KeyStripButton_Click;

            helpStripButton.Image = Properties.Resources.help;
            helpStripButton.Text = "帮助";
            helpStripButton.TextImageRelation = TextImageRelation.ImageAboveText;
            helpStripButton.Click += HelpStripButton_Click;

            clearStripButton.Image = Properties.Resources.clear;
            clearStripButton.Text = "清空";
            clearStripButton.TextImageRelation = TextImageRelation.ImageAboveText;
            clearStripButton.Click += ClearStripButton_Click;

            closeStripButton.Image = Properties.Resources.close;
            closeStripButton.Text = "关闭";
            closeStripButton.Enabled = false;
            closeStripButton.TextImageRelation = TextImageRelation.ImageAboveText;
            closeStripButton.Click += CloseStripButton_Click;

            toolStrip.ImageScalingSize = new Size(20, 20);
            toolStrip.Items.AddRange(new ToolStripItem[] {
            openStripButton,
            saveStripButton,
            new ToolStripSeparator(),
            searchStripTextBox,
            searchStripButton,
            new ToolStripSeparator(),
            keyStripTextBox,
            keyStripButton,
            new ToolStripSeparator(),
            helpStripButton,
            new ToolStripSeparator(),
            clearStripButton,
            closeStripButton});

            listBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
            | AnchorStyles.Left;
            listBox.BorderStyle = BorderStyle.FixedSingle;
            listBox.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            listBox.ItemHeight = 27;
            listBox.Location = new Point(12, 50);
            listBox.ScrollAlwaysVisible = true;
            listBox.SelectionMode = SelectionMode.MultiExtended;
            listBox.Size = new Size(236, 353);
            listBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;

            textBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
            | AnchorStyles.Left
            | AnchorStyles.Right;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            textBox.Location = new Point(254, 50);
            textBox.Multiline = true;
            textBox.ScrollBars = ScrollBars.Vertical;
            textBox.Size = new Size(516, 314);
            textBox.TextChanged += TextBox_TextChanged;

            addTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            | AnchorStyles.Right;
            addTextBox.BorderStyle = BorderStyle.FixedSingle;
            addTextBox.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            addTextBox.Location = new Point(254, 372);
            addTextBox.Size = new Size(436, 34);
            addTextBox.KeyDown += AddTextBox_KeyDown;

            addButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            addButton.BackgroundImage = Properties.Resources.add;
            addButton.BackgroundImageLayout = ImageLayout.Zoom;
            addButton.Location = new Point(696, 370);
            addButton.Size = new Size(34, 34);
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += AddButton_Click;

            removeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            removeButton.BackgroundImage = Properties.Resources.remove;
            removeButton.BackgroundImageLayout = ImageLayout.Zoom;
            removeButton.Location = new Point(736, 370);
            removeButton.Size = new Size(34, 34);
            removeButton.UseVisualStyleBackColor = true;
            removeButton.Click += RemoveButton_Click;

            Text = "YuXiang CipherPad";
            ClientSize = new Size(782, 433);
            Controls.Add(statusStrip);
            Controls.Add(toolStrip);
            Controls.Add(removeButton);
            Controls.Add(addButton);
            Controls.Add(textBox);
            Controls.Add(listBox);
            Controls.Add(addTextBox);
            Icon = Properties.Resources.icon;
            MinimumSize = new Size(600, 400);
            StartPosition = FormStartPosition.CenterScreen;
            FormClosing += MainForm_FormClosing;
            ResumeLayout(false);
        }

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectStripStatusLabel.Text = "选中：" + listBox.SelectedIndices.Count;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("是否退出本软件？", "YuXiang CipherPad", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void ClearStripButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                if (MessageBox.Show("是否清空内容？", "YuXiang CipherPad", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    textBox.Clear();
                    listBox.ClearSelected();
                    if (pool.Count == 1)
                    {
                        pool.RemoveAt(0);
                    }
                }
            }
        }

        private void AddItem()
        {
            if (!string.IsNullOrWhiteSpace(addTextBox.Text))
            {
                if (!addTextBox.Text.Contains(","))
                {
                    pool.Add(addTextBox.Text);
                    textBox.Text = string.Join(",", pool);
                    addTextBox.Text = "";
                    textBox.SelectionStart = textBox.Text.Length;
                    textBox.ScrollToCaret();
                }
                else
                {
                    _ = MessageBox.Show("请不要包含英文逗号“,”。", "YuXiang CipherPad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                addTextBox.Text = "";
            }
        }

        private void AddTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddItem();
                e.SuppressKeyPress = true;
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            ListBox.SelectedIndexCollection selectedIndices = listBox.SelectedIndices;
            for (int i = selectedIndices.Count - 1; i >= 0; i--)
            {
                int selectedIndex = selectedIndices[i];
                if (selectedIndex < pool.Count)
                {
                    pool.RemoveAt(selectedIndex);
                }
            }
            textBox.Text = string.Join(",", pool);
            textBox.SelectionStart = textBox.Text.Length;
            textBox.ScrollToCaret();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AddItem();
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            pool = new List<string>(textBox.Text.Split(','));
            listBox.DataSource = null;
            listBox.DataSource = pool;
            listBox.ClearSelected();
            listBox.SelectedIndex = listBox.Items.Count - 1;
            textBox.ScrollToCaret();
            totalStripStatusLabel.Text = "总数：" + listBox.Items.Count;
        }

        private void CloseStripButton_Click(object sender, EventArgs e)
        {
            filePath = "";
            openStripStatusLabel.Text = "未打开文件";
            textBox.Clear();
            listBox.ClearSelected();
            if (pool.Count == 1)
            {
                pool.RemoveAt(0);
            }
            closeStripButton.Enabled = false;
        }

        private void HelpStripButton_Click(object sender, EventArgs e)
        {
            _ = MessageBox.Show("版本 2.0\n作者 YuXiang187\n\nYuXiang CipherPad 在 GPL-3.0 许可证下发布，源代码位于 Github 上。\n\n本软件用于编辑已加密的文本文档。\n\n如果你正在配合 YuXiang Drawer 使用本软件，请编辑 list.txt 文件。\n\n源代码地址：\nhttps://github.com/YuXiang187/CipherPad", "关于 YuXiang CipherPad", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void KeyStripButton_Click(object sender, EventArgs e)
        {
            string key = keyStripTextBox.Text;
            if (isKey == false)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    if (key.Length == 16 || key.Length == 24 || key.Length == 32)
                    {
                        es.Key(key);
                        keyStripStatusLabel.Text = "已应用密钥";
                        isKey = true;
                    }
                    else
                    {
                        _ = MessageBox.Show("无效的AES密钥长度。\n请使用16字节、24字节或32字节的密钥长度。", "YuXiang CipherPad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                keyStripTextBox.Text = "";
                es.Key("yuxiang118712023");
                keyStripStatusLabel.Text = "默认密钥";
                isKey = false;
            }
        }

        private void SearchStripButton_Click(object sender, EventArgs e)
        {
            string searchText = searchStripTextBox.Text;

            if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrEmpty(textBox.Text))
            {
                int index = textBox.Text.IndexOf(searchText, foundIndex, StringComparison.OrdinalIgnoreCase);

                if (index != -1)
                {
                    textBox.Select(index, searchText.Length);
                    textBox.ScrollToCaret();
                    foundIndex = index + searchText.Length;
                }
                else
                {
                    foundIndex = 0;
                }
            }
        }

        private void SaveStripButton_Click(object sender, EventArgs e)
        {
            if (filePath.Equals(""))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = "list.txt",
                    Filter = "加密文本文档|*.txt"
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, es.Encrypt(string.Join(",", pool)));
                    pool.Clear();
                    textBox.Clear();
                    listBox.DataSource = null;
                    if (pool.Count == 1)
                    {
                        pool.RemoveAt(0);
                    }
                }
            }
            else
            {
                File.WriteAllText(filePath, es.Encrypt(string.Join(",", pool)));
                _ = MessageBox.Show("文件保存成功：" + openStripStatusLabel.Text, "YuXiang CipherPad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                filePath = "";
                openStripStatusLabel.Text = "未打开文件";
                textBox.Clear();
                listBox.ClearSelected();
                if (pool.Count == 1)
                {
                    pool.RemoveAt(0);
                }
                closeStripButton.Enabled = false;
            }
        }

        private void OpenStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "加密文本文档|*.txt",
                Multiselect = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string text = es.Decrypt(reader.ReadLine());
                    if (text != null)
                    {
                        openStripStatusLabel.Text = Path.GetFileName(openFileDialog.FileName);
                        textBox.Text = text;
                        closeStripButton.Enabled = true;
                    }
                    else
                    {
                        filePath = "";
                    }
                }
            }
        }
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
