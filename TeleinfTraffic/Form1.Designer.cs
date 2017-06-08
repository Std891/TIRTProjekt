namespace TeleinfTraffic
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the _contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            //this.button1 = new System.Windows.Forms.Button();
            this.addGenerator = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonUsun = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            /*
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(730, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "Rozpocznij Wszystkie";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            */
            // 
            // addGenerator
            // 
            this.addGenerator.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.addGenerator.Location = new System.Drawing.Point(50, 15);
            this.addGenerator.Name = "addGenerator";
            this.addGenerator.Size = new System.Drawing.Size(120, 30);
            this.addGenerator.TabIndex = 1;
            this.addGenerator.Text = "Dodaj Generator";
            this.addGenerator.UseVisualStyleBackColor = true;
            this.addGenerator.Click += new System.EventHandler(this.addGenerator_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.buttonUsun);
            this.panel1.Controls.Add(this.addGenerator);
            //this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(884, 681);
            this.panel1.TabIndex = 2;
            // 
            // button2
            // 
            
            this.button2.Location = new System.Drawing.Point(199, 15);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 30);
            this.button2.TabIndex = 3;
            this.button2.Text = "Statystyki";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonUsun
            // 
            this.buttonUsun.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonUsun.Location = new System.Drawing.Point(50, 50);
            this.buttonUsun.Name = "buttonUsun";
            this.buttonUsun.Size = new System.Drawing.Size(120, 30);
            this.buttonUsun.TabIndex = 2;
            this.buttonUsun.Text = "Usun Generator";
            this.buttonUsun.UseVisualStyleBackColor = true;
            this.buttonUsun.Click += new System.EventHandler(this.buttonUsun_Click);
            // 
            // button3
            // 
            this.button3.Enabled = true;
            this.button3.Location = new System.Drawing.Point(348, 15);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 30);
            this.button3.TabIndex = 4;
            this.button3.Text = "Wykres Sum.";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 681);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 1080);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Teleinformatic Traffic Generator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        //public System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button addGenerator;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonUsun;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

