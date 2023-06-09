using System;

[Serializable]
public class BlockDirt : Block
{
    public BlockDirt() : base()
    {
    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 1;  // Assuming dirt is one space to the right of stone on the atlas.
        tile.y = 0;  // Assuming dirt is on the same row as stone on the atlas.
        return tile;
    }
}