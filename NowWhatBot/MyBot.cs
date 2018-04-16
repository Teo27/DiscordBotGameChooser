using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace NowWhatBot
{
    class MyBot
    {
        DiscordClient discord;
        CommandService command;
        List<string> gamesArray;
        Random rand;

        public MyBot()
        {
            rand = new Random();

            gamesArray = new List<string>();

            discord = new DiscordClient();

            discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });

            command = discord.GetService<CommandService>();
            AddGame();
            GetList();
            ChooseGame();

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MzIzMTI5NzMxOTI1MTQ3NjQ5.DB2p8w.eB3QExPRsaa26cxaFIlPVUvVOkA", TokenType.Bot);
            });

        }

        private void GetList()
        {
            command.CreateCommand("games").Do(async (e) =>
            {
                string list = string.Join(", \n•", gamesArray.ToArray());
                
                if (string.IsNullOrEmpty(list))
                {
                    await e.Channel.SendMessage("There are no games in the list.");
                }
                else
                {
                    await e.Channel.SendMessage("**Games, currently in the list: **" + Environment.NewLine + "•" + list);
                }
                
            });
        }

        private void ChooseGame()
        {
            command.CreateCommand("play").Do(async (e) =>
            {
                int chosen = rand.Next(gamesArray.Count);
                await e.Channel.SendMessage("I think you should play: " + gamesArray[chosen]);
            });
        }

        private void AddGame()
        {
            command.CreateCommand("add").Parameter("parameters", ParameterType.Multiple).Do(async (e) =>
            {
                string args = string.Join(" ", e.Args);

                if (string.IsNullOrEmpty(args))
                {
                    await e.Channel.SendMessage("Please write a game after add.");
                    return;
                }
                try
                {
                    List<string> addingGames = new List<string>();
                    if (args.Contains(';'))
                    {

                        addingGames = args.Split(';').Select(p => p.Trim()).ToList();
                    }
                    else
                    {
                        addingGames.Add(args);
                    }

                    gamesArray.AddRange(addingGames);

                    string list = string.Join("\n•", addingGames.ToArray());

                    await e.Channel.SendMessage("**Adding games to the list: **" + Environment.NewLine + "•" + list);            
                    
                }
                catch(Exception e2)
                {
                    Console.WriteLine(e2.ToString());
                    await e.Channel.SendMessage("There was an error adding games to the list.");
                }



            });
        }

    }
}
