using Microsoft.Maui.Graphics;

namespace AirIQ.Resources.Drawables;

public class NotchDrawable : IDrawable
{
    public void Draw(ICanvas canvas, RectF rect)
    {
        float w = rect.Width;
        float h = rect.Height;

        float startX = 0;
        float topY = 0;
        float bottomY = h;

        float rightFlatX = w * 0.92f;     // right vertical part
        float curveTopY = h * 0.28f;      // where curve starts
        float curveBottomY = h * 0.72f;   // where curve ends
        float centerY = h * 0.50f;

        var path = new PathF();

        path.MoveTo(startX, topY);
        path.LineTo(w * 0.58f, curveTopY - 35); // top diagonal

        path.CurveTo(
            w * 0.78f, curveTopY - 10,
            rightFlatX, curveTopY + 20,
            rightFlatX, centerY);

        path.CurveTo(
            rightFlatX, curveBottomY - 20,
            w * 0.78f, curveBottomY + 10,
            w * 0.58f, curveBottomY + 35);

        path.LineTo(startX, bottomY); // bottom diagonal back
        path.Close();

        canvas.FillColor = Color.FromArgb("#149FEF");
        canvas.FillPath(path);
    }
}
