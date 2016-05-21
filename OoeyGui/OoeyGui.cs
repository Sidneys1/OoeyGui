using System;
using ExtendedConsole.Structs;
using OoeyGui.Base;
using EConsole = ExtendedConsole.ExtendedConsole;

namespace OoeyGui {
    public static class OoeyGui {
        private static bool _repaintRequested = true;

        internal static void RequestRepaint() {
            _repaintRequested = true;
        }

        internal static ScreenBuffer RootElement { get; private set; }

        public static void Init() {
            RootElement = new ScreenBuffer(0, 0, (short) Console.BufferWidth, (short) Console.BufferHeight, int.MinValue);
        }

        public static void Repaint(bool forceRedraw = false) {
            if (_repaintRequested)
                RootElement.Repaint(forceRedraw);
            _repaintRequested = false;
        }

        public static void AddChild(UiElement child) => RootElement?.AddChild(child);
    }

    internal class ScreenBuffer : ContainerElement {
        private SmallRect _smallRect;

        public ScreenBuffer(short x, short y, short width, short height, int z) : base(x, y, width, height, z) {
            _smallRect = new SmallRect(0, 0, Width, Height);
        }
        
        public override void Repaint(bool force) {
            base.Repaint(force);
            EConsole.UpdateRegion(Buffer, Coord.Zero, new Coord(Width, Height), ref _smallRect);
        }
    }
}