using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Project
{
    public class Application
    {
        public static bool running;

        public static int[] PLAYERS = new int[4] {0, 1, 2, 3};?

		public const int INITCARDS = 7; //number of cards to be given to each player

        public static TimeKeeping time = new TimeKeeping();
        public static Display disp = new Display();

        public static int HIGHSCORE;
        public static int TIME;
        public static int WINNER = -1;

        public static int COUNTER = 0; //current deck position indicator

        public static Card[] c = new Card[52];
        public static Card currentCard; //currently active card

        public static int toDraw = 0; //number of cards to be drawn on a given turn

        public static bool playerHasCards = true;

        public static string[] suitVisual = new string[4] { "Spades", "Hearts", "Clubs", "Diamonds" };
        public static string[] nameVisual = new string[13] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

        public static int[] DECK = new int[52];

        public static void Main()
        {
            string OS = Environment.OSVersion.Version.ToString();
            if ((OS[0] == '6' && Convert.ToByte(OS[2].ToString()) < 2) || (Convert.ToByte(OS[0].ToString()) < 6)) //check version of windows, special characters do not work in some versions
            {
                suitVisual = new string[4] { "♠", "♥", "♣", "♦" };
            }

            Console.ForegroundColor = ConsoleColor.White;

            Console.Title = "Blackjack"; //set window title

            for (int i = 0; i < 52; i++)
            {
                DECK[i] = i; //initialise deck to default, ready to shuffle
            }

            int x = 0;

            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    c[x] = new Card(i, j); //create card for each deck position
                    x = x + 1;
                }
            }

            
            htmlViewer h;
            Thread htmlThread;

            while (true)
            {
                running = true; //true whilst game is running

                int y = Menu(); //call Menu() and store value
                switch (y)
                {
                    case 1:
                        Play();
                        break;

                    case 2:
                        htmlThread = new Thread(() => //execute in separate thread so program can be run alongside
                        {
                            h = new htmlViewer("help.html");
                        });
                        htmlThread.SetApartmentState(ApartmentState.STA);
                        htmlThread.Start();
                        break;

                    case 3:
                        htmlThread = new Thread(() => //execute in separate thread so program can be run alongside
                        {
                            h = new htmlViewer("credits.html");
                        });
                        htmlThread.SetApartmentState(ApartmentState.STA);
                        htmlThread.Start();
                        break;

                    case 4:
                        string hs;
                        HIGHSCORE = ReadHighScore();
                        if (HIGHSCORE != 0)
                        {
                            hs = Convert.ToString(HIGHSCORE);
                            Console.WriteLine("High score: " + hs);
                        }
                        else
                        {
                            Console.WriteLine("No present high score!");
                        }
                        break;
                }
                if (y == 5)
                {
                    break;
                }
            }
        }

        public static int Menu()
        {
            int x = 0;
            Console.WriteLine("Make your selection:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" 1 to play\n 2 to view help\n 3 to view credits\n 4 to view high score\n 5 to exit");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Your choice: ");
            while (x < 1 || x > 5)
            {
                try
                {
                    x = Convert.ToInt16(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.Write("Invalid input; Enter again: ");
                }
            }
            return x;
        }

        public static void Shuffle(int times)
        {
            for (int i = 0; i < times; i++)
            {
                Random rnd = new Random();
                DECK = DECK.OrderBy(x => rnd.Next()).ToArray();
            }
        }

        public static void Play()
        {
            toDraw = 0;
            int players = 0;
            int dif = 0;
            while ((players != 2 && players != 3 && players != 4) || (dif < 1 || dif > 3)) 
            {
                Console.WriteLine("Enter the number of players, between 2 and 4:");
                try
                {
                    players = Convert.ToInt16(Console.ReadLine());
                    if (players != 2 && players != 3 && players != 4)
                    {
                        continue;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    FileStream fse = new FileStream("Error Log.bin", FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fse);
                    bw.Write(e.Message);
                    bw.Write(e.StackTrace);
                    fse.Close();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    Console.WriteLine();
                    continue;
                }
                Console.WriteLine("Enter the difficulty, between 1 and 3:");
                try
                {
                    dif = Convert.ToInt16(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    FileStream fse = new FileStream("Error Log.bin", FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fse);
                    bw.Write(e.Message);
                    bw.Write(e.StackTrace);
                    fse.Close();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    Console.WriteLine();
                }
            }
            Initialise(players, dif);
        }

        public static int Draw()
        {
            COUNTER++;
            
            if (COUNTER > 51)
            {
                COUNTER = 0;
            }

            return DECK[COUNTER];
        }

        public static void Initialise(int players, int difficulty)
        {
            int[] hand = new int[52];
            int currentPlayer = new Int16();
            int nextPlayer = new Int16();

            
            Shuffle(2);
            
            Player p1 = new Player(PLAYERS[0], true);
            Player p2 = new Player(PLAYERS[1], false);
            Player p3 = new Player(PLAYERS[2], false);
            Player p4 = new Player(PLAYERS[3], false);

            currentPlayer = PLAYERS[0];
            //player 1 initialise
            hand = p1.getHand();
            for (int i = 0; i < 52; i++)
            {
                if (i < INITCARDS)
                {
                    hand[i] = Draw();
                }
                else
                {
                    hand[i] = -1;
                }
            }
            p1.setHand(hand);

            //player 2 initialise
            hand = p2.getHand();
            for (int i = 0; i < 52; i++)
            {
                if (i < INITCARDS)
                {
                    hand[i] = Draw();
                }
                else
                {
                    hand[i] = -1;
                }
            }
            p2.setHand(hand);

            if (players > 2)
            {
                //player 3 initialise
                hand = p3.getHand();
                for (int i = 0; i < 52; i++)
                {
                    if (i < INITCARDS)
                    {
                        hand[i] = Draw();
                    }
                    else
                    {
                        hand[i] = -1;
                    }
                    p3.setHand(hand);
                }
            }

            if (players > 3)
            {
                //player 4 initialise
                hand = p4.getHand();

                for (int i = 0; i < 52; i++)
                {
                    if (i < INITCARDS)
                    {
                        hand[i] = Draw();
                    }
                    else
                    {
                        hand[i] = -1;
                    }
                }
                p4.setHand(hand);
            }

            currentCard = c[Draw()];

            //sub threads
            Thread displayThread = new Thread(() => //execute in separate thread so program can be run alongside
            {
                try
                {
                    disp.ShowDialog();
                }
                catch (InvalidOperationException) { }
            })
            {
                IsBackground = true
            };

            displayThread.Start();

            Thread TimeThread = new Thread(() => //execute in separate thread so program can be run alongside
            {
                time.Timeloop();
            })
            {
                IsBackground = true
            };

            TimeThread.Start();

            //game loop
            while (running)
            {
                nextPlayer = getNextPlayer(currentPlayer, players);
                if (currentPlayer == 0)
                {
                    hand = p1.getHand(); //retreive player hand
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("To draw: " + Convert.ToString(toDraw)); //display number of cards to be drawn
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" Current card is: ");
                    if (currentCard.suit == 0 || currentCard.suit == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.WriteLine(Card.getCardName(currentCard.value, currentCard.suit));
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Your cards are:");
                    int x = 0;
                    playerHasCards = true;
                    //print player's cards
                    foreach (int i in hand)
                    {
                        if (i != -1)
                        {
                            if (c[i].suit == 0 || c[i].suit == 2)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            Console.WriteLine("  " + x + ". " + Card.getCardName(c[i].value, c[i].suit));
                        }
                        x++;
                    }

                    Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine("Enter your choice of card, -1 to draw");
                    int choice = -2;
                    try
                    {
                        choice = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    while (true)
                    {
                        try
                        {
                            if (choice != -1)
                            {
                                if (toDraw == 0)
                                {
                                    if (currentCard.CanPlay(c[hand[choice]]))
                                    {
                                        currentCard = c[hand[choice]];
                                        if (currentCard.value == 0)
                                        {
                                            Console.WriteLine("Choose a suit to change to:");
                                            Console.ForegroundColor = ConsoleColor.DarkGray;
                                            Console.WriteLine("0 - Spades");
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("1 - Hearts");
                                            Console.ForegroundColor = ConsoleColor.DarkGray;
                                            Console.WriteLine("2 - Clubs");
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("3 - Diamonds");
                                            Console.ForegroundColor = ConsoleColor.White;

                                            while (true)
                                            {
                                                try
                                                {
                                                    byte input = 0;
                                                    input = Convert.ToByte(Console.ReadLine());
                                                    while (input > 3 || input < 0)
                                                    {
                                                        input = Convert.ToByte(Console.ReadLine());
                                                        if (input < 4 && input >= 0)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Invalid input; please enter again:");
                                                        }
                                                    }
                                                    currentCard.suit = input;
                                                    break;
                                                }
                                                catch (Exception)
                                                {
                                                    Console.WriteLine("Invalid input, please select again:");
                                                }
                                            }
                                        }
                                        hand[choice] = -1;
                                        p1.setHand(hand);
                                        if (currentCard.value == 1 || (currentCard.value == 10 && (currentCard.suit == 0 || currentCard.suit == 2)))
                                        {
                                            if (currentCard.value == 1)
                                            {
                                                toDraw += 2;
                                            }
                                            else if (currentCard.value == 10 && (currentCard.suit == 0 || currentCard.suit == 2))
                                            {
                                                toDraw += 7;
                                            }
                                        }
                                        
                                        Console.Write(" Current card is now: ");
                                        if (currentCard.suit == 0 || currentCard.suit == 2)
                                        {
                                            Console.ForegroundColor = ConsoleColor.DarkGray;
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                        }
                                        Console.WriteLine(Card.getCardName(currentCard.value, currentCard.suit));
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid choice; choose again");
                                    }
                                }
                                else if (currentCard.value == 1 && toDraw > 0)
                                {
                                    if (c[hand[choice]].value == 1)
                                    {
                                        currentCard = c[hand[choice]];
                                        hand[choice] = -1;
                                        p1.setHand(hand);
                                        toDraw += 2;
                                        Console.Write(" Current card is now: ");
                                        if (currentCard.suit == 0 || currentCard.suit == 2)
                                        {
                                            Console.ForegroundColor = ConsoleColor.DarkGray;
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                        }
                                        Console.WriteLine(Card.getCardName(currentCard.value, currentCard.suit));
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid choice; choose again");
                                    }
                                }
                                else
                                {
                                    if (c[hand[choice]].value == 10 && (currentCard.suit == 0 || currentCard.suit == 2) && toDraw > 0)
                                    {
                                        currentCard = c[hand[choice]];
                                        hand[choice] = -1;
                                        p1.setHand(hand);
                                        if (currentCard.suit == 0 || currentCard.suit == 2)
                                        {
                                            toDraw += 7;
                                        }
                                        else
                                        {
                                            toDraw = 0;
                                        }
                                        Console.Write(" Current card is now: ");
                                        if (currentCard.suit == 0 || currentCard.suit == 2)
                                        {
                                            Console.ForegroundColor = ConsoleColor.DarkGray;
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                        }
                                        Console.WriteLine(Card.getCardName(currentCard.value, currentCard.suit));
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                    }
                                    else if (currentCard.CanPlay(c[hand[choice]]) && toDraw == 0)
                                    {
                                        currentCard = c[hand[choice]];
                                        hand[choice] = -1;
                                        p1.setHand(hand);
                                        if ((currentCard.suit == 0 || currentCard.suit == 2) && currentCard.value == 10)
                                        {
                                            toDraw += 7;
                                        }
                                        else if (currentCard.value == 1)
                                        {
                                            toDraw += 2;
                                        }
                                        else
                                        {
                                            toDraw = 0;
                                        }
                                        Console.Write(" Current card is now: ");
                                        if (currentCard.suit == 0 || currentCard.suit == 2)
                                        {
                                            Console.ForegroundColor = ConsoleColor.DarkGray;
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                        }
                                        Console.WriteLine(Card.getCardName(currentCard.value, currentCard.suit));
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid choice; choose again");
                                    }
                                }
                            }
                            else
                            {
                                if (toDraw == 0)
                                {
                                    for (int i = 0; i < 51; i++)
                                    {
                                        if (hand[i] == -1)
                                        {
                                            hand[i] = Draw();
                                            break;
                                        }
                                    }
                                    Console.WriteLine("You draw a card");
                                    break;
                                }
                                else
                                {
                                    if (currentCard.value == 1)
                                    {
                                        for (int j = 0; j < toDraw; j++)
                                        {
                                            for (int i = 0; i < 51; i++)
                                            {
                                                if (hand[i] == -1)
                                                {
                                                    hand[i] = Draw();
                                                    break;
                                                }
                                            }
                                        }
                                        Console.WriteLine("You draw " + Convert.ToString(toDraw));
                                        toDraw = 0;
                                        
                                        break;
                                    }
                                    else
                                    {
                                        if (currentCard.value == 10 && (currentCard.suit == 1 || currentCard.suit == 3))
                                        {
                                            toDraw = 1;
                                        }
                                        for (int j = 0; j < toDraw; j++)
                                        {
                                            for (int i = 0; i < 51; i++)
                                            {
                                                if (hand[i] == -1)
                                                {
                                                    hand[i] = Draw();
                                                    break;
                                                }
                                            }
                                        }
                                        
                                        Console.WriteLine("You draw " + Convert.ToString(toDraw));
                                        toDraw = 0;
                                        break;
                                    }
                                }
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("Invalid index, please enter a new number:");
                        }

                        for (bool i = false; !i; )
                        {
                            try
                            {
                                choice = Convert.ToInt32(Console.ReadLine());
                                i = true;
                            }
                            catch (Exception)
                            {
                                i = false;
                                Console.WriteLine("Invalid choice; choose again");
                            }
                        }

                        
                    }
                    playerHasCards = false;
                    for (int i = 0; i < 51; i++)
                    {
                        if (hand[i] != -1)
                        {
                            playerHasCards = true;
                        }
                    }

                    if (!playerHasCards)
                    {
                        running = false;
                        WINNER = 1;
                        break;
                    }
                }
                else
                {
                    if (WINNER != -1)
                    {
                        running = false;
                        break;
                    }
                    switch (currentPlayer)
                    {
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Player 2 to Play");
                            Console.ForegroundColor = ConsoleColor.White;
                            hand = p2.getHand();
                            hand = AITurn(hand, 2, difficulty);
                            if (WINNER != -1)
                            {
                                running = false;
                                break;
                            }
                            Console.Write(" Current card is now: ");
                            if (currentCard.suit == 0 || currentCard.suit == 2)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            Console.WriteLine(Card.getCardName(currentCard.value, currentCard.suit));
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Player 3 to Play");
                            Console.ForegroundColor = ConsoleColor.White;
                            hand = p3.getHand();
                            hand = AITurn(hand, 3, difficulty);
                            if (WINNER != -1)
                            {
                                running = false;
                                break;
                            }
                            Console.Write(" Current card is now: ");
                            if (currentCard.suit == 0 || currentCard.suit == 2)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            Console.WriteLine(Card.getCardName(currentCard.value, currentCard.suit));
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case 3:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Player 4 to Play");
                            Console.ForegroundColor = ConsoleColor.White;
                            hand = p4.getHand();
                            hand = AITurn(hand, 4, difficulty);
                            if (WINNER != -1)
                            {
                                running = false;
                                break;
                            }
                            Console.Write(" Current card is now: ");
                            if (currentCard.suit == 0 || currentCard.suit == 2)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            Console.WriteLine(Card.getCardName(currentCard.value, currentCard.suit));
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }
                }
                currentPlayer = nextPlayer;
            }

            //end of game
            if (!running) 
            {
                TIME = time.getTime();
                if (WINNER == 1)
                {
                    int score = (difficulty * 10000) / TIME;

                    if (HIGHSCORE < score) 
                    {
                        Console.WriteLine("You got a new high score!");
                        HIGHSCORE = score;
                        WriteHighScore(HIGHSCORE);
                    }
                    Console.WriteLine("You won in " + Convert.ToString(TIME) + " seconds!");
                    Console.WriteLine("Your score was " + Convert.ToString(score));
                    Console.WriteLine("Press a key to continue");
                    WINNER = -1;
                    Console.ReadKey();
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("You lost in " + Convert.ToString(TIME) + " seconds!");
                    Console.WriteLine("The winner was Player " + Convert.ToString(WINNER));
                    Console.WriteLine("Press a key to continue");
                    WINNER = -1;
                    Console.ReadKey();
                    Console.WriteLine();
                }
            }
        }

        public static int[] AITurn(int[] hand, int currentAI, int dif)
        {
            int[] value = new int[52];
            int[] suit = new int[52];
            bool hasPlayed = false;

            if (dif == 2)
            {
                Random rng = new Random();
                int i = Convert.ToInt16(rng.NextDouble());
                i %= 2;
                if (i == 0)
                {
                    dif = 1;
                }
                else if (i == 1)
                {
                    dif = 3;
                }
            }

            switch (dif) {
                case 1:
                if (toDraw == 0)
                {
                    for (int i = 0; i < hand.Length; i++)
                    {
                        if (hand[i] != -1)
                        {
                            if (currentCard.CanPlay(c[hand[i]]))
                            {
                                currentCard = c[hand[i]];
                                hand[i] = -1;
                                hasPlayed = true;
                                if (currentCard.value == 1 || (currentCard.value == 10 && (currentCard.suit == 0 || currentCard.suit == 2)))
                                {
                                    if (currentCard.value == 1)
                                    {
                                        toDraw += 2;
                                    }
                                    else
                                    {
                                        toDraw += 7;
                                    }

                                }
                                else if (currentCard.value == 10 && (currentCard.suit == 1 || currentCard.suit == 3))
                                {
                                    toDraw = 0;
                                }
                                break;
                            }
                        }
                    }
                }
                else if (currentCard.value == 1 && toDraw > 0)
                {
                    for (int i = 0; i < hand.Length; i++)
                    {
                        if (hand[i] != -1)
                        {
                            if (c[hand[i]].value == 1)
                            {
                                currentCard = c[hand[i]];
                                hand[i] = -1;
                                hasPlayed = true;
                                toDraw += 2;
                                break;
                            }
                        }
                    }
                    if (!hasPlayed)
                    {
                        Console.WriteLine("AI Draws " + Convert.ToString(toDraw));
                        for (int k = 0; k < toDraw; k++)
                        {
                            for (int j = 0; j < hand.Length; j++)
                            {
                                if (hand[j] == -1)
                                {
                                    hand[j] = Draw();
                                    break;
                                }
                            }
                        }
                        hasPlayed = true;
                        toDraw = 0;
                    }
                }
                else if ((currentCard.value == 10 && (currentCard.suit == 0 || currentCard.suit == 2)) && toDraw > 0)
                {
                    for (int i = 0; i < hand.Length; i++)
                    {
                        if (hand[i] != -1)
                        {
                            if (c[hand[i]].value == 10)
                            {
                                currentCard = c[hand[i]];
                                hand[i] = -1;
                                hasPlayed = true;
                                if (currentCard.value == 1 || (currentCard.value == 10 && (currentCard.suit == 0 || currentCard.suit == 2)))
                                {
                                    if (currentCard.value == 1)
                                    {
                                        toDraw += 2;
                                    }
                                    else
                                    {
                                        toDraw += 7;
                                    }
                                }
                                else if (currentCard.value == 10 && (currentCard.suit == 1 || currentCard.suit == 3))
                                {
                                    toDraw = 0;
                                }
                                break;
                            }
                        }
                    }
                    if (!hasPlayed)
                    {
                        Console.WriteLine("AI Draws " + Convert.ToString(toDraw));
                        for (int k = 0; k < toDraw; k++)
                        {
                            for (int j = 0; j < hand.Length; j++)
                            {
                                if (hand[j] == -1)
                                {
                                    hand[j] = Draw();
                                    break;
                                }
                            }
                        }
                        hasPlayed = true;
                        toDraw = 0;
                    }
                }
                break;

                case 3:
                for (int j = 0; j < hand.Length; j++)
                {
                    if (hand[j] != -1)
                    {
                        if ((c[hand[j]].value == 1 || (c[hand[j]].value == 10 && (c[hand[j]].suit == 0 || c[hand[j]].suit == 2))) && toDraw == 0 && currentCard.CanPlay(c[hand[j]]))
                        {
                            currentCard = c[hand[j]];
                            hand[j] = -1;
                            hasPlayed = true;
                            if (currentCard.value == 1)
                            {
                                toDraw += 2;
                            }
                            else
                            {
                                toDraw += 7;
                            }
                            
                        }
                        else if (currentCard.value == 10 && (currentCard.suit == 1 || currentCard.suit == 3))
                        {
                            toDraw = 0;
                        }
                        break;
                    }
                }
                if (!hasPlayed)
                {
                    if (toDraw == 0)
                    {
                        for (int i = 0; i < hand.Length; i++)
                        {
                            if (hand[i] != -1)
                            {
                                if (currentCard.CanPlay(c[hand[i]]))
                                {
                                    currentCard = c[hand[i]];
                                    hand[i] = -1;
                                    hasPlayed = true;
                                    if (currentCard.value == 1 || (currentCard.value == 10 && (currentCard.suit == 0 || currentCard.suit == 2)))
                                    {
                                        if (currentCard.value == 1)
                                        {
                                            toDraw += 2;
                                        }
                                        else
                                        {
                                            toDraw += 7;
                                        }
                                    }
                                    else if (currentCard.value == 10 && (currentCard.suit == 1 || currentCard.suit == 3))
                                    {
                                        toDraw = 0;
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    else if (currentCard.value == 1 && toDraw > 0)
                    {
                        for (int i = 0; i < hand.Length; i++)
                        {
                            if (hand[i] != -1)
                            {
                                if (c[hand[i]].value == 1 && currentCard.CanPlay(c[hand[i]]))
                                {
                                    currentCard = c[hand[i]];
                                    hand[i] = -1;
                                    hasPlayed = true;
                                    toDraw += 2;
                                    break;
                                }
                            }
                        }
                        if (!hasPlayed)
                        {
                            Console.WriteLine("AI Draws " + Convert.ToString(toDraw));
                            for (int k = 0; k < toDraw; k++)
                            {
                                for (int j = 0; j < hand.Length; j++)
                                {
                                    if (hand[j] == -1)
                                    {
                                        hand[j] = Draw();
                                        break;
                                    }
                                }
                            }
                            hasPlayed = true;
                            toDraw = 0;
                        }
                    }
                    else if ((currentCard.value == 10 && (currentCard.suit == 0 || currentCard.suit == 2)) && toDraw > 0)
                    {
                        for (int i = 0; i < hand.Length; i++)
                        {
                            if (hand[i] != -1)
                            {
                                if (c[hand[i]].value == 10)
                                {
                                    currentCard = c[hand[i]];
                                    hand[i] = -1;
                                    hasPlayed = true;
                                    if (currentCard.value == 1 || (currentCard.value == 10 && (currentCard.suit == 0 || currentCard.suit == 2)))
                                    {
                                        if (currentCard.value == 1)
                                        {
                                            toDraw += 2;
                                        }
                                        else
                                        {
                                            toDraw += 7;
                                        }
                                    }
                                    else if (currentCard.value == 10 && (currentCard.suit == 1 || currentCard.suit == 3))
                                    {
                                        toDraw = 0;
                                    }
                                    break;
                                }
                            }
                        }
                        if (!hasPlayed)
                        {
                            Console.WriteLine("AI Draws " + Convert.ToString(toDraw));
                            for (int k = 0; k < toDraw; k++)
                            {
                                for (int j = 0; j < hand.Length; j++)
                                {
                                    if (hand[j] == -1)
                                    {
                                        hand[j] = Draw();
                                        break;
                                    }
                                }
                            }
                            hasPlayed = true;
                            toDraw = 0;
                        }
                    }
                }
                break;
            }


            if (!hasPlayed)
            {
                for (int i = 0; i < hand.Length; i++)
                {
                    if (hand[i] == -1)
                    {
                        hand[i] = Draw();
                        break;
                    }
                }
                Console.WriteLine("AI Draws");
            }

            
            for (int i = 0; i < 52; i++)
            {
                if (hand[i] != -1)
                {
                    value[i] = c[hand[i]].value;
                    suit[i] = c[hand[i]].suit;
                }
            }
            bool hasWon = true;
            for (int i = 0; i < 52; i++)
            {
                if (hand[i] != -1)
                {
                    hasWon = false;
                }
            }

            if (hasWon)
            {
                WINNER = currentAI;
                hasWon = false;
                return null;
            }

            return hand;
        }

        public static int getNextPlayer(int currentPlayer, int numberOfPlayers)
        {
            if (currentPlayer < numberOfPlayers - 1)
            {
                return ++currentPlayer;
            }
            else
            {
                return 0;
            }
        }

        public static void WriteHighScore(int score)
        {
            FileStream fsw = new FileStream("High Score.bin", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fsw);
            bw.Write(HIGHSCORE);
            fsw.Close();
        }

        public static int ReadHighScore()
        {
            if (File.Exists("High Score.bin"))
            {
                FileStream fsr = new FileStream("High Score.bin", FileMode.Open);
                BinaryReader br = new BinaryReader(fsr);
                int x = br.ReadInt32();
                fsr.Close();
                return x;
            }
            else
            {
                return 0;
            }
        }

		public class Player
		{
			int[] hand;
			int currentPlayer;
			public Player(int player, bool isCPU) 
			{
				this.currentPlayer = player;
				int[] hand = new int[52];
				this.hand = hand;
			}

			public int[] getHand() 
			{
				return this.hand;
			}

			public void setHand(int[] newhand)
			{
				this.hand = newhand;
			}
		}

        public class Card
        {
            public int value;
            public int suit;
            public Card(int val, int sui)
            {
                value = val;
                suit = sui;
            }

            public bool CanPlay(Card ca)
            {
                if (ca.suit == this.suit || ca.value == this.value || ca.value == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public bool IsPowerCard()
            {
                if (this.value == 0 || this.value == 7 || this.value == 10 || this.value == 12)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public static string getCardName(int val, int suit)
            {
                return (nameVisual[val] + " of " + suitVisual[suit]);
            }
        }
    }
}