using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
namespace BOM
{
    public partial class Cümle : Form
    {
        private char[] x;
        private char[] t;
        private Cell[] list;
        private int m;
        static int karsilasma = 0;
        private static int getTransition(char[] x, int p, BOM.Cell[] list, char c)
        {
            if ((p > 0) && (x[(p - 1)] == c))
            {
                return p - 1;
            }
            Cell cell = list[p];
            while (cell != null)
            {
                if (x[cell.element] == c)
                {
                    return cell.element;
                }
                cell = cell.next;
            }
            return -1;
        }
        private static void setTransition(int p, int q, Cell[] list)
        {
            Cell cell = new Cell();

            cell.element = q;
            cell.next = list[p];
            list[p] = cell;
        }
        private static void oracle(char[] x, char[] t, Cell[] list)
        {
            int q = -1; int m = x.Length - 1; int p;
            int[] s = new int[x.Length];

            s[m] = (m + 1);
            for (int i = m; i > 0; i--)
            {
                char c = x[(i - 1)];
                p = s[i];
                while ((p <= m) && ((q = getTransition(x, p, list, c)) == -1))
                {
                    setTransition(p, i - 1, list);
                    p = s[p];
                }
                s[(i - 1)] = (p == m + 1 ? m : q);
            }
            p = 0;
            while (p <= m)
            {
                t[p] = '1';
                p = s[p];
            }
        }
        public static List<int> findAll(String pattern, String source)
        {
            char[] ptrn = pattern.ToCharArray(); char[] y = source.ToCharArray();
            char[] x = new char[ptrn.Length + 1];
            System.Array.Copy(ptrn, 0, x, 0, ptrn.Length);
            int period = 0; int m = ptrn.Length; int n = y.Length;
            List<int> result = new List<int>();
            int i;
            char[] t = new char[x.Length];
            BOM.Cell[] list = new BOM.Cell[x.Length];
            for (i = 0; i < t.Length; i++)
            {
                t[i] = '0';
            }
            oracle(x, t, list);
            int j = 0,p,shift,q;
            while (j <= n - m)
            {
                i = m - 1;
                p = m;
                shift = m;
                while ((i + j >= 0) &&
                  ((q = getTransition(x, p, list, y[(i + j)])) != -1))
                {
                    p = q;
                    if (t[p] == '1')
                    {
                        period = shift;
                        shift = i;
                    }
                    i--;
                    karsilasma++;
                }
                if (i < 0)
                {
                    result.Add(j);
                    shift = period;
                }
                j += shift;
            }
            return result;
        }
        public static Cümle compile(String pattern)
        {
            char[] ptrn = pattern.ToCharArray();
            char[] x = new char[ptrn.Length + 1];
            System.Array.Copy(ptrn, 0, x, 0, ptrn.Length);
            int m = ptrn.Length;

            char[] t = new char[x.Length];
            Cell[] list = new Cell[x.Length];
            for (int i = 0; i < t.Length; i++)
            {
                t[i] = '0';
            }
            oracle(x, t, list);
            Cümle bom = new Cümle();
            bom.m = m;
            bom.x = x;
            bom.t = t;
            bom.list = list;
            return bom;
        }
        public List<int> findAll(String source)
        {
            char[] y = source.ToCharArray();
            int period = 0; int n = y.Length;
            List<int> result = new List<int>();
            int j = 0;
            while (j <= n - this.m)
            {
                int i = this.m - 1;
                int p = this.m;
                int shift = this.m;
                int q;
                while ((i + j >= 0) &&
                  ((q = getTransition(this.x, p, this.list, y[(i + j)])) != -1))
                {
                    p = q;
                    if (this.t[p] == '1')
                    {
                        period = shift;
                        shift = i;
                    }
                    i--;
                }
                if (i < 0)
                {
                    result.Add(j);
                    shift = period;
                }
                j += shift;
            }
            return result;
        }

        public Cümle()
        {
            InitializeComponent();
        }
        private void Cümle_Load(object sender, EventArgs e)
        {
            Cümle temp = new Cümle();
        }
        private void Sonuc_Click(object sender, EventArgs e)
        {
            Eslesme.Text = "0";
            Karsilasma.Text = "0";
            List <int> result = new List<int>();
            if (richTextBox1.Text != "")
            {
                string text = System.IO.File.ReadAllText(richTextBox1.Text);
                result = findAll(Pattern.Text, text);
            }
            else
                result = findAll(Pattern.Text, Source.Text);
            Eslesme.Text = result.Count.ToString();
            Karsilasma.Text = karsilasma.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt (*.txt)|*.txt|html (*.html)|*.html|Hepsi (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Text = openFileDialog1.FileName;
            }
        }
    }
}
