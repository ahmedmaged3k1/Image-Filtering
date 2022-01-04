using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZGraphTools;

namespace ImageFilters
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        byte[,] ImageMatrix;
        string OpenedFilePath;
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                 OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);

            }
        }

        private void btnZGraph_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0)
            {
                MessageBox.Show("Enter max window size and a trim value.");


            }
            else
            {
                int Max_Window_Size = int.Parse(textBox1.Text);
                int trimValue = int.Parse(textBox2.Text);

                double[] x_values = new double[Max_Window_Size / 2];
                double[] y_values_counting_sort = new double[Max_Window_Size / 2];
                double[] y_values_kth_element = new double[Max_Window_Size / 2];
                double[] y_values_merge_sort = new double[Max_Window_Size / 2];


                int indexOfBothAxis = 0;
                for (int i = 3; i <= Max_Window_Size; i += 2)
                {
                    x_values[indexOfBothAxis] = i;

                    int StartTimeForCountingSort = System.Environment.TickCount;

                    ImageOperations.MakeFilter(ImageMatrix, Max_Window_Size, 0, 0, trimValue, OpenedFilePath);
                    int EndTimeForCountingSort = System.Environment.TickCount;
                    y_values_counting_sort[indexOfBothAxis] = EndTimeForCountingSort - StartTimeForCountingSort;
                    MessageBox.Show("Counting Sort" + (EndTimeForCountingSort - StartTimeForCountingSort).ToString()+" ms");

                    int StartTimeForkthElementSort = System.Environment.TickCount;
                    ImageOperations.MakeFilter(ImageMatrix, Max_Window_Size, 1, 0, trimValue, OpenedFilePath);
                    int EndTimeForkthElementSort = System.Environment.TickCount;
                    y_values_kth_element[indexOfBothAxis] = EndTimeForkthElementSort - StartTimeForkthElementSort;
                    MessageBox.Show("Kth Sort" + (EndTimeForkthElementSort - StartTimeForkthElementSort).ToString() + " ms");


                   


                    indexOfBothAxis++;

                }

                //Create a graph and add two curves to it
                ZGraphForm ZGF = new ZGraphForm("Alpha Trim Graph", "Window Size", "Execution Time");
                ZGF.add_curve("Couting Sort", x_values, y_values_counting_sort, Color.Red);
                ZGF.add_curve("Kth Element", x_values, y_values_kth_element, Color.Blue);
                ZGF.Show();




            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (OpenedFilePath == null)
                return;
            ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
            


            if (textBox1.Text.Length == 0 || comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Enter max window size , select a filter and a sorting algorithm.");


            }

            else
            {
                int Max_Window_Size = int.Parse(textBox1.Text);
                int Sort = comboBox1.SelectedIndex;
                int Filter = comboBox2.SelectedIndex;

                if (Filter == 0)
                {
                    if (textBox2.Text == "")
                    {
                        MessageBox.Show("Enter trim value.");

                    }
                    else
                    {
                        int trimValue = int.Parse(textBox2.Text);
                        int max_trim_value = (((Max_Window_Size * Max_Window_Size) - ((Max_Window_Size - 1) * (Max_Window_Size - 1) + 1)) / 2) - 1;

                        if (trimValue <= max_trim_value)
                        {
                            ImageOperations.MakeFilter(ImageMatrix, Max_Window_Size, Sort, Filter, trimValue, OpenedFilePath);
                            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
                        }
                        else
                        {
                            MessageBox.Show("trim value too big, enter smaller value");
                        }
                    }
                }
                else
                {
                    ImageOperations.MakeFilter(ImageMatrix, Max_Window_Size, Sort, Filter, 0, OpenedFilePath);
                    ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
                }
            }
    

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Enter max window size.");


            }
            else
            {
                int Max_Window_Size = int.Parse(textBox1.Text);

                double[] x_values = new double[Max_Window_Size / 2];
                double[] y_values_counting_sort = new double[Max_Window_Size / 2];
                double[] y_values_quick_sort = new double[Max_Window_Size / 2];

                int indexOfBothAxis = 0;
                for (int i = 3; i <= Max_Window_Size; i += 2)
                {
                    x_values[indexOfBothAxis] = i;

                    int StartTimeForCountingSort = System.Environment.TickCount;

                    ImageOperations.MakeFilter(ImageMatrix, Max_Window_Size, 0, 1, 0, OpenedFilePath);
                    int EndTimeForCountingSort = System.Environment.TickCount;
                    y_values_counting_sort[indexOfBothAxis] = EndTimeForCountingSort - StartTimeForCountingSort;


                    int StartTimeForQuickSort = System.Environment.TickCount;

                    ImageOperations.MakeFilter(ImageMatrix, Max_Window_Size, 2, 1, 0, OpenedFilePath);
                    int EndTimeForQuickSort = System.Environment.TickCount;
                    y_values_quick_sort[indexOfBothAxis] = EndTimeForQuickSort - StartTimeForQuickSort;

                    indexOfBothAxis++;

                }

                //Create a graph and add two curves to it
                ZGraphForm ZGF = new ZGraphForm("Adaptive Median Graph", "Window Size", "Execution Time");
                ZGF.add_curve("Couting Sort", x_values, y_values_counting_sort, Color.Red);
                ZGF.add_curve("Quick Sort", x_values, y_values_quick_sort, Color.Blue);
                ZGF.Show();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}