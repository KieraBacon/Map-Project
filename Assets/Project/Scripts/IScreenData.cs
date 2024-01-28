using System.Collections.Generic;

public interface IScreenData
{
    string HeaderText { get; }
    string BodyText { get; }
    IEnumerable<IScreenData> Links { get; } 
}