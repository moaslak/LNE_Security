﻿using LNE_Security.Screens;
using System;
using TECHCOOL;

namespace LNE_Security;

class Program
{
    public Menu Menu
    {
        get => default;
        set
        {
        }
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome screen");
        Console.WriteLine("Global keys");
        Console.WriteLine("F11 - Toggle full screen");
        Console.WriteLine("Esc - Close App");
        Console.WriteLine();
        Console.WriteLine("Press a key to start program");
        Console.ReadKey();
        MainMenuScreen mainMenu = new MainMenuScreen();
        ScreenHandler.Display(mainMenu);
    }
}
