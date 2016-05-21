using System;
using ExtendedConsole.Structs;

namespace OoeyGui.Base {
    public abstract class UiElement {
        #region Fields

        private bool _dirty=true;

        protected bool Dirty {
            get { return _dirty; }
            set {
                _dirty = value;
                if (_dirty) OoeyGui.RequestRepaint();
            }
        }

        private ConsoleColor _backgroundColor = ConsoleColor.Black;
        private ConsoleColor _foregroundColor = ConsoleColor.Gray;
        private short _height;
        private short _width;
        private short _x;
        private short _y;
        private int _z;

        public CharInfo[] Buffer;
        private CharInfo _defaultChar;

        public ContainerElement ParentContainer { get; set; }

        #endregion Fields

        #region Constructors

        protected UiElement(short x, short y, short width, short height, int z) {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            _z = z;

            Buffer = new CharInfo[Width*Height];
            _defaultChar = new CharInfo(foregroundColor: Console.ForegroundColor,
                backgroundColor: Console.BackgroundColor);
            Refill();
        }

        #endregion Constructors

        #region Properties

        public ConsoleColor BackgroundColor {
            get { return _backgroundColor; }
            set {
                _backgroundColor = value;
                _defaultChar.BackgroundColor = value;
                Dirty = true;
            }
        }

        public ConsoleColor ForegroundColor {
            get { return _foregroundColor; }
            set {
                _foregroundColor = value;
                _defaultChar.ForegroundColor = value;
                Dirty = true;
            }
        }

        public short Height {
            get { return _height; }
            set {
                ParentContainer?.Invalidate(BoundingRect);
                _height = value;
                Buffer = new CharInfo[Width * Height];
                Dirty = true;
            }
        }

        public short Width {
            get { return _width; }
            set {
                ParentContainer?.Invalidate(BoundingRect);
                _width = value;
                Buffer = new CharInfo[Width * Height];
                Dirty = true;
            }
        }

        public short X {
            get { return _x; }
            set {
                ParentContainer?.Invalidate(BoundingRect);
                _x = value;
                Dirty = true;
            }
        }

        public short Y {
            get { return _y; }
            set {
                ParentContainer?.Invalidate(BoundingRect);
                _y = value;
                Dirty = true;
            }
        }

        public int Z {
            get { return _z; }
            //set {
            //    _z = value;
            //    Dirty = true;
            //}
        }

        public SmallRect BoundingRect => new SmallRect(X, Y, (short) (X+Width), (short) (Y+Height));

        public CharInfo DefaultChar => _defaultChar;

        public Coord Location => new Coord(X, Y);

        #endregion Properties

        #region Methods

        public virtual void Repaint(bool force) {
            if (_dirty || force)
                Refill();
            _dirty = false;
        }

        #endregion Methods

        protected void Refill() {
            for (var i = 0; i < Buffer.Length; i++)
                Buffer[i] = DefaultChar;
        }
    }
}