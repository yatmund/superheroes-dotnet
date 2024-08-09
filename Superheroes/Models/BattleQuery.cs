using System;
using System.ComponentModel.DataAnnotations;

public sealed class BattleQuery
{
    public string Hero { get; set; }

    public string Villain { get; set; }

    public void Validate()
    {

       ArgumentNullException.ThrowIfNull(Hero);
       ArgumentNullException.ThrowIfNull(Villain);

       if (Hero == Villain)
       {
           throw new ArgumentException("Hero and Villain must be different");
       }
    }
}
