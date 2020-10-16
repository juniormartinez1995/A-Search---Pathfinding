using System;
using System.Collections.Generic;
using System.Linq;

namespace A_star_Search_Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Map m = new Map();

            List<Char> AuxChars = new List<Char>();

            AddCellValues(AuxChars);

            List<String> map = m.GenerateMap();

            //AuxChars.Contains('B')

            var start = new Tile();
            start.Y = map.FindIndex(x => x.Contains("B"));
            start.X = map[start.Y].IndexOf("B");


            var finish = new Tile();
            finish.Y = map.FindIndex(x => x.Contains("U"));
            finish.X = map[finish.Y].IndexOf("U");

            start.SetDistance(finish.X, finish.Y);

            var activeTiles = new List<Tile>();
            activeTiles.Add(start);
            var visitedTiles = new List<Tile>();

            while (activeTiles.Any())
            {
                var checkTile = activeTiles.OrderBy(x => x.CostDistance).First();

                if (checkTile.X == finish.X && checkTile.Y == finish.Y)
                {
                    var tile = checkTile;
                    Console.WriteLine("Fazendo o caminho de volta");
                    while (true)
                    {
                        Console.WriteLine($"{tile.X} : {tile.Y}");
                        if (map[tile.Y][tile.X] == 'G'
                    || map[tile.Y][tile.X] == 'P' || map[tile.Y][tile.X] == 'A'
                    || map[tile.Y][tile.X] == 'T' || map[tile.Y][tile.X] == 'F')
                        {
                            var newMapRow = map[tile.Y].ToCharArray();
                            newMapRow[tile.X] = '*';
                            map[tile.Y] = new string(newMapRow);
                        }
                        tile = tile.Parent;
                        if (tile == null)
                        {
                            Console.WriteLine("Mapa :");
                            map.ForEach(x => Console.WriteLine(x));
                            return;
                        }
                    }
                }

                visitedTiles.Add(checkTile);
                activeTiles.Remove(checkTile);

                var walkableTiles = GetWalkableTiles(map, checkTile, finish);

                foreach (var walkableTile in walkableTiles)
                {
                    //Ja passamos por essa célula então nao é necessario voltar
                    if (visitedTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                        continue;

                    if (activeTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                    {
                        var existingTile = activeTiles.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
                        if (existingTile.CostDistance > checkTile.CostDistance)
                        {
                            activeTiles.Remove(existingTile);
                            activeTiles.Add(walkableTile);
                        }
                    }
                    else
                    {
                        // Nao foi passada por esta célula antes, então adicione a lista de células ativas
                        activeTiles.Add(walkableTile);
                    }
                }
            }

            Console.WriteLine("No Path Found!");
        }

        private static void AddCellValues(List<Char> l)
        {
            l.Add('G');
            l.Add('P');
            l.Add('A');
            l.Add('T');
            l.Add('F');
        }

        private static List<Tile> GetWalkableTiles(List<string> map, Tile currentTile, Tile targetTile)
        {
            var possibleTiles = new List<Tile>()
            {
                new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1},
                new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
            };

            possibleTiles.ForEach(tile => tile.SetDistance(targetTile.X, targetTile.Y));


            var maxX = map.First().Length - 1;
            var maxY = map.Count - 1;

            int aux = 0;
            //Adiciona custos não genéricos para diferentes tiles do mapa
            foreach (var t in possibleTiles)
            {
                if (t.X >= 0 && t.X <= maxX && t.Y >= 0 && t.Y <= maxY)
                {
                    if (map[t.Y][t.X] == '3') possibleTiles.ElementAt(aux).Cost += 10; //Substituir o 10 por uma variável que armazena o valor do custo daquela celula
                }

                aux += 1;
            }

            return possibleTiles
                    .Where(tile => tile.X >= 0 && tile.X <= maxX)
                    .Where(tile => tile.Y >= 0 && tile.Y <= maxY)
                    .Where(tile => map[tile.Y][tile.X] == ' ' || map[tile.Y][tile.X] == 'U' || map[tile.Y][tile.X] == 'G' 
                    || map[tile.Y][tile.X] == 'P' || map[tile.Y][tile.X] == 'A' 
                    || map[tile.Y][tile.X] == 'T' || map[tile.Y][tile.X] == 'F')
                    .ToList();
        }
    }


}
