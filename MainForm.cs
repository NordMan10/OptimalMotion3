using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OptimalMotion2.Domain;
using OptimalMotion2.Domain.Interfaces;
using OptimalMotion2.Enums;

namespace OptimalMotion2
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            DoubleBuffered = true;

            InitializeComponent();


            InitButtons();
            InitClockLabel();
            
            chart = GetChart(chartGraphicBase);
            table = GetTable();
            topLayout = GetTopLayout();
            mainLayout = GetMainLayout();
            tableLayout = GetRightLayout();
            Controls.Add(mainLayout);


            WindowState = FormWindowState.Maximized;

            model = new Model(1, 1, chart, table);
        }

        private readonly IModel model;
        private readonly IChart chart;
        private readonly ITable table;
        private readonly TableLayoutPanel mainLayout = new TableLayoutPanel();
        private readonly TableLayoutPanel topLayout = new TableLayoutPanel();
        private readonly TableLayoutPanel tableLayout = new TableLayoutPanel();
        private readonly DataGridView tableDataGridView = new DataGridView();
        private readonly Label clock = new Label();
        private readonly PictureBox chartGraphicBase = new PictureBox();

        public Button StartButton { get; private set; }
        public Button StopButton { get; private set; }
        //public Button PauseButton { get; private set; }

        private IChart GetChart(PictureBox chartGraphicBase)
        {
            chartGraphicBase.Dock = DockStyle.Fill;
            return new Chart(chartGraphicBase);
        }

        private ITable GetTable()
        {
            tableDataGridView.Dock = DockStyle.Fill;
            tableDataGridView.Font = new Font("Roboto", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
            tableDataGridView.DefaultCellStyle.Font = new Font("Roboto", 14F, GraphicsUnit.Pixel);
            tableDataGridView.ColumnHeadersHeight = 30;

            return new Table(tableDataGridView);
        }

        private TableLayoutPanel GetMainLayout()
        {
            mainLayout.ColumnCount = 1;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.Controls.Add(topLayout, 0, 0);
            mainLayout.Controls.Add(chartGraphicBase, 0, 1);
            mainLayout.Name = "mainLayout";
            mainLayout.RowCount = 2;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 75F));
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.TabIndex = 0;

            return mainLayout;
        }

        private TableLayoutPanel GetTopLayout()
        {
            topLayout.ColumnCount = 3;
            topLayout.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            topLayout.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            topLayout.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            topLayout.Controls.Add(StartButton, 0, 0);
            topLayout.Controls.Add(StopButton, 1, 0);
            topLayout.Controls.Add(tableLayout, 2, 0);
            //topLayout.Controls.Add(clock, 3, 0);
            topLayout.Name = "topLayout";
            topLayout.RowCount = 1;
            topLayout.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            topLayout.Dock = DockStyle.Fill;
            topLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            return topLayout;
        }

        private TableLayoutPanel GetRightLayout()
        {
            tableLayout.ColumnCount = 1;
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayout.Controls.Add(tableDataGridView, 0, 0);
            tableLayout.Name = "rightLayout";
            tableLayout.RowCount = 1;
            tableLayout.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayout.Dock = DockStyle.Fill;
            tableLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            return tableLayout;
        }

        private void InitClockLabel()
        {
            clock.TextAlign = ContentAlignment.MiddleCenter;
            clock.Dock = DockStyle.Fill;
            clock.Text = $"{0:00}:{0:00}:{0:00}";
            clock.Font = new Font("Roboto", 20f, FontStyle.Bold, GraphicsUnit.Pixel);
        }

        private void InitButtons()
        {
            InitStartButton();
            InitStopButton();
            //InitPauseButton();
        }

        private void InitStartButton()
        {
            StartButton = new Button();
            StartButton.Text = "Start";
            StartButton.Click += StartButtonOnClick;
            StartButton.Font = new Font("Roboto", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
            StartButton.Size = new Size(90, 40);
            StartButton.BackColor = Color.LimeGreen;
            StartButton.FlatStyle = FlatStyle.Flat;

            Controls.Add(StartButton);
        }

        private void InitStopButton()
        {
            StopButton = new Button();
            StopButton.Text = "Stop";
            StopButton.Click += StopButtonOnClick;
            StopButton.Font = new Font("Roboto", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
            StopButton.Size = new Size(90, 40);
            StopButton.BackColor = Color.Red;
            StopButton.FlatStyle = FlatStyle.Flat;
            StartButton.TabIndex = 1;
            Controls.Add(StopButton);
            
        }

        //private void InitPauseButton()
        //{
        //    PauseButton = new Button();
        //    PauseButton.Text = "Pause";
        //    PauseButton.Click += PauseButtonOnClick;
        //    PauseButton.Font = new Font("Roboto", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
        //    PauseButton.Size = new Size(90, 40);
        //    PauseButton.FlatStyle = FlatStyle.Flat;
        //    //PauseButton.BackColor = Color.Yellow;
        //    Controls.Add(PauseButton);
        //}

        private void StartButtonOnClick(object sender, EventArgs e)
        {
            model.ChangeStage(ModelStages.Started);
        }

        private void StopButtonOnClick(object sender, EventArgs e)
        {
            model.ResetIdGenerator();
            model.ChangeStage(ModelStages.Preparing);
        }

        private void PauseButtonOnClick(object sender, EventArgs e)
        {
            model.ChangeStage(ModelStages.Paused);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space) 
                Close();
        }
    }
}
