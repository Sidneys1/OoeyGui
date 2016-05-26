using System;
using ExtendedConsole;
using OoeyGui.Base;

namespace OoeyGui {
    public class Label : UiElement {
        #region Fields

        private FormattedString _text;

        #endregion Fields

        #region Constructors

        public Label(short x, short y, short width, short height, int z, FormattedString text)
            : base(x, y, width, height, z) {
            _text = text;
        }

        public Label(short x, short y, short height, int z, FormattedString text)
            : base(x, y, (short) text.Length, height, z) {
            _text = text;
        }

        #endregion Constructors

        #region Properties

        public FormattedString Text {
            get { return _text; }
            set {
                _text = value;
                if (!Dirty)
                    ParentContainer?.Invalidate(BoundingRect);
                Dirty = true;
            }
        }

        #endregion Properties

        #region Methods

        public override void Repaint(bool force) {
            if (!force && !Dirty) return;
            base.Repaint(force);

            var c = DefaultChar;
            var len = Math.Min(_text.Length, Width);
            var index = 0;
            foreach (var formattedText in _text.Sections) {
                if (formattedText.Reset) {
                    c = DefaultChar;
                }
                else {
                    if (formattedText.ForegroundColor.HasValue)
                        c.ForegroundColor = formattedText.ForegroundColor.Value;
                    if (formattedText.BackgroundColor.HasValue)
                        c.BackgroundColor = formattedText.BackgroundColor.Value;
                }
                var max = Math.Min(len - index, formattedText.Text.Length);
                for (var i = 0; i < max; i++) {
                    c.Char = formattedText.Text[i];
                    Buffer[index++] = c;
                }

                if (index >= len) break;
            }

            Dirty = false;
        }

        #endregion Methods
    }
}