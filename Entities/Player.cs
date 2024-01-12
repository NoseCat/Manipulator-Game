using Godot;
using System;

public partial class Player : Node2D
{
	[Export] float MaxSpeed = 3000;
	[Export] float Acceleration = 3000; //Speed and Acceleration arent in the same units
	[Export] float Reach = 248f * 1.5f;
	float direction = 0;
	[Export] float InterpolSpeed = 0.2f;

	RigidBody2D Base;
	//RigidBody2D RWheel;

	Area2D ArmBase;
	Area2D Arm;
	Vector2 Mouse;
	Marker2D Pointer;
	Marker2D Center;
	Marker2D Mid;
	float ElbowFix = 0f;
	Marker2D RotationCenter;
	Marker2D InterpolRotationCenter;
	Marker2D InterpolPointer;

	public override void _Ready()
	{
		Center = GetNode<Marker2D>("PlayerBase/MarkerCenter");
		Pointer = GetNode<Marker2D>("PlayerBase/MarkerCenter/MarkerPointer");
		InterpolPointer = GetNode<Marker2D>("PlayerBase/InterpolMarkerPointer");
		Mid = GetNode<Marker2D>("PlayerBase/MarkerCenter/MarkerMid");
		RotationCenter = GetNode<Marker2D>("PlayerBase/MarkerCenter/MarkerRotationCenter");
		InterpolRotationCenter = GetNode<Marker2D>("PlayerBase/InterpolMarkerRotationCenter");
		ArmBase = GetNode<Area2D>("PlayerBase/ArmBase");
		Arm = GetNode<Area2D>("PlayerBase/ArmBase/Arm");

		Base = GetNode<RigidBody2D>("PlayerBase");
		//RWheel = GetNode<RigidBody2D>("RWheel");
	}

	//Returns float value of lenght of vector2. If lenght more than limit returns limit
	public static float LenghtVec2Limited(Vector2 Vec, float limit)
	{
		float l = (float)Math.Sqrt(Vec.X * Vec.X + Vec.Y * Vec.Y); 
		if(l > limit)
			return limit;
		else 
			return l; 
	}

	//linear interpolation for degrees
	public static float LerpDegrees(float start, float end, float amount)
    {
        float difference = Math.Abs(end - start);
        if (difference > 180)
            if (end > start)
                start += 360;
            else
                end += 360;
        float value = start + ((end - start) * amount);
        return value;
    }

	public override void _PhysicsProcess(double delta)
	{
		Mouse = GetGlobalMousePosition();
		Center.LookAt(Mouse);
		Pointer.Position = new Vector2(LenghtVec2Limited(Center.GlobalPosition - Mouse, Reach), 0f);
		InterpolPointer.GlobalPosition = InterpolPointer.GlobalPosition.Lerp(Pointer.GlobalPosition, InterpolSpeed);
		Mid.Position = new Vector2(LenghtVec2Limited(Center.GlobalPosition - Mouse, Reach) / 2, 0f); //maybe rewrite so it returns Vector2
		ElbowFix = Mouse.X < Center.GlobalPosition.X ? 1f : -1f;
		RotationCenter.Rotation = ElbowFix * (float)Math.Acos((double)LenghtVec2Limited(Center.GlobalPosition - Mouse, Reach) / Reach); 
		InterpolRotationCenter.GlobalRotationDegrees = LerpDegrees(InterpolRotationCenter.GlobalRotationDegrees, RotationCenter.GlobalRotationDegrees, InterpolSpeed);

		ArmBase.GlobalRotation = InterpolRotationCenter.GlobalRotation;
		Arm.LookAt(InterpolPointer.GlobalPosition);
		direction = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");

		if (Math.Abs(Base.LinearVelocity.X) <= MaxSpeed)
		{
			Base.ApplyCentralForce(new Vector2(direction * Acceleration, 0)); 
			//RWheel.ApplyCentralForce(new Vector2(direction * Acceleration, 0));
			//RWheel.ApplyTorque(direction * Acceleration);
		}
		
		
		GetNode<Label>("PlayerBase/Label").Text = "RotationCenter.RotationDegrees: " + RotationCenter.GlobalRotationDegrees;
	}
}
