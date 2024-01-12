using Godot;
using System;

public partial class Mouse : Marker2D
{
	[Export] float _Force = 10f;
	RigidBody2D RigidBody;
	RigidBody2D CurrentItem;
	RayCast2D Touch;
	bool Grab = false;

	public override void _Ready()
	{
		RigidBody = new RigidBody2D(); //??????????????
		Touch = GetNode<RayCast2D>("RayCast2D");
	}

	public override void _Process(double delta)
	{
		GlobalPosition = GetGlobalMousePosition();
		if (Input.IsActionJustPressed("LeftClick") && CurrentItem != null)
		{
			GD.Print("Click");
			Grab = true;
			//CurrentItem.GravityScale = 0;
		}

		if (Grab && CurrentItem != null)
		{
			GD.Print("Pull");
			//This works stupidly good
			CurrentItem.LinearVelocity = (GetGlobalMousePosition() - CurrentItem.GlobalPosition) * _Force;
		}

		if (Touch.GetCollider() != null && Touch.GetCollider().GetType() == RigidBody.GetType())
		{
			GD.Print("Yay");
			CurrentItem = (RigidBody2D)Touch.GetCollider();
			GD.Print(CurrentItem);
		}
		//if (Touch.GetCollider() == null && !Grab)
		//{
		//	GD.Print("Nay");
		//	CurrentItem = null;
		//}

	}
}
