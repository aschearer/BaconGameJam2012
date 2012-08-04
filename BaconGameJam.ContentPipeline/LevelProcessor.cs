using System;
using System.Collections.Generic;
using BaconGameJam.Common.Models.Doodads;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;
using TiledLib;

namespace BaconGameJam.ContentPipeline
{
    [ContentProcessor(DisplayName = "TMX Processor")]
    public class LevelProcessor : ContentProcessor<MapContent, IEnumerable<DoodadPlacement>>
    {
        public override IEnumerable<DoodadPlacement> Process(MapContent input, ContentProcessorContext context)
        {
            // build the textures
            TiledHelpers.BuildTileSetTextures(input, context);

            // generate source rectangles
            TiledHelpers.GenerateTileSourceRectangles(input);

            List<DoodadPlacement> doodadPlacements = new List<DoodadPlacement>();
            TileLayerContent layer = (TileLayerContent)input.Layers[0];

            for (int column = 0; column < layer.Width; column++)
            {
                for (int row = 0; row < layer.Height; row++)
                {
                    uint tileID = layer.Data[column + row * layer.Width];
                    int tileIndex;
                    SpriteEffects spriteEffects;
                    TiledHelpers.DecodeTileID(tileID, out tileIndex, out spriteEffects);

                    foreach (var tileSet in input.TileSets)
                    {
                        // if our tile index is in this set
                        if (tileIndex - tileSet.FirstId < tileSet.Tiles.Count)
                        {
                            // store the texture content and source rectangle
                            Tile tileDef = tileSet.Tiles[(tileIndex - tileSet.FirstId)];
                            if (!tileDef.Properties.ContainsKey("Type"))
                            {
                                continue;
                            }

                            string value = tileDef.Properties["Type"];
                            DoodadType tileType;
                            if (Enum.TryParse(value, true, out tileType))
                            {
                                DoodadPlacement placement = new DoodadPlacement();
                                placement.DoodadType = tileType;
                                placement.Position = new Vector2(
                                                         column * tileDef.Source.Width + (tileDef.Source.Width / 2f),
                                                         row * tileDef.Source.Height + (tileDef.Source.Height / 2f)) / 30f;
                                placement.Source = tileDef.Source;

                                doodadPlacements.Add(placement);
                            }

                            // and break out of the foreach loop
                            break;
                        }
                    }
                }
            }

            return doodadPlacements;
        }
    }
}