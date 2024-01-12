using Godot;
using System;

public partial class test_world : Node
{
	[Export] PackedScene _player;

	public override void _Ready()
	{
		Node2D player = (Node2D)_player.Instantiate();
		player.GlobalPosition = GetNode<Marker2D>("PlayerSpawner").GlobalPosition; 
		//GetTree().Root.AddChild(player);
		AddChild(player);
	}
}
