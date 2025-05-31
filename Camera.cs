using Silk.NET.Maths;

namespace TheAdventure;

public class Camera
{
    private int _x;
    private int _y;
    private Rectangle<int> _worldBounds = new();

    public int X => _x;
    public int Y => _y;

    public readonly int Width;
    public readonly int Height;

    private int _shakeDuration = 0;
    private readonly Random _random = new();

    public Camera(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public void SetWorldBounds(Rectangle<int> bounds)
    {
        var marginLeft = Width / 2;
        var marginTop = Height / 2;

        if (marginLeft * 2 > bounds.Size.X)
            marginLeft = 48;

        if (marginTop * 2 > bounds.Size.Y)
            marginTop = 48;

        _worldBounds = new Rectangle<int>(
            marginLeft,
            marginTop,
            bounds.Size.X - marginLeft * 2,
            bounds.Size.Y - marginTop * 2
        );

        _x = marginLeft;
        _y = marginTop;
    }

    public void LookAt(int x, int y)
    {
        if (_worldBounds.Contains(new Vector2D<int>(_x, y)))
            _y = y;

        if (_worldBounds.Contains(new Vector2D<int>(x, _y)))
            _x = x;
    }

    public void Shake(int duration = 10)
    {
        _shakeDuration = duration;
    }

    public Vector2D<int> ToScreenCoordinates(Vector2D<int> worldPosition)
    {
        int offsetX = 0;
        int offsetY = 0;

        if (_shakeDuration > 0)
        {
            offsetX = _random.Next(-50, 55);
            offsetY = _random.Next(-50, 55);
            _shakeDuration--;
        }

        return new Vector2D<int>(
            worldPosition.X - _x + Width / 2 + offsetX,
            worldPosition.Y - _y + Height / 2 + offsetY
        );
    }

    public Rectangle<int> ToScreenCoordinates(Rectangle<int> rect)
{
    var translated = ToScreenCoordinates(rect.Origin);
    return new Rectangle<int>(translated, rect.Size);
}


    public Vector2D<int> ToWorldCoordinates(Vector2D<int> point)
    {
        return point - new Vector2D<int>(Width / 2 - _x, Height / 2 - _y);
    }

    public Rectangle<int> ToScreenRect(Rectangle<int> rect)
{
    var topLeft = ToScreenCoordinates(rect.Origin);
    return new Rectangle<int>(topLeft, rect.Size);
}

}
