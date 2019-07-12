namespace MyMail
{
    partial class mail
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除邮件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.放入垃圾箱ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.将用户加入黑名单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.将用户加入白名单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.还原邮件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox1.Font = new System.Drawing.Font("宋体", 15F);
            this.textBox1.ImeMode = System.Windows.Forms.ImeMode.Close;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Margin = new System.Windows.Forms.Padding(0);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(176, 29);
            this.textBox1.TabIndex = 0;
            this.textBox1.TabStop = false;
            this.textBox1.Click += new System.EventHandler(this.textBox4_Click);
            this.textBox1.Enter += new System.EventHandler(this.textBox4_Enter);
            this.textBox1.MouseEnter += new System.EventHandler(this.textBox4_Enter);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除邮件ToolStripMenuItem,
            this.放入垃圾箱ToolStripMenuItem,
            this.将用户加入黑名单ToolStripMenuItem,
            this.将用户加入白名单ToolStripMenuItem,
            this.还原邮件ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(199, 124);
            // 
            // 删除邮件ToolStripMenuItem
            // 
            this.删除邮件ToolStripMenuItem.Name = "删除邮件ToolStripMenuItem";
            this.删除邮件ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.删除邮件ToolStripMenuItem.Text = "删除邮件";
            this.删除邮件ToolStripMenuItem.Click += new System.EventHandler(this.删除邮件ToolStripMenuItem_Click);
            // 
            // 放入垃圾箱ToolStripMenuItem
            // 
            this.放入垃圾箱ToolStripMenuItem.Name = "放入垃圾箱ToolStripMenuItem";
            this.放入垃圾箱ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.放入垃圾箱ToolStripMenuItem.Text = "放入垃圾箱";
            this.放入垃圾箱ToolStripMenuItem.Click += new System.EventHandler(this.放入垃圾箱ToolStripMenuItem_Click);
            // 
            // 将用户加入黑名单ToolStripMenuItem
            // 
            this.将用户加入黑名单ToolStripMenuItem.Name = "将用户加入黑名单ToolStripMenuItem";
            this.将用户加入黑名单ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.将用户加入黑名单ToolStripMenuItem.Text = "将用户加入黑名单";
            this.将用户加入黑名单ToolStripMenuItem.Click += new System.EventHandler(this.将用户加入黑名单ToolStripMenuItem_Click);
            // 
            // 将用户加入白名单ToolStripMenuItem
            // 
            this.将用户加入白名单ToolStripMenuItem.Name = "将用户加入白名单ToolStripMenuItem";
            this.将用户加入白名单ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.将用户加入白名单ToolStripMenuItem.Text = "将用户加入白名单";
            // 
            // 还原邮件ToolStripMenuItem
            // 
            this.还原邮件ToolStripMenuItem.Name = "还原邮件ToolStripMenuItem";
            this.还原邮件ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.还原邮件ToolStripMenuItem.Text = "还原邮件";
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.ContextMenuStrip = this.contextMenuStrip1;
            this.textBox2.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox2.Font = new System.Drawing.Font("宋体", 15F);
            this.textBox2.Location = new System.Drawing.Point(176, 0);
            this.textBox2.Margin = new System.Windows.Forms.Padding(0);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(444, 29);
            this.textBox2.TabIndex = 1;
            this.textBox2.Click += new System.EventHandler(this.textBox4_Click);
            this.textBox2.Enter += new System.EventHandler(this.textBox4_Enter);
            this.textBox2.MouseEnter += new System.EventHandler(this.textBox4_Enter);
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.ContextMenuStrip = this.contextMenuStrip1;
            this.textBox3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox3.Font = new System.Drawing.Font("宋体", 15F);
            this.textBox3.Location = new System.Drawing.Point(614, 0);
            this.textBox3.Margin = new System.Windows.Forms.Padding(0);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(570, 29);
            this.textBox3.TabIndex = 2;
            this.textBox3.Click += new System.EventHandler(this.textBox4_Click);
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            this.textBox3.Enter += new System.EventHandler(this.textBox4_Enter);
            this.textBox3.MouseEnter += new System.EventHandler(this.textBox4_Enter);
            // 
            // textBox4
            // 
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.ContextMenuStrip = this.contextMenuStrip1;
            this.textBox4.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox4.Font = new System.Drawing.Font("宋体", 15F);
            this.textBox4.Location = new System.Drawing.Point(0, 29);
            this.textBox4.Margin = new System.Windows.Forms.Padding(0);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(1184, 29);
            this.textBox4.TabIndex = 3;
            this.textBox4.TabStop = false;
            this.textBox4.Click += new System.EventHandler(this.textBox4_Click);
            this.textBox4.Enter += new System.EventHandler(this.textBox4_Enter);
            this.textBox4.MouseEnter += new System.EventHandler(this.textBox4_Enter);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.Location = new System.Drawing.Point(0, 56);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(1184, 1);
            this.button1.TabIndex = 4;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(1124, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Rubbish!";
            this.label1.Visible = false;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // mail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "mail";
            this.Size = new System.Drawing.Size(1184, 58);
            this.Click += new System.EventHandler(this.textBox4_Click);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 删除邮件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 放入垃圾箱ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 将用户加入黑名单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 将用户加入白名单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 还原邮件ToolStripMenuItem;
    }
}
