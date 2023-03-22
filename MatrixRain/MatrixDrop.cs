namespace MatrixRain
{
    class MatrixDrop
    {
        private int _x;              // x-ova pozicia vramci konzoly kvapky
        private int _y;              // y-ova pozicia vramci konzoly kvapky
        private int _length;         // dlzka (pocet riadkov) kvapky
        private int _count;          // pocet viditelnych znakov kvapky
        private bool _first;         // bool, ci je prva kvapka viditelna na konzoli
        private ConsoleColor _color; // farba danej kvapky
        private char[] _characters;  // pole o dlzke kvapky naplnene vygenerovanymi znakmi

        public MatrixDrop(int x, int y, int length, ConsoleColor color)
        {
            X = x;
            Y = y;
            Length = length;
            Count = 0;
            First = false;
            Color = color;
            _characters = new char[Length];
        }

        // Gettery a settery
        public char[] Characters
        { get { return _characters; } }  

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public int Length
        { 
            get { return _length; } 
            set { _length = value; }
        }

        public int Count
        { 
            get { return _count; } 
            set { _count = value;  }
        }

        public bool First
        {
            get { return _first; }
            set { _first = value; }
        }

        public ConsoleColor Color 
        {
            get { return _color; } 
            set {  _color = value; } 
        }
    }
}
