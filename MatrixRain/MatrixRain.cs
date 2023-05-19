namespace MatrixRain
{
    class MatrixRain
    {
        // Atributy rainu, podla ktorych sa bude rain vykreslovat
        private readonly bool _direction;        // smer "padania", prednastavene zhora nadol
        private readonly ConsoleColor _color;    // farba znakov, prednastavena zelena
        private readonly bool _randomColors;     // priznak, ci sa budu vykreslovat nahodne farby
        private readonly int _delay;             // spomalenie rychlosti vykreslovania znakov
        private readonly Type _characters;       // typ vykreslovanych znakov, predvolene x = alpha numeric (0...9 a 'A'...'Z')
        private readonly bool _randomChars;      // bool, ci znaky maju byt nahodne pri kazdom "pade" kvapky, alebo maju byt rovnake pocas celeho padania kvapky

        // Trieda Random pre generovanie nahodnych hodnot
        private readonly Random _rnd = new();

        // Konstanty pre rozmery konzoly, v pripade zmeny velkosti pred spustenim samotneho "rainu" - metoda ReloadFieldsConsole
        private int ConsoleHeight = Console.WindowHeight;  
        private int ConsoleWidth = Console.WindowWidth;

        // Pole pre vsetky dostupne druhy farieb konzole
        private readonly ConsoleColor[] AVAILABLE_COLORS = 
            { ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Magenta, ConsoleColor.Cyan, ConsoleColor.Green, 
            ConsoleColor.Yellow, ConsoleColor.DarkBlue, ConsoleColor.DarkCyan, ConsoleColor.DarkGray, ConsoleColor.DarkGreen,
            ConsoleColor.DarkMagenta, ConsoleColor.DarkRed, ConsoleColor.DarkYellow, ConsoleColor.Gray, ConsoleColor.Green };

        /*
         * Konstruktor triedy MatrixRain.
         * Do fieldov sa nastavia zadane parametre.
         * Ak bol zadany argument programu "--random-color", nastavi sa randomColors field na true.
         */
        public MatrixRain(bool direction, ConsoleColor color, int delay, Type characters, bool randomChars) 
        {
            this._direction = direction;
            this._color = color;
            if (color == ConsoleColor.Gray) 
            {
                this._randomColors = true;
            }
            this._delay = delay;
            this._characters = characters;
            this._randomChars = randomChars;
        }

        /*
         * Hlavna operacia programu.
         * Inicializuje potrebne polia a nasledne podla zadanych parameterov spusti nekonecne generovanie a updatovanie kvapiek na konzolu.
         * Ak uzivatel stlaci akukolvek klavesu, program sa ukonci.
         */
        public void Matrix() 
        {
            Console.WriteLine("PRESS ANY KEY TO START MATRIX RAIN");
            Console.WriteLine("<for better experience press F11>");

            Console.ReadKey();                  // Konzola caka na zadanie klavesy ... napr. kvoli zmene na full-screen
            Console.Clear();                    // Vycistenie konzoly 
            Console.CursorVisible = false;      // Schovanie kurzora
            ReloadFieldsConsole();              // Update fieldov kvoli moznemu zmeneniu velkosti okna konzoly

            List<MatrixDrop> drops = new();         // List so vsetkymi instanciami triedy MatrixDrop
            List<MatrixDrop> removeDrops = new();   // List so vsetkymi instanciami triedy MatrixDrop, ktore sa mozu odstranit

            /*
             * Nekonecny cyklus, ktory stale vykresluje rain a generuje nove kvapky.
             * Ak sa okno zvacsi, kvapky sa zacnu generovat podla velkosti okna konzole. // ak sa zmensi, program padne
             * Ak sa stlaci hocijaky klaves, resetnu sa farby, vycisti konzola, zapne sa kurzor a vypne sa aplikacia - podla zadania.
             */
            while (true) 
            {
                Update(ref drops, ref removeDrops);
                ReloadFieldsConsole();
                GenerateDrop(ref drops);

                if (Console.KeyAvailable) 
                {
                    EndApp();
                }
            }
        }

        private static void EndApp()
        {
            Console.ResetColor();           // Reset farby konzole na default
            Console.CursorVisible = true;   // Zviditelnenie kurzora
            Console.Clear();                // Vycistenie konzole
            Environment.Exit(0);            // Ukoncenie aplikacie
        }

        private void ReloadFieldsConsole()
        {
            ConsoleHeight = Console.WindowHeight;
            ConsoleWidth = Console.WindowWidth;
        }

        private void Update(ref List<MatrixDrop> drops, ref List<MatrixDrop> removeDrops)
        {
            foreach (MatrixDrop drop in drops) // Prechadzam vsetky aktualne kvapky
            {
                /*
                 * Vypis prvej bielej kvapky na konzolu
                 */
                if (!drop.First)  // Ak prva kvapka nie je skryta, vykreslim ju bielou farbou
                Console.ForegroundColor = ConsoleColor.White;
                if (_randomChars) PrintChar(drop.X, drop.Y, GetCharacter(_characters).ToString());
                else PrintChar(drop.X, drop.Y, drop.Characters[0].ToString());

                /*
                 * Aktualizacia atributov[poctov] vramci kvapky po kazdej iteracii
                 */
                if (drop.Count >= drop.Length) drop.Count = drop.Length;
                else drop.Count++;

                /*
                 * Vypis ostatnych znakov kvapky
                 */
                Console.ForegroundColor = drop.Color;
                
                if (drop.Count > 0)
                {
                    for (int j = 1; j < drop.Count; j++) // Opakujeme pre kazdy znak kvapky
                    {
                        if (!_direction && drop.Y > 0)   // Ak je padanie smerom nadol
                        {
                            int newYPos = drop.Y - j;   // Zaciname index j od 1, kvoli dekrementovani
                            if (_randomChars) PrintChar(drop.X, newYPos, GetCharacter(_characters).ToString());
                            else PrintChar(drop.X, newYPos, drop.Characters[j].ToString());
                        }
                        else    // Ak je padanie smerom nahor
                        {
                            if (drop.Y < ConsoleHeight - 1)
                            {
                                int newYPos = drop.Y + j;
                                if (_randomChars) PrintChar(drop.X, newYPos, GetCharacter(_characters).ToString());
                                else PrintChar(drop.X, newYPos, drop.Characters[j].ToString());
                            }
                        }
                    }
                }

                Thread.Sleep(_delay);    // Delay medzi vykreslovanim

                if (!_direction) // smer padania zhora nadol
                {
                    if ((drop.Y - drop.Length) >= 0)    
                    {
                        int newYPos = drop.Y - drop.Length;
                        PrintChar(drop.X, newYPos, " ");    // vykreslenie medzery
                    }
                    drop.Y++;

                    if (drop.Y >= ConsoleHeight) // Ak kvapka narazi na koniec konzoly
                    {
                        drop.Y = ConsoleHeight - 1;
                        drop.Length--;
                        drop.First = true;
                        if (drop.Length == -1) removeDrops.Add(drop);
                    }
                }
                else    // smer padania zdola nahor
                {
                    if ((drop.Y + drop.Length) <= ConsoleHeight - 1)
                    {
                        int newYPos = drop.Y + drop.Length;
                        PrintChar(drop.X, newYPos, " ");
                    }
                    drop.Y--;

                    if (drop.Y < 0)
                    {
                        drop.Y = 0;
                        drop.Length--;
                        drop.First = true;
                        if (drop.Length == -1) removeDrops.Add(drop);
                    }
                }
            }

            /*
             * Vymazanie nepotrebnych instancii kvapiek vramci pola
             */
            foreach (MatrixDrop drop in removeDrops)
            {
                drops.Remove(drop);
            }

        }

        /*
         * Vypise string na konzolu na urcene suradnice.
         */
        private static void PrintChar(int x, int y, string what)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(what);
        }

        /*
         * Vygeneruje instanciu triedy kvapky.
         */
        private void GenerateDrop(ref List<MatrixDrop> drops)
        {
            MatrixDrop drop;
            int x = _rnd.Next(ConsoleWidth);        // max sirka je velkost konzole
            int y = 0;                              // podla smeru vygenerujem kvapku bud na horny alebo dolny okraj konzoly
            int length = _rnd.Next(ConsoleHeight);  // max dlzka je velkost konzole
            int minLength = 2;                      // minimalna dlzka je 2
            ConsoleColor value_color;
            value_color = this._color;

            if (_randomColors)                       // ak bol zadany argument na random farby
            {
                value_color = AVAILABLE_COLORS[_rnd.Next(0,AVAILABLE_COLORS.Length)];
            }

            if (_direction)              // dolny okraj obrazovky
            {
                y = ConsoleHeight - 1; 
                drop = new MatrixDrop(x, y, length + minLength, value_color);
                if (!_randomChars) FillDrop(drop, length);
                drops.Add(drop);
            }
            else                        // horny okraj obrazovky
            {
                drop = new MatrixDrop(x, y, length + minLength, value_color);
                if (!_randomChars) FillDrop(drop, length);
                drops.Add(drop);
            } 
        }

        /*
         * Vygeneruje hodnoty znakov pre celu kvapku.
         */
        private void FillDrop(MatrixDrop drop, int length)
        {
            for (int i = 0; i < length; i++)
            {
                drop.Characters[i] = GetCharacter(_characters);
            }
        }

        /*
         * Returne znak(char) podla uzivatelom zadaneho parametra ziskaneho z main-u.
         */
        private char GetCharacter(Type characters)
        {
            char alpha = (char)(65 + _rnd.Next(0, 25));
            char numeric = (char)(48 + _rnd.Next(0, 9));

            if (characters == Type.Alphanumeric)          // Alpha 
            {
                return alpha;
            }
            else if (characters == Type.Numeric)     // Numeric
            {
                return numeric;
            }
            else                            // AlphaNumeric - default
            {
                char[] values = { alpha, numeric };
                int random = _rnd.Next(values.Length);
                return values[random];
            }
        }
    }
}