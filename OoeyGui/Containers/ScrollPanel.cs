using System;
using System.Linq;
using ExtendedConsole.Structs;
using OoeyGui.Base;

namespace OoeyGui.Containers {
    public class ScrollPanel : ContainerElement {
        public double ScrollPosition {
            get { return _scrollPosition; }
            set {
                _scrollPosition = value;
                var child = Children.First();
                var extra = child.Height - Height;
                if (extra < 1) return;
                child.Y = (short)-(extra * _scrollPosition);
            }
        }

        private readonly CharInfo _upArrow = new CharInfo('\x18', ConsoleColor.Black, ConsoleColor.Gray) {
            Underscore = true
        };

        private readonly CharInfo _downArrow = new CharInfo('\x19', ConsoleColor.Black, ConsoleColor.Gray) {
            Overscore = true
        };

        private readonly CharInfo _barChar = new CharInfo(' ', ConsoleColor.Black, ConsoleColor.White);
        private readonly CharInfo _bgChar = new CharInfo('\xB1', ConsoleColor.Black, ConsoleColor.DarkGray);
        private double _scrollPosition;

        public ScrollPanel(short x, short y, short width, short height, int z) : base(x, y, width, height, z) {}

        public override void AddChild(UiElement child) {
            if (Children.Any())
                throw new ArgumentException("Child already set", nameof(child));
            if (child.Width >= Width)
                child.Width = (short) (Width - 1);
            child.X = 0;
            child.Y = 0;
            base.AddChild(child);
        }

        public override void Repaint(bool force) {
            //if (Dirty || force)
            //    Refill();
            
            var child = Children.First();

            child.Repaint(force);
            CopyBuffer(child.BoundingRect, child.Buffer);

            Dirty = false;

            Buffer[Width - 1] = _upArrow;
            Buffer[Buffer.Length - 1] = _downArrow;


            var pcent = Height/(double) child.Height;
            if (pcent >= 1) return;

            var height = Math.Max((Height - 2)*pcent, 1);
            var aboveLines = (Height - 2 - height)*_scrollPosition + 1;
            var barend = aboveLines + height;

            var i = 1;
            for (; i < aboveLines; i++)
                Buffer[(i + 1)*Width - 1] = _bgChar;
            for (; i < barend; i++)
                Buffer[(i + 1)*Width - 1] = _barChar;
            for (; i < Height - 1; i++)
                Buffer[(i + 1)*Width - 1] = _bgChar;
        }
    }
}