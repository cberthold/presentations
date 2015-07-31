namespace Web3WinForm
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
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label fromLabel;
            this.fromTextBox = new System.Windows.Forms.TextBox();
            this.notificationInputBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.SendToMeButton = new System.Windows.Forms.Button();
            this.SendToEveryoneButton = new System.Windows.Forms.Button();
            this.JoinExcitingGroupButton = new System.Windows.Forms.Button();
            this.SendToExcitingGroupButton = new System.Windows.Forms.Button();
            this.OutputTextBox = new System.Windows.Forms.TextBox();
            fromLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.notificationInputBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // fromLabel
            // 
            fromLabel.AutoSize = true;
            fromLabel.Location = new System.Drawing.Point(12, 42);
            fromLabel.Name = "fromLabel";
            fromLabel.Size = new System.Drawing.Size(33, 13);
            fromLabel.TabIndex = 1;
            fromLabel.Text = "From:";
            // 
            // fromTextBox
            // 
            this.fromTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notificationInputBindingSource, "From", true));
            this.fromTextBox.Location = new System.Drawing.Point(51, 39);
            this.fromTextBox.Name = "fromTextBox";
            this.fromTextBox.Size = new System.Drawing.Size(147, 20);
            this.fromTextBox.TabIndex = 2;
            // 
            // notificationInputBindingSource
            // 
            this.notificationInputBindingSource.DataSource = typeof(Web3WinForm.NotificationInput);
            // 
            // SendToMeButton
            // 
            this.SendToMeButton.Location = new System.Drawing.Point(51, 65);
            this.SendToMeButton.Name = "SendToMeButton";
            this.SendToMeButton.Size = new System.Drawing.Size(147, 23);
            this.SendToMeButton.TabIndex = 3;
            this.SendToMeButton.Text = "Send To Me";
            this.SendToMeButton.UseVisualStyleBackColor = true;
            this.SendToMeButton.Click += new System.EventHandler(this.SendToMeButton_Click);
            // 
            // SendToEveryoneButton
            // 
            this.SendToEveryoneButton.Location = new System.Drawing.Point(51, 95);
            this.SendToEveryoneButton.Name = "SendToEveryoneButton";
            this.SendToEveryoneButton.Size = new System.Drawing.Size(147, 23);
            this.SendToEveryoneButton.TabIndex = 4;
            this.SendToEveryoneButton.Text = "Send To Everyone";
            this.SendToEveryoneButton.UseVisualStyleBackColor = true;
            this.SendToEveryoneButton.Click += new System.EventHandler(this.SendToEveryoneButton_Click);
            // 
            // JoinExcitingGroupButton
            // 
            this.JoinExcitingGroupButton.Location = new System.Drawing.Point(51, 125);
            this.JoinExcitingGroupButton.Name = "JoinExcitingGroupButton";
            this.JoinExcitingGroupButton.Size = new System.Drawing.Size(147, 23);
            this.JoinExcitingGroupButton.TabIndex = 5;
            this.JoinExcitingGroupButton.Text = "Join Exciting Group";
            this.JoinExcitingGroupButton.UseVisualStyleBackColor = true;
            this.JoinExcitingGroupButton.Click += new System.EventHandler(this.JoinExcitingGroupButton_Click);
            // 
            // SendToExcitingGroupButton
            // 
            this.SendToExcitingGroupButton.Location = new System.Drawing.Point(51, 154);
            this.SendToExcitingGroupButton.Name = "SendToExcitingGroupButton";
            this.SendToExcitingGroupButton.Size = new System.Drawing.Size(147, 23);
            this.SendToExcitingGroupButton.TabIndex = 6;
            this.SendToExcitingGroupButton.Text = "Send To Exciting Group";
            this.SendToExcitingGroupButton.UseVisualStyleBackColor = true;
            this.SendToExcitingGroupButton.Click += new System.EventHandler(this.SendToExcitingGroupButton_Click);
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputTextBox.Location = new System.Drawing.Point(204, 12);
            this.OutputTextBox.Multiline = true;
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.Size = new System.Drawing.Size(408, 237);
            this.OutputTextBox.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 261);
            this.Controls.Add(this.OutputTextBox);
            this.Controls.Add(this.SendToExcitingGroupButton);
            this.Controls.Add(this.JoinExcitingGroupButton);
            this.Controls.Add(this.SendToEveryoneButton);
            this.Controls.Add(this.SendToMeButton);
            this.Controls.Add(fromLabel);
            this.Controls.Add(this.fromTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.notificationInputBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource notificationInputBindingSource;
        private System.Windows.Forms.TextBox fromTextBox;
        private System.Windows.Forms.Button SendToMeButton;
        private System.Windows.Forms.Button SendToEveryoneButton;
        private System.Windows.Forms.Button JoinExcitingGroupButton;
        private System.Windows.Forms.Button SendToExcitingGroupButton;
        private System.Windows.Forms.TextBox OutputTextBox;
    }
}

