using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlayFair
{
    public partial class Form1 : Form
    {
        private char[,] keyMatrix;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string key = textBox2.Text.Trim().ToUpper();
            string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ"; // Loại bỏ chữ 'J'

            if (string.IsNullOrEmpty(key))
            {
                keyMatrix = GenerateMatrix(alphabet);
            }
            else
            {
                key = key.Replace("J", "I");
                string keyWithoutDuplicates = new string(key.Distinct().ToArray());
                string keyWithRemainingAlphabet = keyWithoutDuplicates + alphabet;

                keyMatrix = GenerateMatrix(keyWithRemainingAlphabet);
            }

            DisplayKeyMatrix();
        }
        private char[,] GenerateMatrix(string key)
        {
            char[,] matrix = new char[5, 5];
            int keyIndex = 0;

            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    matrix[row, col] = key[keyIndex];
                    keyIndex++;
                }
            }

            return matrix;
        }

        private void DisplayKeyMatrix()
        {
            groupBox1.Controls.Clear();

            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Text = keyMatrix[row, col].ToString();
                    textBox.ReadOnly = true;
                    textBox.Width = 30;
                    textBox.TextAlign = HorizontalAlignment.Center;

                    groupBox1.Controls.Add(textBox);
                    groupBox1.SetCellPosition(textBox, new TableLayoutPanelCellPosition(col, row));
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                string plaintext = textBox1.Text.Trim().ToUpper();
                string ciphertext = Encrypt(plaintext);
                textBox3.Text = ciphertext;
            }
            else if (!string.IsNullOrEmpty(textBox3.Text))
            {
                string ciphertext = textBox3.Text.Trim().ToUpper();
                string plaintext = Decrypt(ciphertext);
                textBox1.Text = plaintext;
            }
        }
        private string Encrypt(string plaintext)
        {
            StringBuilder ciphertext = new StringBuilder();

            for (int i = 0; i < plaintext.Length; i += 2)
            {
                char char1 = plaintext[i];
                char char2 = (i + 1 < plaintext.Length) ? plaintext[i + 1] : 'X';

                int row1 = -1, col1 = -1, row2 = -1, col2 = -1;
                FindCharacter(char1, out row1, out col1);
                FindCharacter(char2, out row2, out col2);

                if (row1 == row2)
                {
                    ciphertext.Append(keyMatrix[row1, (col1 + 1) % 5]);
                    ciphertext.Append(keyMatrix[row2, (col2 + 1) % 5]);
                }
                else if (col1 == col2)
                {
                    ciphertext.Append(keyMatrix[(row1 + 1) % 5, col1]);
                    ciphertext.Append(keyMatrix[(row2 + 1) % 5, col2]);
                }
                else
                {
                    ciphertext.Append(keyMatrix[row1, col2]);
                    ciphertext.Append(keyMatrix[row2, col1]);
                }
            }

            return ciphertext.ToString();
        }

        private string Decrypt(string ciphertext)
        {
            StringBuilder plaintext = new StringBuilder();

            for (int i = 0; i < ciphertext.Length; i += 2)
            {
                char char1 = ciphertext[i];
                char char2 = (i + 1 < ciphertext.Length) ? ciphertext[i + 1] : 'X';

                int row1 = -1, col1 = -1, row2 = -1, col2 = -1;
                FindCharacter(char1, out row1, out col1);
                FindCharacter(char2, out row2, out col2);

                if (row1 == row2)
                {
                    plaintext.Append(keyMatrix[row1, (col1 + 4) % 5]);
                    plaintext.Append(keyMatrix[row2, (col2 + 4) % 5]);
                }
                else if (col1 == col2)
                {
                    plaintext.Append(keyMatrix[(row1 + 4) % 5, col1]);
                    plaintext.Append(keyMatrix[(row2 + 4) % 5, col2]);
                }
                else
                {
                    plaintext.Append(keyMatrix[row1, col2]);
                    plaintext.Append(keyMatrix[row2, col1]);
                }
            }

            return plaintext.ToString();
        }

        private void FindCharacter(char c, out int row, out int col)
        {
            row = -1;
            col = -1;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (keyMatrix[i, j] == c)
                    {
                        row = i;
                        col = j;
                        return;
                    }
                }
            }
        }
    }
}
