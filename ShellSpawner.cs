using Godot;
using Godot.Collections;

public partial class ShellSpawner : MultiplayerSpawner
{
    [Export]
    public PackedScene Shell { get; set; }

    public ShellSpawner() => this.SpawnFunction = new Callable(this, MethodName.SpawnShell);

    private Node SpawnShell(Array data)
    {
        var globalPosition = data[0].AsVector2();
        var targetName = data[1].AsString();
        var damage = data[2].AsInt32();

        var shell = this.Shell.Instantiate<Shell>();
        shell.Target = this.GetTree().Root.GetNode<Node2D>($"Game/{targetName}");
        shell.GlobalPosition = globalPosition;
        shell.Damage = damage;
        return shell;
    }
}
