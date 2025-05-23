using System.Linq;

namespace MyChildCore { public static class SpouseNames { public static readonly string[] Female = new[] { "Maddie", "Penny", "Blair", "Corine", "Flor", "Abigail", "Leah", "Haley", "Emily", "Maru", "Faye", "Paula", "Daia", "Sophia", "Isabelle", "Olivia", "Kiarra", "Irene", "Alissa", "Claire" };

public static readonly string[] Male = new[]
    {
        "Alex", "Anton", "Bryle", "Elliott", "Harvey", "Ian", "Jeric", "Jio", "June", "Kenneth",
        "Lance", "Magnus", "Philip", "Sam", "Sean", "Sebastian", "Shane", "Shiro", "Victor", "Wizard", "Zayne"
    };

    public static readonly string[] SpouseList = Female.Concat(Male).ToArray();
}

}

