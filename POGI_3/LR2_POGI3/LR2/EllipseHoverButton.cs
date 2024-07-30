using System;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace LR2
{
    internal class EllipseHoverButton : HoverButton
    {
        /// <summary>
        /// Создаёт новую кнопку.
        /// </summary>
        public EllipseHoverButton() : base() { }

        /// <summary>
        /// Создаёт новую кнопку на основе старой.
        /// </summary>
        /// <param name="lastButton">Старая кнопка.</param>
        public EllipseHoverButton(HoverButton lastButton) : base(lastButton) { }

        protected override void OnResize(EventArgs e)
        {
            var graphicsPath = new GraphicsPath();
            graphicsPath.AddEllipse(new Rectangle(0, 0, Width, Height));
            Region = new Region(graphicsPath);
        }
    }
}
