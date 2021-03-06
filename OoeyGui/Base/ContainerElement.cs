using System;
using System.Collections.Generic;
using System.Linq;
using ExtendedConsole.Structs;

namespace OoeyGui.Base {
    public abstract class ContainerElement : UiElement {
        private readonly SortedList<int, UiElement> _children = new SortedList<int, UiElement>();
        public IEnumerable<UiElement> Children => _children.Values;

        protected readonly Stack<SmallRect> _invalidatedAreas = new Stack<SmallRect>();

        protected ContainerElement(short x, short y, short width, short height, int z) : base(x, y, width, height, z) {
            Refill();
        }

        public virtual void AddChild(UiElement child) {
            _children.Add(child.Z, child);
            child.ParentContainer = this;
            Invalidate();
        }

        public virtual void Invalidate() => Dirty = true;

        public virtual void Invalidate(SmallRect rect) {
            _invalidatedAreas.Push(rect);
            foreach (var containerElement in Children.OfType<ContainerElement>().Where(e => e.BoundingRect.Intersects(rect)))
                containerElement.Invalidate(rect + Location);
        }

        public Coord Translate(Coord input) => new Coord((short) (input.X + X), (short) (input.Y + Y));

        public SmallRect Translate(SmallRect input) =>
                new SmallRect((short) (input.Left + X), (short) (input.Top + Y), (short) (input.Right + X),
                    (short) (input.Bottom + Y));

        public override void Repaint(bool force) {
            if (Dirty || force)
                Refill();

            if (_invalidatedAreas.Count > 0)
                RefillAreas();

            foreach (var uiElement in Children) {
                uiElement.Repaint(force);
                CopyBuffer(uiElement.BoundingRect, uiElement.Buffer);
            }

            Dirty = false;
        }

        protected void CopyBuffer(SmallRect rect, CharInfo[] buffer) {
            var start = Math.Max((int)rect.Top,0)*Width + rect.Left;
            int stride = Width;// - rect.Width;

            var sHeight = Math.Max(0, -rect.Top);
            var mHeight = Math.Min(Height - rect.Top, rect.Height);
            var mWidth = Math.Min(Width - rect.Left, rect.Width);
            //var cutoff = Math.Max(rect.Width - (Width - rect.Left), 0);
            //stride += cutoff;
            for (var y = sHeight; y < mHeight; y++) {
                Array.Copy(buffer, y*rect.Width, Buffer, start, mWidth);
                start += stride;
            }
        }

        protected void RefillAreas() {
            while (_invalidatedAreas.Count > 0) {
                var area = _invalidatedAreas.Pop();
                CopyBuffer(area, new CharInfo[area.Area]);
            }
        }
    }
}