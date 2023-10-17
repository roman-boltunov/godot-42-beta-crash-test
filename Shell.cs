using Godot;

public partial class Shell : Area2D
{
    public int Damage { get; set; }

    [Export]
    public int Speed { get; set; } = 1500;

    public Node2D Target
    {
        get => this.target;
        set
        {
            this.target = value;
            this.targerRef = WeakRef(value);
        }
    }

    private Node2D target;

    private WeakRef targerRef;

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        // Check for existence
        if (this.targerRef.GetRef().Obj == null)
        {
            this.CallDeferred(Node.MethodName.QueueFree);
            return;
        }
        var direction = this.GlobalPosition.DirectionTo(this.Target.GlobalPosition);
        var speed = this.Speed * (float)delta;
        var nextPosition = this.GlobalPosition + new Vector2(speed * direction.X, speed * direction.Y);
        this.GlobalPosition = nextPosition;
    }

    public void OnAreaEntered(Area2D area)
    {
        if (!this.IsMultiplayerAuthority())
        {
            return;
        }

        switch (area)
        {
            case Mob mob:
                this.QueueFree();
                break;
            default:
                break;
        }
    }
}
