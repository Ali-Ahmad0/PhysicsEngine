﻿using GameEngine.src.input;
using GameEngine.src.physics.component;
using Raylib_cs;
using System.Numerics;

namespace GameEngine.src.physics.body;

public enum PlayerStates
{
    IDLE,
    WALK,
    JUMP,
    FALL,
    CROUCH,
    DIE
}

internal struct Animation
{
    public Texture2D atlas;
    public int framesPerSecond;
    public List<Rectangle> rectangles;

    public Animation(Texture2D atlas, int framesPerSecond, List<Rectangle> rectangles)
    {
        this.atlas = atlas;
        this.framesPerSecond = framesPerSecond;
        this.rectangles = rectangles;
    }
}
public class PlayerBody2D : RigidBox2D
{
    internal PlayerStates State { get; set; }

    private Animation[] animations;
    public PlayerBody2D(Vector2 position, float rotation, float width, float height, List<Component> components) :
        base(position, rotation, 0.985f * width * height, 0.985f, width * height, 0f, width, height, components) 
    {
        // Initialize the player
        State = PlayerStates.IDLE;
        animations = new Animation[6];

        // Initialize the player animations
        createAnimations();

    }

    public void DrawPlayer()
    {
        Animation currAnimation = animations[0];
        switch (State)
        {
            case PlayerStates.IDLE:
                currAnimation = animations[0];
                break;
            case PlayerStates.WALK:
                currAnimation = animations[1];
                break;
            case PlayerStates.JUMP:
                currAnimation = animations[2];
                break;
            case PlayerStates.FALL:
                currAnimation = animations[3];
                break;
            case PlayerStates.CROUCH:
                currAnimation = animations[4];
                break;
            case PlayerStates.DIE:
                currAnimation = animations[5];
                break;
            default:
                break;
        }


        Rectangle dest = new Rectangle(Transform.Translation.X, Transform.Translation.Y, Dimensions.Height, Dimensions.Height);
        Vector2 origin = new Vector2(Dimensions.Height / 2.75f, Dimensions.Height / 2);
        
        int index = (int)(Raylib.GetTime() * currAnimation.framesPerSecond) % currAnimation.rectangles.Count;
        Raylib.DrawTexturePro(currAnimation.atlas, currAnimation.rectangles[index], dest, origin, 0, Color.White);
    }

    private void createAnimations()
    {
        // Implement the new AddAnimation method
        String path = "C:/Users/saadk/Desktop/NUST/Semester 2/Object Oriented Programming/End Semester Project/sprites/Hero Knight/Sprites/";
        AddAnimation(PlayerStates.IDLE, path + "_Idle.png", 1, 10, new Rectangle(0, 40, 40, 40));
        AddAnimation(PlayerStates.WALK, path + "Run.png", 10, 8, new Rectangle(0, 0, 180, 180));
    }

    public void AddAnimation(PlayerStates state, String path, int framesPerSecond, int numberOfSprite, Rectangle spriteSize)
    {
        List<Rectangle> rectangles = new List<Rectangle>();
        for (int i = 0; i < numberOfSprite; i++)
        {
            rectangles.Add(new Rectangle(spriteSize.X + (i * 3 + 1) * spriteSize.Width, spriteSize.Y, spriteSize.Width, spriteSize.Height));
        }
        Animation anim = new Animation(Raylib.LoadTexture(path), framesPerSecond, rectangles);
        animations[(int)state] = anim;
    }
}

