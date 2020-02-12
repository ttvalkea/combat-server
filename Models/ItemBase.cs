using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Enums;

public class ItemBase
{
    public string id { get; set; }
    public int positionX { get; set; }
    public int positionY { get; set; }
    public Direction direction { get; set; }
    public int sizeX { get; set; }
    public int sizeY { get; set; }
}