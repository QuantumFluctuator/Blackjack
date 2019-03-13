using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Project
{
    public partial class Display : Form
    {
        public Image[] cards;
        public Thread textThread;

        public Display()
        {
            InitializeComponent();
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            if (this.CurrentCardText.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                if (this.CurrentCardText.Text != text)
                {
                    this.CurrentCardText.Text = text;
                    SetImage(Application.currentCard.suit, Application.currentCard.value);
                }
            }
        }

        private void CreateBitmaps()
        {
            string strAppPath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string strFilePath = Path.Combine(strAppPath, "res");

            cards = new Bitmap[52];
            cards[0] = Bitmap.FromFile(Path.Combine(strFilePath, "ace_of_spades.png"));
            cards[1] = Bitmap.FromFile(Path.Combine(strFilePath, "2_of_spades.png"));
            cards[2] = Bitmap.FromFile(Path.Combine(strFilePath, "3_of_spades.png"));
            cards[3] = Bitmap.FromFile(Path.Combine(strFilePath, "4_of_spades.png"));
            cards[4] = Bitmap.FromFile(Path.Combine(strFilePath, "5_of_spades.png"));
            cards[5] = Bitmap.FromFile(Path.Combine(strFilePath, "6_of_spades.png"));
            cards[6] = Bitmap.FromFile(Path.Combine(strFilePath, "7_of_spades.png"));
            cards[7] = Bitmap.FromFile(Path.Combine(strFilePath, "8_of_spades.png"));
            cards[8] = Bitmap.FromFile(Path.Combine(strFilePath, "9_of_spades.png"));
            cards[9] = Bitmap.FromFile(Path.Combine(strFilePath, "10_of_spades.png"));
            cards[10] = Bitmap.FromFile(Path.Combine(strFilePath, "jack_of_spades2.png"));
            cards[11] = Bitmap.FromFile(Path.Combine(strFilePath, "queen_of_spades2.png"));
            cards[12] = Bitmap.FromFile(Path.Combine(strFilePath, "king_of_spades2.png"));
            cards[13] = Bitmap.FromFile(Path.Combine(strFilePath, "ace_of_hearts.png"));
            cards[14] = Bitmap.FromFile(Path.Combine(strFilePath, "2_of_hearts.png"));
            cards[15] = Bitmap.FromFile(Path.Combine(strFilePath, "3_of_hearts.png"));
            cards[16] = Bitmap.FromFile(Path.Combine(strFilePath, "4_of_hearts.png"));
            cards[17] = Bitmap.FromFile(Path.Combine(strFilePath, "5_of_hearts.png"));
            cards[18] = Bitmap.FromFile(Path.Combine(strFilePath, "6_of_hearts.png"));
            cards[19] = Bitmap.FromFile(Path.Combine(strFilePath, "7_of_hearts.png"));
            cards[20] = Bitmap.FromFile(Path.Combine(strFilePath, "8_of_hearts.png"));
            cards[21] = Bitmap.FromFile(Path.Combine(strFilePath, "9_of_hearts.png"));
            cards[22] = Bitmap.FromFile(Path.Combine(strFilePath, "10_of_hearts.png"));
            cards[23] = Bitmap.FromFile(Path.Combine(strFilePath, "jack_of_hearts2.png"));
            cards[24] = Bitmap.FromFile(Path.Combine(strFilePath, "queen_of_hearts2.png"));
            cards[25] = Bitmap.FromFile(Path.Combine(strFilePath, "king_of_hearts2.png"));
            cards[26] = Bitmap.FromFile(Path.Combine(strFilePath, "ace_of_clubs.png"));
            cards[27] = Bitmap.FromFile(Path.Combine(strFilePath, "2_of_clubs.png"));
            cards[28] = Bitmap.FromFile(Path.Combine(strFilePath, "3_of_clubs.png"));
            cards[29] = Bitmap.FromFile(Path.Combine(strFilePath, "4_of_clubs.png"));
            cards[30] = Bitmap.FromFile(Path.Combine(strFilePath, "5_of_clubs.png"));
            cards[31] = Bitmap.FromFile(Path.Combine(strFilePath, "6_of_clubs.png"));
            cards[32] = Bitmap.FromFile(Path.Combine(strFilePath, "7_of_clubs.png"));
            cards[33] = Bitmap.FromFile(Path.Combine(strFilePath, "8_of_clubs.png"));
            cards[34] = Bitmap.FromFile(Path.Combine(strFilePath, "9_of_clubs.png"));
            cards[35] = Bitmap.FromFile(Path.Combine(strFilePath, "10_of_clubs.png"));
            cards[36] = Bitmap.FromFile(Path.Combine(strFilePath, "jack_of_clubs2.png"));
            cards[37] = Bitmap.FromFile(Path.Combine(strFilePath, "queen_of_clubs2.png"));
            cards[38] = Bitmap.FromFile(Path.Combine(strFilePath, "king_of_clubs2.png"));
            cards[39] = Bitmap.FromFile(Path.Combine(strFilePath, "ace_of_diamonds.png"));
            cards[40] = Bitmap.FromFile(Path.Combine(strFilePath, "2_of_diamonds.png"));
            cards[41] = Bitmap.FromFile(Path.Combine(strFilePath, "3_of_diamonds.png"));
            cards[42] = Bitmap.FromFile(Path.Combine(strFilePath, "4_of_diamonds.png"));
            cards[43] = Bitmap.FromFile(Path.Combine(strFilePath, "5_of_diamonds.png"));
            cards[44] = Bitmap.FromFile(Path.Combine(strFilePath, "6_of_diamonds.png"));
            cards[45] = Bitmap.FromFile(Path.Combine(strFilePath, "7_of_diamonds.png"));
            cards[46] = Bitmap.FromFile(Path.Combine(strFilePath, "8_of_diamonds.png"));
            cards[47] = Bitmap.FromFile(Path.Combine(strFilePath, "9_of_diamonds.png"));
            cards[48] = Bitmap.FromFile(Path.Combine(strFilePath, "10_of_diamonds.png"));
            cards[49] = Bitmap.FromFile(Path.Combine(strFilePath, "jack_of_diamonds2.png"));
            cards[50] = Bitmap.FromFile(Path.Combine(strFilePath, "queen_of_diamonds2.png"));
            cards[51] = Bitmap.FromFile(Path.Combine(strFilePath, "king_of_diamonds2.png"));

        }

        private void Display_Load(object sender, EventArgs e)
        {
            CreateBitmaps();
            textThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        SetText(Application.Card.getCardName(Application.currentCard.value, Application.currentCard.suit));
                        
                    }
                    catch(Exception ex) {
                        Console.WriteLine(ex.Message);
                    }
                    Thread.Sleep(50/3);
                }
            });
            textThread.IsBackground = true;
            textThread.Start();
        }

        private void SetImage(int suit, int value)
        {
            switch (suit)
            {
                case 0: //spades
                    switch (value)
                    {
                        case 0:
                            currentCardImage.Image = cards[0];
                            break;
                        case 1:
                            currentCardImage.Image = cards[1];
                            break;
                        case 2:
                            currentCardImage.Image = cards[2];
                            break;
                        case 3:
                            currentCardImage.Image = cards[3];
                            break;
                        case 4:
                            currentCardImage.Image = cards[4];
                            break;
                        case 5:
                            currentCardImage.Image = cards[5];
                            break;
                        case 6:
                            currentCardImage.Image = cards[6];
                            break;
                        case 7:
                            currentCardImage.Image = cards[7];
                            break;
                        case 8:
                            currentCardImage.Image = cards[8];
                            break;
                        case 9:
                            currentCardImage.Image = cards[9];
                            break;
                        case 10:
                            currentCardImage.Image = cards[10];
                            break;
                        case 11:
                            currentCardImage.Image = cards[11];
                            break;
                        case 12:
                            currentCardImage.Image = cards[12];
                            break;
                    }
                    break;
                case 1: //hearts
                    switch (value)
                    {
                        case 0:
                            currentCardImage.Image = cards[13];
                            break;
                        case 1:
                            currentCardImage.Image = cards[14];
                            break;
                        case 2:
                            currentCardImage.Image = cards[15];
                            break;
                        case 3:
                            currentCardImage.Image = cards[16];
                            break;
                        case 4:
                            currentCardImage.Image = cards[17];
                            break;
                        case 5:
                            currentCardImage.Image = cards[18];
                            break;
                        case 6:
                            currentCardImage.Image = cards[19];
                            break;
                        case 7:
                            currentCardImage.Image = cards[20];
                            break;
                        case 8:
                            currentCardImage.Image = cards[21];
                            break;
                        case 9:
                            currentCardImage.Image = cards[22];
                            break;
                        case 10:
                            currentCardImage.Image = cards[23];
                            break;
                        case 11:
                            currentCardImage.Image = cards[24];
                            break;
                        case 12:
                            currentCardImage.Image = cards[25];
                            break;
                    }
                    break;
                case 2: //clubs
                    switch (value)
                    {
                        case 0:
                            currentCardImage.Image = cards[26];
                            break;
                        case 1:
                            currentCardImage.Image = cards[27];
                            break;
                        case 2:
                            currentCardImage.Image = cards[28];
                            break;
                        case 3:
                            currentCardImage.Image = cards[29];
                            break;
                        case 4:
                            currentCardImage.Image = cards[30];
                            break;
                        case 5:
                            currentCardImage.Image = cards[31];
                            break;
                        case 6:
                            currentCardImage.Image = cards[32];
                            break;
                        case 7:
                            currentCardImage.Image = cards[33];
                            break;
                        case 8:
                            currentCardImage.Image = cards[34];
                            break;
                        case 9:
                            currentCardImage.Image = cards[35];
                            break;
                        case 10:
                            currentCardImage.Image = cards[36];
                            break;
                        case 11:
                            currentCardImage.Image = cards[37];
                            break;
                        case 12:
                            currentCardImage.Image = cards[38];
                            break;
                    }
                    break;
                case 3: //diamonds
                    switch (value)
                    {
                        case 0:
                            currentCardImage.Image = cards[39];
                            break;
                        case 1:
                            currentCardImage.Image = cards[40];
                            break;
                        case 2:
                            currentCardImage.Image = cards[41];
                            break;
                        case 3:
                            currentCardImage.Image = cards[42];
                            break;
                        case 4:
                            currentCardImage.Image = cards[43];
                            break;
                        case 5:
                            currentCardImage.Image = cards[44];
                            break;
                        case 6:
                            currentCardImage.Image = cards[45];
                            break;
                        case 7:
                            currentCardImage.Image = cards[46];
                            break;
                        case 8:
                            currentCardImage.Image = cards[47];
                            break;
                        case 9:
                            currentCardImage.Image = cards[48];
                            break;
                        case 10:
                            currentCardImage.Image = cards[49];
                            break;
                        case 11:
                            currentCardImage.Image = cards[50];
                            break;
                        case 12:
                            currentCardImage.Image = cards[51];
                            break;
                    }
                    break;
            }
        }

        private void currentCardImage_Click(object sender, EventArgs e) { }
    }
}
