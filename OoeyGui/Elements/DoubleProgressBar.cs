using System;
using OoeyGui.Base;

namespace OoeyGui.Elements {
    public class DoubleProgressBar : UiElement {
        private double _progress;

        public double Progress {
            get { return _progress; }
            set {
                _progress = value;
                if (!Dirty)
                    ParentContainer?.Invalidate(BoundingRect);
                Dirty = true;
            }
        }

        private double _subProgress;

        public double SubProgress {
            get { return _subProgress; }
            set {
                _subProgress = value;
                if (!Dirty)
                    ParentContainer?.Invalidate(BoundingRect);
                Dirty = true;
            }
        }

        public DoubleProgressBar(short x, short y, short width, short height, int z) : base(x, y, width, height, z) {
            BackgroundColor = ConsoleColor.DarkGray;
            ForegroundColor = ConsoleColor.Gray;
        }

        public override void Repaint(bool force) {
            base.Repaint(force);

            int w = (int) (Progress*Width);
            int w2 = (int) (SubProgress*Width);
            for (int i = 0; i < Width; i++) {
                if (i < w)
                    Buffer[i].Char = i < w2 ? '\xdb' : '\xdf';
                else
                    Buffer[i].Char = i < w2 ? '\xdc' : ' ';
            }
        }
    }
}