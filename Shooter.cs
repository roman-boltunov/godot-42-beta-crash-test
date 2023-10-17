using Godot;
using Godot.Collections;

public partial class Shooter : Area2D
{
	[Export]
	public Timer ShootTimer {get;set;}

	[Export]
	public int Damage {get;set;}

	private ShellSpawner spawner;

    private Array<Node2D> targets = new();

	private bool canFire;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.spawner = this.GetTree().Root.GetNode<ShellSpawner>("Game/ShellSpawner");
		this.canFire = true;
	}

	public override void _PhysicsProcess(double delta)
    {
        if (!this.IsMultiplayerAuthority())
        {
            return;
        }

        if (!this.canFire || this.targets.Count == 0)
        {
            return;
        }

        this.canFire = false;
        this.ShootTimer.Start();
        this.FireShell();
    }

    public void OnShootTimerTimeout() => this.canFire = true;

	public void OnAreaEntered(Area2D area)
    {
        if (!this.IsMultiplayerAuthority())
        {
            return;
        }

        switch (area)
        {
            case Mob:
                this.targets.Add(area);
                break;
            default:
                break;
        }
    }

    public void OnAreaExited(Area2D area)
    {
        if (!this.IsMultiplayerAuthority())
        {
            return;
        }

        switch (area)
        {
            case Mob:
                this.targets.Remove(area);
                break;
            default:
                break;
        }
    }

	private void FireShell()
    {
		GD.Print("FIRE");
        var data = new Array()
        {
            this.GlobalPosition,
            this.targets[0].Name,
            this.Damage,
        };
        this.spawner.Spawn(data);
    }
}
