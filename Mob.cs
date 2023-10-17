using Godot;

public partial class Mob : Area2D
{
    public override void _PhysicsProcess(double delta)
    {
        this.Position += new Vector2(1, 0) * -300  * (float)delta;
    }

    public void OnAreaEntered(Area2D area)
    {
        if (!this.IsMultiplayerAuthority())
        {
            return;
        }

        switch (area)
        {
            case Shell:
                this.QueueFree();
                break;
        }
    }
}
