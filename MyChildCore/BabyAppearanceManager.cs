using StardewValley.Characters;

namespace MyChildCore { public static class BabyAppearanceManager { public static void Apply(Child child) { if (child == null) return;

// 배우자 이름 확인 또는 fallback
        string spouse = !string.IsNullOrWhiteSpace(child.spouse)
            ? child.spouse
            : (child.modData.TryGetValue("mychild.spouse", out var stored) ? stored : "Default");

        // 스프라이트 적용
        BabySpriteLoader.ApplyBabySprite(child, spouse);

        // 적용 여부 기록
        child.modData["mychild.applied"] = "true";
        child.modData["mychild.spouse"] = spouse;
    }
}

}

