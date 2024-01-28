using System;

public interface IInfoScreenAnimator
{
    void Show();
    void Hide();
    void Crossfade(Action transitionCallback);
}