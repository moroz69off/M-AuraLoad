
namespace M_AuraLoad_F7
{
    partial class AForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AForm));
            this.sceneControl = new SharpGL.SceneControl();
            ((System.ComponentModel.ISupportInitialize)(this.sceneControl)).BeginInit();
            this.SuspendLayout();
            // 
            // sceneControl
            // 
            this.sceneControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sceneControl.DrawFPS = false;
            this.sceneControl.Location = new System.Drawing.Point(0, 0);
            this.sceneControl.Name = "sceneControl";
            this.sceneControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL3_3;
            this.sceneControl.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.sceneControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.sceneControl.Size = new System.Drawing.Size(914, 614);
            this.sceneControl.TabIndex = 6;
            this.sceneControl.OpenGLDraw += new SharpGL.RenderEventHandler(this.sceneControl_OpenGLDraw);
            this.sceneControl.Load += new System.EventHandler(this.sceneControl_Load);
            this.sceneControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.sceneControl_MouseDown);
            this.sceneControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.sceneControl_MouseMove);
            this.sceneControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.sceneControl_MouseUp);
            // 
            // AForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 614);
            this.Controls.Add(this.sceneControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AForm";
            this.Text = "Aura load";
            ((System.ComponentModel.ISupportInitialize)(this.sceneControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SharpGL.SceneControl sceneControl;
    }
}
