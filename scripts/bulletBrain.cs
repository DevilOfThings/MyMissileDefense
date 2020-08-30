using Godot;
using System;

public class bulletBrain : Node
{
    Scenes scenes = new Scenes();
    Timer enemySpawner;
    [Export]
    public float maxSpawnInterval = 3f;
    [Export]
    public float minSpawnInterval = 0.5f;
    [Export]
    public float spawnIntervalDecrease = 0.2f;
    public float spawnInterval = 0f;

    [Export]
    public int playerBulletSpeed =300;

    [Export]
    public int enemyBulletSpeed = 200;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
         enemySpawner =(Timer)GetNode("enemySpawner");
         spawnInterval = maxSpawnInterval;
    }

    public void increseDifficulty()
    {
        var newSpawnInterval =spawnInterval - spawnIntervalDecrease;
        newSpawnInterval = Math.Max(newSpawnInterval, minSpawnInterval);
        enemySpawner.WaitTime = newSpawnInterval;
        enemySpawner.Start();
    }
    public void _on_enemySpawner_timeout()
    {
        spawnEnemy();
    }

    public void _on_cloudSpawner_timeout()
    {
        spawnCloud();
    }

    public void spawnCloud()
    {
        var cloud = (AnimatedSprite) scenes._sceneCloud.Instance();
        GetNode("/root/game/foreground").AddChild(cloud);
        
        cloud.Frame = Convert.ToInt32(Math.Floor(GD.RandRange(0,3)));
        
        Vector2 spawnPosition = new Vector2(-100, Convert.ToSingle(GD.RandRange(0,400)));
        cloud.GlobalPosition = spawnPosition;

        var randomScale = Convert.ToSingle(GD.RandRange(0,1));
        cloud.Scale = new Vector2(randomScale, randomScale);

    }

    public void spawnEnemy()
    {
        Vector2 spawnPosition = new Vector2(Convert.ToSingle(GD.RandRange(0,1000)), -30);
        Vector2 targetPosition = new Vector2(Convert.ToSingle(GD.RandRange(0,1000)), 550);

        spawnBullet(spawnPosition, targetPosition, "enemy");
    }

    public void spawnBullet(Vector2 spawnPosition, Vector2 targetPosition, string animationName)
    {
        
        // spawn bullet
        var bullet = (bullet) scenes._sceneBullet.Instance();
        GetNode("/root/game/bullets").AddChild(bullet);
        bullet.GlobalPosition = spawnPosition;
        bullet.LookAt(targetPosition);
        GD.Print($"spawnPosition: {spawnPosition} targetPosition: {targetPosition} name: {animationName}");
        // animation
        var bulletSprite = (AnimatedSprite)bullet.GetNode("AnimatedSprite");
        bulletSprite.Play(animationName);
        GD.Print("animateBullet");

        if(animationName == "player") {
            bullet.speed = playerBulletSpeed;
        }
        else if(animationName == "enemy") {
            bullet.speed = enemyBulletSpeed;
        }
    }

    public void spawnExplosion(Vector2 spawnPosition, string animationName)
    {
        // spawn explosion
        var explosion = (Area2D) scenes._sceneExplosion.Instance();
        GetNode("/root/game/bullets").AddChild(explosion);
        explosion.GlobalPosition = spawnPosition;

        var explosionSprite = (AnimatedSprite) explosion.GetNode("AnimatedSprite");
        explosionSprite.Play(animationName);

    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
