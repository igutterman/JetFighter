using SignalRGame.GameLogic;
using System.Windows.Forms;

namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Game game;
        private FighterJet myJet;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(92, 89);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 1000);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += Form1_KeyPress;
            this.ResumeLayout(false);

        }

        private void StartGame()
        {
            game = new Game();

            game.OnSendState = OnStateChanged;

            myJet = game.AddJet(GameConfig.canvasWidth / 2, GameConfig.canvasHeight / 2, 0f);

            game.AddParkedJet(GameConfig.canvasWidth / 3,  GameConfig.canvasHeight / 3, 0f);

            game.Start();
            button1.Hide();
        }

        private void OnStateChanged(FighterJet[] jets)
        {
            System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
            System.Drawing.Graphics formGraphics;

            formGraphics = this.CreateGraphics();
            formGraphics.Clear(BackColor);
            foreach (var jet in jets)
            {
                myPen.Color = Color.Red;

                formGraphics.DrawEllipse(myPen, new RectangleF(jet.X - 5, jet.Y - 5, 10f, 10f));

                formGraphics.DrawPolygon(myPen, new PointF[] { 
                    new PointF(jet.Hitboxes[0].BL.x,jet.Hitboxes[0].BL.y ),
                new PointF(jet.Hitboxes[0].TL.x,jet.Hitboxes[0].TL.y ),
                new PointF(jet.Hitboxes[0].TR.x,jet.Hitboxes[0].TR.y ),
                new PointF(jet.Hitboxes[0].BR.x,jet.Hitboxes[0].BR.y )});

                foreach (var bullet in jet.Bullets.ToArray())
                {
                    myPen.Color = Color.Blue;
                    formGraphics.DrawEllipse(myPen, new RectangleF(bullet.X, bullet.Y, 3f, 3f));
                }
            }
            myPen.Dispose();
            formGraphics.Dispose();
        }

        private void Form1_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                myJet.Rotate(-0.2f);
            }
            else if (e.KeyCode == Keys.D)
            {
                myJet.Rotate(0.2f);
            }
            else if (e.KeyCode == Keys.Space)
            {
                myJet.FireBullet();
            }
        }

        #endregion

        private Button button1;
    }
}