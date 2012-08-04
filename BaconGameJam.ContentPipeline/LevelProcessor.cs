using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
            foreach (LayerContent layer in input.Layers)
            {
                if (layer is TileLayerContent)
                {
                    this.ProcessTileLayer((TileLayerContent)layer, input.TileSets, doodadPlacements);
                }
                else if (layer is MapObjectLayerContent)
                {
                    this.ProcessObjectLayer((MapObjectLayerContent)layer, input.TileSets, doodadPlacements);
                }
            }

            return doodadPlacements;
        }

        private void ProcessObjectLayer(
            MapObjectLayerContent layer, 
            IEnumerable<TileSetContent> tileSets, 
            List<DoodadPlacement> doodadPlacements)
        {
            foreach (MapObjectContent mapObject in layer.Objects)
            {
                Tile tile = this.GetTile(mapObject.GID, tileSets);
                if (!tile.Properties.ContainsKey("Type"))
                {
                    continue;
                }


                string value = tile.Properties["Type"];
                DoodadType tileType;
                if (Enum.TryParse(value, true, out tileType))
                {
                    DoodadPlacement placement = new DoodadPlacement();
                    placement.DoodadType = tileType;

                    placement.Position = new Vector2(
                        mapObject.Bounds.Center.X + 25,
                        mapObject.Bounds.Center.Y - 25) / 30f;
                    Team team;
                    if (!Enum.TryParse(tile.Properties["Team"], true, out team))
                    {
                        throw new InvalidEnumArgumentException("Team");
                    }

                    placement.Team = team;

                    doodadPlacements.Add(placement);
                }
                else
                {
                    throw new InvalidEnumArgumentException("Type");
                }
            }
        }

        private void ProcessTileLayer(
            TileLayerContent layer,
            IEnumerable<TileSetContent> tileSets,
            List<DoodadPlacement> doodadPlacements)
        {

            for (int column = 0; column < layer.Width; column++)
            {
                for (int row = 0; row < layer.Height; row++)
                {
                    uint tileID = layer.Data[column + row * layer.Width];
                    int tileIndex;
                    SpriteEffects spriteEffects;
                    TiledHelpers.DecodeTileID(tileID, out tileIndex, out spriteEffects);

                    Tile tileDef = this.GetTile(tileIndex, tileSets);
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
                }
            }
        }

        private Tile GetTile(int gid, IEnumerable<TileSetContent> tileSets)
        {
            return (from tileSet in tileSets
                    let adjustedTileIndex = gid - tileSet.FirstId
                    where adjustedTileIndex >= 0 && adjustedTileIndex < tileSet.Tiles.Count
                    select tileSet.Tiles[adjustedTileIndex]).FirstOrDefault();
        }
    }
}