using System;
using Microsoft.Xna.Framework;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace PopupChat
{
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        public override string Author
        {
            get
            {
                return "Hunter101";
            }
        }

        public override string Description
        {
            get
            {
                return "A plugin that creates chat above your head (in your role color) when you talk.";
            }
        }

        public override string Name
        {
            get
            {
                return "Pop-up Chat";
            }
        }

        public override Version Version
        {
            get
            {
                return new Version(1, 1);
            }
        }

        public Plugin(Main game) : base(game)
        {
            Order = 0;
        }

        public override void Initialize()
        {
            ServerApi.Hooks.ServerChat.Register(this, OnChat);
            Commands.ChatCommands.Add(new Command("popupchat.quiet", Quiet, "quiet"));
        }

        void Quiet(CommandArgs args)
        {
            NetMessage.SendData(119, -1, -1, Terraria.Localization.NetworkText.FromLiteral(String.Join(" ", args.Parameters)), (int)new Color(args.Player.Group.R, args.Player.Group.G, args.Player.Group.B).PackedValue, args.Player.X + 8, args.Player.Y + 32);
        }

        void OnChat(ServerChatEventArgs args)
        {
            if (args.Text.StartsWith(Commands.Specifier) || args.Text.StartsWith(Commands.SilentSpecifier))
                return;
            NetMessage.SendData(119, -1, -1, Terraria.Localization.NetworkText.FromLiteral(args.Text), (int)new Color(TShock.Players[args.Who].Group.R, TShock.Players[args.Who].Group.G, TShock.Players[args.Who].Group.B).PackedValue, TShock.Players[args.Who].X + 8, TShock.Players[args.Who].Y + 32);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.ServerChat.Deregister(this, OnChat);
            }
            base.Dispose(disposing);
        }
    }
}