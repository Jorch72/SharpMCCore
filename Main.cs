﻿using System;
using SharpMC.API;
using SharpMC.Entity;
using SharpMC;

namespace SharpCore
{
    [Plugin]
    public class Main : IPlugin
    {
        private PluginContext _context;
        public void OnEnable(PluginContext context)
        {
            _context = context;
        }

        public void OnDisable()
        {

        }

        [Command(Command = "world")]
        public void WorldCommand(Player player, string world)
        {
            switch (world)
            {
                case "overworld":
                    player.SendChat("Teleporting you to the Overworld!");
                    _context.LevelManager.TeleportToMain(player);
                    break;
                case "nether":
                    player.SendChat("Teleporting you to the Nether!");
                    _context.LevelManager.TeleportToLevel(player, "nether");
                    break;
                default:
                    player.SendChat("Unknown world! Choices: overworld, nether");
                    break;
            }
        }

        [Command(Command = "tnt")]
        public void TntCommand(Player player)
        {
            var rand = new Random();
            new ActivatedTNTEntity(player.Level)
            {
                KnownPosition = player.KnownPosition,
                Fuse = (rand.Next(0, 20) + 10)
            }.SpawnEntity();
        }

        [Command(Command = "TPS")]
        public void TpsCommand(Player player)
        {
            _context.LevelManager.MainLevel.CalculateTps(player);
        }

        [Command]
        public void Hello(Player player)
        {
            player.SendChat("Hello there!");
        }

        [Command(Command = "save-all")]
        public void SaveAllCommand(Player player)
        {
            foreach (var lvl in _context.LevelManager.GetLevels())
            {
                lvl.SaveChunks();
            }
            _context.LevelManager.MainLevel.SaveChunks();
        }

        [Command(Command = "gamemode")]
        public void Gamemode(Player player, int gamemode)
        {
            switch (gamemode)
            {
                case 0:
                    player.SetGamemode(SharpMC.Enums.Gamemode.Survival);
                    break;
                case 1:
                    player.SetGamemode(SharpMC.Enums.Gamemode.Creative);
                    break;
                case 2:
                    player.SetGamemode(SharpMC.Enums.Gamemode.Adventure);
                    break;
                case 3:
                    player.SetGamemode(SharpMC.Enums.Gamemode.Spectator);
                    break;
            }
        }

        [Command(Command = "stopserver")]
        public void StopServer(Player player, string message)
        {
            Globals.StopServer(message);
        }

        [Command(Command = "time")]
        public void Time(Player player)
        {
            player.SendChat(player.Level.GetWorldTime().ToString());
        }

        [Command(Command = "settime")]
        public void SetTime(Player player, int time)
        {
            player.Level.SetWorldTime(time);
            player.SendChat("Time set to: " + time);
        }

        [Command(Command = "rain")]
        public void Rain(Player player)
        {
            player.Level.timetorain = 0;
        }

        [Command(Command = "raintime", Description = "Set time until next rain or length of current rain")]
        public void Rain(Player player, int time)
        {
            player.Level.timetorain = time;
        }

        [Command(Command = "msg")]
        public void Msg(Player player, Player target, string message)
        {
            target.SendChat(player.Username + ": " + message);
            player.SendChat("Message sent to: " + target.Username);
        }

        [Command(Command = "tp")]
        public void Tp(Player player, Player target, Player target2 = null)
        {
            if(target2 != null)
            {
                target.PositionChanged(target2.KnownPosition.ToVector3(), target2.KnownPosition.Yaw);
                player.SendChat("Teleported " + target.Username + "to: " + target2.Username);
                target.SendChat("You've been teleported to: " + target2.Username);
            }
            else
            {
                player.PositionChanged(target.KnownPosition.ToVector3(), target.KnownPosition.Yaw);
                player.SendChat("Teleported you to: " + target.Username);

            }   
        }

        [Command(Command = "say")]
        public void Say(Player player, string message)
        {
            Globals.BroadcastChat(message, player);
            player.SendChat("Message sent to all players!");
        }
    }
}
