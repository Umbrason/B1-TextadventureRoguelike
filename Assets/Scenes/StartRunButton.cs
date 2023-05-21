
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class StartRunButton : MonoBehaviour
{

    public (string, PlayableCombatActor)[] startClasses = new (string, PlayableCombatActor)[] {
        ("RANGER", new Ranger()),
        ("FIGHTER", new Fighter()),
        ("PYROMANCER", new Pyromancer()),
        ("CRYOMANCER", new Cryomancer()),
        ("AEROMANCER", new Aeromancer()),
        ("TERRAMANCER", new Terramancer()),
        ("MYCOMANCER", new Mycomancer()),
        ("NECROMANCER", new Necromancer()),
    };

    [SerializeField] TMP_Text classText;
    [SerializeField] TMP_InputField NameInput;
    [SerializeField] TMP_InputField SeedInput;

    private int startClassIndex = 0;
    public void ChangeStartClass(int amount)
    {
        startClassIndex += amount;
        startClassIndex += startClasses.Length;
        startClassIndex %= startClasses.Length;
        classText.text = startClasses[startClassIndex].Item1;
    }

    public void StartNewRun()
    {
        if (NameInput.text.Length == 0) return;
        var actor = startClasses[startClassIndex].Item2;
        actor.Name = NameInput.text;
        int seed = int.TryParse(SeedInput.text, out var s) ? s : SeedInput.text.Sum(ch => (int)ch);
        RunManager.BeginRun(actor, seed);
    }
}


