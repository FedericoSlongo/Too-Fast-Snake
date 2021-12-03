using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    // Le 4 direzioni in cui si può muovere il serpente
    enum direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    /// <summary>
    /// La classe del serpente
    /// </summary>
    class Snake
    {
        public static string body = "*";
        public static string head = "'";
        // La lista dei segmenti che lo compongono
        public List<Segment> segments = new List<Segment> { new Segment(10, 10), new Segment(9, 10), new Segment(8, 10), new Segment(7, 10), new Segment(6, 10) };
        // La direzione attuale
        private direction _currentDirection;
        // La direzione nell'ultimo rendering
        public direction oldDirection = direction.UP;
        // Getter e Setter
        public direction currentDirection
        {
            get => _currentDirection;
            set
            {
                // Controllo che la direzione inserita non sia in contrasto con quella vecchia
                switch (value)
                {
                    case direction.UP: if (oldDirection != direction.DOWN)
                        {
                            _currentDirection = value;
                        }
                        break;
                    case direction.DOWN: if (oldDirection != direction.UP)
                        {
                            _currentDirection = value;
                        }
                        break;
                    case direction.LEFT: if (oldDirection != direction.RIGHT) {
                            _currentDirection = value;
                        }
                        break;
                    case direction.RIGHT: if (oldDirection  != direction.LEFT) {
                            _currentDirection = value;
                        }
                        break;
                    // Non dovrebbe mai accadere ma nel caso...
                    default:
                        throw new Exception("Wtf just happened? -_-");
                }
            }
        }
        // Indica se il serpente è morto
        public bool isAlive = true;
    }
    /// <summary>
    /// Il singolo segmento che compone il serpente
    /// </summary>
    class Segment
    {
        // x e y che indicano le coordinate
        private int _x;
        public int x { get => _x; set => _x = value; }
        private int _y;
        public int y { get => _y; set => _y = value; }
        public Segment(int x, int y)
        {
            SetXY(x, y);
        }
        public Segment(Segment segment)
        {
            SetXY(segment);
        }
        /// <summary>
        /// Settaggio di x e y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetXY(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public void SetXY(Segment segment)
        {
            this.x = segment.x;
            this.y = segment.y;
        }
    }
    // Non serviva reimplementare la x e y quindi il frutto prende da segmento
    class Fruit : Segment
    {        
        public static string fruitChar = "+";
        public Fruit(int x, int y) : base(x, y) { }
        public bool isEaten = false;

    }
}
