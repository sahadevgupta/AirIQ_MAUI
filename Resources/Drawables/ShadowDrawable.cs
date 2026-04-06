using System;

namespace AirIQ.Resources.Drawables;

public class ShadowDrawable : IDrawable
{
    public float ShadowHeight { get; set; } = 10f;
    public float Opacity { get; set; } = 0.25f;
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();

        // Draw a soft shadow below the rectangle
        var shadowRect = new RectF(
            0,
            dirtyRect.Height, // slightly overlapping bottom
            dirtyRect.Width,
            ShadowHeight
        );

        canvas.SetShadow(new SizeF(0, 4), ShadowHeight, Colors.Black.WithAlpha(Opacity));
        canvas.FillColor = Colors.White;
        canvas.FillRectangle(shadowRect);

        canvas.RestoreState();
    }
}
