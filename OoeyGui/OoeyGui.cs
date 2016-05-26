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
        public ScreenBuffer(short x, short y, short width, short height, int z) : base(x, y, width, height, z) {}
        
        public override void Repaint(bool force) {
            var arr = new SmallRect[_invalidatedAreas.Count];
            _invalidatedAreas.CopyTo(arr, 0);
            base.Repaint(force);
            var s = new SmallRect(0,0,Width, Height);
            if (arr.Length > 0) {
                s = arr[0];
                for (int i = 1; i < arr.Length; i++) {
                    if (arr[i].Left < s.Left)
                        s.Left = arr[i].Left;
                    if (arr[i].Top < s.Top)
                        s.Top = arr[i].Top;

                    if (arr[i].Right > s.Right)
                        s.Right = arr[i].Right;
                    if (arr[i].Bottom > s.Bottom)
                        s.Bottom = arr[i].Bottom;
                }
            }
            EConsole.UpdateRegion(Buffer, new Coord(s.Left, s.Top), new Coord(Width, Height), ref s);
        }
    }
}