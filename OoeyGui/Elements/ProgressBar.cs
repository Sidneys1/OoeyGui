using System;
using OoeyGui.Base;

namespace OoeyGui.Elements {
    public class ProgressBar : UiElement {
        private double _progress;

        public double Progress {
            get { return _progress; }
            set {
                _progress = value;
                Dirty = true;
                ParentContainer?.Invalidate(BoundingRect);
            }
        }

        public string Format { get; set; } = "{0:N0}%";

        public ProgressBar(short x, short y, short width, short height, int z) : base(x, y, width, height, z) {
            BackgroundColor = ConsoleColor.DarkGray;
            ForegroundColor = ConsoleColor.Black;
        }

        public override void Repaint(bool force) {
            base.Repaint(force);

            int w = (int) (Progress*Width);
            var text = string.Format(Format, Progress*100);
            var tpos = Width/2 - text.Length/2;

            for (int i = 0; i < Width; i++) {
                Buffer[i].BackgroundColor = i < w ? ConsoleColor.Gray : BackgroundColor;
                if (i >= tpos && i < tpos + text.Length)
                    Buffer[i].Char = text[i - tpos];
            }
        }
    }
}