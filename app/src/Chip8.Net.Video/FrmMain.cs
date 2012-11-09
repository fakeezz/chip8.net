﻿namespace Chip8.Net.Video
{
    using System.Timers;
    using System.Windows.Forms;

    using Chip8.Net.Video.Settings;

    using CycleTimer = System.Timers.Timer;

    public partial class FrmMain : Form
    {
        private Processor processor;
        private CycleTimer cycleTimer;
        
        public FrmMain()
        {
            this.InitializeComponent();
            this.Initialize();
        }
        
        private void Initialize()
        {
            this.processor = new Processor(new VideoRender(this));
            this.processor.Memory.LoadRom(Loader.LoadRom(@"E:\Github\chip8.net\roms\PUZZLE.rom"));
            this.processor.Memory.LoadCharacters();

            this.cycleTimer = new CycleTimer
            {
                Enabled = true,
                Interval = 0.1666666666667,
                AutoReset = true
            };
            this.cycleTimer.Elapsed += this.CycleProcess;
        }

        private void CycleProcess(object sender, ElapsedEventArgs e)
        {
            this.cycleTimer.Stop();
            this.processor.StepRun();
            this.cycleTimer.Start();
        }
    }
}
