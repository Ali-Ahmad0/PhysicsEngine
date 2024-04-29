﻿using GameEngine.src.physics.body;
using System.Numerics;
using Raylib_cs;
using GameEngine.src.world;

namespace GameEngine.src.tilemap;

public struct TileMapProps
{
    public int[,] tileMap;
    public int[,] textureMap;
    public Texture2D[,]? backGround;
    public int size;
}


internal static class TileMap
{
    public static void GenerateTileMapTerrain(int[,] grid, int size, List<PhysicsBody2D> bodies)
    {
        // Find all the boxes in the grid
        List<Rectangle> boxes = FindBoxes(grid);

        // Create the bodies
        AddBodies(boxes, bodies, size);
    }
    public static List<Rectangle> FindBoxes(int[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        List<Rectangle> boxes = new List<Rectangle>();

        for (int i = 0; i < rows; i++)
        {
            int start, end;
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == 1)
                {
                    start = j;
                    while (j < cols && grid[i, j] == 1)
                    {
                        j++;
                    }
                    end = j;
                    int width = end - start;
                    boxes.Add(new Rectangle(start, i, width, 1));
                }
            }
        }
        mergeVerticalBoxes(boxes);
        return boxes;
    }


    public static void AddBodies(List<Rectangle> boxes, List<PhysicsBody2D> bodies, int size)
    {
        Console.WriteLine(boxes.Count);
        // Iterate through the edges and create the bodies
        foreach (Rectangle box in boxes)
        {
            Console.WriteLine(box);
            Vector2 position = size * (box.Position + new Vector2(box.Width / 2.0f, box.Height / 2.0f));

            Console.WriteLine(position);
            float width = box.Width * size;
            float height = box.Height * size;
            WorldCreation.CreateStaticBody(position, 0f, Vector2.One, 0.5f, width, height, out StaticBody2D staticBody);
            bodies.Add(staticBody);
        }
    }

    private static void mergeVerticalBoxes(List<Rectangle> boxes)
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            for (int j = i + 1; j < boxes.Count; j++)
            {
                if (boxes[i].X == boxes[j].X && boxes[i].Width == boxes[j].Width)
                {
                    if (boxes[i].Y + boxes[i].Height == boxes[j].Y)
                    {
                        boxes[i] = new Rectangle(boxes[i].X, boxes[i].Y, boxes[i].Width, boxes[i].Height + boxes[j].Height);
                        boxes.RemoveAt(j);
                    }
                }
            }
        }
    }

    public static Texture2D[,] GenerateBackground(int[,] textureMap)
    {
        Texture2D[,] backGround = new Texture2D[textureMap.GetLength(0), textureMap.GetLength(1)];
        for (int i = 0; i < textureMap.GetLength(0); i++)
        {
            for (int j = 0; j < textureMap.GetLength(1); j++)
            {

                if (textureMap[i, j] != 0)
                {
                    backGround[i, j] = Raylib.LoadTexture("C:/Users/saadk/Desktop/NUST/Semester 2/Object Oriented Programming/End Semester Project/sprites/Dungeon Tile Set/Dungeon Tile Set");
                }
                else
                {
                    backGround[i, j] = Raylib.LoadTexture("C:/Users/saadk/Desktop/NUST/Semester 2/Object Oriented Programming/End Semester Project/Physics Engine/res/scenes/Terrain (16x16).png");
                }
            }
        }

        return backGround;
    }

    public static void DrawBackground(Texture2D[,] backGround, int size)
    {
        for (int i = 0; i < backGround.GetLength(0); i++)
        {
            for (int j = 0; j < backGround.GetLength(1); j++)
            {
                Rectangle sourceRec = new Rectangle(0, 0, 48, 48);
                Rectangle destRec = new Rectangle(j * size, i * size, size, size);
                // Raylib.DrawTextureRec(backGround[i, j], sourceRec, new Vector2(i * 60, j * 60), Color.White);

                Raylib.DrawTexturePro(backGround[i, j], sourceRec, destRec, new Vector2(0, 0), 0, Color.White);
            }
        }
    }

    public static void DrawBackground(TileMapProps tileMapProps)
    {
        DrawBackground(tileMapProps.backGround, tileMapProps.size);
    }

    // Make a method that takes in both tilemap and background and draws the tilemap
    public static void GenerateTileMap(ref TileMapProps tileMapProps, List<PhysicsBody2D> bodies)
    {
        tileMapProps.size = (int)Math.Pow(2, tileMapProps.size + 2);
        GenerateTileMapTerrain(tileMapProps.tileMap, tileMapProps.size, bodies);

        Texture2D[,] backGround = GenerateBackground(tileMapProps.textureMap);

        tileMapProps.backGround = backGround;
    }
}
