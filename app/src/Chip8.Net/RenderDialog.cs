﻿namespace Chip8.Net
{
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    using Chip8.Net.Assets;
    using Chip8.Net.Engine;
    using Chip8.Net.Presenter;
    using Chip8.Net.Settings;
    using Chip8.Net.View;

    using Graphics = Chip8.Net.Settings.Graphics;

    public partial class RenderDialog : Form, IRenderDialog
    {
        private readonly VirtualMachine virtualMachine;
        private readonly RenderPresenter presenter;

        public RenderDialog()
        {
            this.InitializeComponent();
            this.KeyDown += this.FrmKeyDown;
            this.KeyUp += this.FrmKeyUp;
            this.virtualMachine = new VirtualMachine(new VideoRender(this.pbRender));
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

        protected override void OnClosed(EventArgs e)
        {
            this.virtualMachine.Stop();
            Thread.Sleep(20);
            base.OnClosed(e);
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

        private void StmSmallGraphicsClick(object sender, EventArgs e)
        {
            this.presenter.SetGraphics(Graphics.Small());
        }

        private void StmExitClick(object sender, EventArgs e)
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
                Filter = AppResources.Settings_DialogFileType
            };
            openDialog.FileOk += (o, args) => this.presenter.InitializeRom(openDialog.FileName);
            openDialog.ShowDialog();
        }

        private void StmPowerOffClick(object sender, EventArgs e)
        {
            this.virtualMachine.Stop();
        }

        private void StmResetClick(object sender, EventArgs e)
        {
            this.virtualMachine.Reset();
        }

        private void StmEnableSoundClick(object sender, EventArgs e)
        {
            if (this.stmEnableSound.CheckState == CheckState.Checked)
            {
                this.stmEnableSound.CheckState = CheckState.Unchecked;
                this.virtualMachine.Sound.Enabled = false;
            }
            else
            {
                this.stmEnableSound.CheckState = CheckState.Checked;
                this.virtualMachine.Sound.Enabled = true;
            }
        }

        private void StmSaveStateClick(object sender, EventArgs e)
        {
            this.virtualMachine.SaveState();
        }

        private void StmLoadStateClick(object sender, EventArgs e)
        {

        }

        private void StmAboutClick(object sender, EventArgs e)
        {
            var aboutDialog = new AboutDialog();
            aboutDialog.Show(this);
        }

        private void StmPauseClick(object sender, EventArgs e)
        {
            if (this.virtualMachine.ProcessingStatus == ProcessingStatus.Paused)
            {
                this.virtualMachine.Run();
                this.stmPause.Text = AppResources.Settings_MenuPause;
            }
            else if(this.virtualMachine.ProcessingStatus == ProcessingStatus.Running)
            {
                this.virtualMachine.Pause();
                this.stmPause.Text = AppResources.Settings_MenuPlay;
            }
        }

        private void StmDebuggerClick(object sender, EventArgs e)
        {
            this.virtualMachine.Pause();
            this.stmPause.Text = AppResources.Settings_MenuPlay;
            var debuggerDialog = new DebuggerDialog(this.virtualMachine);
            debuggerDialog.Show(this);
        }
    }
}
