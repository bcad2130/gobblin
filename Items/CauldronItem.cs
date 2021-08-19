﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronItem : Item
{
    public override string Name     { get { return "Cauldron"; } }

    public override bool Equipment  { get { return true; } }

    public override bool Meal       { get { return false; } }

    public override bool Treat      { get { return false; } }

    public override bool Trick      { get { return false; } }

    public override bool Drink      { get { return false; } }

    public override bool Food       { get { return false; } }
}
