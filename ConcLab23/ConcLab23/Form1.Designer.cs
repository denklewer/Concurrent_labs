namespace ConcLab23
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btCalculate = new System.Windows.Forms.Button();
            this.tbRes = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.btnParallelCalculate = new System.Windows.Forms.Button();
            this.tbResParallel = new System.Windows.Forms.TextBox();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.btCancel = new System.Windows.Forms.Button();
            this.btCancelParallel = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.tbDownload = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCalculate
            // 
            this.btCalculate.Dock = System.Windows.Forms.DockStyle.Left;
            this.btCalculate.Location = new System.Drawing.Point(184, 307);
            this.btCalculate.Name = "btCalculate";
            this.btCalculate.Size = new System.Drawing.Size(185, 78);
            this.btCalculate.TabIndex = 0;
            this.btCalculate.Text = "Calculate";
            this.btCalculate.UseVisualStyleBackColor = true;
            this.btCalculate.Click += new System.EventHandler(this.btCalculate_Click);
            // 
            // tbRes
            // 
            this.tbRes.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbRes.Location = new System.Drawing.Point(0, 33);
            this.tbRes.Multiline = true;
            this.tbRes.Name = "tbRes";
            this.tbRes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbRes.Size = new System.Drawing.Size(367, 251);
            this.tbRes.TabIndex = 15;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar1.Location = new System.Drawing.Point(0, 284);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(367, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // progressBar2
            // 
            this.progressBar2.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar2.Location = new System.Drawing.Point(0, 284);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(367, 23);
            this.progressBar2.TabIndex = 4;
            // 
            // btnParallelCalculate
            // 
            this.btnParallelCalculate.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnParallelCalculate.Location = new System.Drawing.Point(189, 307);
            this.btnParallelCalculate.Name = "btnParallelCalculate";
            this.btnParallelCalculate.Size = new System.Drawing.Size(178, 78);
            this.btnParallelCalculate.TabIndex = 3;
            this.btnParallelCalculate.Text = "Calculate Parallel";
            this.btnParallelCalculate.UseVisualStyleBackColor = true;
            this.btnParallelCalculate.Click += new System.EventHandler(this.btnParallelCalculate_Click);
            // 
            // tbResParallel
            // 
            this.tbResParallel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbResParallel.Location = new System.Drawing.Point(0, 33);
            this.tbResParallel.Multiline = true;
            this.tbResParallel.Name = "tbResParallel";
            this.tbResParallel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbResParallel.Size = new System.Drawing.Size(367, 251);
            this.tbResParallel.TabIndex = 5;
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // btCancel
            // 
            this.btCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btCancel.Location = new System.Drawing.Point(0, 307);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(184, 78);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btCancelParallel
            // 
            this.btCancelParallel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btCancelParallel.Location = new System.Drawing.Point(0, 307);
            this.btCancelParallel.Name = "btCancelParallel";
            this.btCancelParallel.Size = new System.Drawing.Size(185, 78);
            this.btCancelParallel.TabIndex = 7;
            this.btCancelParallel.Text = "Cancel";
            this.btCancelParallel.UseVisualStyleBackColor = true;
            this.btCancelParallel.Click += new System.EventHandler(this.btCancelParallel_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Enabled = false;
            this.btnDownload.Location = new System.Drawing.Point(325, 411);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 31);
            this.btnDownload.TabIndex = 8;
            this.btnDownload.Text = "Загрузить";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // tbDownload
            // 
            this.tbDownload.Location = new System.Drawing.Point(183, 415);
            this.tbDownload.Name = "tbDownload";
            this.tbDownload.Size = new System.Drawing.Size(100, 22);
            this.tbDownload.TabIndex = 9;
            this.tbDownload.TextChanged += new System.EventHandler(this.tbDownload_TextChanged);
            this.tbDownload.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 415);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "Количество элементов";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btCalculate);
            this.panel1.Controls.Add(this.btCancel);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.tbRes);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(369, 387);
            this.panel1.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(369, 307);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(3);
            this.label3.Size = new System.Drawing.Size(0, 33);
            this.label3.TabIndex = 26;
            this.label3.Text = "Однопоточная версия";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(3);
            this.label2.Size = new System.Drawing.Size(367, 33);
            this.label2.TabIndex = 25;
            this.label2.Text = "Однопоточная версия";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnParallelCalculate);
            this.panel2.Controls.Add(this.btCancelParallel);
            this.panel2.Controls.Add(this.progressBar2);
            this.panel2.Controls.Add(this.tbResParallel);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Enabled = false;
            this.panel2.Location = new System.Drawing.Point(389, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(369, 387);
            this.panel2.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(3);
            this.label4.Size = new System.Drawing.Size(367, 33);
            this.label4.TabIndex = 26;
            this.label4.Text = "Параллельная версия";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 440);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(226, 17);
            this.label5.TabIndex = 13;
            this.label5.Text = "файл D:\\C#\\data mining\\input.data";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 466);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbDownload);
            this.Controls.Add(this.btnDownload);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCalculate;
        private System.Windows.Forms.TextBox tbRes;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Button btnParallelCalculate;
        private System.Windows.Forms.TextBox tbResParallel;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btCancelParallel;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.TextBox tbDownload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

