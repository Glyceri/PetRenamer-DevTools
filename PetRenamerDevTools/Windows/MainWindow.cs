using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.System.String;
using ImGuiNET;
using ImGuiScene;
using FFCompanion = FFXIVClientStructs.FFXIV.Client.Game.Character.Companion;

namespace SamplePlugin.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin Plugin;

    public MainWindow(Plugin plugin) : base(
        "PetRenamer Dev Window", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        IsOpen = true;

        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(675, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.Plugin = plugin;
    }

    public void Dispose()
    {
        
    }

    public unsafe override void Draw()
    {

        ImGui.Text("You have the following pets summoned:");

        GameObject* currentObject = GameObjectManager.GetGameObjectByIndex(0);
        if (currentObject == null) return;

        Character* plCharacter = (Character*)currentObject;
        if (plCharacter == null) return;

        FFCompanion* plCompanion = plCharacter->Companion.CompanionObject;

        uint objectID = currentObject->ObjectID;

        int minionID = -1;
        string minionName = "[ERROR]";
        if (plCompanion != null)
        {
            minionID = plCompanion->Character.CharacterData.ModelSkeletonId;
            Utf8String str = new Utf8String();
            str.SetString(plCompanion->Character.GameObject.Name);
            minionName = str.ToString();
        }
        ImGui.Text($"Your Minion has the ID of: {minionID} with the name: {minionName}");
        ImGui.Text($"Battle Pet Data: ");
        for (int i = 0; i < 5000; i++)
        {
            GameObject* currentObject2 = GameObjectManager.GetGameObjectByIndex(i);
            if (currentObject2 == null) continue;

            Character* plCharacter2 = (Character*)currentObject2;
            if (plCharacter2 == null) continue;

            if(currentObject2->OwnerID == objectID)
            {
                Utf8String str = new Utf8String();
                str.SetString(plCharacter2->GameObject.Name);
                minionName = str.ToString();
                ImGui.Text($"Your BattlePet has the ID of: {plCharacter2->CharacterData.ModelCharaId} with the name: {minionName}");
            }
        }
    }
}
