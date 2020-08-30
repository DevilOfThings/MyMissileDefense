using Godot;
using System;

public class player : Node
{
    bulletBrain bulletBrain;
    public bool canShoot = true;

    [Export]
    public int health = 3;
    public int score = 0;
    public bool gameOver = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bulletBrain =(bulletBrain)GetNode("/root/game/bullets/bulletBrain");
        updateUI();
        canShoot = true;
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("leftMouseClick") && gameOver == true) {
            GetTree().ReloadCurrentScene();
        }
    }

    public void updateUI()
    {
        var healthAndScore = (Label)GetNode("/root/game/hud/healthAndScore");
        healthAndScore.Text = $"Score: {score} Health: {health}";
    }

    public void hitPlayer(int damageAmount = 1)
    {
        health = Math.Max(health-damageAmount, 0);
        updateUI();

        if(health <= 0 && gameOver != true) {
            gameOver = true;
            canShoot = false;
            var gameOverScreen = (Node2D) GetNode("/root/game/hud/gameOverScreen");
            gameOverScreen.Visible = true;
            
            var cannon = (Node2D)GetNode("/root/game/foreground/cannon");
            //bulletBrain.spawnExplosion(cannon.GlobalPosition, "enemy");
            bulletBrain.CallDeferred("spawnExplosion", cannon.GlobalPosition, "enemy");
           cannon.QueueFree();
        }
    }

    public void addScore(int scoreAmount =1)
    {
        score += scoreAmount;
        updateUI();
        bulletBrain.increseDifficulty();
    }

    public void _on_playerHitZone_area_entered(Area2D bullet)
    {
        var bulletType = (AnimatedSprite)bullet.GetNodeOrNull("AnimatedSprite");
        

        if((bulletType != null) && (bulletType.Animation == "enemy") && bullet is bullet)  {
            //bulletBrain.spawnExplosion(bullet.GlobalPosition, "enemy");
            bulletBrain.CallDeferred("spawnExplosion", bullet.GlobalPosition, "enemy");
            bullet.QueueFree();
            hitPlayer();
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
