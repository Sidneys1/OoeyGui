using System.Linq;
using OoeyGui.Base;

namespace OoeyGui.Containers {
    public class StackPanel : Panel {
        public bool Autosize { get; }

        public StackPanel(short x, short y, short width, short height, int z) : base(x, y, height, 1, z) {}

        public StackPanel(short x, short y, short width, int z) : base(x, y, width, 1, z) {
            Autosize = true;
        }

        public override void AddChild(UiElement child) {
            var pos = Children.Sum(c => c.Height);
            child.Y = (short) pos;
            child.X = 0;
            base.AddChild(child);
            pos = Children.Sum(c => c.Height);
            if (Autosize && pos > Height)
                Height = (short) pos;
        }
    }
}