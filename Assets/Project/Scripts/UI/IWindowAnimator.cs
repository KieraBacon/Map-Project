using System;

public interface IWindowAnimator
{
    void Init();
    void Show();
    void Hide();
    void Crossfade(Action transitionCallback);
}