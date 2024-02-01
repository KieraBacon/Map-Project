public interface IWindow
{
    void Show();
    void Hide();
    void Toggle();
    bool IsVisible { get; set; }
}