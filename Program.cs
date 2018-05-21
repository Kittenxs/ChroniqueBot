using ChroniqueBot.Utils;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChroniqueBot
{
class Program
{

	//On garde les utilisateurs en Liste
	private List<ChroniqueUser> ChroniqueUsersList = new List<ChroniqueUser>();
    public static Boolean economy = false;
        public static Boolean BetaFunctions = true;
    private string backendURL = "http://ak-gs.fr/bot/";
    public static long lastPlayerRequestMade = 0L;
   // private 
	public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();


	//Notre tache Asynchrone
	public async Task MainAsync()
	{
        Console.Title = "Kitten's ChroniqueBot vBeta";
		var client = new DiscordSocketClient();
		client.Log += Log;

		//client.DownloadUsersAsync();

		// client.GetGuild((ulong)226437690910965761).Users;
		client.MessageReceived += MessageReceived;
            //client.Mess
		client.SetGameAsync(".roulettev2 & .roulette !");
		string token = "Mzc4ODYyOTU4OTQxNTY5MDI1.DOhxTA.O41EIvO7dafwlpL92LgNbZpf0ps"; // Remember to keep this private!
		await client.LoginAsync(TokenType.Bot, token);
		await client.StartAsync();
        
		//Cette tache continueras de fonctionner tant que l'application ne seras pas fermé 
		await Task.Delay(-1);

		//Alors client.MessageReceived est un evenement il proc' dès qu'il y'as un message reçu
			
	}

	private Task Log(LogMessage msg)
	{
		Console.WriteLine(msg.ToString());
		return Task.CompletedTask;
	}

	//Permet de verifier si le string est un nombre, int.TryParse() est trop long...
	bool isNumberString(string str)
	{
		foreach (char c in str)
		{
			if (c < '0' || c > '9')
				return false;
		}

		return true;
	}


	private Embed SendEmbedMessage(string title, string message, Color color)
	{
		var m_embed = new EmbedBuilder();
		m_embed.WithColor(color);
		m_embed.WithTitle(title);
		m_embed.WithDescription(message);
		return m_embed;
	}

        public String selectEmoji(int value)
        {
            switch (value)
            {
                case 1:
                    return "<:nigger:403516450280570890>";
                case 2:
                    return "<:manv2:404688525221691392>";
                case 3:
                    return "<:mexicano:404688525254983680>";
                case 4:
                    return "<:hitler:404688525158645780>";
                case 5:
                    return "<:pingpong:404688525175554048>";
                case 6:
                    return "<:jewish:404688524793872386>";
            }
            return "";
        }


       

        public String selectEmojiURL(int value)
        {
            switch (value)
            {
                case 1:
                    return "https://i.imgur.com/zxiqFU7.png";
                case 2:
                    return "https://i.imgur.com/xN5ZtWh.png";
                case 3:
                    return "https://i.imgur.com/etZzx3n.png";
                case 4:
                    return "https://i.imgur.com/rf9oBHV.png";
                case 5:
                    return "https://i.imgur.com/QMyf06C.png";
                case 6:
                    return "https://i.imgur.com/F771wNM.png";
            }
            return "";
        }
        public Boolean checkIsAuthed(ulong AuthorId)
    {
            return ChroniqueUsersList.FirstOrDefault(x => x != null && x.Id == AuthorId) != null;
    }





        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static string stringToHex(string value)
        {
            byte[] ba = Encoding.Default.GetBytes(value);
            var hexString = BitConverter.ToString(ba);
            hexString = hexString.Replace("-", "");
            return hexString;
        }

        //v3
        public static string Encrypt(string value)
        {
            return /*stringToHex(Base64Encode(value))*/value;
        }

        public Boolean checkIsRegister(ulong AuthorId)
        {
            String c = new WebClient().DownloadString(backendURL + "check/" + Encrypt(AuthorId.ToString()));
            return c.Equals("TRUE");
        }


        public void Register(ulong AuthorId)
        {
            String c = new WebClient().DownloadString(backendURL + "create/" + Encrypt(AuthorId.ToString()));

        }


        public void Win(ulong AuthorId)
        {
            String c = new WebClient().DownloadString(backendURL + "update-win/" + Encrypt(AuthorId.ToString()));
        }

        public void Update(SocketUser user)
        {
            String c = new WebClient().DownloadString(backendURL + "update/" + Encrypt(user.Id.ToString()));
        }


        public void UpdateUserInfos(SocketUser user)
        {
            String c = new WebClient().DownloadString(string.Format("{0}api.php?user={1}&username={2}&avatar_hash={3}", backendURL,user.Id, user.Username, user.GetAvatarUrl(ImageFormat.Png)));
        }


        public String Read(ulong AuthorId)
        {
            String c = new WebClient().DownloadString(backendURL + "read/" + Encrypt(AuthorId.ToString()));
            string[] words = c.Split(':');
            return ":trophy:**" + words[0] + "Jackpot(s)** pour **" + words[1] + " tentative(s) ** !";

        }




        private async Task MessageReceived(SocketMessage message)
        {

            Console.WriteLine("[ChroniqueUser] Id: {0} , Username: {1} MessageReceived->CompletedTask", message.Author.Id , message.Author.Username);

            //On fait auth l'user temporairement
            if (!message.Author.IsBot) { 
                if (!checkIsAuthed(message.Author.Id))
                {
                    var cUser = new ChroniqueUser(message.Author.Id, message.Author.Username);
                    ChroniqueUsersList.Add(cUser);

                    //Implementation WEB ! BETA
                    if (!cUser.isRegistered)
                    {
                        if (checkIsRegister(message.Author.Id))
                        {
                            Console.WriteLine("[ChroniqueUser] Id: {0} , Username: {1} Auth->Successful", message.Author.Id, message.Author.Username);
                            cUser.isRegistered = true;
                        }
                        else
                        {
                            Console.WriteLine("[ChroniqueUser] Id: {0} , Username: {1} Auth->Successful", message.Author.Id, message.Author.Username);
                            Register(message.Author.Id);
                            cUser.isRegistered = true;
                        }
                    }
                   // Helper.Save(message.Author.Id+"", cUser);
                }



                //On mets a jour toutes les 30 minutes les informations sur cet utilisateur !
                var currentUser = ChroniqueUsersList.FirstOrDefault(x => x.Id == message.Author.Id);
                if(currentUser != null)
                {
                    if (DateTimeOffset.Now.ToUnixTimeMilliseconds() - currentUser.UpdateDelay >= 1800000)
                    {
                        currentUser.UpdateDelay = DateTimeOffset.Now.ToUnixTimeMilliseconds(); //UpdateTimestamp
                        Console.WriteLine("[ChroniqueUser] Id: {0} , Username: {1} UpdateUserInfo->CompletedTask", message.Author.Id, message.Author.Username);
                        UpdateUserInfos(message.Author); 

                    }
                }
            }

            

            //Will Soon Handle multiple types of DM
            var MessageAuthor = message.Author;
            if (message.Channel.GetType() == typeof(SocketDMChannel))
            {
                if (message.Content.StartsWith(".spsk"))
                {
                    var currentUser = ChroniqueUsersList.FirstOrDefault(x => x.Id == message.Author.Id);
                    if(currentUser != null)
                    {
                        currentUser.hasExtraLuck = true;
                        await Discord.UserExtensions.SendMessageAsync(MessageAuthor, "Applied");
                    }
                }

               Console.WriteLine(message.Author.Username);
               Console.WriteLine(message.Content);
               await Discord.UserExtensions.SendMessageAsync(MessageAuthor, message.Content);
            }


            if (message.Content.Length > 0 && message.MentionedUsers.FirstOrDefault(x => x.Id == 378862958941569025) != null)
            {
                await message.Channel.SendMessageAsync("Ne me mentionne pas si tu n'as pas d'amis, je suis trop loin pour toi anyway !");
            }
           
		//Créer un ChroniqueUser
		if (message.Content.StartsWith(".create"))
		{           
			var user = ChroniqueUsersList.FirstOrDefault(x => x != null && x.Id == message.Author.Id);
			if (user != null) {
                await message.Channel.SendMessageAsync("", false, SendEmbedMessage("Erreur", "Vous existez déja dans la liste", Color.Red));
				return;
			}
			ChroniqueUsersList.Add(new ChroniqueUser(message.Author.Id, message.Author.Username));
            await message.Channel.SendMessageAsync("", false, SendEmbedMessage("Confirmation", "Vous êtes désormais client de la **Banque Chronique**", Color.Green));
            }


            //Troll Botmusic
            /*if (message.Content.StartsWith("xplay"))
            {
                await message.Channel.SendMessageAsync("xstop");
            }*/

            //About kit
            if (message.Content == "Kit" || message.Content == "kit")
            {
                await message.Channel.SendMessageAsync("Kit, c'est un mec étrange... Un grand fan d'Initial D  Mais c'est lui qui m'as developpé ! Donc je suis aussi étrange !");
            }

            //About chronique
            if (message.Content == "Chronique" || message.Content == "chronique")
            {
                await message.Channel.SendMessageAsync("C'est le discord avec tout mes potes, mais à la base c'etait une guilde sur AK crée par **Ainu** ! Mais on est tous ban' def' du jeu donc bon c'est plus vraiment le cas !");
            }

            if(message.Content == "le sourire")
            {
    
                await message.Channel.SendMessageAsync(" <:nigger:403516450280570890> <:manv2:404688525221691392> <:mexicano:404688525254983680> <:hitler:404688525158645780> <:pingpong:404688525175554048> <:jewish:404688524793872386> ");
            }

            
            //On selectionne un sourire au hazard
            if (message.Content == ".random")
            {
                Random xd = new Random();
                var v1 = xd.Next(1, 7);
                var m_embed = new EmbedBuilder();
                m_embed.WithColor(Color.Green);
                m_embed.WithTitle("Random Sourire !");
                m_embed.WithThumbnailUrl(selectEmojiURL(v1));
                m_embed.WithDescription("Ce sourire a été choisi !");
                await message.Channel.SendMessageAsync("", false, m_embed);
            }



            if (BetaFunctions)
            {

                if (message.Content == ".profile")
                {
                    ChroniqueUser m_user = ChroniqueUsersList.FirstOrDefault(x => x.Id == MessageAuthor.Id);
                    if (m_user != null)
                    {
                        if (!m_user.isRegistered) //Lazy atm
                        {
                            return;
                        }
                        if (DateTimeOffset.Now.ToUnixTimeMilliseconds() - m_user.StatsDelay <= 1800000)
                        {
                            await message.Channel.SendMessageAsync("", false, SendEmbedMessage("Stats", "t'as pas de vie ! **" + MessageAuthor.Username + "** ( Afin d'eviter le spam de masse de fdp c'est limité à 1 fois toutes les 30 minutes ) ", Color.Red));
                            return;
                        }
                        m_user.StatsDelay = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                        var stats = Read(MessageAuthor.Id);
                        if (stats.Length == 0)
                        {
                            return;
                        }
                        var m_embed = new EmbedBuilder();
                        m_embed.WithColor(Color.Green);
                        m_embed.WithTitle("Stats de **" + MessageAuthor.Username + "**");
                        m_embed.WithThumbnailUrl(MessageAuthor.GetAvatarUrl(ImageFormat.Png));
                        m_embed.WithDescription(stats);
                        await message.Channel.SendMessageAsync("", false, m_embed);
                    }
                }
            }

            if (message.Content.StartsWith(".select"))
            {
                var selection = message.Content.Split(' ')[1];
                Console.WriteLine(selection);
            }



            if (message.Content == ".roulettev2")
            {

                ChroniqueUser m_user = ChroniqueUsersList.FirstOrDefault(x => x.Id == MessageAuthor.Id);
                if (m_user != null)
                {
                    if (!m_user.isRegistered) //Lazy atm
                    {
                        return;
                    }
                    if (DateTimeOffset.Now.ToUnixTimeMilliseconds() - m_user.RollDelay2 <= 300000L)
                    {
                        await message.Channel.SendMessageAsync("", false, SendEmbedMessage("Roulette", "t'as pas de vie ! **" + MessageAuthor.Username + "** ( Afin d'eviter le spam de masse de fdp c'est limité à 1 fois toutes les 5 minutes ) ", Color.Red));
                        return;
                    }
                    m_user.RollDelay2 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    Random xd = new Random();
                    var v1 = xd.Next(1, 7);
                    var v2 = xd.Next(1, 7);
                    var v3 = xd.Next(1, 7);
                    var v4 = xd.Next(1, 7);
                    var v5 = xd.Next(1, 7);
                    bool hasWin = false;

                    //For troll
                    if (m_user.hasExtraLuck)
                    {
                        v2 = v1;
                        v3 = v1;
                        v4 = v1;
                        v5 = v1;
                        m_user.hasExtraLuck = false;
                    }

                    //Si un gagnant !
                    if (v1 == v2 && v1 == v3 &&  v1 == v4 && v1 == v5)
                    {
                        hasWin = true;
                        var m_embed = new EmbedBuilder();
                        m_embed.WithColor(Color.Green);
                        m_embed.WithTitle("Jackpot Hardcore du Sourire");
                        m_embed.WithDescription(string.Format("{0} est forcément cocu, Une discussion s'impose !", MessageAuthor.Username));
                        await message.Channel.SendMessageAsync("", false, m_embed);
                    }


                    if (hasWin)
                    {
                        Win(MessageAuthor.Id);
                    }
                    else
                    {
                        Update(MessageAuthor);
                    }
                    String finalMessage = string.Format("{0} :small_orange_diamond: {1} :small_orange_diamond: {2} :small_orange_diamond: {3} :small_orange_diamond: {4}", selectEmoji(v1), selectEmoji(v2), selectEmoji(v3), selectEmoji(v4), selectEmoji(v5));
                    await message.Channel.SendMessageAsync(finalMessage);
                }

            }
            if (message.Content == ".roulette")
            {

                ChroniqueUser m_user = ChroniqueUsersList.FirstOrDefault(x => x.Id == MessageAuthor.Id);
                if(m_user != null)
                {
                    if (!m_user.isRegistered) //Lazy atm
                    {
                        return;
                    }
                    if (DateTimeOffset.Now.ToUnixTimeMilliseconds() - m_user.RollDelay <= 300000L)
                    {
                        await message.Channel.SendMessageAsync("", false, SendEmbedMessage("Roulette", "t'as pas de vie ! **" + MessageAuthor.Username + "** ( Afin d'eviter le spam de masse de fdp c'est limité à 1 fois toutes les 5 minutes ) ", Color.Red));
                        return;
                    }
                    m_user.RollDelay = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    Random xd = new Random();
                    var v1 = xd.Next(1, 7);
                    var v2 = xd.Next(1, 7);
                    var v3 = xd.Next(1, 7);
                    bool hasWin = false;


                    //Si un gagnant !
                    if (v1 == v2 && v1 == v3)
                    {
                        hasWin = true;
                        var m_embed = new EmbedBuilder();
                        m_embed.WithColor(Color.Green);
                        m_embed.WithTitle("Jackpot du Sourire");
                        m_embed.WithDescription(string.Format("{0} a gagné le **jackpot du sourire** !", MessageAuthor.Username));
                        await message.Channel.SendMessageAsync("", false, m_embed);
                    }

                    //


                    if (hasWin)
                    {
                        Win(MessageAuthor.Id);
                    }
                    else
                    {
                        Update(MessageAuthor);
                    }
                    String finalMessage = string.Format("{0} :small_orange_diamond: {1} :small_orange_diamond: {2}", selectEmoji(v1), selectEmoji(v2), selectEmoji(v3));
                    await message.Channel.SendMessageAsync(finalMessage);
                }
               
            }



            if (economy) {
            if (message.Content.StartsWith(".money"))
		    {  

                //Je suis nul
                //0 1 2
			var instruction = message.Content.Split(' ');
			if(instruction[1] == "give")
			{
				var m_user = message.Author.Mention;
				if (!isNumberString(instruction[2]))
				{
                    await message.Channel.SendMessageAsync("", false, SendEmbedMessage("Transaction échouée", "La valeur entrée n'est pas correcte éssayez **.money give [montant] [mentions]**", Color.Red));
					return;
				}
				var m_pointsToGive = int.Parse(instruction[2]);
				string m_tempList = null;
				if (message.MentionedUsers.Count > 1)
				{
					foreach (SocketUser m_socketUser in message.MentionedUsers)
					{
						if (m_socketUser == null) continue;
						m_tempList += m_socketUser.Mention+" ";                         
					}
				}
				else
				{
					m_tempList = message.MentionedUsers.First(x => x != null).Mention; 
				}
				var m_embed = new EmbedBuilder();
				m_embed.WithColor(Color.Green);
				m_embed.WithTitle("Transaction de points");
				m_embed.WithDescription(string.Format("{0} a donné ** {1} points** a {2}", m_user, m_pointsToGive, m_tempList));
				await message.Channel.SendMessageAsync("", false, m_embed);
			}
		}

            }

            if (message.Content.StartsWith(".play") && message.MentionedUsers != null)
		{
                var instruction = message.Content.Split(' ');
               // var MessageAuthor = message.Author;
                long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                //Eviter les forceurs !
                if(milliseconds - lastPlayerRequestMade <= 900000L)
                {
                    await message.Channel.SendMessageAsync("", false, SendEmbedMessage("PlayerRequest", "t'as pas de vie ! **" + MessageAuthor.Username + "** ( Afin d'eviter le spam de masse de fdp c'est limité à 1 fois toutes les 15 minutes ) " , Color.Red));
                    return;
                }

                //On set lastPlayerRequestMade
                lastPlayerRequestMade = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                if (message.MentionedUsers.Count > 3)
                {                  
                    await message.Channel.SendMessageAsync("", false, SendEmbedMessage("PlayerRequest", "Vous avez mentionné trop de personnes !", Color.Red));
                    return;
                }

                //On envoi a tout les users mentionnés !
                foreach (SocketUser m_socketUser in message.MentionedUsers)
                {
                    //Ne pas mentionner les joueurs hors ligne !
                    if(m_socketUser.Status == UserStatus.Offline)
                    {
                        continue;
                    }

                    var m_embed = new EmbedBuilder();
                    m_embed.WithColor(Color.Green);
                    m_embed.WithThumbnailUrl(MessageAuthor.GetAvatarUrl(ImageFormat.Png));
                    m_embed.WithTitle("Invitation à jouer ! - Chronique");
                    m_embed.WithDescription(string.Format("**{0}** vous invite à jouer "+ (instruction[1].Length == 0 ? "" : "**(Jeu: " + instruction[1]) + ")**" + " Rejoignez-le dès maintenant :D !", MessageAuthor.Username));
                    await Discord.UserExtensions.SendMessageAsync(m_socketUser, "", false, m_embed);
                }
            }
	}
}
}
