//============================================================================
// Name        : Form1.cs
// Author      : Abdurrahman Nurhakim
// Version     : 1.0
// Copyright   : Your copyright notice
// Description : Draw like Paint Software and Show the Axis Coordinat (X & Y) on Json Format
//============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HariffDispensing
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Width = 620;
            this.Height = 620;
            bm = new Bitmap(pic.Width, pic.Height);
            g = Graphics.FromImage(bm);
            //g.Clear(Color.White);
            timer1.Start();
        }

        private List<Point> points = new List<Point>();
        Bitmap bm, Save;
        Graphics g, gb;
        String savePrint, saveLocal;
        float m_gradien, m_gradien_1, valSelect, valSelect2;
        int val_gradien, val_gradien_1;
        bool paint = false;
        Point px, py;
        Pen Pencil;
        int index, cntErase, cntPen, cntPrint, flagPointing;
        int x, y, nX, nY, nX_1, nY_1, dX, dY, x_view, y_view;
        int x_out, y_out;
        int cntPencil = 0;

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            if (index == 3) { setPointAdd(e); Refresh(); } else { points.Clear(); }
            py = e.Location;
        }

        void setPointAdd(MouseEventArgs e)
        {
            if(flagPointing == 0)
            {
                points.Add(e.Location);
            }
            
        }

        void TXT_Save()
        {
            SetDT();

            /*
            if (cntPen > 1)
            {
                //textBox1.AppendText(nX.ToString() + " " + nY.ToString() + " " + nX_1.ToString() + " " + nY_1.ToString() + " " + dX.ToString() + " " + dY.ToString() + " " + "\r\n");
                //textBox1.AppendText("{" + "\"" + "SD" + "\"" + ":" + "1" + ", " + "\"" + "X" + "\"" + ":" + nX.ToString() + ", " + "\"" + "Y" + "\"" + ":" + " " + nY.ToString() + ", " + "\"" + "spd" + "\"" + ":" + " " + speed.Value.ToString() + "}" + "\r\n");
            } else { }
            */
        }

        void SetDT()
        {
            if (cntPen > 0)
            {
                dX = nX - nX_1;
                dY = nY - nY_1;
                m_gradien = setinfinity(Math.Abs(dX), Math.Abs(dY));
                m_gradien_1 = setinfinity(Math.Abs(dY), Math.Abs(dX));
                val_gradien = setInt(m_gradien);
                val_gradien_1 = setInt(m_gradien_1);
            }

        }

        int setInt(float input)
        {
            float valFloat;
            int floatToInt, output;
            
            floatToInt = (int)input;
            valFloat = Math.Abs(input - floatToInt);
            if (valFloat <= 0.5) {
                output = floatToInt;
            }
            else {
                output = floatToInt + 1;
            }
            return output;
        }

        float setinfinity(int inputA, int inputB)
        {
            float output;
            if (inputB == 0)
            {
              output = 0;
            }
            else
            {
              output =  (float)inputB / (float)inputA;
            }
            return output;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Pencil.Width = (float)lineMark.Value;
        }

        void foreachSet()
        {
            foreach (Point point in points)
            {
                nX_1 = nX;
                nY_1 = nY;
                nX = point.X;
                nY = point.Y;
            }
        }


        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            if (flagPointing == 0)
            {
                whenClick();
            } else { }
        }

        void whenClick()
        {
            if (index == 3)
            {
                cntPen++;
                foreachSet();
                g.FillEllipse(SelectBrushColor(cntPencil), nX-3, nY-3, Pencil.Width, Pencil.Width);
                cntPencil++;
                if (points.Count < 2) return;
                g.DrawLines(Pencil, points.ToArray());
                TXT_Save();
                SetprintData();
                //trial();
                //setView();
                //printValNormal();
                //cntPrint = 0;
            }
            else
            {
                cntPen = 0;
                cntPrint = 0;
                flagPointing = 0;
                timer3.Stop(); 
            }
        }

        Brush SelectBrushColor(int input)
        {
            Brush output;
            if (input == 0)
            {
                output = Brushes.Red;
            }
            else
            {
                output = Brushes.Black;
            }
            return output;
        }

        void SetprintData()
        {
            timer3.Start();
        }

        /*
        void trial()
        {
            textBox1.AppendText(setAlwayPlus(convertToRealVal(nX)).ToString() + " " + setAlwayPlus(convertToRealVal(nY)).ToString() + " " + setAlwayPlus(convertToRealVal(nX_1)).ToString() + " " + setAlwayPlus(convertToRealVal(nY_1)).ToString() + " " + dX.ToString() + " " + dY.ToString() + " " + m_gradien.ToString() + " " + val_gradien.ToString() + " " + flagPointing.ToString() + "\r\n");
        }
        */

        private void timer3_Tick(object sender, EventArgs e)
        {
            cntPrint++;
            if(cntPrint <= 30) {
                flagPointing = 1;
                if (cntPen > 0)
                {
                    setXY_View();
                    saveView();
                } else { }
            } else {
                Blokinfinity();
                flagPointing = 0;
                cntPrint = 0;
                timer3.Stop();
                x_view = 0;
                y_view = 0;
            }
        }



        void Blokinfinity()
        {
            if(dX == 0 || dY == 0)
            {
                printValNormal();
            } else
            {
                setView();
                printValNormal();
            }
        }

        String toJsonNquotationMark(String Input)
        {
            String output = "\"" + Input + "\"" + " : ";
            return output;
        }

        String valColorView(int input)
        {
            String output;
            if (input == 1 || input == 0)
            {
                output = "true";
            } else
            {
                output = "false";
            }
            return output;
        }

        void printValNormal()
        {
            //setAlwayPlus(convertToRealVal(nY, 1000))
            textBox1.AppendText("{" + toJsonNquotationMark("begin") + valColorView(cntPencil) + ", " + toJsonNquotationMark("X") + setAlwayPlus(convertToRealVal(nX, 1000)).ToString() + ", " + toJsonNquotationMark("Y") + setAlwayPlus(convertToRealVal(nY, 1000)).ToString() + ", " + toJsonNquotationMark("Z") + z_val.Value.ToString() + ", " + toJsonNquotationMark("f") + " " + speed.Value.ToString() + "}" + "\r\n");
            //textBox1.AppendText(cntPen.ToString() + "\r\n");
        }
        /*
        int averageValdXY(int input)
        {
            int output;
            float calVal;
            calVal = input/30;
        }
        */

        void setXY_View()
        {
            if (m_gradien < 1)
            {
                set_Vertical();
                valSelect = ((float)Math.Abs(dX)) / ((float)30); //x
                valSelect2 = ((float)Math.Abs(dY)) / ((float)30); //y
            }
            else if (m_gradien == 1)
            {
                x_view++;
                y_view++;
                valSelect = ((float)Math.Abs(dY)) / ((float)30);
            }
            else if (m_gradien > 1)
            {
                set_Horizontal();
                valSelect2 = ((float)Math.Abs(dY))/((float)30); //y
                valSelect = ((float)Math.Abs(dX)) / ((float)30); //x
            }
            else {
                x_view = 0;
                y_view = 0;

            }
            x_out = MinusPlus(x_view, dX);
            y_out = MinusPlus(y_view, dY);
        }

        void set_Vertical()
        {
            y_view++;
            if (y_view % val_gradien_1 == 0 && y_view!=0)
            {
                x_view=y_view;
            }
            else { }
        }
        void set_Horizontal()
        {
            x_view++;
            if (x_view % val_gradien == 0 && x_view!=0)
            {
                y_view=x_view;
            } else { }
        }

        int MinusPlus(int input, int parameter)
        {
            int output;
            if (parameter >= 0)
            {
                output = 1 * input;
            }
            else {
                output = -1 * input;
            }
            return output;
        }

        int convertToReal(int input, int var)
        {
            int output;
            output = input * var;
            return output;
        }

        int sumValTotal(int A, int B)
        {
            int output;
            output = A + B;
            return output;
        }

        String valColorViewPenSet(int inputA, int inputB)
        {
            String output;
            if ((inputA == 2 || inputA == 1 || inputA == 0) && (inputB == 0 || inputB == 1))
            {
                output = "true";
            }
            else
            {
                output = "false";
            }
            return output;
        }

        void saveView()
        {
            int viewX = setAlwayPlus(convertToRealVal(sumValTotal(nX_1, convertToReal(x_out, ((int)valSelect))), 1000));
            int viewY = setAlwayPlus(convertToRealVal(sumValTotal(nY_1, convertToReal(y_out, ((int)valSelect2))), 1000));
            //int viewX = convertToRealVal(sumValTotal(nX_1, convertToReal(x_out, (int)valSelect)),1000);
            //int viewY = convertToRealVal(sumValTotal(nY_1, convertToReal(y_out, (int)valSelect2)),1000);
            //savePrint += "{" + "\"" + "start" + "\"" + ":" + " true" + ", " + "\"" + "X" + "\"" + ":" + viewX.ToString() + ", " + "\"" + "Y" + "\"" + ":" + " " + viewY.ToString() + ", " + "\"" + "Z" + "\"" + ":" + z_val.Value.ToString() + ", " + "\"" + "fX" + "\"" + ":" + " " + speed.Value.ToString() + ", " + "\"" + "fY" + "\"" + ":" + " " + speed.Value.ToString() + ", " + "\"" + "fZ" + "\"" + ":" + " " + speed.Value.ToString() + "}" + "\r\n";
            //savePrint = "{" + "\"" + "start" + "\"" + ":" + " true" + ", " + "\"" + "X" + "\"" + ":" + x_out.ToString() + ", " + "\"" + "Y" + "\"" + ":" + " " + y_out.ToString() + ", " + "\"" + "Z" + "\"" + ":" + val_gradien.ToString() + ", " + "\"" + "fX" + "\"" + ":" + " " + speed.Value.ToString() + ", " + "\"" + "fY" + "\"" + ":" + " " + speed.Value.ToString() + ", " + "\"" + "fZ" + "\"" + ":" + " " + speed.Value.ToString() + "}" + "\r\n";
            //savePrint += "{" + toJsonNquotationMark("begin") + valColorViewPenSet(cntPencil, cntPrint) + ", " + toJsonNquotationMark("start") + "true" + ", " + toJsonNquotationMark("X") + viewX.ToString() + ", " + toJsonNquotationMark("Y") + viewY.ToString() + ", " + toJsonNquotationMark("Z") + z_val.Value.ToString() + ", " + toJsonNquotationMark("fX") + " " + speed.Value.ToString() + ", " + toJsonNquotationMark("fY") + " " + speed.Value.ToString() + ", " + toJsonNquotationMark("fZ") + " " + speed.Value.ToString() + "}" + "\r\n";
            savePrint += "{" + toJsonNquotationMark("begin") + valColorViewPenSet(cntPencil, cntPrint) + ", " + toJsonNquotationMark("X") + viewX.ToString() + ", " + toJsonNquotationMark("Y") + viewY.ToString() + ", " + toJsonNquotationMark("Z") + z_val.Value.ToString() + ", " + toJsonNquotationMark("f") + " " + speed.Value.ToString() + "}" + "\r\n";
            //savePrint += valSelect.ToString() + " " + x_out.ToString() + " " + convertToReal(x_out, (int)valSelect).ToString() + " " + nX_1.ToString() + " " + (nX_1+ convertToReal(x_out, (int)valSelect)).ToString() + " " + dX.ToString()  + "     " + m_gradien.ToString() + "      " + y_out.ToString() + " " + convertToReal(y_out, (int)valSelect2).ToString() + " " + nY_1.ToString() + " " + (nY_1 + convertToReal(y_out, (int)valSelect2)).ToString() + " " + dY.ToString() + "     " + m_gradien_1.ToString() + "\r\n";

        }

        void setView()
        {
            //saveView();
            //textBox1.AppendText("{" + "\"" + "start" + "\"" + ":" + " true" + ", " + "\"" + "X" + "\"" + ":" + setAlwayPlus(convertToRealVal(nX_1+x_view)).ToString() + ", " + "\"" + "Y" + "\"" + ":" + " " + setAlwayPlus(convertToRealVal(nY_1+y_view)).ToString() + ", " + "\"" + "Z" + "\"" + ":" + z_val.Value.ToString() + ", " + "\"" + "fX" + "\"" + ":" + " " + speed.Value.ToString() + ", " + "\"" + "fY" + "\"" + ":" + " " + speed.Value.ToString() + ", " + "\"" + "fZ" + "\"" + ":" + " " + speed.Value.ToString() + "}" + "\r\n");
            textBox1.AppendText(savePrint);
            savePrint = "";
            //textBox1.AppendText(x_out.ToString() + "\r\n");
        }

        int selectMaxVal(int A, int B)
        {
            int output;
            if(A > B) {
              output = A;
            }
            else {
              output = B;
            }
            return output;
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                if (index == 1)
                {
                    penTXT();
                    px = e.Location;
                    g.DrawLine(Pencil, px, py);
                    py = px;
                } else if (index == 2)
                {
                    timer2.Start();
                } 
            }


            pic.Refresh();
            x = e.X;
            y = e.Y;
        }


        void penTXT() {
            cntPencil++;
            textBox1.AppendText("{" + toJsonNquotationMark("begin") + valColorView(cntPencil) + ", " + toJsonNquotationMark("X") + setAlwayPlus(convertToRealVal(x, 1000)).ToString() + ", " + toJsonNquotationMark("Y") + setAlwayPlus(convertToRealVal(y, 1000)).ToString() + ", " + toJsonNquotationMark("Z") + z_val.Value.ToString() + ", " + toJsonNquotationMark("f") + " " + speed.Value.ToString() + "}" + "\r\n");
        }
        
        int convertToRealVal(int input, float val)
        {
            int output;
            float converter;
            converter = ((float)input / 20)* val;
            output = (int)converter;
            return output;
        }

        int setAlwayPlus(int input)
        {
            int output;
            if (input<0)
            {
                output = 0;
            } else if (input > 31000)
            {
                output = 31000;
            }
            else
            {
                output = input;
            }
            return output;
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog isave = new SaveFileDialog();
            isave.Filter = "txt files (*.txt) |*.txt";
            isave.FilterIndex = 2;
            isave.RestoreDirectory = false;

            if (isave.ShowDialog() == DialogResult.OK)
                System.IO.File.WriteAllText(isave.FileName, textBox1.Text);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            cntErase++;
            if (cntErase > 3) //erase
            {
                cntErase = 0;
                cntPencil = 0;
                textBox1.Clear();
                this.Width = 620;
                this.Height = 620;
                bm = new Bitmap(pic.Width, pic.Height);
                g = Graphics.FromImage(bm);
                pic.BackgroundImage = Save;
                timer2.Stop();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // open file dialog   
            OpenFileDialog open = new OpenFileDialog();
            // image filters  
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                Save = new Bitmap(open.FileName);
                gb = Graphics.FromImage(Save);
                saveLocal = open.FileName;
                textBox2.Text = saveLocal;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            setStartPencil();
            pic.Image = bm;
            pic.BackgroundImage = Save;
        }

        void setStartPencil()
        {
            if (flagPointing == 0)
            {
               Pencil = new Pen(SelectColor(cntPencil), (float)lineMark.Value);
            }
            else { }
        }

        Color SelectColor(int input)
        {
            Color output;
            if (input == 0)
            {
                output = Color.Red;
            }
            else
            {
                output = Color.Black;
            }
            return output;
        }



        private void btn_line_Click(object sender, EventArgs e)
        {
            index = 3;
        }

        private void btn_eraser_Click(object sender, EventArgs e)
        {
            index = 2;
        }

        private void btn_pencil_Click(object sender, EventArgs e)
        {
            index = 1;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {


        }

        private void speed_ValueChanged(object sender, EventArgs e)
        {

        }

        private void speed_Click(object sender, EventArgs e)
        {

        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
