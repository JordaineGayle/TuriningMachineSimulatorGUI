using System;

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



        private void Reset(object sender, EventArgs e)
        {
            textBox1.ResetText();
            textBox1.Clear();
            machine = TuringMachine.CreateTuringMachineForAnagramAndOrPalindromeOfRacecar();
            textBox1.Enabled = true;
            button2.Enabled = false;
            ResetCells();
        }



        private void ViewTransitions(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.Visible = !listBox1.Visible;
            foreach (var ev in machine.Events)
            {
                listBox1.Items.Add(ev.Transition);
            }
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
                textBox1.Enabled = false;
                button2.Enabled = true;
                var movement = machine.GetLastMovement();
                if (movement != null)
                {
                    var currentTapeCellControl = tableLayoutPanel1.Controls.Find(GetTableCellLabelName(movement.TapeHead.Position, 1), false).First();
                    currentTapeCellControl.BackColor = machine.GetCurrentState().Accept ? Color.FromArgb(75, 153, 88) : Color.FromArgb(153, 75, 75);
                }
            }
            else
            {
                Render();
            }

        }




        private void LoadTape()
        {
            var tape = machine.GetTape();
            var font = new Font(DefaultFont.FontFamily.Name, 12, FontStyle.Bold);
            for (int x = 0; x < tape.Length; x++)
            {
                tableLayoutPanel1.Controls.Add(new Label() { Name = $"C{x}R0", Text = "", Width = 50, Height = 50, TextAlign = ContentAlignment.MiddleCenter, Font = font }, x, 0);
                tableLayoutPanel1.Controls.Add(new Label() { Name = $"C{x}R1", Text = tape[x].ToString(), Width = 50, Height = 50, BackColor = Color.FromArgb(165, 165, 165), TextAlign = ContentAlignment.MiddleCenter }, x, 1);
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
            var tEvent = machine.GetLastMovement();
            var tapeHead = tEvent?.TapeHead;
            var tape = machine.GetTape();
            if (tapeHead != null && tEvent != null)
            {
                var previousTapeHeadControl = tableLayoutPanel1.Controls.Find(GetTableCellLabelName(tapeHead.PreviousPosition), false).First();
                var currentTapeHeadControl = tableLayoutPanel1.Controls.Find(GetTableCellLabelName(tapeHead.Position), false).First();
                var currentTapeCellControl = tableLayoutPanel1.Controls.Find(GetTableCellLabelName(tapeHead.Position, 1), false).First();
                previousTapeHeadControl.Text = "";
                currentTapeHeadControl.Text = "↓";
                currentTapeCellControl.Text = tape[(int)tapeHead.Position].ToString();
                switch (tEvent.TapeDirection)
                {
                    case TDirection.S:
                        tableLayoutPanel1.HorizontalScroll.Value = tableLayoutPanel1.HorizontalScroll.Value + 50;
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
            var tape = machine.GetTape();
            for (int x = 0; x < tape.Length; x++)
            {
                tableLayoutPanel1.Controls.Find($"C{x}R0", false).LastOrDefault()!.Text = "";
                tableLayoutPanel1.Controls.Find($"C{x}R1", false).LastOrDefault()!.Text = tape[x].ToString();
                tableLayoutPanel1.Controls.Find($"C{x}R1", false).LastOrDefault()!.BackColor = Color.FromArgb(165, 165, 165);
            }
            var control = tableLayoutPanel1.Controls.Find(GetTableCellLabelName(machine.GetTapeHead().Position), false).First();
            control.Text = "↓";
            tableLayoutPanel1.HorizontalScroll.Value = 0;
            tableLayoutPanel1.HorizontalScroll.Visible = false;
            tableLayoutPanel1.VerticalScroll.Visible = false;
            tableLayoutPanel1.HorizontalScroll.Enabled = false;
            tableLayoutPanel1.ScrollControlIntoView(control);
            tableLayoutPanel1.HorizontalScroll.Value = tableLayoutPanel1.HorizontalScroll.Value + 250;
        }





        private string GetTableCellLabelName(long pos, int row = 0) => $"C{pos}R{row}";

    }
}