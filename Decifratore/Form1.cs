using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using Decipher.Properties;
using System.IO;
using Microsoft.VisualBasic.ApplicationServices;
using System;

namespace Decipher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            OutputTextBox.Text = InputTextBox.Text;
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

        }

        public static void drawGraph(PictureBox pb, PictureBox pb2)
        {
            int x = 0;
            pb.Refresh();
            pb2.Refresh();
            Font drawFont = new Font(FontFamily.GenericMonospace, 10, FontStyle.Bold);

            float totale = 0;
            foreach (KeyValuePair<string, int> kvp in frequenza)
            {
                totale = totale + kvp.Value;
            }
            Console.WriteLine("Totale " + totale);

            foreach (KeyValuePair<string, int> kvp in frequenza)
            {
                x += (pb.Size.Width / 27);
                int y = pb.Size.Height - kvp.Value * 32;
                Graphics g1 = pb.CreateGraphics();
                Graphics g2 = pb2.CreateGraphics();
                int percentuale = (int)(kvp.Value / totale * 100);

                g1.DrawLine(new Pen(Color.Lime, 7f), new Point(x, pb.Size.Height), new Point(x, y));
                
                //Disegna Frequenza
                if (kvp.Value > 0)
                {
                    g2.DrawString(kvp.Key.ToString().ToUpper(), drawFont, Brushes.Lime, new Point(x - 8, 5));
                    g2.DrawString(percentuale.ToString() + "%", drawFont, Brushes.Lime, new Point(x - 10, 25));
                }
            }
        }

        static string[] arrParoleCifrate;
        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            OutputTextBox.Text = converStringToCharArray(InputTextBox.Text);
            lettersFrequency(InputTextBox.Text);
            drawGraph(pictureBox1, pictureBox2);

            Console.WriteLine(testoCifrato);

            //divido la stringa in parole
            arrParoleCifrate = testoCifrato.Split(' ');

            //Sort l'array dalla parola più lunga alla più corta
            Array.Sort(arrParoleCifrate, (x, y) => y.Length.CompareTo(x.Length));
        }

        private void buttonIpotizza_Click(object sender, EventArgs e)
        { 
            //Ipotizza
            int indexParola = 0;
            string rigaFileTesto;
            string output = null;
            int n = 0;
            //per ogni parola cifrata
            foreach (string parolaCifrata in arrParoleCifrate)
            {
                Console.WriteLine("Parola da Decifrare: [" + indexParola + "]" + parolaCifrata);
                consoleTextBox.AppendText("Ipotizzazione [" + (indexParola + 1) + "]" + Environment.NewLine);
                //parola cifrata in arr di char
                char[] parolaCifrataInCaratteri = arrParoleCifrate[indexParola].ToCharArray();
                indexParola++;
                using (StreamReader sr = new StreamReader(@"listaParole.txt"))
                {
                    //legge ogni linea del file di testo
                    while ((rigaFileTesto = sr.ReadLine()) != null)
                    {
                        //trasforma parola nel file di testo in array di char
                        char[] rigaFileTestoInCaratteri = rigaFileTesto.ToCharArray();

                        //se parolaTesto==ParolaCifrata in lunghezza
                        if (rigaFileTesto.Length == parolaCifrata.Length)
                        {
                            string consoleOutput = null;
                            //per ogni lettera della parola cifrata
                            for (int j = 0; j < parolaCifrata.Length; j++)
                            {
                                //le lettere nella parola nel testo sono messe nell'output cifrato
                                consoleOutput = swapLetters(parolaCifrataInCaratteri[j], rigaFileTestoInCaratteri[j]);
                            }

                            //Console.WriteLine("Possibile Deficrazione: " + consoleOutput);
                            //le parole di due lettere devono per forza essere una delle parole nel file di testo

                            string[] arrParoleDecifrate = consoleOutput.Split(' ');
                            //Console.Write("Frase Divisa: ");
                            foreach (string temp in arrParoleDecifrate)
                            {
                                if (temp.Equals(rigaFileTesto))
                                {
                                    //se ci parole di 2 lettere che non sono "la le mi ma ..." allora non le stampare
                                    if (temp.Length == 2 && !temp.Contains("*"))
                                    {
                                        using (StreamReader r = new StreamReader(@"listaParoleDueLettere.txt"))
                                        {
                                            string line;
                                            while ((line = r.ReadLine()) != null)
                                            {
                                                if (temp.Equals(line))
                                                {
                                                    Console.WriteLine("\tPossibile Soluzione: " + consoleOutput);
                                                    consoleTextBox.AppendText("\t" + consoleOutput + Environment.NewLine);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\tPossibile Soluzione: " + consoleOutput);
                                        consoleTextBox.AppendText("\t" + consoleOutput + Environment.NewLine);
                                    }
                                }
                            }
                        }
                        else { }
                        //continua fino a provare tutte le parole
                    }
                }

            }
            OutputTextBox.Text = converStringToCharArray(InputTextBox.Text);
        }

        static Dictionary<string, int> frequenza;
        public static string lettersFrequency(string testoCifrato)
        {
            testoCifrato = testoCifrato.ToLower();
            frequenza = new Dictionary<string, int>();

            char[] alfabeto = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
                                'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q',
                                'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

            string output = null;

            for (int i = 0; i < alfabeto.Length; i++)
            {
                int f = 0;
                for (int k = 0; k < arrCharTestoCifratoOutput.Length; k++)
                {
                    if (arrCharTestoCifratoInput[k].Equals(alfabeto[i]))
                        f++;
                }
                frequenza.Add(alfabeto[i].ToString(), f);
                f = 0;

            }

            foreach (KeyValuePair<string, int> kvp in frequenza)
            {
                //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                Console.WriteLine(string.Format("Lettera {0} compare {1} volte", kvp.Key, kvp.Value));
                output = output + string.Format("'{0}'={1}\n", kvp.Key, kvp.Value);
            }

            return output;
        }

        static char[] arrCharTestoCifratoInput;
        static char[] arrCharTestoCifratoOutput;

        /*Converte la stringa in caratteri, e i caratteri in asterischi "*" */
        static string testoCifrato = null;
        public static string converStringToCharArray(string testoCifratoInput)
        {
            testoCifrato = testoCifratoInput.ToLower();

            arrCharTestoCifratoInput = testoCifrato.ToCharArray();
            arrCharTestoCifratoOutput = testoCifrato.ToCharArray();

            Console.WriteLine("New Ciphred Input Detected: " + testoCifrato);

            for (int i = 0; i < arrCharTestoCifratoOutput.Length; i++)
            {
                if (char.IsWhiteSpace(arrCharTestoCifratoOutput[i]))
                {
                    arrCharTestoCifratoOutput[i] = ' ';
                }
                else
                    arrCharTestoCifratoOutput[i] = '*';
            }

            string output = new string(arrCharTestoCifratoOutput);
            return output;
        }

        public static string swapLetters(char charToSwap, char newChar)
        {
            string output = new string(arrCharTestoCifratoOutput);

            for (int i = 0; i < arrCharTestoCifratoOutput.Length; i++)
            {
                if (arrCharTestoCifratoInput[i].Equals(charToSwap))
                {
                    arrCharTestoCifratoOutput[i] = newChar;
                }
                else if (arrCharTestoCifratoInput[i] == ' ')
                    arrCharTestoCifratoOutput[i] = ' ';
                else if (arrCharTestoCifratoOutput[i] == '*')
                    arrCharTestoCifratoOutput[i] = '*';

            }

            output = new string(arrCharTestoCifratoOutput);

            return output;
        }

        private void textBoxInputA_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputA.Text))
            {
                OutputTextBox.Text = swapLetters('a', '*');
            }
            else
            {
                char[] ch = textBoxInputA.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('a', ch[0]);
            }
        }

        private void textBoxInputB_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputB.Text))
            {
                OutputTextBox.Text = swapLetters('b', '*');
            }
            else
            {
                char[] ch = textBoxInputB.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('b', ch[0]);
            }
        }

        private void textBoxInputC_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputC.Text))
            {
                OutputTextBox.Text = swapLetters('c', '*');
            }
            else
            {
                char[] ch = textBoxInputC.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('c', ch[0]);
            }
        }

        private void textBoxInputD_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputD.Text))
            {
                OutputTextBox.Text = swapLetters('d', '*');
            }
            else
            {
                char[] ch = textBoxInputD.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('d', ch[0]);
            }
        }

        private void textBoxInputE_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputE.Text))
            {
                OutputTextBox.Text = swapLetters('e', '*');
            }
            else
            {
                char[] ch = textBoxInputE.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('e', ch[0]);
            }
        }

        private void textBoxInputF_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputF.Text))
            {
                OutputTextBox.Text = swapLetters('f', '*');
            }
            else
            {
                char[] ch = textBoxInputF.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('f', ch[0]);
            }
        }

        private void textBoxInputG_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputG.Text))
            {
                OutputTextBox.Text = swapLetters('g', '*');
            }
            else
            {
                char[] ch = textBoxInputG.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('g', ch[0]);
            }
        }

        private void textBoxInputH_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputH.Text))
            {
                OutputTextBox.Text = swapLetters('h', '*');
            }
            else
            {
                char[] ch = textBoxInputH.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('h', ch[0]);
            }
        }

        private void textBoxInputI_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputI.Text))
            {
                OutputTextBox.Text = swapLetters('i', '*');
            }
            else
            {
                char[] ch = textBoxInputI.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('i', ch[0]);
            }
        }

        private void textBoxInputL_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputL.Text))
            {
                OutputTextBox.Text = swapLetters('l', '*');
            }
            else
            {
                char[] ch = textBoxInputL.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('l', ch[0]);
            }
        }

        private void textBoxInputM_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputM.Text))
            {
                OutputTextBox.Text = swapLetters('m', '*');
            }
            else
            {
                char[] ch = textBoxInputM.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('m', ch[0]);
            }
        }

        private void textBoxInputN_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputN.Text))
            {
                OutputTextBox.Text = swapLetters('n', '*');
            }
            else
            {
                char[] ch = textBoxInputN.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('n', ch[0]);
            }
        }

        private void textBoxInputO_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputO.Text))
            {
                OutputTextBox.Text = swapLetters('o', '*');
            }
            else
            {
                char[] ch = textBoxInputO.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('o', ch[0]);
            }
        }

        private void textBoxInputP_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputP.Text))
            {
                OutputTextBox.Text = swapLetters('p', '*');
            }
            else
            {
                char[] ch = textBoxInputP.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('p', ch[0]);
            }
        }

        private void textBoxInputQ_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputQ.Text))
            {
                OutputTextBox.Text = swapLetters('q', '*');
            }
            else
            {
                char[] ch = textBoxInputQ.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('q', ch[0]);
            }
        }

        private void textBoxInputS_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputS.Text))
            {
                OutputTextBox.Text = swapLetters('s', '*');
            }
            else
            {
                char[] ch = textBoxInputS.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('s', ch[0]);
            }
        }

        private void textBoxInputT_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputT.Text))
            {
                OutputTextBox.Text = swapLetters('t', '*');
            }
            else
            {
                char[] ch = textBoxInputT.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('t', ch[0]);
            }
        }

        private void textBoxInputU_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputU.Text))
            {
                OutputTextBox.Text = swapLetters('u', '*');
            }
            else
            {
                char[] ch = textBoxInputU.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('u', ch[0]);
            }
        }

        private void textBoxInputV_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputV.Text))
            {
                OutputTextBox.Text = swapLetters('v', '*');
            }
            else
            {
                char[] ch = textBoxInputV.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('v', ch[0]);
            }
        }

        private void textBoxInputZ_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputZ.Text))
            {
                OutputTextBox.Text = swapLetters('z', '*');
            }
            else
            {
                char[] ch = textBoxInputZ.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('z', ch[0]);
            }
        }

        private void textBoxInputR_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputR.Text))
            {
                OutputTextBox.Text = swapLetters('r', '*');
            }
            else
            {
                char[] ch = textBoxInputR.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('r', ch[0]);
            }
        }

        private void textBoxInputJ_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputJ.Text))
            {
                OutputTextBox.Text = swapLetters('j', '*');
            }
            else
            {
                char[] ch = textBoxInputJ.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('j', ch[0]);
            }
        }

        private void textBoxInputK_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputK.Text))
            {
                OutputTextBox.Text = swapLetters('k', '*');
            }
            else
            {
                char[] ch = textBoxInputK.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('k', ch[0]);
            }
        }

        private void textBoxInputW_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputW.Text))
            {
                OutputTextBox.Text = swapLetters('w', '*');
            }
            else
            {
                char[] ch = textBoxInputW.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('w', ch[0]);
            }
        }

        private void textBoxInputX_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputX.Text))
            {
                OutputTextBox.Text = swapLetters('x', '*');
            }
            else
            {
                char[] ch = textBoxInputX.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('x', ch[0]);
            }
        }

        private void textBoxInputY_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxInputY.Text))
            {
                OutputTextBox.Text = swapLetters('y', '*');
            }
            else
            {
                char[] ch = textBoxInputY.Text.ToCharArray();
                OutputTextBox.Text = swapLetters('y', ch[0]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void consoleTextBox_TextChanged(object sender, EventArgs e)
        {
            consoleTextBox.SelectionStart = consoleTextBox.Text.Length;
            consoleTextBox.ScrollToCaret();
        }

        private void OutputTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}