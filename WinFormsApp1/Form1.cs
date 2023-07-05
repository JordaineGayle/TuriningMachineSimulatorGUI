namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        TuringMachine machine;

        public Form1()
        {
            machine = TuringMachine.CreateTuringMachineForAnagramAndOrPalindromeOfRacecar();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadTape();
        }

        private void OnUserInput(object? sender, KeyEventArgs e)
        {
            Write((char)e.KeyCode);
        }

        private void Reset()
        {
            textBox1.ResetText();
            textBox1.Clear();
            machine = TuringMachine.CreateTuringMachineForAnagramAndOrPalindromeOfRacecar();
            ResetCells();
        }


        private void Write(char character)
        {
            var hasMoreCharacter = machine.Write(character);
            if (hasMoreCharacter is not true)
            {
                machine.ProcessTape(Render);
                var output = machine.GetOutput();
                richTextBox1.Text = output.UserInput;
                richTextBox2.Text = output.TapeOutput;
                richTextBox3.Text = output.StateLabel.ToUpper();
                richTextBox4.Text = output.AcceptedOrRejected;
                //Reset();
            }
            else
            {

                Render();
            }

        }

        private void LoadTape()
        {
            var tape = machine.GetTape();
            for (int x = 0; x < tape.Length; x++)
            {
                tableLayoutPanel1.Controls.Add(new Label() { Name = $"C{x}R0", Text = "", Width = 20, Height = 20 }, x, 0);
                tableLayoutPanel1.Controls.Add(new Label() { Name = $"C{x}R1", Text = tape[x].ToString(), Width = 20, Height = 20 }, x, 1);
            }
            var control = tableLayoutPanel1.Controls.Find(GetTableCellLabelName(machine.GetTapeHead().Position), false).First();
            control.Text = "↓";
            tableLayoutPanel1.HorizontalScroll.Visible = false;
            tableLayoutPanel1.VerticalScroll.Visible = false;
            tableLayoutPanel1.HorizontalScroll.Enabled = false;
            tableLayoutPanel1.ScrollControlIntoView(control);
            tableLayoutPanel1.HorizontalScroll.Value = tableLayoutPanel1.HorizontalScroll.Value + 250;
        }


        private void Render()
        {
            var movement = machine.GetLastMovement();
            var tapeHead = movement?.TapeHead;
            if (tapeHead != null)
            {
                var previousTapeHeadControl = tableLayoutPanel1.Controls.Find(GetTableCellLabelName(machine.GetTapeHead().PreviousPosition), false).First();
                var currentTapeHeadControl = tableLayoutPanel1.Controls.Find(GetTableCellLabelName(machine.GetTapeHead().Position), false).First();
                var currentTapeCellControl = tableLayoutPanel1.Controls.Find(GetTableCellLabelName(machine.GetTapeHead().Position, 1), false).First();
                previousTapeHeadControl.Text = "";
                currentTapeHeadControl.Text = "↓";
                currentTapeCellControl.Text = movement?.Input.ToString();
                switch (machine.GetTapeHead().Direction)
                {
                    case TDirection.S:
                        tableLayoutPanel1.HorizontalScroll.Value = tableLayoutPanel1.HorizontalScroll.Value + 0;
                        break;
                    case TDirection.R:
                        tableLayoutPanel1.HorizontalScroll.Value = tableLayoutPanel1.HorizontalScroll.Value + 50;
                        break;
                    case TDirection.L:
                        tableLayoutPanel1.HorizontalScroll.Value = tableLayoutPanel1.HorizontalScroll.Value - 50;
                        break;
                    default:
                        throw new NotImplementedException();
                };
            }
        }


        private void ResetCells()
        {
            tableLayoutPanel1.Controls.Clear();
            LoadTape();
        }


        private string GetTableCellLabelName(long pos, int row = 0) => $"C{pos}R{row}";

    }
}