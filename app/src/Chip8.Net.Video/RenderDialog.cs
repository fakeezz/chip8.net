﻿namespace Chip8.Net.Video
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    using Chip8.Net.Video.Presenter;
    using Chip8.Net.Video.Settings;
    using Chip8.Net.Video.View;

    using Graphics = Chip8.Net.Video.Settings.Graphics;

    public partial class RenderDialog : Form, IRenderDialog
    {
        private readonly VirtualMachine virtualMachine;
        private readonly RenderPresenter presenter;

        public RenderDialog()
        {
            this.InitializeComponent();
            this.virtualMachine = new VirtualMachine(new VideoRender(this.pbRender));
            this.KeyDown += this.FrmKeyDown;
            this.KeyUp += this.FrmKeyUp;
            this.presenter = new RenderPresenter(this, this.virtualMachine);
        }

        public Size RenderSize
        {
            get
            {
                return this.pbRender.Size;
            }
            set
            {
                this.pbRender.Size = value;
            }
        }

        public Size WindowSize
        {
            get
            {
                return this.Size;
            }
            set
            {
                this.Size = value;
            }
        }

        private void FrmKeyUp(object sender, KeyEventArgs e)
        {
            char key = (char)e.KeyData;
            this.virtualMachine.Keyboard.ReleaseKey(key);
        }

        private void FrmKeyDown(object sender, KeyEventArgs e)
        {
            char key = (char)e.KeyData;
            this.virtualMachine.Keyboard.PressKey(key);
        }

        private void StmSmallGraphicsClick(object sender, System.EventArgs e)
        {
            this.presenter.SetGraphics(Graphics.Small());
        }

        private void StmExitClick(object sender, System.EventArgs e)
        {
            Environment.Exit(0);
        }

        private void StmLargeGraphicsClick(object sender, EventArgs e)
        {
            this.presenter.SetGraphics(Graphics.Large());
        }

        private void StmLoadRomClick(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                Filter = "ROM Files (*.rom)|*.rom"
            };
            openDialog.FileOk += (o, args) => this.presenter.InitializeRom(openDialog.FileName);
            openDialog.ShowDialog();
        }
    }
}